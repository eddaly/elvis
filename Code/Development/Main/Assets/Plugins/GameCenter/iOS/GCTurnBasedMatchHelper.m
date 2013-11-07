//
//  GCTurnBasedMatchHelper.m
//  Unity-iPhone
//
//  Created by Ed Daly on 29/08/2013.
//
//

#import "GCTurnBasedMatchHelper.h"
#import "FlurryManager.h"

const char GameObjectName[] = "GCTurnBasedMatchHelper_GameObject";

@implementation GCTurnBasedMatchHelper

@synthesize gameCenterAvailable;
@synthesize currentMatch;
@synthesize delegate;

#pragma mark Initialization

static GCTurnBasedMatchHelper *sharedHelper = nil;
+ (GCTurnBasedMatchHelper *) sharedInstance {
    if (!sharedHelper) {
        sharedHelper = [[GCTurnBasedMatchHelper alloc] init];
    }
    return sharedHelper;
}

- (BOOL)isGameCenterAvailable {
    // check for presence of GKLocalPlayer API
    Class gcClass = (NSClassFromString(@"GKLocalPlayer"));
    
    // check if the device is running iOS 4.1 or later
    NSString *reqSysVer = @"4.1";
    NSString *currSysVer = [[UIDevice currentDevice] systemVersion];
    BOOL osVersionSupported = ([currSysVer compare:reqSysVer
                                           options:NSNumericSearch] != NSOrderedAscending);
    
    return (gcClass && osVersionSupported);
}


- (id)init {
    if ((self = [super init])) {
        gameCenterAvailable = [self isGameCenterAvailable];
        if (gameCenterAvailable) {
            NSNotificationCenter *nc =
            [NSNotificationCenter defaultCenter];
            [nc addObserver:self
                   selector:@selector(authenticationChanged)
                       name:GKPlayerAuthenticationDidChangeNotificationName
                     object:nil];
        }
    }
    return self;
}

- (void)authenticationChanged {
    
    if ([GKLocalPlayer localPlayer].isAuthenticated &&
        !userAuthenticated) {
        NSLog(@"Authentication changed: player authenticated.");
        userAuthenticated = TRUE;
    } else if (![GKLocalPlayer localPlayer].isAuthenticated &&
               userAuthenticated) {
        NSLog(@"Authentication changed: player not authenticated");
        UnitySendMessage (GameObjectName, "AuthenticationChangeNotAuthenticatedCallback", "");
        userAuthenticated = FALSE;
    }
    
}

#pragma mark User functions

