//-----------------------------------------------------------------------------
// File: InspectorBitmapFont.cs
//
// Desc:	import / re-import bitmap font from FNT
//
// Copyright Echo Peak Ltd 2013
// <Paul Sinnett>
//-----------------------------------------------------------------------------
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

[CustomEditor(typeof(BitmapFont))]
public class InspectorBitmapFont : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		
		BitmapFont font = serializedObject.targetObject as BitmapFont;
		
		if( GUILayout.Button( "Import" ))
		{
			string filename = EditorUtility.OpenFilePanel( "Import BMFont Xml file", "", "fnt" );
			if (filename != null)
			{
				font.Load ( filename );
				EditorUtility.SetDirty(font);
			}	
		}
	}
}
