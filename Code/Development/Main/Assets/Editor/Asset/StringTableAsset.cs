//-----------------------------------------------------------------------------
// File: StringTableAsset.cs
//
// Desc:	create a string table asset object
//
// Copyright Echo Peak Ltd 2013
// <Paul Sinnett>
//-----------------------------------------------------------------------------

using UnityEditor;
using UnityEngine;

public class StringTableAsset
{
    [MenuItem("Assets/Create/StringTable")]
    public static void CreateTable ()
    {
        CustomAssetUtility.CreateAsset<StringTable> ();
    }

	[MenuItem("Assets/Create/StringTableImporter")]
    public static void CreateImporter ()
    {
        CustomAssetUtility.CreateAsset<StringTableImporter> ();
    }
}
