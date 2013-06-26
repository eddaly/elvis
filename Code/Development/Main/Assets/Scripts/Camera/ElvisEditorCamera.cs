//-----------------------------------------------------------------------------
// File: ElvisEditorCamera.cs
//
// Desc:	Helper class for Elvis main camera.
//
//			Lets you specify which layer to look at in the game view in editor
//			mode. In play mode it just sits at 0,0
//
//			Also contains debug rendering to show how the various device
//			resolutions and aspect ratios will fit onto the screen.
//
// Copyright Echo Peak Ltd 2013
//-----------------------------------------------------------------------------
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class ElvisEditorCamera : MonoBehaviour 
{
	public enum EnvironmentLayer
	{
		SKYBOX,
		BACKGROUND,
		FOREGROUND,
		OBSTACLE
	}
	public EnvironmentLayer m_CurrentLayer = EnvironmentLayer.SKYBOX;

	Transform m_mainCamera;
	List<Transform> m_skyBoxes = new List<Transform>();
	List<Transform> m_backgroundPieces = new List<Transform>();
	List<Transform> m_foregroundPieces = new List<Transform>();
	
	
	void Start() 
	{
		if( !Application.isPlaying )
			EditorApplication.hierarchyWindowChanged += sceneHierarchyChanged;
	}
	
	void Update() 
	{
		if( Application.isPlaying )
			updatePlayMode();
		else
			updateEditMode();
	}
	
	void updateEditMode()
	{
		
	}
	
	void updatePlayMode()
	{
	}
	
	void sceneHierarchyChanged()
	{
		gatherBackgroundPieces();
	}
	
	void gatherBackgroundPieces()
	{
		m_mainCamera = GameObject.Find( "MainCamera" ).transform;
		
		int childIndex = 1;
		Transform childTransform;
					
		//	Fill out skybox list
		Transform skyBoxes = transform.FindChild( "SkyBoxes" );
		m_skyBoxes.Clear();
		
		if( skyBoxes != null )
		{
			childTransform = skyBoxes.FindChild( "SkyBox" + childIndex.ToString() );
			while( childTransform != null )
			{
				m_skyBoxes.Add( childTransform );
				childIndex++;
			}
		}
			
		//	Fill out background list
		Transform backgroundPieces = transform.FindChild( "BackgroundPieces" );
		m_backgroundPieces.Clear();
		
		if( backgroundPieces != null )
		{
			childIndex = 1;
			childTransform = backgroundPieces.FindChild( "BackgroundPiece" + childIndex.ToString() );
			while( childTransform != null )
			{
				m_backgroundPieces.Add( childTransform );
				childIndex++;
			}
		}
		
		//	Fill out foreground list
		Transform foregroundPieces = transform.FindChild( "ForegroundPieces" );
		m_foregroundPieces.Clear();
		
		if( foregroundPieces != null )
		{
			childIndex = 1;
			childTransform = backgroundPieces.FindChild( "ForegroundPiece" + childIndex.ToString() );
			while( childTransform != null )
			{
				m_foregroundPieces.Add( childTransform );
				childIndex++;
			}
		}		
		
		logEnvironmentPieces();
	}
	
	void logEnvironmentPieces()
	{
		string piecesLog = "*********************\n";
		piecesLog += "Environment Pieces:\n";
		
		foreach( Transform skybox in m_skyBoxes )
		{
			piecesLog += "    " + skybox.gameObject.name + "\n";
		}
		
		piecesLog += "*********************\n";
		Debug.Log( piecesLog );
	}
}
