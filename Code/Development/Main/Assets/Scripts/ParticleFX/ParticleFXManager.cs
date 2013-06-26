//-----------------------------------------------------------------------------
// File: ParticleFXManager.cs
//
// Desc:	Creates and manages the pools of FX
//
// Copyright Echo Peak Ltd 2013
//-----------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

public class ParticleFXManager : MonoBehaviour 
{
	//	Static accessor
	private static ParticleFXManager ms_instance = null;
    public static ParticleFXManager Get()
    {
		//	Cache the object to avoid frequent string lookups
		if( ms_instance != null )
			return ms_instance;

		GameObject instanceObject = GameObject.Find( "ParticleFXManager" );
        if( instanceObject == null )
		{
            Console.WriteLine( "!** No particle FX Manager object found (ParticleFXManager)" );
			ms_instance = null;
		}
        else
            ms_instance = (ParticleFXManager)instanceObject.GetComponent<ParticleFXManager>();

        return ms_instance;
    }
	
	
	void Start() 
	{
	}
	
	void Update() 
	{
	}
}
