using UnityEngine;
using System.Collections;

public class StringTester : MonoBehaviour 
{
	public BitmapFontRenderer m_text;
	public BitmapFont m_font;
	public Color m_color;
	public string m_stringID;
	
	private BitmapFont m_currentFont = null;
	private Color m_currentColor;
	private string m_currentStringID = null;
	
	// Use this for initialization
	void Start() 
	{
	}
	
	// Update is called once per frame
	void Update() 
	{
		if( m_text != null )
		{
			bool reload = false;
			if( m_currentFont != m_font )
			{
				m_currentFont = m_font;
				m_text.m_font = m_currentFont;
				reload = true;
			}
			
			if( m_currentColor != m_color )
			{
				m_currentColor = m_color;
				reload = true;
			}
			
			if( m_currentStringID != m_stringID )
			{
				m_currentStringID = m_stringID;
				m_text.m_text = StringLookup.Get.GetString( m_currentStringID );
				reload = true;
			}
			
			if( reload )
			{
				m_text.Reload();
				m_text.SetColor( m_currentColor );
			}
		}
	}
}
