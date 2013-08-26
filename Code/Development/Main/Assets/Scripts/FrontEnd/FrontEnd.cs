using UnityEngine;
using System.Collections;

public class FrontEnd : MonoBehaviour {
	
	[HideInInspector]
	public PersistentData persistentData;
	private Metagame metagame;
	public string m_AtlasFile = "Frontend_Atlas";
	public int m_AtlasOriginalWidth = 4096;
	public int m_AtlasOriginalHeight = 4096;
	private static string displayMessage = null;
	private static GUISkin displayMessageGUISkin;
	
	// This is effectively a singleton
	public static FrontEnd instance {get; private set;}
	void Awake ()
	{
		// Singleton
		if (instance == null) {
			instance = FindObjectOfType (typeof(FrontEnd)) as FrontEnd;
			if (instance == null)
				instance = this;
		}
				
		// Need to specify if want to set UVs with original pixels because Unity rescales texture on load
		FastGUIElement.SetOriginalAtlasPixels (m_AtlasOriginalWidth, m_AtlasOriginalHeight);
	}
	
	// Use this for initialization
	void Start () {
		
		// Use editor PersistentData if available, otherwise create one
		Transform t;
		if ((t = transform.FindChild("PersistentData")) == null)
		{
			persistentData = gameObject.AddComponent<PersistentData>();
		}
		else
			persistentData = t.GetComponent<PersistentData>();
		
		// Ensure camera as assumed by FastGUI
		Camera.main.transform.position = new Vector3 (0, 0, -10);
		Camera.main.transform.rotation = Quaternion.identity;
		Camera.main.transform.localScale = new Vector3 (1, 1, 1);
		Camera.main.orthographic = true;
		Camera.main.orthographicSize = 768;
		Camera.main.nearClipPlane = .3f;
		Camera.main.farClipPlane = 11;
		
		// Load the GUISkin used for messages
		displayMessageGUISkin = Resources.Load ("FrontEnd_GUISkin") as GUISkin;
	}
	
	// Update is called once per frame
	void Update ()
	{
		FastGUIElement.DebugDrawSafeArea ();
	}
	
	// Ignore all inputs if true (required for ModalWindow as Unity still updating Monos, I guess just not GUIs)
	public static bool IgnoreInputs ()
	{
		return displayMessage != null;
	}
	
	// Display a modal message to the user, tap to dismiss, style as per Resource FrontEnd_GUISkin
	public static void DisplayMessage (string message)
	{
		displayMessage = message;
	}

	// Used for DisplayMessage()
	void OnGUI ()
	{
		if (displayMessage != null)
		{
			GUI.ModalWindow (0, new Rect (0, 0, Screen.width, Screen.height), 
				ModalUserMessage, displayMessage, displayMessageGUISkin.GetStyle ("Box"));
		}
	}
	
	// Display the message until tapped to dismiss
	private void ModalUserMessage (int windowID)
	{
		if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Ended)
			displayMessage = null;
		if (Application.isEditor)
		{
			if (Input.GetMouseButtonUp(0))
				displayMessage = null;
		}
	}

}
