//-----------------------------------------------------------------------------
// File: BitmapFont.cs
//
// Desc:	BMFont bitmap based font data
//
// Copyright Echo Peak Ltd 2013
//-----------------------------------------------------------------------------

using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;
using System;

[System.Serializable]
public class BitmapFont : ScriptableObject
{
	[System.Serializable]
	class Glyph
	{
		public int unicode;
		
		// where the bits are on the texture
		public float u0;
		public float u1;
		public float v0;
		public float v1;
		
		// relative on screen tile positions to get 1:1 mapping
		public float x;
		public float y;
		public float w;
		public float h;
		public float advance;
		
		public int page;
		public int kerning;

		public Glyph Clone() { return (Glyph)this.MemberwiseClone(); }
	}
	
	[System.Serializable]
	class KerningPair
	{
		public int unicodeFirst;
		public int unicodeSecond;
		public int offset;
	}
	
	[SerializeField] private Texture2D [] m_bitmapPages = null;
	[SerializeField] private Glyph [] m_glyphTable = null;
	[SerializeField] private KerningPair [] m_kerningTable = null;

	public int m_interCharacterSpacing = 0;
	
	private Texture2D LoadPage( int index, XmlNode page )
	{
		Texture2D bitmap = null;
		string pageName = page.Attributes["file"].Value;
		if( pageName != null && index >= 0 && index < m_bitmapPages.Length )
		{
			bitmap = Resources.Load( string.Format( "Fonts/{0}", 
						Path.GetFileNameWithoutExtension( pageName ))) as Texture2D;
			
			m_bitmapPages[index] = bitmap;
			
			if( bitmap == null )
			{
				EchoDebugLog.LogError( string.Format( "failed to load bitmap: {0}", pageName ));
			}
		}
		return bitmap;
	}
	
	private int LoadIntAttribute( XmlNode node, string name )
	{
		int attribute = -1;
		XmlAttribute xmlAttribute = node.Attributes[name];
		string attributeText = null;
		
		if( xmlAttribute != null )
			attributeText = xmlAttribute.Value;
		
		if( attributeText != null )
		{
			if (!int.TryParse( attributeText, out attribute ))
			{
				EchoDebugLog.LogError( string.Format( "could not load attribute {0}", name ));
				Debug.DebugBreak();
			}
		}
		
		return attribute;
	}
	
	private void LoadKerningPair( int index, XmlNode kerningPair )
	{
		if( m_kerningTable != null && index >= 0 && index < m_kerningTable.Length )
		{
			m_kerningTable[index] = new KerningPair();
			m_kerningTable[index].unicodeFirst = LoadIntAttribute( kerningPair, "first" );
			m_kerningTable[index].unicodeSecond = LoadIntAttribute( kerningPair, "second" );
			m_kerningTable[index].offset = LoadIntAttribute( kerningPair, "amount" );
		}
	}

	private int FindFirstKerningPair( int unicode )
	{
		int index = -1;
		
		if( m_kerningTable != null )
		{
			index = Array.FindIndex( m_kerningTable, 
				delegate( KerningPair entry )
				{
					return entry.unicodeFirst == unicode;
				});
		}
		
		return index;
	}

	private int LoadGlyph( int index, XmlNode glyph )
	{
		if( index >= 0 && index < m_glyphTable.Length )
		{
			int page = LoadIntAttribute( glyph, "page" );
			if ( page >= 0 && page < m_bitmapPages.Length && m_bitmapPages[page] != null )
			{
				float width = (float)m_bitmapPages[page].width;
				float height = (float)m_bitmapPages[page].height;

				float x = (float)LoadIntAttribute( glyph, "x" );
				float y = (float)LoadIntAttribute( glyph, "y" );
				float w = (float)LoadIntAttribute( glyph, "width" );
				float h = (float)LoadIntAttribute( glyph, "height" );

				int unicode = LoadIntAttribute( glyph, "id" );
				
				m_glyphTable[index] = new Glyph();
				m_glyphTable[index].page = page;
				m_glyphTable[index].unicode = unicode;
				m_glyphTable[index].u0 = x / width;
				m_glyphTable[index].u1 = (x + w) / width;
				m_glyphTable[index].v0 = y / height;
				m_glyphTable[index].v1 = (y + h) / height;
				m_glyphTable[index].x = (float)LoadIntAttribute( glyph, "xoffset" );
				m_glyphTable[index].y = (float)LoadIntAttribute( glyph, "yoffset" );
				m_glyphTable[index].w = w;
				m_glyphTable[index].h = h;
				m_glyphTable[index].advance = (float)LoadIntAttribute( glyph, "xadvance" );
				m_glyphTable[index].kerning = FindFirstKerningPair( unicode );

				return unicode;
			}
		}
		return -1;
	}

