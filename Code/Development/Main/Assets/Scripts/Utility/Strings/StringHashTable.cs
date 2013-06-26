//-----------------------------------------------------------------------------
// File: StringHashTable.h
//
// Desc:	A hash table for looking up translations by their string id
//
// Copyright Echo Peak Ltd 2013
// <Paul Sinnett>
//-----------------------------------------------------------------------------

using UnityEngine;
using System.Collections.Generic;

public class StringHashTable
{
	class Entry
	{
		public string key;
		public string translation;
	};

	private static int m_tableSize = 1024;
	
	private List<Entry> [] m_table = new List<Entry> [m_tableSize];

	public void LogTableStats()
	{
		int maxSize = 0;
		int totalSize = 0;

		for( int i = 0; i < m_tableSize; i++)
		{
			List<Entry> list = m_table[i];
			if( list != null )
			{
				if( list.Count > maxSize ) maxSize = list.Count;
				totalSize += list.Count;
			}
		}

		EchoDebugLog.Log( string.Format( "Hash table entries {0}, average collisions {1}, max collisions {2}",
				totalSize, (float)totalSize / m_tableSize, maxSize ));
	}

	static uint Hash( string text )
	{
		// Dan Bernstein's DJB2 hash algorithm...

		uint hash = 5381;
		int MAGIC_NUMBER = 33;

		foreach( char character in text )
			hash = (uint)(hash * MAGIC_NUMBER + character);

		return hash;
	}

	private bool FindKeyIndex( string key, out int hashIndex, out int listIndex )
	{
		listIndex = -1;
		hashIndex = (int)(Hash( key ) % m_tableSize);
		List<Entry> list = m_table[ hashIndex ];
		if( list != null )
		{
			listIndex = list.FindIndex( delegate( Entry entry )
			{
				return key.CompareTo( entry.key ) == 0;
			});
		}

		return listIndex >= 0;
	}

	public bool ContainsKey( string key )
	{
		int listIndex;
		int hashIndex;
		return FindKeyIndex( key, out hashIndex, out listIndex );
	}

	public bool TryGetValue( string key, out string text )
	{
		int listIndex;
		int hashIndex;
		text = null;

		if( FindKeyIndex( key, out hashIndex, out listIndex ))
		{
			List<Entry> list = m_table[ hashIndex ];
			if( list != null )
			{
				text = list[listIndex].translation;
			}
		}

		return text != null;
	}

	public void Add( string key, string translation )
	{
		int listIndex;
		int hashIndex;

		List<Entry> list = null;
		bool found = FindKeyIndex( key, out hashIndex, out listIndex );
		list = m_table[hashIndex];
		if( found ) list.RemoveAt( listIndex );

		if( list == null )
		{
			list = m_table[hashIndex] = new List<Entry>();
		}

		if( list != null )
		{
			Entry entry = new Entry();
			entry.key = key;
			entry.translation = translation;
			list.Add( entry );
		}
	}
}
