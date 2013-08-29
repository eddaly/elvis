using UnityEngine;
using System.Collections;

public class FrontEnd : MonoBehaviour
{
#pragma warning disable 414
	private PersistentData persistentData;	// Create one of these (a persistent singleton)
#pragma warning restore 414

	private Metagame metagame;
	private static string displayMessage = null;
	private static GUISkin displayMessageGUISkin;
	
	private class Atlas
	{
		internal string file;
		internal int originalWidth;
		internal int originalHeight;
		internal TextureAtlas textureAtlas;
		internal GameObject quadBatch;
	};
	private static Atlas[] atlases = {
		new Atlas {file = "EdTestAtlas",		originalWidth = 2048, originalHeight = 1536, textureAtlas = null},//TEST
		new Atlas {file = "Frontend_Atlas",		originalWidth = 4096, originalHeight = 4096, textureAtlas = null},
		new Atlas {file = "Challenges_Atlas",	originalWidth = 4096, originalHeight = 2048, textureAtlas = null},
		new Atlas {file = "PreGame_Atlas",		originalWidth = 2048, originalHeight = 2048, textureAtlas = null},
		new Atlas {file = "Record_Atlas",		originalWidth = 4096, originalHeight = 4096, textureAtlas = null},
		new Atlas {file = "PostGame_Atlas",		originalWidth = 4096, originalHeight = 2048, textureAtlas = null},
		//new Atlas {file = "Missions_Atlas",		originalWidth = 4096, originalHeight = 4096, textureAtlas = null},
		//new Atlas {file = "HUB",		originalWidth = 4096, originalHeight = 4096, textureAtlas = null},
	};
		
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
		
		// Set-up the FrontEnd Atlases
		for (int n = 0; n < atlases.Length; n++)
		{
			// Unless already loaded (FrontEnd comes and gos)
			if (atlases[n].textureAtlas == null)
			{
				atlases[n].textureAtlas = new TextureAtlas();
				atlases[n].textureAtlas.m_TexturePage = Resources.Load (atlases[n].file) as Texture;
				if (atlases[n].textureAtlas.m_TexturePage == null)
					Debug.Log ("Failed to load Atlas " + atlases[n].textureAtlas.m_TexturePage);
				atlases[n].textureAtlas.m_NumElements = 128;		
				atlases[n].textureAtlas.m_UVSet = new Vector4 [atlases[n].textureAtlas.m_NumElements];
	
				atlases[n].quadBatch = PrimitiveQuadBatch.MakeQuadBatch ( 
					128, 
					atlases[n].textureAtlas, 
					-1,	// Flag that not using PrimitiveLibrary 
					false, false, false, 0);
				atlases[n].quadBatch.transform.localPosition = Vector3.zero;
				GameObject.DontDestroyOnLoad (atlases[n].quadBatch);
				atlases[n].quadBatch.SetActive (false);	// Switched on by SelectAtlas()
			}
		}
	}
	
	public void SelectAtlas (string atlasFilename)
	{
		Atlas atlas = null;
		for (int n = 0; n < atlases.Length; n++)
		{
			if (atlases[n].file == atlasFilename)
			{
				atlas = atlases[n];
				break;
			}
		}
		if (atlas == null)
		{
			Debug.Log ("Atlas: " + atlasFilename + " Not found in Frontend object, prepare to fallover");
			return;
		}
		
		atlas.quadBatch.SetActive (true);
		
		// Need to specify if want to set UVs with original pixels because Unity rescales texture on load
		FastGUIElement.SelectAtlas (atlas.textureAtlas, atlas.originalWidth, atlas.originalHeight, atlas.quadBatch);
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
		
		// Load the GUISkin used for messages (unless already loaded as is static)
		if (displayMessageGUISkin == null)
			displayMessageGUISkin = Resources.Load ("FrontEnd_GUISkin") as GUISkin;
	}
	
	// Update is called once per frame
	void Update ()
	{
		FastGUIElement.DebugDrawSafeArea ();
	}
	
	// Draw the quads
	void LateUpdate ()
	{
		for (int n = 0; n < atlases.Length; n++)		
			PrimitiveQuadBatch.SetQuadVB (atlases[n].quadBatch);
	}
	
	// Unload the FE Atlases (and Quad primitive data) as are static and by default DontDestroyOnLoad
	static bool unloadAtlases = false;
	static public void UnloadAtlases ()
	{
		unloadAtlases = true;
	}
	
	// Unload the Atlases which are static so otherwise hangaround (useful across FE scenes but need to free up the memory for the game)
	// There is forum chat on unloading not working properly but may be out of date
	static private void ForceUnload ()
	{
		for (int n = 0; n < atlases.Length; n++)
		{
			Resources.UnloadAsset (atlases[n].textureAtlas.m_TexturePage);	// Note Unity doc says something about it being unloaded when it is accessed (eh?)
			atlases[n].textureAtlas.m_TexturePage = null;
			atlases[n].textureAtlas = null;
			if (atlases[n].quadBatch != null)	// Can be destroyed before get here when application quits
			{
				atlases[n].quadBatch.GetComponent<PrimitiveQuadBatchInfo>().m_Atlas = null;	// Remove reference to held garbage collection
				PrimitiveQuadBatch.ReclaimQuadDefs (atlases[n].quadBatch);	// May not be needed
				GameObject.Destroy (atlases[n].quadBatch);
				atlases[n].quadBatch = null;
			}
		}
		Resources.UnloadUnusedAssets ();	// Unity docs and forums suggest this sometimes needed for Textures, I'm unclear on how works, should examine memory
	}										// Note in debugger UnloadAsset is slow and this is quick so perhaps not doing anything
	
	// Prepare to switch screens
	void OnDestroy ()
	{
		// Force unload?
		if (unloadAtlases)
		{
			ForceUnload ();
			unloadAtlases = false;
			return;
		}
		
		// Or just release the quad defs
		for (int n = 0; n < atlases.Length; n++)
		{
			if (atlases[n].quadBatch != null)	// Can be destroyed before get here when application quits
			{
				// Marks all the quads as not active so they can be used again if/when FastGUIElements return to recreate
				PrimitiveQuadBatch.ReclaimQuadDefs (atlases[n].quadBatch);	
				atlases[n].quadBatch.SetActive (false);
			}
		}
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
