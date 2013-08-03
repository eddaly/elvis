using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;

public class FastGUIElement
{
	// Screen and Frontend Atlas size
	static public float safeScreenWidth = 2048f, safeScreenHeight = 1536f;
	static protected TextureAtlas frontendAtlas;
	static protected float originalAtlasPixelsWidth, originalAtlasPixelsHeight;
	private static int instanceCount = 0;			// Number of FastGUIElement instances
	
	// Size of element
	public float widthNormalised, heightNormalised;	// Normalised within bounds of safeScreenWidth / Height, e.g. width of .5 means half the safe width
	public float width, height; 					// In pixels
	
	// Relative position for screen position
	public enum Position {XCENTRED, YCENTRED, XYCENTRED, TOPLEFT};
		
	// Other stuff
	protected internal BatchedQuadDef quad;			// The quad that's rendered
	protected internal int textureIdx;				// Index in the Frontend Atlas
	protected Rect screenRect;						// Screen co-ordinates rect
	protected bool displayed = true;				// Is Element diplayed
	
	// Child elements
	protected List <FastGUIElement> children;
	
	// To enable specifying UVs in original pixels need to provide as Unity will rescale Texture
	static public void SetOriginalAtlasPixels (int width, int height)
	{
		originalAtlasPixelsWidth = width;
		originalAtlasPixelsHeight = height;
	}
	
	// Set if using UV data from XML file
	static public string uvxmlFile = "";
	
	static public void DebugDrawSafeArea ()
	{
		// Safe area guides
		Debug.DrawLine (new Vector3 (-FastGUIElement.safeScreenWidth/2f, -FastGUIElement.safeScreenHeight/2f, 0), 
			new Vector3 (-FastGUIElement.safeScreenWidth/2f, +FastGUIElement.safeScreenHeight/2f, 0), Color.red);
		Debug.DrawLine (new Vector3 (+FastGUIElement.safeScreenWidth/2f, -FastGUIElement.safeScreenHeight/2f, 0), 
			new Vector3 (+FastGUIElement.safeScreenWidth/2f, +FastGUIElement.safeScreenHeight/2f, 0), Color.red);
	}
	
	// Make a GUIElement and add to the ScrollWindow will immediately render
	public FastGUIElement (
		FastGUIScrollWindow scrollWindow,	// Add this element to ScrollWindow
		Vector2 scrollWindowPos,			// Position in window (origin is top-left) in pixels
		Vector4 atlasRect) 					// Atlas rectangle (x,y is top-leftx, z,w is width and height) in pixels
		: this (scrollWindowPos, atlasRect, Position.TOPLEFT)
	{
		scrollWindow.Add (this, scrollWindowPos);
	}

	// Make a GUIElement, will immediately render
	public FastGUIElement (
		Vector2 screenPos,	// Position on screen (relative to Position)
		Vector4 atlasRect, 	// Atlas rectangle (x,y is top-leftx, z,w is width and height) in pixels
		Position pos = Position.TOPLEFT)
	{
		// There is a fixed number of possible FastGUIElements
		frontendAtlas = PrimitiveLibrary.Get.m_Atlases[(int)TextureAtlas.AtlasID.FRONTEND];		
		if (instanceCount + 1 > frontendAtlas.m_NumElements)		
		{
			Debug.LogError ("Need to increase frontendAtlas.m_NumElements and PrimitiveLibrary.g_FrontendBatchSize");
			return;
		}
		textureIdx = instanceCount++;

		// Initialise the UVs
		frontendAtlas.m_UVSet[textureIdx] = new Vector4 (
			atlasRect.x / originalAtlasPixelsWidth,
			((originalAtlasPixelsHeight - atlasRect.y) - atlasRect.w) / originalAtlasPixelsHeight, // Note quad render has bottom zeroed Vs
			(atlasRect.x + atlasRect.z) / originalAtlasPixelsWidth,	
			(((originalAtlasPixelsHeight - atlasRect.y) - atlasRect.w) + atlasRect.w) / originalAtlasPixelsHeight);	
		
		// Setup the quad
		quad = PrimitiveLibrary.Get.GetQuadDefinition (PrimitiveLibrary.QuadBatch.FRONTEND_BATCH);
		quad.m_TextureIdx = textureIdx;
		quad.m_Colour = Color.white;
		
		// Use UVs for size
		widthNormalised = frontendAtlas.m_UVSet[textureIdx].z - frontendAtlas.m_UVSet[textureIdx].x;
		heightNormalised = frontendAtlas.m_UVSet[textureIdx].w - frontendAtlas.m_UVSet[textureIdx].y;
		width = atlasRect.z;
		height = atlasRect.w;		
		
		// The centre of the quad in world space which is mapped by camera to the safeSceen size
		if (pos == Position.TOPLEFT) {
			quad.m_Position.x = (-safeScreenWidth * .5f + screenPos.x) + (width * .5f);
			quad.m_Position.y = (+safeScreenHeight * .5f - screenPos.y) - (height * .5f);
		}
		else if (pos == Position.XYCENTRED) {
			quad.m_Position.x = (-safeScreenWidth * .5f + screenPos.x);
			quad.m_Position.y = (+safeScreenHeight * .5f - screenPos.y);
		}
		else if (pos == Position.XCENTRED) {
			quad.m_Position.x = (-safeScreenWidth * .5f + screenPos.x);
			quad.m_Position.y = (+safeScreenHeight * .5f - screenPos.y) - (height * .5f);
		}
		else if (pos == Position.YCENTRED) {
			quad.m_Position.x = (-safeScreenWidth * .5f + screenPos.x) + (width * .5f);
			quad.m_Position.y = (+safeScreenHeight * .5f - screenPos.y);
		}
		quad.m_Position.z = 0f;
		quad.m_Scale.x = width;
		quad.m_Scale.y = height;
		UpdatedQuad ();
		children = new List<FastGUIElement>();
	}
	
