using UnityEngine;
using System.Collections;

// A simple progress bar, fills the stated rectangle with a provided colour
public class FastGUIProgressBar : FastGUIElement
{
	private Rect	barRectScreenCoords;	// Transformed for Graphics.DrawTexture with GL.LoadPixelMatrix()
	private Color	fillColour;
	private	Texture tex;
	private Material mat;
	
	// Make a Progress Bar, will immediately render
	public FastGUIProgressBar (
		Vector2 screenPos,					// Position on screen (relative to Position)
		Rect	atlasRect, 					// Atlas rectangle in pixels
		Rect	aRect, 						// Atlas rectangle for the progress bar (will be filled horizontally left to right)
		Color	aColour,					// Colour progress bar will be filled with
		Position pos = Position.TOPLEFT)
		: base (screenPos, atlasRect, pos)
	{
		// First need to transform from FastGUI coords to Unity screen-shots
		Vector3 sp0 = Camera.main.WorldToScreenPoint ( new Vector3 (
			aRect.x - FastGUIElement.safeScreenWidth/2,
			aRect.y - FastGUIElement.safeScreenHeight/2, 0));
		Vector3 sp1 = Camera.main.WorldToScreenPoint ( new Vector3 (
			aRect.x - FastGUIElement.safeScreenWidth/2 + aRect.width,
			aRect.y - FastGUIElement.safeScreenHeight/2 + aRect.height, 0));
		
		// Then need to flip Y as required by Graphics.DrawTexture
		sp0.y = Camera.main.pixelHeight - sp0.y;
		sp1.y = Camera.main.pixelHeight - sp1.y;

		// Ready for rendering (phew)
		barRectScreenCoords = new Rect (sp0.x, sp0.y, sp1.x - sp0.x, sp1.y - sp0.y);
		
		// Set up texture and material for the bar
		fillColour = aColour;
		tex = new Texture2D (16, 16, TextureFormat.ARGB32, false);
		mat = new Material (Shader.Find ("Self-Illumin/VertexLit"));
		mat.color = fillColour;
	}
	
	// Draw the progress bar at provided position, note must call from MonoBehaviour.OnRenderObject() to render over other elements
	public void Update (
		float t)	// 0 to 1 to fill stated rectangle with provided colour
	{	
		GL.PushMatrix();
        GL.LoadPixelMatrix();
		Graphics.DrawTexture (new Rect (barRectScreenCoords.x, barRectScreenCoords.y, barRectScreenCoords.width * Mathf.Min (t,1), barRectScreenCoords.height), tex, mat);
		GL.PopMatrix();
	}
}

