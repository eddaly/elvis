using UnityEngine;
using System.Collections;

public class FrontEnd : MonoBehaviour {
	
	private PersistentData persistentData;
	public Metagame metagame;
	public static string atlasFile = "test_atlus";
	
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
		Debug.Log (persistentData);
		
		// Ensure camera as assumed by FastGUI
		Camera.mainCamera.transform.position = new Vector3 (0, 0, -10);
		Camera.mainCamera.transform.rotation = Quaternion.identity;
		Camera.mainCamera.transform.localScale = new Vector3 (1, 1, 1);
		Camera.mainCamera.orthographic = true;
		Camera.mainCamera.orthographicSize = 768;
		Camera.mainCamera.nearClipPlane = .3f;
		Camera.mainCamera.farClipPlane = 11;
		
		// Need to specify if want to set UVs with original pixels because Unity rescales texture on load
		FastGUIElement.SetOriginalAtlasPixels (4096, 4096);
	}
	
	// Update is called once per frame
	void Update ()
	{
		FastGUIElement.DebugDrawSafeArea ();
	}
}
