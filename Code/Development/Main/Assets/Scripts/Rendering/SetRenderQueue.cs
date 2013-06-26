//-----------------------------------------------------------------------------
// File: SetRenderQueue.cs
//
// Desc:	Simple script for forcing the render queue of all materials found
//			on the owning object or any of it's children
//
// Note:	The default render queues are as follows:
//
//			Background 	1000
//			Geometry	2000
//			AlphaTest	2450
//			Transparent	3000
//			Overlay		4000
//
// Copyright Echo Peak Ltd 2013
//-----------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class SetRenderQueue : MonoBehaviour 
{
	public int m_RenderQueue = 2000;
	int m_OldRenderQueue;
	
	void Start () 
	{	
		m_OldRenderQueue = m_RenderQueue;		
		setQueues();
	}
	
	void Update () 
	{	
		if( m_OldRenderQueue != m_RenderQueue )
		{
			if( m_RenderQueue < 0 )
				m_RenderQueue = 0;
			if( m_RenderQueue > 4000 )
				m_RenderQueue = 4000;

			m_OldRenderQueue = m_RenderQueue;		
			setQueues();
		}
	}

	void setQueues()
	{
		MeshRenderer renderer = GetComponent<MeshRenderer>();
		
		if( renderer )
		{
			int numMaterials = renderer.sharedMaterials.Length;
			Material[] materials = renderer.sharedMaterials;

			for( int m = 0; m<numMaterials; m++ )
			{
				materials[m].renderQueue = m_RenderQueue;
			}

			renderer.sharedMaterials = materials;
		}
		
		for( int i = 0; i < transform.GetChildCount(); i++ )
		{
			renderer = transform.GetChild(i).gameObject.GetComponent<MeshRenderer>();
			
			if( renderer )
			{
				int numMaterials = renderer.sharedMaterials.Length;
				Material[] materials = renderer.sharedMaterials;

				for( int m = 0; m<numMaterials; m++ )
				{
					materials[m].renderQueue = m_RenderQueue;
				}

				renderer.sharedMaterials = materials;
			}
		}
	}
}
