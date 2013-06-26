//-----------------------------------------------------------------------------
// File: PrimitiveLibraryUpdater.cs
//
// Desc:	Script that attaches to the PrimitiveLibrary top level heirarchy
//			object. Performs frame-by-frame updates on the PrimitiveLibrary.
//			The script is set to update after all others
//
// Copyright Echo Peak Ltd 2013
//-----------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

public class PrimitiveLibraryUpdater : MonoBehaviour
{
	void LateUpdate()
	{
		PrimitiveLibrary.Get.UpdateBatches();		
	}
}
