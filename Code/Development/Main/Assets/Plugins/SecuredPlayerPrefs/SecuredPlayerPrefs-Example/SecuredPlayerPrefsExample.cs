using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SecuredPlayerPrefsExample : MonoBehaviour {
	int clicksCounter;
	float floatTest;
	string stringTest;
	
	void Start() {
		// Init by setting the secret key, never ever change this line in your app !
		SecuredPlayerPrefs.SetSecretKey("FhuDwyXfFQO9gD9tg9PRq");
		
		// Get an int attribute
		clicksCounter = SecuredPlayerPrefs.GetInt("button-clicks", 0);

		// Get a float attribute
		floatTest = SecuredPlayerPrefs.GetFloat("float-test", 0f);

		// Get a string attribute
		stringTest = SecuredPlayerPrefs.GetString("string-test", "");
	}
		
	void OnApplicationQuit() {
		// Good practise to save Prefs when application quit
		SecuredPlayerPrefs.Save();
	}
	
	// Note: In this example we save after each modification. This can be a serious performance problem if you do that
	// in your app. You should call .Save() only when it's necessary. For instance:
	//   - When the user quits your app
	//   - When the user changes settings
	//   - When the user ends a game
	void OnGUI() {
		if (GUI.Button (new Rect (10, 10, 200, 200), "Clicked:" + clicksCounter)) {
			clicksCounter += 1;
			SecuredPlayerPrefs.SetInt("button-clicks", clicksCounter);
			SecuredPlayerPrefs.Save();
		}
		if (GUI.Button (new Rect (220, 10, 200, 200), "Float test:" + floatTest)) {
			floatTest += Random.Range(1f, 10f);
			SecuredPlayerPrefs.SetFloat("float-test", floatTest);
			SecuredPlayerPrefs.Save();
		}
		if (GUI.Button(new Rect (10, 220, 200, 200), "String test:" + stringTest)) {
			stringTest = "string-" + Random.Range(0, 10000);
			SecuredPlayerPrefs.SetString("string-test", stringTest);
			SecuredPlayerPrefs.Save();
		}
		if (GUI.Button(new Rect (220, 220, 200, 200), "Run test and delete All")) {
			floatTest = 0; stringTest = ""; clicksCounter = 0;
			SecuredPlayerPrefs.DeleteAll();
			SecuredPlayerPrefsTests.RunTests();
			SecuredPlayerPrefs.Save();
		}
		if (GUI.Button(new Rect (10, 430, 200, 200), "Open New Scene")) {
			Application.LoadLevelAsync("SecuredPlayerPrefsExampleScene2");
		}
		GUI.Label(new Rect (10, 430, 400, 200), "Debug: " + SecuredPlayerPrefs.ToPrettyString());
	}
}