	// Add a child Element at the provided position
	public void Add (FastGUIElement child,
		Vector2 pos)		// x,y is top-left in pixels
	{
		children.Add (child);
		child.AddedBy (this, pos);
	}
	
	// Added by this parent Element at the provided position
	protected internal virtual void AddedBy (FastGUIElement parent,
		Vector2 pos)		// x,y is top-left in pixels
	{
		quad.m_Position =  new Vector3 (
			parent.quad.m_Position.x - parent.width/2 + width/2 + pos.x,
			parent.quad.m_Position.y + parent.height/2 - height/2 - pos.y, 0);
		UpdatedQuad ();
	}

	// Display the element
	public void SetDisplayed (bool show)
	{
		if (!show && displayed)
		{
			// Pull away the quad
			quad.m_Position -= new Vector3 (10000, 0, 0);
			displayed = false;
		}
		else if (show && !displayed)
		{
			// Replace the quad
			quad.m_Position += new Vector3 (10000, 0, 0);
			displayed = true;
		}
		foreach (FastGUIElement child in children)
		{
			child.SetDisplayed (show);
		}
	}
	
	// Was the GUIElement tapped
	public bool Tapped ()
	{
		if (!displayed)
			return false;
		
		Vector2 tapPos;
		if (!getTapPos (out tapPos))
			return false;
		
		return (screenRect.Contains (tapPos));
	}

	protected bool getTapPos (out Vector2 tapPos)
	{
		tapPos = Vector2.zero;
#if UNITY_IPHONE || UNITY_ANDROID
		int n, touchIdx = -1;
		for (n = 0; n < Input.touchCount; n++)
		{
			if (Input.GetTouch(n).phase == TouchPhase.Ended) {
				touchIdx = n;
				break;
			}
		}
		if (touchIdx == -1)
			return false;
		tapPos = Input.GetTouch(n).position;
#else
	if (!Input.GetMouseButtonUp (0))
		return false;
	else
		tapPos = Input.mousePosition;
#endif
		return true;
	}
	
	protected bool getTouchPos (out Vector2 touchPos)
	{
		touchPos = Vector2.zero;
#if UNITY_IPHONE || UNITY_ANDROID
		int n, touchIdx = -1;
		for (n = 0; n < Input.touchCount; n++)
		{
			if (Input.GetTouch(n).phase == TouchPhase.Began ||
				Input.GetTouch(n).phase == TouchPhase.Moved ||
				Input.GetTouch(n).phase == TouchPhase.Stationary) {
				touchIdx = n;
				break;
			}
		}
		if (touchIdx == -1)
			return false;
		touchPos = Input.GetTouch(n).position;
#else
	if (!Input.GetMouseButton (0))
		return false;
	else
		touchPos = Input.mousePosition;
#endif
		return true;
	}
	
	// Call if ever update the quad position (not in accessor to save on the calls)
	protected internal void UpdatedQuad ()
	{
		Vector3 wp0 = new Vector3 (quad.m_Position.x + -.5f * quad.m_Scale.x, quad.m_Position.y + -.5f * quad.m_Scale.y, 0);
		Vector3 wp1 = new Vector3 (wp0.x + quad.m_Scale.x, wp0.y + quad.m_Scale.y, 0);
		// Note unoptimal as assuming camera position elsewhere so could derive directly (person effort efficient if not CPU, and halfway toward supporting anywhere cameras!)
		Vector3 sp0 = Camera.main.WorldToScreenPoint (wp0);
		Vector3 sp1 = Camera.main.WorldToScreenPoint (wp1);

		screenRect = new Rect (sp0.x, sp0.y, sp1.x - sp0.x, sp1.y - sp0.y);
	}
	
	// Return UVs from provided textureFile (the original file used to create the xml), must first have set uvxmlFile
	public static Vector4 UVsFrom (string textureFile)
	{
		if (uvxmlFile.Length == 0)
		{
			Debug.LogError ("uvxmlFile not set, can't use UVsFrom");
			return Vector4.zero;
		}
		
		//*** Note could get/check/override atlas name and original width and size from this file
		//*** Note could also read this into memory once then dump out rather than keep reading
		
		// Create an XmlReader
		Vector4 v = Vector4.zero;
		using (XmlReader reader = XmlReader.Create (uvxmlFile))
		{
			bool fail = true;
			do {
				
			    if (!reader.ReadToFollowing ("sprite"))
					break;
			    if (!reader.MoveToAttribute (@"n"))
					break;
			    string aTextureFile = reader.Value;
				if (aTextureFile.Equals (textureFile, System.StringComparison.OrdinalIgnoreCase))
				{
					if (!reader.MoveToAttribute (@"x"))
						break;
					v.x = reader.ReadContentAsFloat();
					if (!reader.MoveToAttribute (@"y"))
						break;
					v.y = reader.ReadContentAsFloat();
					if (!reader.MoveToAttribute (@"w"))
						break;
					v.z = reader.ReadContentAsFloat();
					if (!reader.MoveToAttribute (@"h"))
						break;
					v.w = reader.ReadContentAsFloat();
					fail = false;
					break;
				}
			} while (true);
			if (fail)
				Debug.LogError ("Error reading XML");
		}
		return v;
	}

}

/* Note in case want to disable warnings from just constructing and not 'using'
 #pragma warning disable 414
	FastGUIElement ge;
#pragma warning restore 414
*/

