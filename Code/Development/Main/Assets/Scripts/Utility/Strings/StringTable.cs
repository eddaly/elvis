//-----------------------------------------------------------------------------
// File: StringTable.h
//
// Desc:	Raw data for string tables. Used by StringLookup 
//          Singleton class. We have one instance of each string table
//			for each platform and for each language as well a common table.
//
// Copyright Echo Peak Ltd 2013
// <Paul Sinnett>
//-----------------------------------------------------------------------------

using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class StringTable : ScriptableObject 
{
	public enum Language
	{
		kEnglish,
		kFrench,
		kItalian,
		kGerman,
		kSpanish
	}

	public enum Platform
	{
		kCommon,
		kPS3,
		kXbox360
	}


	[System.Serializable]
	class Entry 
	{
		public string key;
		public string translation;
	};
	
	[SerializeField] private List<Entry> m_list;
	
	public void Clear()
	{
		m_list = new List<Entry>();
		Index();
	}

	public void Index()
	{
		m_dictionary = new StringHashTable();
		Add( m_dictionary );
	}
	
	public void LoadString(string key, string translation)
	{
		key = key.ToUpper();

		if( m_dictionary == null )
		{
			Index();
		}
		
		Entry entry = new Entry();
		
		if( m_dictionary.ContainsKey( key ))
		{
			entry = m_list.Find(delegate( Entry match ) { return match.key == key; });
			entry.translation = translation;
		}
		else
		{
			entry.key = key;
			entry.translation = translation;
			m_list.Add( entry );
		}
	}
	
	private StringHashTable m_dictionary;
	
	public void Add( StringHashTable dictionary )
	{
		foreach( Entry entry in m_list )
		{
			if( !dictionary.ContainsKey( entry.key ))
			{
				dictionary.Add( entry.key, entry.translation );
			}
		}
	}
	
	public void SortStringIDs()
	{
		m_list.Sort( delegate ( Entry lhs, Entry rhs ) { return lhs.key.CompareTo(rhs.key); });
	}
}
