using UnityEngine;
using System.Collections;

// A simple progress bar, fills the stated rectangle with a provided colour
public class FastGUIProgressBar : FastGUIElement
{
	private float	barComplete = 0;
	private Rect	barRect;
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
		barRect = atlasRect;
		fillColour = aColour;
		//tex = new Texture2D (100, 100, TextureFormat.ARGB32, false);
		tex = (Texture)Resources.Load( "Coins_Atlas" );
		mat = new Material (Shader.Find ("VertexLit"));// Use a editor matieral as easier and can be tweaked
		mat.color = fillColour;
	}
	
	// Draw the progress bar at provided position
	public void Update (
		float t)	// 0 to 1 to fill stated rectangle with provided colour
	{
		Debug.Log (t);
		Graphics.DrawTexture (new Rect (10, 10, 256, 512), tex);//, mat);
	}
}

