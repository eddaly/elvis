//-----------------------------------------------------------------------------
// File: ReferenceLibrary.cs
//
// Desc:	A central place to look for major game objects.
//
//			Also orchestrates these main objects in getting their references
//			when the scene changes.
//
// Copyright Echo Peak Ltd 2013
//-----------------------------------------------------------------------------
using UnityEngine;
using UnityEditor;
using System.Collections;

[ExecuteInEditMode]
public class ReferenceLibrary : MonoBehaviour 
{
	public static Camera m_MainCamera;
	public static EnvironmentEditor m_EnvironmentEditor;
	public static ObstacleManager m_ObstacleManager;
	public static SequenceManager m_SequenceManager;
	
	public bool m_ForcePopulate = false;
	
	public bool m_ForceRefresh = false;
	
	void OnEnable()
	{
		populateReferences();		
		refreshReferenceObjects();
				
		Debug.Log (" enabled library");
	}

	void OnDisable() 
	{
		Debug.Log (" disabled library");
	}
	
	void Start() 
	{
		if( !Application.isPlaying )
			EditorApplication.hierarchyWindowChanged += sceneHierarchyChanged;
	}
	
	void Update()
	{	
		if( m_ForcePopulate )
		{
			sceneHierarchyChanged();
			m_ForcePopulate = false;
		}
		
		if( m_ForceRefresh )
		{
			refreshReferenceObjects();
			m_ForceRefresh = false;
		}
	}
	
	void sceneHierarchyChanged()
	{
		Debug.Log( "Hierarchy change" );
		
		populateReferences();

		if( m_EnvironmentEditor != null )
			m_EnvironmentEditor.PopulateReferences();
	}
	
	void populateReferences()		
	{
		GameObject foundObject;
		
		foundObject = GameObject.Find( "MainCamera" );
		if( foundObject != null )
			m_MainCamera = foundObject.GetComponent<Camera>();
		
		foundObject = GameObject.Find( "Environment" );
		if( foundObject != null )
			m_EnvironmentEditor = foundObject.GetComponent<EnvironmentEditor>();
		
		foundObject = GameObject.Find( "Obstacles" );
		if( foundObject != null )
			m_ObstacleManager = foundObject.GetComponent<ObstacleManager>();
		
		foundObject = GameObject.Find( "Sequencer" );
		if( foundObject != null )
			m_SequenceManager = foundObject.GetComponent<SequenceManager>();
	}
	
	
	//-----------------------------------------------------------------------------
	// Method:	refreshReferenceObjects()
	// Desc:	In editor mode, some of the referenced objects need to be told to
	//			refresh themselves to pick up script or scene changes that can't
	//			be detected with an editor callback
	void refreshReferenceObjects()
	{
		if( m_ObstacleManager != null )
			m_ObstacleManager.RefreshPieces();
	}
}
