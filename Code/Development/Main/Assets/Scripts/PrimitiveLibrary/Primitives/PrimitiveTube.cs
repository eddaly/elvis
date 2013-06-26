//-----------------------------------------------------------------------------
// File: PrimitiveTube.h
//
// Desc:	An extruded ring creator and modifier.
//
//			Static functions are used to create a Mesh, attached to a
//			GameObject as a MeshFilter. Further static functions are used to
//			Modify the Mesh in place.
//
// Note: 	The GameObject must be managed by the calling object
//
// Copyright Echo Peak Ltd 2013
//-----------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

//-----------------------------------------------------------------------------
// Class:	PrimitiveTubeInfo
// Desc:	Data class attached to each GameObject returned by
//			PrimitiveTube.MakeTube()
//			Provides permanent state information needed by the modification
//			functions.
public class PrimitiveTubeInfo : MonoBehaviour
{
	public int m_Sections;
	public int m_Stacks;
	
	public float m_StartRadius;
	public float m_EndRadius;
	
	public Color m_StartColour;
	public Color m_EndColour;
	public bool m_FadeEnds;

	public float m_Length;
	
	public float m_TexUTile;
	public float m_TexVTile;
	
	public float m_TexUOffset;
	public float m_TexVOffset;
}

public class PrimitiveTube
{
	static int m_uniqueTubeID = 0;
	
	public static GameObject MakeTube( 
		int sections, int stacks, float start_radius, float end_radius, float length )
	{
		return MakeTube ( sections, stacks, start_radius, end_radius, length, 0 );
	}
	public static GameObject MakeTube( 
		int sections, int stacks, float start_radius, float end_radius, float length, int blend )
	{
		GameObject gameObject = new GameObject();	
		gameObject.name = "Primitive" + m_uniqueTubeID.ToString() + " [TUBE]";
		m_uniqueTubeID++;		
		
		PrimitiveTubeInfo info = gameObject.AddComponent<PrimitiveTubeInfo>();
		info.m_Sections = sections;
		info.m_Stacks = stacks;
		info.m_StartRadius = start_radius;
		info.m_EndRadius = end_radius;
		info.m_StartColour = new Color( 1.0f, 1.0f, 1.0f, 1.0f );
		info.m_EndColour = new Color( 1.0f, 1.0f, 1.0f, 1.0f );
		info.m_FadeEnds = false;
		info.m_Length = length;
		info.m_TexUTile = 1.0f;
		info.m_TexVTile = 1.0f;
		info.m_TexUOffset = 0.0f;
		info.m_TexVOffset = 0.0f;
		
		MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
		MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();

		if( meshFilter.sharedMesh == null )
		{
			meshFilter.sharedMesh = new Mesh();
		}
		Mesh mesh = meshFilter.sharedMesh;
 
		mesh.Clear();
		
		int numVerts = (sections + 1)*(stacks + 1);
		int numIndices = sections*stacks*6;
				
		Vector3[] myVertices = new Vector3[numVerts];
		Vector2[] myUVs = new Vector2[numVerts];
		Color[] myColours = new Color[numVerts];
		int[] myTris = new int[numIndices];

		float angle = 0;
		float dAngle = (Mathf.PI*2.0f)/(float)sections;

		float zPos = 0.0f;
		float dZPos = length/(float)stacks;
		
		float radius = start_radius;
		float dRadius = (end_radius - start_radius)/(float)stacks;
		
		float texU = 0.0f;
		float texV = 0.0f;
		float dTexU = 1.0f/(float)sections;
		float dTexV = 1.0f/(float)stacks;
		
		int vertIdx = 0;
		int indexIdx = 0;
		
		int numLayers = stacks + 1;
		int numSpokes = sections + 1;
		for( int layer = 0; layer<numLayers; layer++ )
		{
			for( int spoke = 0; spoke<numSpokes; spoke++ )
			{
				myVertices[vertIdx] = new Vector3(
					radius*Mathf.Cos(angle),
					radius*Mathf.Sin(angle),
					zPos );
				
				myUVs[vertIdx] = new Vector2( texU, texV );

				myColours[vertIdx] = new Color( 1.0f, 1.0f, 1.0f, 1.0f );

				if( spoke < sections && layer < stacks )
				{
					//	Now set the triangles for this section
					myTris[indexIdx++] = vertIdx;
					myTris[indexIdx++] = vertIdx + numSpokes;
					myTris[indexIdx++] = vertIdx + 1;
					
					myTris[indexIdx++] = vertIdx + 1;
					myTris[indexIdx++] = vertIdx + numSpokes;
					myTris[indexIdx++] = vertIdx + numSpokes + 1;
				}
				
				vertIdx++;
				angle += dAngle;
				texU += dTexU;
			}
			
			angle = 0.0f;
			radius += dRadius;
			zPos += dZPos;
			
			texU = 0.0f;
			texV += dTexV;
		}
		
		//	Apply the verts to make the ring
		mesh.vertices = myVertices;
		mesh.uv = myUVs;
		mesh.colors = myColours;
		mesh.triangles = myTris;
		
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
 
		//	Create a unique material with the alpha shader
		Material newMaterial;
		if( blend == 0 )
			newMaterial = new Material( Shader.Find( "EchoShader/AlphaBlended" ) );
		else
			newMaterial = new Material( Shader.Find( "EchoShader/AlphaAdditive" ) );
		
		meshRenderer.sharedMaterial = newMaterial;
		meshRenderer.sharedMaterial.color = new Color( 1.0f, 1.0f, 1.0f, 1.0f );
		
		return gameObject;
	} 

