using UnityEngine;
using System.Collections;

public class FrontEnd : MonoBehaviour {
	
	[HideInInspector]
	public PersistentData persistentData;
	public Metagame metagame;
	public string m_AtlasFile = "Frontend_Atlas";
	public int m_AtlasOriginalWidth = 4096;
	public int m_AtlasOriginalHeight = 4096;
	
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
		Debug.Log (persistentData);
		
		// Ensure camera as assumed by FastGUI
		Camera.main.transform.position = new Vector3 (0, 0, -10);
		Camera.main.transform.rotation = Quaternion.identity;
		Camera.main.transform.localScale = new Vector3 (1, 1, 1);
		Camera.main.orthographic = true;
		Camera.main.orthographicSize = 768;
		Camera.main.nearClipPlane = .3f;
		Camera.main.farClipPlane = 11;
	}
	
	// Update is called once per frame
	void Update ()
	{
		FastGUIElement.DebugDrawSafeArea ();
	}
}
