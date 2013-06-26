//-----------------------------------------------------------------------------
// File: StringLookup.h
//
// Desc:	Singleton object for loading and looking up strings from the
//          string table resources
//
// Copyright Echo Peak Ltd 2013
// <Paul Sinnett>
//-----------------------------------------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StringLookup
{
	static public StringLookup m_instance = new StringLookup();

	public static StringLookup Get
	{
		get
		{
			 return m_instance;
		}
	}
	
	private StringHashTable m_dictionary = new StringHashTable();

	string LanguageName(StringTable.Language language)
	{
		switch( language )
		{
			case StringTable.Language.kEnglish: return "English";
			case StringTable.Language.kFrench: return "French";
			case StringTable.Language.kItalian: return "Italian";
			case StringTable.Language.kGerman: return "German";
			case StringTable.Language.kSpanish: return "Spanish";
		}
		return "LanguageNotFound";
	}
	
	void LoadPlatformTable( string language, string platform )
	{
		LoadTable( language + platform );
	}
	
	void LoadTable( string language )
	{
		StringTable table = Resources.Load( string.Format( "Strings/{0}", language )) as StringTable;
		table.Add( m_dictionary );
		m_dictionary.LogTableStats();
	}
	
	public void Poke() {}
		
	StringLookup()
	{
		RuntimePlatform platform = Application.platform;
		SystemLanguage systemLanguage = Application.systemLanguage;

		StringTable.Language lang = StringTable.Language.kEnglish;
		switch (systemLanguage)
		{
			case SystemLanguage.French:
			lang = StringTable.Language.kFrench;
			break;

			case SystemLanguage.German:
			lang = StringTable.Language.kGerman;
			break;

			case SystemLanguage.Italian:
			lang = StringTable.Language.kItalian;
			break;

			case SystemLanguage.Spanish:
			lang = StringTable.Language.kSpanish;
			break;
		}
		
		lang = StringTable.Language.kEnglish;
		
		string language = LanguageName(lang);
		switch( platform )
		{
		case RuntimePlatform.XBOX360:
			LoadPlatformTable( language, "Xbox" );
			break;
			
		case RuntimePlatform.PS3:
			LoadPlatformTable( language, "PS3" );
			break;
		}
		
		LoadTable( language );
	}
	
	public void Log()
	{
		m_dictionary.LogTableStats();
	}
	
	public string GetString( string key )
	{
		string translation;
		if( key == null || key == "" )
		{
			translation = "NO STRING ID";
			Console.WriteLine( "Missing string ID" );
		}
		else
		{
			key = key.ToUpper();
			if( !m_dictionary.TryGetValue( key, out translation ))
			{
				translation = "NO TRANSLATION";
				Console.WriteLine(Channels.General | Channels.Warning, "No translation loaded for string ID '{0}'", key );
			}
		}
		
		return translation;
	}

	public bool HasString( string key )
	{
		return m_dictionary.ContainsKey( key.ToUpper() );
	}
}
