//-----------------------------------------------------------------------------
// File: PrimitiveVolume.cs
//
// Desc:	Creates a particle with volume from 3 orthogonal planes
//
// Copyright Echo Peak Ltd 2013
//-----------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

public class PrimitiveVolume : MonoBehaviour
{
	static int m_uniqueVolumeID = 0;
	
	
	public static GameObject MakeVolume()
	{
		GameObject gameObject = new GameObject();
		gameObject.name = "Primitive" + m_uniqueVolumeID.ToString() + " [VOLUME]";
		m_uniqueVolumeID++;
		
		MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
		MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
		
		Mesh mesh = meshFilter.mesh; 
		mesh.Clear();
		
		//	3 orthogonal planes
		int numVerts = 12;
		int numIndices = 18;
		
		Vector3[] myVertices = new Vector3[numVerts];
		Vector2[] myUVs = new Vector2[numVerts];
		Color[] myColours = new Color[numVerts];
		int[] myTris = new int[numIndices];
		
		for( int v = 0; v<numVerts; v++ )
			myColours[v] = new Color( 1.0f, 1.0f, 1.0f, 1.0f );			

		
		for( int p = 0; p<3; p++ )
		{
			int vertBase = p*4;
			myUVs[vertBase + 0] = new Vector2( 0.0f, 0.5f );
			myUVs[vertBase + 1] = new Vector2( 0.0f, 1.0f );
			myUVs[vertBase + 2] = new Vector2( 0.5f, 1.0f );
			myUVs[vertBase + 3] = new Vector2( 0.5f, 0.5f );
		}

		myVertices[0] = new Vector3( -0.5f, -0.5f, 0.0f );
		myVertices[1] = new Vector3( -0.5f, 0.5f, 0.0f );
		myVertices[2] = new Vector3( 0.5f, 0.5f, 0.0f );
		myVertices[3] = new Vector3( 0.5f, -0.5f, 0.0f );

		myVertices[4] = new Vector3( -0.5f, 0.0f, -0.5f );
		myVertices[5] = new Vector3( -0.5f, 0.0f, 0.5f );
		myVertices[6] = new Vector3( 0.5f, 0.0f, 0.5f );
		myVertices[7] = new Vector3( 0.5f, 0.0f, -0.5f );

		myVertices[8] = new Vector3( 0.0f, -0.5f, -0.5f );
		myVertices[9] = new Vector3( 0.0f, -0.5f, 0.5f );
		myVertices[10] = new Vector3( 0.0f, 0.5f, 0.5f );
		myVertices[11] = new Vector3( 0.0f, 0.5f, -0.5f );

				
		myTris[0] = 0;
		myTris[1] = 1;
		myTris[2] = 3;
		myTris[3] = 1;
		myTris[4] = 2;
		myTris[5] = 3;
		
		myTris[6] = 4;
		myTris[7] = 5;
		myTris[8] = 7;
		myTris[9] = 5;
		myTris[10] = 6;
		myTris[11] = 7;
		
		myTris[12] = 8;
		myTris[13] = 9;
		myTris[14] = 11;
		myTris[15] = 9;
		myTris[16] = 10;
		myTris[17] = 11;
		

		//	Apply the verts to make the plane
		mesh.vertices = myVertices;
		mesh.uv = myUVs;
		mesh.colors = myColours;
		mesh.triangles = myTris;
		
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
 
		//	Create a unique material with the alpha shader
		Material newMaterial = new Material( Shader.Find( "EchoShader/AlphaBlended" ) );
		meshRenderer.sharedMaterial = newMaterial;
		meshRenderer.sharedMaterial.color = new Color( 0.0f, 0.0f, 0.0f, 0.0f );
		
		return gameObject;
	}
	
	public static void SetAtlasUVs( GameObject volume_object, int atlas_idx )
	{
		MeshFilter meshFilter = volume_object.GetComponent<MeshFilter>();
		Mesh mesh = meshFilter.mesh; 
		
		//	3 orthogonal planes
		int numVerts = 12;
		
		Vector2[] myUVs = new Vector2[numVerts];
		
		if( atlas_idx == 0 )
		{
			for( int p = 0; p<3; p++ )
			{
				int vertBase = p*4;
				myUVs[vertBase + 0] = new Vector2( 0.0f, 0.0f );
				myUVs[vertBase + 1] = new Vector2( 0.0f, 0.5f );
				myUVs[vertBase + 2] = new Vector2( 0.5f, 0.5f );
				myUVs[vertBase + 3] = new Vector2( 0.5f, 0.0f );
			}
		}
		else if( atlas_idx == 1 )
		{
			for( int p = 0; p<3; p++ )
			{
				int vertBase = p*4;
				myUVs[vertBase + 0] = new Vector2( 0.5f, 0.0f );
				myUVs[vertBase + 1] = new Vector2( 0.5f, 0.5f );
				myUVs[vertBase + 2] = new Vector2( 1.0f, 0.5f );
				myUVs[vertBase + 3] = new Vector2( 1.0f, 0.0f );
			}
		}
		else if( atlas_idx == 2 )
		{
			for( int p = 0; p<3; p++ )
			{
				int vertBase = p*4;
				myUVs[vertBase + 0] = new Vector2( 0.0f, 0.5f );
				myUVs[vertBase + 1] = new Vector2( 0.0f, 1.0f );
				myUVs[vertBase + 2] = new Vector2( 0.5f, 1.0f );
				myUVs[vertBase + 3] = new Vector2( 0.5f, 0.5f );
			}
		}
		else
		{
			for( int p = 0; p<3; p++ )
			{
				int vertBase = p*4;
				myUVs[vertBase + 0] = new Vector2( 0.5f, 0.5f );
				myUVs[vertBase + 1] = new Vector2( 0.5f, 1.0f );
				myUVs[vertBase + 2] = new Vector2( 1.0f, 1.0f );
				myUVs[vertBase + 3] = new Vector2( 1.0f, 0.5f );
			}
		}
		

		//	Apply the verts to make the plane
		mesh.uv = myUVs;
		mesh.RecalculateBounds();
	}
}