	public BitmapFont()
	{
	}
	
	public void Load( string filename )
	{
		m_bitmapPages = null;
		m_glyphTable = null;
		m_kerningTable = null;
		
		StreamReader reader = new StreamReader( filename );

		XmlDocument fontXml = new XmlDocument();

		fontXml.LoadXml( reader.ReadToEnd() );

		XmlNodeList list = fontXml.ChildNodes[1].ChildNodes;
		XmlNode info = null;
		XmlNode common = null;

		XmlNodeList pages = null;
		XmlNodeList glyphs = null;
		XmlNodeList kerning = null;

		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].Name == "info")
			{
				info = list[i];
			}

			if (list[i].Name == "common")
			{
				common = list[i];
			}

			if (list[i].Name == "pages")
			{
				pages = list[i].ChildNodes;
			}

			if (list[i].Name == "chars")
			{
				glyphs = list[i].ChildNodes;
			}

			if (list[i].Name == "kernings")
			{
				kerning = list[i].ChildNodes;
			}
		}	

		bool addNonBreakingSpace = true;

		// does the font contain a non-breaking space?
		const int NON_BREAKING_SPACE = 0xA0;
		foreach( XmlNode glyph in glyphs )
		{
			int unicode = LoadIntAttribute( glyph, "id" );
			if( unicode == NON_BREAKING_SPACE )
			{
				addNonBreakingSpace = false;
				break;
			}
		}


		if( info != null && common != null && pages != null && glyphs != null && 
				pages.Count > 0 && glyphs.Count > 0 )
		{
			m_bitmapPages = new Texture2D[pages.Count];
			int pagesLoaded = 0;
		
			foreach( XmlNode page in pages )
			{
				string idString = page.Attributes["id"].Value;
				int id = 0;
				if( idString != null && int.TryParse( idString, out id ) && 
						id >= 0 && id < pages.Count )
				{
					if( m_bitmapPages[id] )
					{
						EchoDebugLog.LogError( string.Format( "bitmap already loaded in slot {0}", id ));
						Debug.DebugBreak();
					}
					else
					{
						Texture2D bitmap = LoadPage( id, page );
						if( bitmap )
						{
							pagesLoaded++;
						}
						m_bitmapPages[id] = bitmap;
					}
				}
			}

			if( pagesLoaded < pages.Count )
			{
				EchoDebugLog.LogError( string.Format( "{0} bitmaps loaded out of {1}", 
													pagesLoaded, pages.Count ));
				Debug.DebugBreak();
			}
		
			if( kerning != null && kerning.Count > 0 )
			{
				m_kerningTable = new KerningPair[kerning.Count];
				int pairIndex = 0;
				foreach( XmlNode kerningPair in kerning )
				{
					LoadKerningPair( pairIndex, kerningPair );
					pairIndex++;
				}
			
				Array.Sort( m_kerningTable, 
					delegate( KerningPair lhs, KerningPair rhs )
					{
						return lhs.unicodeFirst.CompareTo( rhs.unicodeFirst );
					});
			
				if( pairIndex < kerning.Count )
				{
					EchoDebugLog.LogError( string.Format( "{0} kerning pairs loaded out of {1}", 
														pairIndex, kerning.Count ));
					Debug.DebugBreak();
				}				
			}

			m_glyphTable = new Glyph[glyphs.Count + (addNonBreakingSpace? 1 : 0)];

			int glyphsIndex = 0;
		
			foreach( XmlNode glyph in glyphs )
			{
				int unicode = LoadGlyph( glyphsIndex, glyph );

				if( unicode == ' ' && addNonBreakingSpace && glyphsIndex + 1 < m_glyphTable.Length )
				{
					Glyph nonBreakingSpace = m_glyphTable[glyphsIndex].Clone();
					nonBreakingSpace.unicode = NON_BREAKING_SPACE;
					glyphsIndex++;
					m_glyphTable[glyphsIndex] = nonBreakingSpace;
				}

				if( unicode != -1 ) glyphsIndex++;
			}

			if( glyphsIndex < glyphs.Count )
			{
				EchoDebugLog.LogError( string.Format( "{0} glyphs loaded out of {1}", 
													glyphsIndex, glyphs.Count ));
				Debug.DebugBreak();
			}
		}
	}
	
	public int FindCharacterCodeIndex( int unicode )
	{
		int index = -1;
		
		if( m_glyphTable != null )
		{
			index = Array.FindIndex( m_glyphTable, 
				delegate( Glyph entry )
				{
					return entry.unicode == unicode;
				});

			if( index == -1 )
			{
				index = Array.FindIndex( m_glyphTable,
					delegate( Glyph entry )
					{
						return entry.unicode == '_';
					});
			}
		}
		
		return index;
	}
	
	public int FindKerningPairIndex( int start, int unicode )
	{
		int index = -1;
		
		if( m_kerningTable != null )
		{
			index = Array.FindIndex( m_kerningTable, start,
				delegate( KerningPair entry )
				{
					return entry.unicodeSecond == unicode;
				});
		}
		
		return index;
	}
	
	public Texture2D BitmapForCharacterIndex( int index )
	{
		Texture2D bitmap = null;
		
		if( m_glyphTable != null && index >= 0 && index < m_glyphTable.Length)
		{
			int page = m_glyphTable[index].page;
			if( m_bitmapPages != null && page >= 0 && page < m_bitmapPages.Length )
			{
				bitmap = m_bitmapPages[page];
			}
		}
		
		return bitmap;
	}
	
	public bool UVsForCharacterIndex( int index, 
					out float u0, out float v0, out float u1, out float v1 )
	{
		bool found = false;
		
		if( m_glyphTable != null && index >= 0 && index < m_glyphTable.Length)
		{
			u0 = m_glyphTable[index].u0;
			v0 = m_glyphTable[index].v0;
			u1 = m_glyphTable[index].u1;
			v1 = m_glyphTable[index].v1;
			found = true;
		}
		else
		{
			u0 = 0.0f;
			v0 = 0.0f;
			u1 = 0.0f;
			v1 = 0.0f;
		}
		
		return found;	
	}
	
	public bool DimensionsForCharacterIndex( 
					int index, int next, out float x, 
					out float y, out float w, out float h, 
					out float advance )
	{
		bool found = false;
		
		if( m_glyphTable != null && index >= 0 && index < m_glyphTable.Length)
		{
			x = m_glyphTable[index].x;
			y = m_glyphTable[index].y;
			w = m_glyphTable[index].w;
			h = m_glyphTable[index].h;
			advance = m_glyphTable[index].advance + m_interCharacterSpacing;
			
			if( next >= 0 && m_glyphTable[index].kerning >= 0 )
			{
				int kerning = FindKerningPairIndex( 
									m_glyphTable[index].kerning, next );
				
				if( kerning >= 0 )
				{
					advance += m_kerningTable[kerning].offset;
				}
			}
			
			found = true;
		}
		else
		{
			x = 0.0f;
			y = 0.0f;
			w = 0.0f;
			h = 0.0f;
			advance = 0.0f;
		}
		
		return found;
	}

	public float Width( string line )
	{
		int character = -1;
		int characterIndex = -1;
		int nextCharacter = -1;
		float x, y, w, h, advance;
		float width = 0.0f;
		int textIndex = 0;
		for( textIndex = 0; textIndex < line.Length; ++textIndex )
		{
			character = line[textIndex];
			characterIndex = FindCharacterCodeIndex( character );
			if( characterIndex != -1 )
			{
				nextCharacter = -1;
				if( textIndex + 1 < line.Length )
				{
					nextCharacter = line[textIndex + 1];
				}
				
				DimensionsForCharacterIndex( characterIndex, nextCharacter,
							out x, out y, out w, out h, out advance );

				width += advance;
			}
		}

		return width;
	}

	public float OneEm()
	{
		float width = 0.0f;
		string line = "\u2014"; // the em dash character
		int index = FindCharacterCodeIndex( line[0] );
		float x, y, w, h;
		if( index != -1 )
			DimensionsForCharacterIndex( index, -1, out x, out y, out w, out h, out width );

		return width;
	}
}
