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
#if !UNITY_IPHONE && !UNITY_ANDROID
using UnityEditor;
#endif
using System.Collections;

[ExecuteInEditMode]
public class RL : MonoBehaviour 
{
	public static Camera m_MainCamera;
	public static EnvironmentManager m_Environment;
	public static ObstacleManager m_Obstacles;
	public static SequenceManager m_Sequencer;
	public static PrototypeConfiguration m_Prototype;
	public static Player m_Player;
	
	public bool m_ForcePopulate = false;
	
	public bool m_ForceRefresh = false;
	
	void OnEnable()
	{
		populateReferences();		
		refreshReferenceObjects();
	}

	void OnDisable() 
	{
	}
	 
	void Start() 
	{
#if !UNITY_IPHONE && !UNITY_ANDROID
		if( !Application.isPlaying )
			EditorApplication.hierarchyWindowChanged += sceneHierarchyChanged;
#endif
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
		populateReferences();

		if( m_Environment != null )
			m_Environment.PopulatePieceArray();
	}
	
	void populateReferences()		
	{
		GameObject foundObject;
		
		foundObject = GameObject.Find( "MainCamera" );
		if( foundObject != null )
			m_MainCamera = foundObject.GetComponent<Camera>();
		
		foundObject = GameObject.Find( "Environment" );
		if( foundObject != null )
			m_Environment = foundObject.GetComponent<EnvironmentManager>();
		
		foundObject = GameObject.Find( "Obstacles" );
		if( foundObject != null )
			m_Obstacles = foundObject.GetComponent<ObstacleManager>();
		
		foundObject = GameObject.Find( "Sequencer" );
		if( foundObject != null )
			m_Sequencer = foundObject.GetComponent<SequenceManager>();
		
		foundObject = GameObject.Find( "Prototype" );
		if( foundObject != null )
			m_Prototype = foundObject.GetComponent<PrototypeConfiguration>();		
		
		foundObject = GameObject.Find( "Player" );
		if( foundObject != null )
			m_Player = foundObject.GetComponent<Player>();		
	}
	
	
	//-----------------------------------------------------------------------------
	// Method:	refreshReferenceObjects()
	// Desc:	In editor mode, some of the referenced objects need to be told to
	//			refresh themselves to pick up script or scene changes that can't
	//			be detected with an editor callback
	void refreshReferenceObjects()
	{
		if( m_Environment != null )
			m_Environment.PopulatePieceArray();
		
		if( m_Obstacles != null )
			m_Obstacles.RefreshPieces();
	}
}
