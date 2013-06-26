//-----------------------------------------------------------------------------
// File: XMLHelper.cs
//
// Desc:	Utilities for converting to and from xml format
//
// Copyright Echo Peak Ltd 2013
//-----------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

using System.Xml.Serialization;
using System.IO;


public class XMLHelper
{
	//-----------------------------------------------------------------------------
	// Method:	ToXML()
	public static string ToXML( object obj )
	{
		string xml = null;
		if( obj != null )
		{
			XmlSerializer serializer = new XmlSerializer( obj.GetType() );

			using( StringWriter writer = new StringWriter() )
			{
				serializer.Serialize( writer, obj );
				xml = writer.ToString();
			}
		}
		
		return xml;
	}

	//-----------------------------------------------------------------------------
	// Method:	ToXML()
	public static string ToXML<T>( object obj )
	{
		XmlSerializer serializer = new XmlSerializer( typeof(T) );
		string xml = null;

		using( StringWriter writer = new StringWriter() )
		{
			serializer.Serialize( writer, obj );
			xml = writer.ToString();
		}

		return xml;
	}

	//-----------------------------------------------------------------------------
	// Method:	FromXML()
	public static object FromXML( string data, System.Type type )
	{
		if( !System.String.IsNullOrEmpty( data ) )
		{
			XmlSerializer serializer = new XmlSerializer( type );

			using( StringReader reader = new StringReader( data ) )
			{
				return serializer.Deserialize( reader );
			}
		}

		return null;
	}

	//-----------------------------------------------------------------------------
	// Method:	FromXML()
	public static T FromXML<T>( string data ) where T: class
	{
		if( !System.String.IsNullOrEmpty( data ) )
		{
			XmlSerializer serializer = new XmlSerializer( typeof(T) );

			using( StringReader reader = new StringReader(data) )
			{
				T obj = (T)serializer.Deserialize( reader );
				return obj;
			}
		}

		return null;
	}
}
