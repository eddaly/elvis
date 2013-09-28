//-----------------------------------------------------------------------------
// File: InspectorStringTable.cs
//
// Desc:	import / re-import string table from CSV
//
// Copyright Echo Peak Ltd 2013
// <Paul Sinnett>
//-----------------------------------------------------------------------------
#if !UNITY_IPHONE

using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

[CustomEditor(typeof(StringTableImporter))]
public class InspectorStringTable : Editor
{
	class CSVParser
	{
		// a regex to split comma separated and optionally quoted strings
		// (not including commas between quotes)
		private Regex m_parseString = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

		public string [] Parse( string line )
		{
			string [] fields = m_parseString.Split( line );

			for( int i = 0; i < fields.Length; i++ )
			{
				string field = fields[i];
				field = PreProcessText( field );

				// remove bookending quotes
				if( field.StartsWith( "\"" ) && field.EndsWith( "\"" ) )
					field = field.Substring(1, field.Length - 2);

				field = field.Replace( "\"\"", "\"" ); // replace quoted quotes with ordinary quotes

				fields[i] = field;
			}

			return fields;
		}
	}

	enum FieldNames
	{
		kStringID,
		kPlatform,
		kEnglish,
		kFrench,
		kItalian,
		kGerman,
		kSpanish
	}

	public static string PreProcessText( string text )
	{
		char [] trimSpaces = { ' ' }; // trim leading and trailing spaces
		text = text.Replace( "\\n", "\n" ); // handle embedded carriage return
		text = text.Trim( trimSpaces );
		return text;
	}

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		
		StringTableImporter importer = serializedObject.targetObject as StringTableImporter;

		if( GUILayout.Button( "Clean" ))
		{
			importer.Clear();
			importer.ApplyChanges();
		}

		if( GUILayout.Button( "Test" ))
		{
			Debug.Log( "testing a quoted strings" );
			Debug.Log( new CSVParser().Parse( "\"X\"" )[0] );
			Debug.Log( new CSVParser().Parse( "\"X\",\"Y\"" )[1] );

			Debug.Log( "testing quoted quoted strings" );
			Debug.Log( new CSVParser().Parse( "\"\"\"X\"\"\"" )[0] );
			Debug.Log( new CSVParser().Parse( "\"\"\"X\"\"\",\"\"\"Y\"\"\"" )[1] );

			Debug.Log( "testing multiple quoted quoted strings" );
			Debug.Log( new CSVParser().Parse( "\"\"\"X\"\", \"\"Y\"\", \"\"Z\"\"\"" )[0] );
		}

		if( GUILayout.Button( "Import" ))
		{
			string filename = EditorUtility.OpenFilePanel( "Import String Table CSV", "", "csv" );
			if (filename != null)
			{
				using( StreamReader reader = new StreamReader( filename, Encoding.GetEncoding(1252)))
				{
					CSVParser parser = new CSVParser();

					string line = reader.ReadLine(); // first line contains headings
					string [] fields = parser.Parse( line );

					string [] fieldNames = 
					{
						"StringID",
						"Platform",
						"English",
						"French",
						"Italian",
						"German",
						"Spanish"
					};
					
					int [] lookup = new int[fieldNames.Length];
					
					for( int i = 0; i < lookup.Length; i++ )
					{
						lookup[i] = -1;
					}

					int highest = -1;

					for( int i = 0; i < lookup.Length; i++ )
					{
						for( int k = 0; k < fields.Length; k++ )
						{
							if( fields[k].Contains( fieldNames[i] ))
							{
								lookup[i] = k;
								if( k > highest ) highest = k;
								break;
							}
						}
					}
					
					while( !reader.EndOfStream )
					{
						line = reader.ReadLine();
        				fields = parser.Parse( line );
						if( fields.Length <= lookup[(int)FieldNames.kSpanish ] )
						{
	        				Debug.Log( string.Format( "Error: {0}", line ));
							continue;
						}

						string stringID = "";
                        string platform = "";
						string english = "";
						string french = "";
						string italian = "";
						string german = "";
						string spanish = "";

						if( lookup[(int)FieldNames.kStringID] != -1 )
							stringID = fields[lookup[(int)FieldNames.kStringID]];

						if( lookup[(int)FieldNames.kPlatform] != -1 )
							platform = fields[lookup[(int)FieldNames.kPlatform]];

						if( lookup[(int)FieldNames.kEnglish] != -1 )
							english  = fields[lookup[(int)FieldNames.kEnglish ]];

						if( lookup[(int)FieldNames.kFrench] != -1 )
							french   = fields[lookup[(int)FieldNames.kFrench  ]];

						if( lookup[(int)FieldNames.kItalian] != -1 )
							italian  = fields[lookup[(int)FieldNames.kItalian ]];

						if( lookup[(int)FieldNames.kGerman] != -1 )
							german   = fields[lookup[(int)FieldNames.kGerman  ]];

						if( lookup[(int)FieldNames.kSpanish] != -1 )
							spanish  = fields[lookup[(int)FieldNames.kSpanish ]];

						StringTable.Platform platformID = StringTable.Platform.kCommon;
						
						if( platform.ToUpper() == "XBOX" )
						{
							platformID = StringTable.Platform.kXbox360;
						}
						else if( platform.ToUpper() == "PS3" )
						{
							platformID = StringTable.Platform.kPS3;
						}
						
						if( stringID != string.Empty )
						{
							if( english != string.Empty )
							{
								importer.LoadString(stringID, StringTable.Language.kEnglish, platformID, english);
							}

							if( french != string.Empty )
							{
								importer.LoadString(stringID, StringTable.Language.kFrench, platformID, french);
							}
							if( italian != string.Empty )
							{
								importer.LoadString(stringID, StringTable.Language.kItalian, platformID, italian);
							}

							if( german != string.Empty )
							{
								importer.LoadString(stringID, StringTable.Language.kGerman, platformID, german);
							}

							if( spanish != string.Empty )
							{
								importer.LoadString(stringID, StringTable.Language.kSpanish, platformID, spanish);
							}
						}
					}
					
					importer.SortStringIDs();
					importer.ApplyChanges();
				}
			}	
		}
	}
}

#endif
