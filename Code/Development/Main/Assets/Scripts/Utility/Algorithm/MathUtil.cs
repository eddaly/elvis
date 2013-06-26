//-----------------------------------------------------------------------------
// File: MathUtil.cs
//
// Desc:	Maths utilities
//
// Copyright Echo Peak Ltd 2013
//-----------------------------------------------------------------------------
using UnityEngine;
using System.Collections;


public class MathUtil
{
	//-----------------------------------------------------------------------------
	// Method:	Range()
	// Desc:	Returns a float ranged within the specified min and max values
	public static float Range( float val, float min, float max )
	{
		float diff = max - min;
		while( val < min )
			val += diff;
		while( val > max )
			val -= diff;
		
		return val;
	}
	
	//-----------------------------------------------------------------------------
	// Method:	Range()
	// Desc:	Returns a int ranged within the specified min and max values
	public static int Range( int val, int min, int max )
	{
		int diff = max - min;
		while( val < min )
			val += diff;
		while( val > max )
			val -= diff;
		
		return val;
	}
		
	//-----------------------------------------------------------------------------
	// Method:	CountBits()
	public static int CountBits( long n ) 
	{     
  		int c;
		
  		for( c = 0; n != 0; c++ )
    		n &= n - 1;

		return c;
	}
}