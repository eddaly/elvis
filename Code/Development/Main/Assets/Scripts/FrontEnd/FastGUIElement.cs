using UnityEngine;
using System.Collections;

public class FastGUIElement// : MonoBehaviour
{
	static public float safeScreenWidth = 2048f, safeScreenHeight = 1536f;
	static private int originalAtlasPixelsWidth, originalAtlasPixelsHeight;
	public float width, height; // Normalised within bounds of safeScreenWidth / Height, e.g. width of .5 means half the safe width
	
	public enum Position {XCENTRED, YCENTRED, XYCENTRED, TOPLEFT};
	
	private static int instanceCount = 0;
	private BatchedQuadDef quad;
	private Rect rect;
	
	// To enable specifying UVs in original pixels need to provide as Unity will rescale Texture
	static public void SetOriginalAtlasPixels (int width, int height)
	{
		originalAtlasPixelsWidth = width;
		originalAtlasPixelsHeight = height;
	}
	
	// Make a GUIElement, will immediately render
	public FastGUIElement (
		Vector2 topLeft,	// Co-ords origin is top-left, 1,1 is edge of safe area
		Vector4 uvs, 		// x,y is top-left, z,w is width and height in pixels
		Position pos = Position.XCENTRED)
	{
		TextureAtlas frontendAtlas = PrimitiveLibrary.Get.m_Atlases[(int)TextureAtlas.AtlasID.FRONTEND];		
		if (instanceCount + 1 > frontendAtlas.m_NumElements)		
		{
			Debug.LogError ("Need to increase frontendAtlas.m_NumElements and PrimitiveLibrary.g_FrontendBatchSize");
			return;
		}
		int textureIdx = instanceCount++;

		// Initialise the UVs
		//***frontendAtlas.m_TexturePage.width and height are both 1024?
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
		width = frontendAtlas.m_UVSet[textureIdx].z - frontendAtlas.m_UVSet[textureIdx].x;
		height = frontendAtlas.m_UVSet[textureIdx].w - frontendAtlas.m_UVSet[textureIdx].y;
		
		// The centre of the quad in world space which is mapped by camera to the safeSceen size ***could set camera in script to be safe
		if (pos == Position.TOPLEFT) {
			quad.m_Position.x = (-safeScreenWidth * .5f + topLeft.x * safeScreenWidth) + (width * .5f * safeScreenWidth);
			quad.m_Position.y = (+safeScreenHeight * .5f - height - topLeft.y * safeScreenHeight) - (height * .5f * safeScreenHeight);
		}
		else if (pos == Position.XYCENTRED) {
			quad.m_Position.x = (-safeScreenWidth * .5f + topLeft.x * safeScreenWidth);
			quad.m_Position.y = (+safeScreenHeight * .5f - height - topLeft.y * safeScreenHeight);
		}
		else if (pos == Position.XCENTRED) {
			quad.m_Position.x = (-safeScreenWidth * .5f + topLeft.x * safeScreenWidth);
			quad.m_Position.y = (+safeScreenHeight * .5f - height - topLeft.y * safeScreenHeight) - (height * .5f * safeScreenHeight);
		}
		else if (pos == Position.YCENTRED) {
			quad.m_Position.x = (-safeScreenWidth * .5f + topLeft.x * safeScreenWidth) + (width * .5f * safeScreenWidth);
			quad.m_Position.y = (+safeScreenHeight * .5f - height - topLeft.y * safeScreenHeight);
		}
		quad.m_Position.z = 0f;
		quad.m_Scale.x = width * safeScreenWidth;
		quad.m_Scale.y = height * safeScreenHeight;		// *** size needs to be the UVs as it's 1-1
		rect = ScreenRect (quad);
	}
	
	// Was the GUIElement tapped
	public bool Tapped ()
	{
		Vector2 tapPos;
#if UNITY_IPHONE || UNITY_ANDROID
		int touchIdx = -1;
		for (int n = 0; n < Input.touchCount; n++)
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
		return (rect.Contains (tapPos));
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

