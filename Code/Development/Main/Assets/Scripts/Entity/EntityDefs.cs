//-----------------------------------------------------------------------------
// File: EntityDefs.cs
//
// Desc:	Contains enums, types, and constants needed for the entity scripts
//
// Copyright Echo Peak Ltd 2013
//-----------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

public class EntityDefs
{
	//	The active area offsets are used to position the logical game
	//	viewport such that (0, 0) is bottom left and (width, width/2) is top right.
	//	The active area is a section within the main rendering viewport - positioned
	//	to fit inside the widest view (16:9) with minimal clipping, while allowing
	//	space above for the HUD. See ElvisEditor.cs for how the frames are currently
	//	fitted.
	public const float gActiveAreaWidth = 20.0f;
	public const float gActiveAreaXOffset = gActiveAreaWidth*0.5f;
	public const float gActiveAreaYOffset = gActiveAreaWidth*0.275f;
	
	
	
	//	Environment layers form distinct sections of the environment separated on the
	//	z-axis. Each layer has it's own set of Environment pieces, defined in the 
	//	scene hierarchy.
	public enum EnvLayer
	{
		SKYBOX = 0,
		BACKGROUND,
		MIDGROUND,
		FOREGROUND,
		
		NUM
	}
}

