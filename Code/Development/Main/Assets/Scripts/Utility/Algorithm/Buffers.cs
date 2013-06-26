//-----------------------------------------------------------------------------
// File: Buffer.cs
//
// Desc:	Helpers for converting data buffers
//
// Copyright Echo Peak Ltd 2013
//-----------------------------------------------------------------------------
using UnityEngine;
using System.Collections;


public delegate void VoidDelegate();
public delegate void BoolDelegate( bool complete );

public static class Buffer
{
	//-----------------------------------------------------------------------------
	// Method:	GetBytes()
	// Desc:	Takes a string and returns the equivalent array of bytes
	public static byte[] GetBytes(string str)
	{
		byte[] bytes = new byte[str.Length * sizeof(char)];
		System.Buffer.BlockCopy( str.ToCharArray(), 0, bytes, 0, bytes.Length );
		
		return bytes;
	}

	//-----------------------------------------------------------------------------
	// Method:	GetString()
	// Desc:	Takes an array of bytes and returns the equivalent string
	public static string GetString(byte[] bytes)
	{
		char[] chars = new char[bytes.Length / sizeof(char)];
		System.Buffer.BlockCopy( bytes, 0, chars, 0, bytes.Length );
		
		return new string( chars );
	}
}