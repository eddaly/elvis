using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SecuredPlayerPrefsExample2 : MonoBehaviour {
	int clicksCounter;
	
	void Start() {
		// Init by setting the secret key, never ever change this line in your app !
		SecuredPlayerPrefs.SetSecretKey("FhuDwyXfFQO9gD9tg9PRq");

		// Get an int attribute
		clicksCounter = SecuredPlayerPrefs.GetInt("button-clicks", 0);
	}

	void OnGUI() {
		if (GUI.Button (new Rect (10, 10, 200, 200), "Clicked:" + clicksCounter)) {
			clicksCounter += 1;
			SecuredPlayerPrefs.SetInt("button-clicks", clicksCounter);
			SecuredPlayerPrefs.Save();
		}
		if (GUI.Button(new Rect (220, 10, 200, 200), "Open New Scene")) {
			Application.LoadLevelAsync("SecuredPlayerPrefsExampleScene");
		}
	}
}
