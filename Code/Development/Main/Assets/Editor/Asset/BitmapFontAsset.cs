//-----------------------------------------------------------------------------
// File: BitmapFontAsset.cs
//
// Desc:	create a string table asset object
//
// Copyright Echo Peak Ltd 2013
// <Paul Sinnett>
//-----------------------------------------------------------------------------

#if !UNITY_IPHONE

using UnityEditor;
using UnityEngine;

public class BitmapFontAsset
{
    [MenuItem("Assets/Create/BitmapFont")]
    public static void CreateAsset ()
    {
        CustomAssetUtility.CreateAsset<BitmapFont> ();
    }
}

#endif