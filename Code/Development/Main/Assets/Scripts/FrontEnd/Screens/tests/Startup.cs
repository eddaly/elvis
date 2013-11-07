using UnityEngine;
using System.Collections;

using UnityEngine.SocialPlatforms.GameCenter;

public class Startup : MonoBehaviour {
	
	FastGUIElement playFGE, persistentDataFGE;
	
	// NOTE: iPad Retina is 2048 x 768
	
	// Use this for initialization
	void Start () {
	
		FrontEnd.instance.SelectAtlas ("EdTestAtlas");
			
		playFGE = new FastGUIElement (
			new Vector2 (0, 0),					// Screen position
			new Rect (0, 0, 2048, 768));		// Atlas position
		
		persistentDataFGE = new FastGUIElement (
			new Vector2 (0, playFGE.height),	// Screen position
			new Rect (0, 768, 2048, 768));	// Atlas position
		
		/*Social.localUser.Authenticate (success => {
			if (success) {
				Debug.Log ("Authentication successful");
				string userInfo = "Username: " + Social.localUser.userName + 
					"\nUser ID: " + Social.localUser.id + 
					"\nIsUnderage: " + Social.localUser.underage;
				Debug.Log (userInfo);
			}
			else
				Debug.Log ("Authentication failed");
		});*/
		GCTurnBasedMatchHelper.Start ();
	}
	
	void AchievementCallback (bool success)
	{
		Debug.Log ("Achievement Callback!! " + success);
	}
	
	void OnGUI ()
	{
		if (GUI.Button (new Rect (0, 0, 200, 100), "SHOW GC"))
		{
			Social.ShowLeaderboardUI ();
			Social.ShowAchievementsUI ();
		}
		else if (GUI.Button (new Rect (0, 200, 200, 100), "DO ACHIEVEMENT"))
		{
			Social.ReportProgress ("com.echopeak.elvis.achievement.test", 100, AchievementCallback);
		}
		else if (GUI.Button (new Rect (0, 400, 200, 100), "GET SCORE"))
		{
			Social.ReportScore (999, "com.echopeak.elvis.lb.test", success => {
				Debug.Log(success ? "Reported score successfully" : "Failed to report score");
			});
		}

		if (GUI.Button(new Rect (Screen.width / 2 + 50, 50, 75, 50), "Find Match"))
		{
			GCTurnBasedMatchHelper.FindMatchWithMaxPlayers (2);
			//callbacks?
		}
		
		if (GUI.Button(new Rect (Screen.width / 2 + 50, 150, 75, 50), "Send Turn"))
		{
			GCTurnBasedMatchHelper.SendTurn(345);
			//now look out for callback
		}

		//*** Note if querying call every 10 frames or so as these calls slow
	}
		
	// Update is called once per frame
	void Update () {		
		
		/*if (playFGE.Tapped ())
		{
			Debug.Log ("PLAY");
			FrontEnd.UnloadAtlases ();
			Application.LoadLevel ("RunningProto3");
		}
		else if (persistentDataFGE.Tapped ())
		{
			Debug.Log ("PERSISTENT DATA");
			Application.LoadLevel ("Store");
		}*/
	}
}


