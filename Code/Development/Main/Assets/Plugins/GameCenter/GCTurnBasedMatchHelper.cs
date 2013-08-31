using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class GCTurnBasedMatchHelper : MonoBehaviour
{
	// Interface to native implementation
	
	[DllImport ("__Internal")]
	private static extern bool _IsGameCenterAvailable ();

	[DllImport ("__Internal")]
	private static extern void _Start ();

	[DllImport ("__Internal")]
	private static extern void _FindMatchWithMaxPlayers (int maxPlayers);
		
	[DllImport ("__Internal")]
	private static extern void _SendTurn (int score);

	
	/* Public interface for use inside C# / JS code */
	
	// Singleton
	public static GCTurnBasedMatchHelper instance {get; set;}//new GCTurnBasedMatchHelper();
	void Awake ()
	{
		// Singleton
		if (instance == null) {
			instance = FindObjectOfType (typeof(GCTurnBasedMatchHelper)) as GCTurnBasedMatchHelper;
			if (instance == null)
				instance = this;
			DontDestroyOnLoad (gameObject);
		}
		gameObject.name = "GCTurnBasedMatchHelper_GameObject";
	}
	
	public static bool IsGameCenterAvailable
	{
		get {
			if (Application.platform == RuntimePlatform.IPhonePlayer)
				return _IsGameCenterAvailable();
			else
				return false;
		}	
	}
	
	// Tried to authenticates local user
	public static void Start ()
	{
		Debug.Log ("name = " + instance.gameObject.name);
		// Call plugin only when running on real device
		if (Application.platform == RuntimePlatform.IPhonePlayer)
			_Start ();
	}
	
	public static void FindMatchWithMaxPlayers (int maxPlayers)
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
			_FindMatchWithMaxPlayers (maxPlayers);
	}
	
	public static void SendTurn (int score)
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
			_SendTurn (score);
	}
	
	// Can trigger after Start() but also later if user changes GameCenter account login
	void AuthenticationChangeNotAuthenticatedCallback (string empty)
	{
		Debug.Log ("AuthenticationChangeNotAuthenticatedCallback");
	}
	
	void SendTurnSuccessCallback (string empty)
	{
		Debug.Log ("SendTurnSuccessCallback");
	}
	
	void SendTurnErrorCallback (string localizedErrorDescription)
	{
		Debug.Log ("SendTurnErrorCallback " + localizedErrorDescription);
	}
	
	void EndMatchSuccessCallback (string empty)
	{
		Debug.Log ("EndMatchSuccessCallback");
	}
	
	void EndMatchErrorCallback (string localizedErrorDescription)
	{
		Debug.Log ("EndMatchErrorCallback " + localizedErrorDescription);
	}

	// It's a new game
	void EnterNewGameCallback (string matchID)
	{
		Debug.Log ("EnterNewGame " + matchID);
	}

	// It's your turn
	void TakeTurnCallback (string matchID)
	{
		Debug.Log ("TakeTurn " + matchID);
	}
	
	// It's not your turn, just display the game state.
	void LayoutMatchCallback (string matchID)
	{
		Debug.Log ("LayoutMatch " + matchID);
		
		// Note may need to split this into 2 calls  (trying to keep parallel with Ray tutorial's delegate protocol
		/*if (match.status == GKTurnBasedMatchStatusEnded) {
        	statusString = @"Match Ended";
   		 } else {
        int playerNum = [match.participants 
          indexOfObject:match.currentParticipant] + 1;
        statusString = [NSString stringWithFormat:
          @"Player %d's Turn", playerNum];
    }*/
	}
	
	// Either a game has ended or it's your turn, either case for a match other than 'current'
	void SendNotice (string message)
	{
		Debug.Log ("SendNotce: User msg-> Another game needs your attention " + message);
	}

	void HandleMatchEnded (string matchID)
	{
		Debug.Log ("HandleMatchEnded " + matchID);
		// Note in Ray's sample call layoutMatch here to display results
	}
}

