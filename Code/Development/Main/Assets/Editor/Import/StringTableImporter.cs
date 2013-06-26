//-----------------------------------------------------------------------------
// File: StringTableImporter.h
//
// Desc:	A collection of string tables with a custom inspector to allow
//          for importing string tables from CSV
//
// Copyright Echo Peak Ltd 2013
//-----------------------------------------------------------------------------

using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class StringTableImporter : ScriptableObject 
{
	[System.Serializable]
	public class StringTableEntry
	{
		public StringTable table;
		public StringTable.Language language;
		public StringTable.Platform platform;
	}
	
	public StringTableEntry [] m_tables = null;
	
	public void Clear()
	{
		foreach( StringTableEntry entry in m_tables )
		{
			entry.table.Clear();
		}
	}

	public void LoadString(string key, StringTable.Language language, StringTable.Platform platform, string translation)
	{
		foreach( StringTableEntry entry in m_tables )
		{
			if( entry.language == language && entry.platform == platform )
			{
				entry.table.LoadString( key, translation );
			}
		}
	}

	public void SortStringIDs()
	{
		foreach( StringTableEntry entry in m_tables )
		{
			entry.table.SortStringIDs();
		}
	}
	
	public void ApplyChanges()
	{
		EditorUtility.SetDirty( this );
		foreach( StringTableEntry entry in m_tables )
		{
			EditorUtility.SetDirty( entry.table );
		}		
	}
}
