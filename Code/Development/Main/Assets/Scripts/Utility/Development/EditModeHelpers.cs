//-----------------------------------------------------------------------------
// File: EditModeHelpers.cs
//
// Desc:	Unity doesn't update the transform.position attribute correctly
//			when you're in edit mode. So if a parent object alter's it's
//			local position, the extra translation doesn't appear in a child's
//			world position.
//
// Copyright Echo Peak Ltd 2013
//-----------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

public class EditModeHelpers
{
	public static Vector3 WorldPosition( Transform this_transform )
	{
		Vector3 totalPosition = this_transform.localPosition;

		//	Recurse up to the top of the hierarchy
		if( this_transform.parent != null )
			totalPosition += WorldPosition( this_transform.parent );
		
		return totalPosition;
	}
}