	public static void ModifyVerts( GameObject tube_object, float start_radius, float end_radius, float length )
	{
		PrimitiveTubeInfo info = tube_object.GetComponent<PrimitiveTubeInfo>();
		info.m_StartRadius = start_radius;
		info.m_EndRadius = end_radius;
		info.m_Length = length;

		int sections = info.m_Sections;
		int stacks = info.m_Stacks;

		int numVerts = (sections + 1)*(stacks + 1);
		Vector3[] myVertices = new Vector3[numVerts];
		Color[] myColours = new Color[numVerts];

		float angle = 0;
		float dAngle = (Mathf.PI*2.0f)/(float)sections;

		float zPos = 0.0f;
		float dZPos = length/(float)stacks;
		
		float red = info.m_StartColour.r;
		float dRed = (info.m_EndColour.r - red)/(float)stacks;
		float green = info.m_StartColour.g;
		float dGreen = (info.m_EndColour.g - green)/(float)stacks;
		float blue = info.m_StartColour.b;
		float dBlue = (info.m_EndColour.b - blue)/(float)stacks;
		float alpha = info.m_StartColour.a;
		float dAlpha = (info.m_EndColour.a - alpha)/(float)stacks;
		
		float radius = start_radius;
		float dRadius = (end_radius - start_radius)/(float)stacks;
		
		int vertIdx = 0;
		
		int numLayers = stacks + 1;
		int numSpokes = sections + 1;
		for( int layer = 0; layer<numLayers; layer++ )
		{			
			Color layerColour = new Color( red, green, blue, alpha );
			
			if( ( layer == 0 || layer == stacks ) && info.m_FadeEnds )
				layerColour = new Color( 0.0f, 0.0f, 0.0f, 0.0f );				
			
			for( int spoke = 0; spoke<numSpokes; spoke++ )
			{
				myVertices[vertIdx] = new Vector3(
					radius*Mathf.Cos(angle),
					radius*Mathf.Sin(angle),
					zPos );
				
				
				myColours[vertIdx] = layerColour;
				
				vertIdx++;
				
				angle += dAngle;
			}
			
			angle = 0.0f;
			radius += dRadius;
			zPos += dZPos;
			
			red += dRed;
			green += dGreen;
			blue += dBlue;
			alpha += dAlpha;
		}
		
		//	Apply the verts to make the ring
		MeshFilter meshFilter = tube_object.GetComponent<MeshFilter>();
		Mesh mesh = meshFilter.sharedMesh;
		mesh.vertices = myVertices;		
		mesh.colors = myColours;
	}
	
	public static void ModifyUVs( GameObject tube_object, 
		float tex_u_tile, float tex_v_tile, float tex_u_offset, float tex_v_offset )
	{
		PrimitiveTubeInfo info = tube_object.GetComponent<PrimitiveTubeInfo>();
		info.m_TexUTile = tex_u_tile;
		info.m_TexVTile = tex_v_tile;
		info.m_TexUOffset = tex_u_offset;
		info.m_TexVOffset = tex_u_offset;

		int sections = info.m_Sections;
		int stacks = info.m_Stacks;

		int numVerts = (sections + 1)*(stacks + 1);
		Vector2[] myUVs = new Vector2[numVerts];

		float texU = tex_u_offset;
		float texV = tex_v_offset;
		float dTexU = tex_u_tile/(float)sections;
		float dTexV = tex_v_tile/(float)stacks;
		
		int vertIdx = 0;
		
		int numLayers = stacks + 1;
		int numSpokes = sections + 1;
		for( int layer = 0; layer<numLayers; layer++ )
		{
			for( int spoke = 0; spoke<numSpokes; spoke++ )
			{
				myUVs[vertIdx++] = new Vector2( texU, texV );

				texU += dTexU;
			}
			
			texU = tex_u_offset;
			texV += dTexV;
		}
		
		//	Apply the verts to make the ring
		MeshFilter meshFilter = tube_object.GetComponent<MeshFilter>();
		Mesh mesh = meshFilter.sharedMesh;
		mesh.uv = myUVs;
	}
}