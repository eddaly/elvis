//-----------------------------------------------------------------------------
// File: ElvisEditor.cs
//
// Desc:	Helper class for editing in-game scenes.
//
//			Lets you specify which layer to look at in the game view in editor
//			mode. It does this by moving the various game objects around the
//			scene. The main camera (must be called "MainCamera") is set so that 
//			
//
//			Also contains debug rendering to show how the various device
//			resolutions and aspect ratios will fit onto the screen.
//
// Copyright Echo Peak Ltd 2013
//-----------------------------------------------------------------------------
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[ExecuteInEditMode]
public class EnvironmentEditor : MonoBehaviour 
{	
	//	For showing environment pieces in edit mode:	
	public EntityDefs.EnvLayer m_CurrentLayer = EntityDefs.EnvLayer.SKYBOX;
	EntityDefs.EnvLayer m_oldCurrentLayer = EntityDefs.EnvLayer.SKYBOX;
	
	public int m_ElementNumber = 0;
	int m_oldElementNumber = 0;

	
	List<EnvironmentPiece>[] m_environmentLayers = new List<EnvironmentPiece>[(int)(EntityDefs.EnvLayer.NUM)];
	string[] m_parentObjectNames = new string[(int)(EntityDefs.EnvLayer.NUM)];
	string[] m_childObjectNames = new string[(int)(EntityDefs.EnvLayer.NUM)];
		
	public bool m_ShowGuides = true;
	
	void Start() 
	{	
		m_parentObjectNames[(int)(EntityDefs.EnvLayer.SKYBOX)] = "SkyBoxes";
		m_childObjectNames[(int)(EntityDefs.EnvLayer.SKYBOX)] = "SkyBox";
		m_parentObjectNames[(int)(EntityDefs.EnvLayer.BACKGROUND)] = "BackgroundPieces";
		m_childObjectNames[(int)(EntityDefs.EnvLayer.BACKGROUND)] = "BackgroundPiece";
		m_parentObjectNames[(int)(EntityDefs.EnvLayer.MIDGROUND)] = "MidgroundPieces";
		m_childObjectNames[(int)(EntityDefs.EnvLayer.MIDGROUND)] = "MidgroundPiece";
		m_parentObjectNames[(int)(EntityDefs.EnvLayer.FOREGROUND)] = "ForegroundPieces";
		m_childObjectNames[(int)(EntityDefs.EnvLayer.FOREGROUND)] = "ForegroundPiece";		
		
		PopulateReferences();
	}
	
	void Update() 
	{
		if( m_CurrentLayer != m_oldCurrentLayer )
		{
			if( (int)m_CurrentLayer < (int)(EntityDefs.EnvLayer.SKYBOX) )
				m_CurrentLayer = EntityDefs.EnvLayer.SKYBOX;
			if( (int)m_CurrentLayer > (int)(EntityDefs.EnvLayer.FOREGROUND) )
				m_CurrentLayer = EntityDefs.EnvLayer.FOREGROUND;
		}
		
		if( m_ElementNumber != m_oldElementNumber )
		{
			if( m_ElementNumber < 0 )
				m_ElementNumber = 0;
			
			int layerElements = m_environmentLayers[(int)m_CurrentLayer].Count; 
			
			if( m_ElementNumber > layerElements - 1 )
				m_ElementNumber = layerElements - 1;
		}
		
		m_oldCurrentLayer = m_CurrentLayer;
		m_oldElementNumber = m_ElementNumber;
				
		
		if( Application.isPlaying )
			updatePlayMode();
		else
			updateEditMode();
		
	
		if( m_ShowGuides )
			drawResolutionGuides();
	}
	
	void updateEditMode()
	{
		
	}
	
	void updatePlayMode()
	{
	}
		
