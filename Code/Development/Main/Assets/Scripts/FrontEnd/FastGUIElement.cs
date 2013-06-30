using UnityEngine;
using System.Collections;

public class FastGUIElement
{
	static public float safeScreenWidth = 2048f, safeScreenHeight = 1536f;
	static private int originalAtlasPixelsWidth, originalAtlasPixelsHeight;
	public float widthNormalised, heightNormalised;	// Normalised within bounds of safeScreenWidth / Height, e.g. width of .5 means half the safe width
	public float width, height; 					// In pixels
	
	public enum Position {XCENTRED, YCENTRED, XYCENTRED, TOPLEFT};
	
	private static int instanceCount = 0;
	protected BatchedQuadDef quad;
	protected int textureIdx;
	private Rect rect;
	
	// To enable specifying UVs in original pixels need to provide as Unity will rescale Texture
	static public void SetOriginalAtlasPixels (int width, int height)
	{
		originalAtlasPixelsWidth = width;
		originalAtlasPixelsHeight = height;
	}
	
	static public void DebugDrawSafeArea ()
	{
		// Safe area guides
		Debug.DrawLine (new Vector3 (-FastGUIElement.safeScreenWidth/2f, -FastGUIElement.safeScreenHeight/2f, 0), 
			new Vector3 (-FastGUIElement.safeScreenWidth/2f, +FastGUIElement.safeScreenHeight/2f, 0), Color.red);
		Debug.DrawLine (new Vector3 (+FastGUIElement.safeScreenWidth/2f, -FastGUIElement.safeScreenHeight/2f, 0), 
			new Vector3 (+FastGUIElement.safeScreenWidth/2f, +FastGUIElement.safeScreenHeight/2f, 0), Color.red);
	}
	
	// Make a GUIElement, will immediately render
	public FastGUIElement (
		Vector2 topLeft,	// Co-ords origin is top-left in pixels
		Vector4 uvs, 		// x,y is top-left, z,w is width and height in pixels
		Position pos = Position.TOPLEFT)
	{
		TextureAtlas frontendAtlas = PrimitiveLibrary.Get.m_Atlases[(int)TextureAtlas.AtlasID.FRONTEND];		
		if (instanceCount + 1 > frontendAtlas.m_NumElements)		
		{
			Debug.LogError ("Need to increase frontendAtlas.m_NumElements and PrimitiveLibrary.g_FrontendBatchSize");
			return;
		}
		textureIdx = instanceCount++;

		// Initialise the UVs
		frontendAtlas.m_UVSet[textureIdx] = new Vector4 (
			uvs.x / originalAtlasPixelsWidth,
			((originalAtlasPixelsHeight - uvs.y) - uvs.w) / originalAtlasPixelsHeight, // Note quad render has bottom zeroed Vs
			(uvs.x + uvs.z) / originalAtlasPixelsWidth,	
			(((originalAtlasPixelsHeight - uvs.y) - uvs.w) + uvs.w) / originalAtlasPixelsHeight);	
		
		// Setup the quad
		quad = PrimitiveLibrary.Get.GetQuadDefinition (PrimitiveLibrary.QuadBatch.FRONTEND_BATCH);
		quad.m_TextureIdx = textureIdx;
		quad.m_Colour = Color.white;
		
		// Use UVs for size
		widthNormalised = frontendAtlas.m_UVSet[textureIdx].z - frontendAtlas.m_UVSet[textureIdx].x;
		heightNormalised = frontendAtlas.m_UVSet[textureIdx].w - frontendAtlas.m_UVSet[textureIdx].y;
		width = uvs.z;
		height = uvs.w;		
		
		// The centre of the quad in world space which is mapped by camera to the safeSceen size ***could set camera in script to be safe
		if (pos == Position.TOPLEFT) {
			quad.m_Position.x = (-safeScreenWidth * .5f + topLeft.x) + (width * .5f);
			quad.m_Position.y = (+safeScreenHeight * .5f - topLeft.y) - (height * .5f);
		}
		else if (pos == Position.XYCENTRED) {
			quad.m_Position.x = (-safeScreenWidth * .5f + topLeft.x);
			quad.m_Position.y = (+safeScreenHeight * .5f - topLeft.y);
		}
		else if (pos == Position.XCENTRED) {
			quad.m_Position.x = (-safeScreenWidth * .5f + topLeft.x);
			quad.m_Position.y = (+safeScreenHeight * .5f - topLeft.y) - (height * .5f);
		}
		else if (pos == Position.YCENTRED) {
			quad.m_Position.x = (-safeScreenWidth * .5f + topLeft.x) + (width * .5f);
			quad.m_Position.y = (+safeScreenHeight * .5f - topLeft.y);
		}
		quad.m_Position.z = 0f;
		quad.m_Scale.x = width;
		quad.m_Scale.y = height;
		rect = ScreenRect (quad);
	}
	
	// Was the GUIElement tapped
	public bool Tapped ()
	{
		Vector2 tapPos;
		if (!getTapPos (out tapPos))
			return false;
		
		return (rect.Contains (tapPos));
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
		tapPos = Input.GetTouch(n).position;
#else
	if (!Input.GetMouseButton (0))
		return false;
	else
		touchPos = Input.mousePosition;
#endif
		return true;
	}
	
	private Rect ScreenRect (BatchedQuadDef quad)
	{
		Vector3 wp0 = new Vector3 (quad.m_Position.x + -.5f * quad.m_Scale.x, quad.m_Position.y + -.5f * quad.m_Scale.y, 0);
		Vector3 wp1 = new Vector3 (wp0.x + quad.m_Scale.x, wp0.y + quad.m_Scale.y, 0);
		Vector3 sp0 = Camera.mainCamera.WorldToScreenPoint (wp0);
		Vector3 sp1 = Camera.mainCamera.WorldToScreenPoint (wp1);

		return new Rect (sp0.x, sp0.y, sp1.x - sp0.x, sp1.y - sp0.y);
	}
}