- (void)authenticateLocalUser {
    
    if (!gameCenterAvailable) return;
    
    void (^setGKEventHandlerDelegate)(NSError *) = ^ (NSError *error)
    {
        GKTurnBasedEventHandler *ev =
        [GKTurnBasedEventHandler sharedTurnBasedEventHandler];
        ev.delegate = self;
    };
    
    NSLog(@"Authenticating local user...");
    if ([GKLocalPlayer localPlayer].authenticated == NO) {
#ifdef DELETEALLMATCHES
        ^(NSArray *matches, NSError *error){
            for (GKTurnBasedMatch *match in matches) {
                NSLog(@"%@", match.matchID);
                [match removeWithCompletionHandler:^(NSError *error){
                    NSLog(@"%@", error);}];
            }
#endif
        //***deprecated should update
            [[GKLocalPlayer localPlayer]
         authenticateWithCompletionHandler:
         setGKEventHandlerDelegate];
    } else {
        NSLog(@"Already authenticated!");
        setGKEventHandlerDelegate(nil);
    }
}

- (void)findMatchWithMinPlayers:(int)minPlayers
                     maxPlayers:(int)maxPlayers
                 viewController:(UIViewController *)viewController {
    if (!gameCenterAvailable) return;
    
    presentingViewController = viewController;
    
    GKMatchRequest *request = [[GKMatchRequest alloc] init];
    request.minPlayers = minPlayers;
    request.maxPlayers = maxPlayers;
    
    GKTurnBasedMatchmakerViewController *mmvc =
    [[GKTurnBasedMatchmakerViewController alloc]
     initWithMatchRequest:request];
    mmvc.turnBasedMatchmakerDelegate = self;
    mmvc.showExistingMatches = YES;
    
    [presentingViewController presentModalViewController:mmvc
                                                animated:YES];
}

- (void)sendTurn:(int)score
{
    NSData *data = [NSData dataWithBytes: &score length: sizeof(score)];
    // Note to read back [data getBytes: &score length: sizeof(score)];
    
    NSUInteger currentIndex = [currentMatch.participants
                               indexOfObject:currentMatch.currentParticipant];
    GKTurnBasedParticipant *nextParticipant;
    
    NSUInteger nextIndex = (currentIndex + 1) %
    [currentMatch.participants count];
    nextParticipant =
    [currentMatch.participants objectAtIndex:nextIndex];
    
    for (int i = 0; i < [currentMatch.participants count]; i++) {
        nextParticipant = [currentMatch.participants
                           objectAtIndex:((currentIndex + 1 + i) %
                                          [currentMatch.participants count ])];
        if (nextParticipant.matchOutcome !=
            GKTurnBasedMatchOutcomeQuit) {
            break;
        }
    }
    //***Note depreciated so should update
    [currentMatch endTurnWithNextParticipant:nextParticipant
                                matchData:data completionHandler:^(NSError *error)
    {
        if (error) {
            NSLog(@"%@", error);
            UnitySendMessage(GameObjectName, "SendTurnErrorCallback", [error.localizedDescription UTF8String]);
        } else {
            UnitySendMessage(GameObjectName, "SendTurnSuccessCallback", "");
        }
    }];
    NSLog(@"Send Turn, %@, %@", data, nextParticipant);
}
    
- (void)endMatch
    {
        //NSData *data = [NSData dataWithBytes: &score length: sizeof(score)];
        // Note to read back [data getBytes: &score length: sizeof(score)];
  
        //*** tbd if game over, api to do, e.g. remove from sendTurn and make a endMatch method
        for (GKTurnBasedParticipant *part in currentMatch.participants) {
            part.matchOutcome = GKTurnBasedMatchOutcomeTied;    //***tbd Need to set correct outcome here (e.g. send in scores as param?)
        }
        [currentMatch endMatchInTurnWithMatchData:nil   //*** Sending nil as not sure what else to send or why?
                                completionHandler:^(NSError *error) {
                                    if (error) {
                                            NSLog(@"%@", error);
                                            UnitySendMessage(GameObjectName, "EndMatchErrorCallback", [error.localizedDescription UTF8String]);
                                        } else {
                                            UnitySendMessage(GameObjectName, "EndMatchSuccessCallback", "");
                                        }
                                    }];
        NSLog(@"End Match");
    }


#pragma mark GKTurnBasedMatchmakerViewControllerDelegate

-(void)turnBasedMatchmakerViewController:
(GKTurnBasedMatchmakerViewController *)viewController
                            didFindMatch:(GKTurnBasedMatch *)match {
    //***deprecated should update
    [presentingViewController
     dismissModalViewControllerAnimated:YES];
    self.currentMatch = match;
    GKTurnBasedParticipant *firstParticipant =
    [match.participants objectAtIndex:0];
    if (firstParticipant.lastTurnDate == NULL) {
        // It's a new game!
        //[delegate enterNewGame:match];
        UnitySendMessage(GameObjectName, "EnterNewMatchCallback", [match.matchID UTF8String]);
    } else {
        if ([match.currentParticipant.playerID
             isEqualToString:[GKLocalPlayer localPlayer].playerID]) {
            // It's your turn!
            //[delegate takeTurn:match];
            UnitySendMessage(GameObjectName, "TakeTurnCallback", [match.matchID UTF8String]);
        } else {
            // It's not your turn, just display the game state.
            //[delegate layoutMatch:match];
            UnitySendMessage(GameObjectName, "LayoutMatchCallback", [match.matchID UTF8String]);
        }
    }
}

-(void)turnBasedMatchmakerViewControllerWasCancelled:
(GKTurnBasedMatchmakerViewController *)viewController {
    //***deprecated should update
    [presentingViewController
     dismissModalViewControllerAnimated:YES];
    NSLog(@"has cancelled");
}

-(void)turnBasedMatchmakerViewController:
(GKTurnBasedMatchmakerViewController *)viewController
                        didFailWithError:(NSError *)error {
    //***deprecated should update
    [presentingViewController
     dismissModalViewControllerAnimated:YES];
    NSLog(@"Error finding match: %@", error.localizedDescription);
}

-(void)turnBasedMatchmakerViewController:
(GKTurnBasedMatchmakerViewController *)viewController
                      playerQuitForMatch:(GKTurnBasedMatch *)match {
    NSUInteger currentIndex =
    [match.participants indexOfObject:match.currentParticipant];
    GKTurnBasedParticipant *part;
    
    for (int i = 0; i < [match.participants count]; i++) {
        part = [match.participants objectAtIndex:
                (currentIndex + 1 + i) % match.participants.count];
        if (part.matchOutcome != GKTurnBasedMatchOutcomeQuit) {
            break;
        }
    }
    NSLog(@"playerquitforMatch, %@, %@",
          match, match.currentParticipant);
    [match participantQuitInTurnWithOutcome:
     GKTurnBasedMatchOutcomeQuit nextParticipant:part
                                  matchData:match.matchData completionHandler:nil];
}
    
#pragma mark GKTurnBasedEventHandlerDelegate
    
    // Note called when player invites a friend from the GM app and then loads the game
    // this is who the game needs to invite to play (i.e. it is not receiving an invite)
    -(void)handleInviteFromGameCenter:(NSArray *)playersToInvite {
        //***deprecated should update
        [presentingViewController
         dismissModalViewControllerAnimated:YES];
        GKMatchRequest *request =
        [[[GKMatchRequest alloc] init] autorelease];
        request.playersToInvite = playersToInvite;
        request.maxPlayers = 2; // Note hardcoding 2 players despite C interface allowing n (should fix)
        request.minPlayers = 2;
        GKTurnBasedMatchmakerViewController *viewController =
        [[GKTurnBasedMatchmakerViewController alloc]
         initWithMatchRequest:request];
        viewController.showExistingMatches = NO;
        viewController.turnBasedMatchmakerDelegate = self;
        //***deprecated should update
        [presentingViewController
         presentModalViewController:viewController animated:YES];
    }
    
    -(void)handleTurnEventForMatch:(GKTurnBasedMatch *)match {
        NSLog(@"Turn has happened");
        if ([match.matchID isEqualToString:currentMatch.matchID]) {
            if ([match.currentParticipant.playerID
                 isEqualToString:[GKLocalPlayer localPlayer].playerID]) {
                // it's the current match and it's our turn now
                self.currentMatch = match;
                //[delegate takeTurn:match];
                UnitySendMessage(GameObjectName, "TakeTurnCallback", [match.matchID UTF8String]);
            } else {
                // it's the current match, but it's someone else's turn
                self.currentMatch = match;
                //[delegate layoutMatch:match];
                UnitySendMessage(GameObjectName, "LayoutMatchCallback", [match.matchID UTF8String]);
            }
        } else {
            if ([match.currentParticipant.playerID
                 isEqualToString:[GKLocalPlayer localPlayer].playerID]) {
                // it's not the current match and it's our turn now
                //[delegate sendNotice:@"It's your turn for another match"
                  //          forMatch:match];
                UnitySendMessage(GameObjectName, "SendNotice", "It's your turn for another match");//[match.matchID UTF8String]);
            } else {
                // it's the not current match, and it's someone else's
                // turn
            }
        }
    }
    
    -(void)handleMatchEnded:(GKTurnBasedMatch *)match {
        NSLog(@"Game has ended");
        if ([match.matchID isEqualToString:currentMatch.matchID]) {
            //[delegate recieveEndGame:match];
            UnitySendMessage(GameObjectName, "HandleMatchEnded", [match.matchID UTF8String]);
        } else {
            //[delegate sendNotice:@"Another Game Ended!" forMatch:match];
            UnitySendMessage(GameObjectName, "SendNotice", "Another Game Ended!");
        }
    }

@end

// Converts C style string to NSString
NSString* CreateNSString (const char* string)
{
	if (string)
		return [NSString stringWithUTF8String: string];
	else
		return [NSString stringWithUTF8String: ""];
}

// Helper method to create C string copy
char* MakeStringCopy (const char* string)
{
	if (string == NULL)
		return NULL;
	
	char* res = (char*)malloc(strlen(string) + 1);
	strcpy(res, string);
	return res;
}

// When native code plugin is implemented in .mm / .cpp file, then functions
// should be surrounded with extern "C" block to conform C function naming rules
//extern "C" {

bool _IsGameCenterAvailable ()
{
    return [[GCTurnBasedMatchHelper sharedInstance] isGameCenterAvailable];
}
    
void _Start ()
{
    [[GCTurnBasedMatchHelper sharedInstance] authenticateLocalUser];
}

void _FindMatchWithMaxPlayers (int maxPlayers)
{
    [[GCTurnBasedMatchHelper sharedInstance]
     findMatchWithMinPlayers:2 maxPlayers:maxPlayers viewController: [FlurryManager unityViewController]] ; //UnityGetGLViewController a pain to link with (mm/m issue I think)
}

void _SendTurn (int score)
{
    [[GCTurnBasedMatchHelper sharedInstance] sendTurn: score];
}

void _EndMatch ()
{
    [[GCTurnBasedMatchHelper sharedInstance] endMatch];
}



/*	const char* _GetLookupStatus ()
	{
		// By default mono string marshaler creates .Net string for returned UTF-8 C string
		// and calls free for returned value, thus returned strings should be allocated on heap
		return MakeStringCopy([[matchObject getStatus] UTF8String]);
	}
*/