	public void PopulateReferences()
	{		
		int childIndex = 0;
		Transform childTransform;
					
		if( m_environmentLayers[0] == null )
		{
			for( int l = 0; l<(int)(EntityDefs.EnvLayer.NUM); l++ )
				m_environmentLayers[l] = new List<EnvironmentPiece>();
		}
		
		//	Fill out the non-object layer reference lists
		for( int l = 0; l<(int)(EntityDefs.EnvLayer.NUM); l++ )
		{
			Transform layerParent = transform.FindChild( m_parentObjectNames[l] );
			m_environmentLayers[l].Clear();
			childIndex = 0;
			
			if( layerParent != null )
			{
				childTransform = layerParent.FindChild( m_childObjectNames[l] + childIndex.ToString() );
				while( childTransform != null )
				{
					EnvironmentPiece pieceComponent = childTransform.GetComponent<EnvironmentPiece>();
					
					if( pieceComponent != null )
						m_environmentLayers[l].Add( pieceComponent );
					else
						Debug.Log( "!** No EnvironmnetPiece on envrionment parent object: " + childTransform.name );
						
					childIndex++;
					childTransform = layerParent.FindChild( m_childObjectNames[l] + childIndex.ToString() );
				}
			}
		}
		
		logEnvironmentPieces();
	}
	
	void logEnvironmentPieces()
	{
		string piecesLog = "Environment Pieces:\n\n";
		piecesLog += "*********************\n";
		
		for( int l = 0; l<(int)(EntityDefs.EnvLayer.NUM); l++ )
		{
			piecesLog += "    " + m_parentObjectNames[l] + ":\n";

			if( m_environmentLayers[l].Count < 1 )
				piecesLog += "        empty\n";
			else
			{
				foreach( EnvironmentPiece piece in m_environmentLayers[l] )
				{
					piecesLog += "        " + piece.gameObject.name + "\n";
				}
			}
			
			piecesLog += "\n";
		}
				
		piecesLog += "*********************\n";
		Debug.Log( piecesLog );
	}
	
	void drawResolutionGuides()
	{
		float alpha = 0.5f;
		
		Vector3 activeAreaOffset = 
			new Vector3( EntityDefs.gActiveAreaXOffset, EntityDefs.gActiveAreaYOffset, 0.0f );
		
		float viewWidth = EntityDefs.gActiveAreaWidth;
		float viewHalfWidth = EntityDefs.gActiveAreaWidth*0.5f;
		
		//	The different aspect ratios are set up so that each one has the same visible horizontal distance.
		//	More of the viewport is cropped from the top than the bottom to try and keep the balance correct.
		//	The active area fits inside the widest aspect ratio leaving room for the HUD at the top.
		
		//	4:3
		float ratio = 3.0f/4.0f;
		DebugRenderHelpers.Draw2DBox( new Vector3( -viewHalfWidth, ratio*viewHalfWidth, 0.0f ) + activeAreaOffset, viewWidth, ratio*viewWidth, 
			new Color( 1.0f, 0.0f, 0.0f, alpha ) );

		//	3:2
		ratio = 2.0f/3.0f;
		DebugRenderHelpers.Draw2DBox( new Vector3( -viewHalfWidth, ratio*viewHalfWidth*0.95f, 0.0f ) + activeAreaOffset, viewWidth, ratio*viewWidth,
			new Color( 1.0f, 0.2f, 0.2f, alpha ) );

		//	16:9
		ratio = 9.0f/16.0f;
		DebugRenderHelpers.Draw2DBox( new Vector3( -viewHalfWidth, ratio*viewHalfWidth*0.9f, 0.0f ) + activeAreaOffset, viewWidth, ratio*viewWidth,
			new Color( 1.0f, 0.4f, 0.4f, alpha ) );
		
		//	Active area
		ratio = 1.0f/2.0f;
		DebugRenderHelpers.Draw2DBox( new Vector3( -viewHalfWidth, ratio*viewHalfWidth*0.8f, 0.0f ) + activeAreaOffset, viewWidth, ratio*viewWidth,
			new Color( 0.0f, 1.0f, 0.0f, alpha ) );
	}
	
}
