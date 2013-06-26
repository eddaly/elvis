//-----------------------------------------------------------------------------
// File: BitmapFontRenderer.cs
//
// Desc:	BMFont bitmap based font renderer
//
// Copyright Echo Peak Ltd 2013
// <Paul Sinnett>
//-----------------------------------------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BitmapFontRenderer : MonoBehaviour 
{
	public enum HorizontalAlignment { Left, Centre, Right };
	public enum VerticalAlignment { Top, Centre, Bottom };
	
	public string m_text = "";
	public float m_scale;
	public int m_Layer = 8;
	public HorizontalAlignment m_horizonalAlignment = HorizontalAlignment.Left;
	public VerticalAlignment m_verticalAlignment = VerticalAlignment.Top;
	public int m_LineWidth = 0;
	public int m_LineSpacing = 0;
	public bool m_outlineFont = false;
	
	public BitmapFont m_font = null;
	public List<GameObject> m_glyphs = null;
	float m_leftEdge;
	float m_rightEdge;

	int CountCharacters( string line )
	{
		int count = 0;
		for( int textIndex = 0; textIndex < line.Length; ++textIndex )
		{
			if( m_font.FindCharacterCodeIndex( line[textIndex] ) != -1 )
			{
				count++;
			}
		}
		
		return count;
	}

	public float OneEm()
	{
		return m_font.OneEm();
	}

	public float Width()
	{
		return Mathf.Clamp( Width( m_text ), 0.0f, m_LineWidth );
	}

	public float Width( string line )
	{
		if (m_font != null && line != null)
		{
			return m_font.Width(line) * m_scale;
		}
		else
		{
			Console.WriteLine(Channels.Debug | Channels.Error, "Something is null in BitmapFontRenderer, m_font==null: {0}, line==null: {1}", m_font == null, line == null);
			return 0.0f;
		}
	}
	
	GameObject [] CreateGlyphs( string line, float yOffset )
	{
#if WOTD_MOBILE
		m_scale = 0.5f;	//half the glyphs size for retina textures
#endif

		float width = Width ( line );
		GameObject [] glyphs = new GameObject[CountCharacters( line )];
		
		int glyphIndex = 0;
		float offset = 0.0f;
		switch( m_horizonalAlignment )
		{
		case HorizontalAlignment.Right:
			offset = -width;
			break;
			
		case HorizontalAlignment.Centre:
			offset = -width / 2.0f;
			break;
		}
		
		m_leftEdge = m_rightEdge = offset;
		
		int character = -1;
		int characterIndex = -1;
		int nextCharacter = -1;
		float x, y, w, h, advance;
		for( int textIndex = 0; textIndex < line.Length; ++textIndex )
		{
			character = line[textIndex];
			nextCharacter = -1;
			if( textIndex + 1 < line.Length )
			{
				nextCharacter = line[textIndex + 1];
			}
			
			characterIndex = m_font.FindCharacterCodeIndex( character );
			if( characterIndex >= 0 )
			{				
				glyphs[glyphIndex] = PrimitiveLibrary.Get.GetText();

				if (glyphs[glyphIndex] != null)
				{
					glyphs[glyphIndex].layer = m_Layer;

					m_font.DimensionsForCharacterIndex(characterIndex, nextCharacter,
								out x, out y, out w, out h, out advance);

					Vector3 position = new Vector3((x + w * 0.5f) * m_scale + offset,
													(y + h * 0.5f) * -m_scale + yOffset,
													0.0f);

					glyphs[glyphIndex].transform.parent = transform;
					glyphs[glyphIndex].transform.localPosition = position;
					glyphs[glyphIndex].transform.localEulerAngles =
						new Vector3(0.0f, 0.0f, 0.0f);

					MeshRenderer renderer = glyphs[glyphIndex].GetComponent<MeshRenderer>();
					renderer.material.mainTexture = m_font.BitmapForCharacterIndex(characterIndex);

					if( m_outlineFont )
						renderer.material.shader = Shader.Find( "WOTD/UnlitOutlineFont" );
					else
						renderer.material.shader = Shader.Find( "WOTD/UnlitAlpha" );

					renderer.material.SetColor("_Color", new Color(1.0f, 1.0f, 1.0f, 0.0f));

					Vector3 scale = new Vector3(w * m_scale, h * m_scale, 0.0f);
					glyphs[glyphIndex].transform.localScale = scale;

					float u0, v0, u1, v1;
					m_font.UVsForCharacterIndex(characterIndex, out u0, out v0, out u1, out v1);
					PrimitivePlane.ModifyUVs(glyphs[glyphIndex], u0, 1.0f - v1, u1 - u0, v1 - v0);

					offset += m_scale * advance;

					glyphIndex++;
				}
				else
				{
					Console.WriteLine("Failed to get char from text pool");
				}
			}
		}

		m_leftEdge = Mathf.Min( m_leftEdge, offset );
		m_rightEdge = Mathf.Max( m_rightEdge, offset );

		return glyphs;
	}
	
	public float LeftEdge() { return m_leftEdge; }
	public float RightEdge() { return m_rightEdge; }
	
	bool LineBreakCharacter( int characterCode )
	{
		return characterCode == ' ';
	}
	
	// limitation: this won't word break words longer than the width...
	public string [] BreakLines( string text )
	{
		if( m_LineWidth > 0 )
		{
			List<string> lines = new List<string>();
			char [] splitCharacters = new char[] { '\n' };
			string [] paragraphs = text.Split( splitCharacters, System.StringSplitOptions.None );
			foreach( string paragraph in paragraphs )
			{
				string [] words = paragraph.Split( ' ' );
				string line = "";
				foreach( string word in words )
				{
					if( word.Trim() != "" )
					{
						string checkLine;
						if( line.Length > 0 )
						{
							checkLine = line + " " + word;
							if( Width( checkLine ) > m_LineWidth )
							{
								lines.Add( line );
								line = word;
							}
							else
							{
								line = checkLine;
							}
						}
						else
						{
							line = word;
						}
					}	
				}
				lines.Add( line );
			}
			return lines.ToArray();
		}
		else
		{
			return new string [1] { text };
		}
	}
	
	void CreateGlyphs()
	{
		if( m_glyphs != null )
		{
			EchoDebugLog.LogError( "call Release before calling CreateGlyphs" );
		}
		else if( m_font == null )
		{
			EchoDebugLog.LogError( "no font set" );
		}
		else
		{
			if (m_text != null)
			{
				string[] lines = BreakLines(m_text);
				m_glyphs = new List<GameObject>();
				float yOffset = 0.0f;
				switch (m_verticalAlignment)
				{
					case VerticalAlignment.Centre:
					yOffset = (lines.Length) * m_LineSpacing * 0.5f;
					break;
					case VerticalAlignment.Bottom:
					yOffset = (lines.Length) * m_LineSpacing;
					break;
				}
				foreach (string line in lines)
				{
					if (line != null)
					{
						m_glyphs.AddRange(CreateGlyphs(line, yOffset));
						yOffset -= m_LineSpacing;
					}
					else
					{
						Console.WriteLine(Channels.Debug | Channels.Error, "A line in {0} of FontRenderer {1}",m_text, name);
					}
				}
			}
			else
			{
				Console.WriteLine(Channels.Debug | Channels.Error, "m_text is null for FontRenderer {0}",name);
			}
		}
	}
	
	public void SetColor(Color color) 
	{
		if ( m_glyphs != null )
		{
			foreach( GameObject glyph in m_glyphs )
			{
				if( glyph != null )
				{
					glyph.renderer.material.color = color;
				}
			}
		}
	}

	public void SetOutlineColor(Color color)
	{
		if ( m_glyphs != null && m_outlineFont )
		{
			foreach( GameObject glyph in m_glyphs )
			{
				if( glyph != null )
				{
					glyph.renderer.material.SetColor( "_OutlineColor", color );
				}
			}
		}
	}

	public void Release()
	{
		if ( m_glyphs != null )
		{
			foreach( GameObject glyph in m_glyphs )
			{
				if( glyph != null )
				{
					PrimitiveLibrary.Get.ReleaseText( glyph );
				}
			}
		}
		
		m_glyphs = null;
	}

	void OnDestroy()
	{
		Release();
	}

	public void Reload()
	{
		Release();
		CreateGlyphs();
	}

	public void Refresh()
	{
		if( m_glyphs != null && m_glyphs.Count > 0 && m_glyphs[0] != null )
		{
			Color color = m_glyphs[0].renderer.material.color;
			Reload();
			SetColor( color );
		}
	}
}
