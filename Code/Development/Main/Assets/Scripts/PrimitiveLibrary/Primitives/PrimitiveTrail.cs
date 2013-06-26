//-----------------------------------------------------------------------------
// File: PrimitiveTrail.cs
//
// Desc:	A triangle strip attached to a moving point in space
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
// Class:	PrimitiveTrailInfo
// Desc:	Data class attached to each GameObject returned by
//			PrimitiveTrail.MakeTrail()
//			Provides permanent state information needed by the modification
//			functions.
public class PrimitiveTrailInfo : MonoBehaviour
{
	public int m_Sections;
	public Vector3[] m_TrailPoints;
	public Vector3[] m_TrailDirections;
	public Vector3 m_TrailNormal;
	public int m_TrailHead;
	public int m_TrailLength;

	public float m_FadePoint;

	public float m_StartWidth;
	public float m_EndWidth;

	//	TexU = 0.0f is the head of the trail
	public float m_TexUTile;
	public float m_TexVTile;
	
	public float m_TexUOffset;
	public float m_TexVOffset;
}

public class PrimitiveTrail
{
	static int m_uniqueTrailID = 0;
	
	public static GameObject MakeTrail( int sections, float fade_point, float start_width, float end_width )
	{
		GameObject gameObject = new GameObject();	
		gameObject.name = "Primitive" + m_uniqueTrailID.ToString() + " [TRAIL]";
		m_uniqueTrailID++;		
		
		PrimitiveTrailInfo info = gameObject.AddComponent<PrimitiveTrailInfo>();
		info.m_Sections = sections;
		info.m_TrailPoints = new Vector3[sections + 1];
		info.m_TrailDirections = new Vector3[sections + 1];
		info.m_TrailNormal = new Vector3( 1.0f, 0.0f, 0.0f );
		info.m_TrailHead = 0;
		info.m_TrailLength = -1;	//	You need at least 2 points before you've made a section
		info.m_FadePoint = fade_point;
		info.m_StartWidth = start_width;
		info.m_EndWidth = end_width;
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
		
		int numVerts = (sections + 1)*2;
		int numIndices = sections*6;
				
		Vector3[] myVertices = new Vector3[numVerts];
		Vector2[] myUVs = new Vector2[numVerts];
		Color[] myColours = new Color[numVerts];
		int[] myTris = new int[numIndices];

		float texU = 0.0f;
		float dTexU = 1.0f/(float)sections;

		int vertIdx = 0;
		int indexIdx = 0;
		
		int numSpokes = sections + 1;
		for( int spoke = 0; spoke<numSpokes; spoke++ )
		{
			//	2 verts per spoke
			myVertices[vertIdx] = new Vector3( 0.0f, 0.0f, 0.0f );
			myUVs[vertIdx] = new Vector2( texU, 0.0f );
			myColours[vertIdx++] = new Color( 1.0f, 1.0f, 1.0f, 0.0f );

			myVertices[vertIdx] = new Vector3( 0.0f, 0.0f, 0.0f );
			myUVs[vertIdx] = new Vector2( texU, 1.0f );
			myColours[vertIdx++] = new Color( 1.0f, 1.0f, 1.0f, 0.0f );


			if( spoke < sections )
			{
				//	Now set the triangles for this section
				myTris[indexIdx++] = vertIdx - 2;
				myTris[indexIdx++] = vertIdx;
				myTris[indexIdx++] = vertIdx - 1;

				myTris[indexIdx++] = vertIdx - 1;
				myTris[indexIdx++] = vertIdx;
				myTris[indexIdx++] = vertIdx + 1;
			}

			texU += dTexU;
		}

		//	Apply the verts to make the trail
		mesh.vertices = myVertices;
		mesh.uv = myUVs;
		mesh.colors = myColours;
		mesh.triangles = myTris;
		
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
 
		//	Create a unique material with the alpha shader
		Material newMaterial = new Material( Shader.Find( "EchoShader/AlphaBlended" ) );
		meshRenderer.sharedMaterial = newMaterial;
		meshRenderer.sharedMaterial.color = new Color( 1.0f, 1.0f, 1.0f, 1.0f );
		
		return gameObject;
	} 
	
	public static void AddTrailPoint( GameObject trail_object, Vector3 new_point )
	{
		PrimitiveTrailInfo info = trail_object.GetComponent<PrimitiveTrailInfo>();
		
		//	Calculate the tangent
		int previousPointIdx = info.m_TrailHead - 1;
		if( previousPointIdx < 0 )
			previousPointIdx = info.m_Sections;
		
		Vector3 direction = new_point - info.m_TrailPoints[previousPointIdx];
		direction.Normalize();
		
		//	Add trail point directly into the list
		info.m_TrailPoints[info.m_TrailHead] = new_point;
		info.m_TrailDirections[info.m_TrailHead] = direction;

		info.m_TrailHead++;

		//	If the trail head has moved beyond the maximum number of spokes, then recycle it
		if( info.m_TrailHead > info.m_Sections )
			info.m_TrailHead = 0;

		if( info.m_TrailLength < info.m_Sections )
			info.m_TrailLength++;
	}

	public static void ResetTrail( GameObject trail_object )
	{
		PrimitiveTrailInfo info = trail_object.GetComponent<PrimitiveTrailInfo>();

		info.m_TrailHead = 0;
		info.m_TrailLength = -1;

		//	Set all verts to be transparent
		MeshFilter meshFilter = trail_object.GetComponent<MeshFilter>();
		Mesh mesh = meshFilter.sharedMesh;

		int numVerts = mesh.vertexCount;

		Color[] myColours = new Color[numVerts];

		for( int v = 0; v<numVerts; v++ )
		{
			myColours[v] = new Color( 1.0f, 1.0f, 1.0f, 0.0f );
		}

		mesh.colors = myColours;
	}

	//	Trails need to be reconstructed each frame to reflect the new trail points
	public static void ModifyUVs( GameObject trail_object )
	{
		PrimitiveTrailInfo info = trail_object.GetComponent<PrimitiveTrailInfo>();

		int trailHead = info.m_TrailHead;
		int trailLength = info.m_TrailLength;
		int sections = info.m_Sections;
		
		float uOffset = info.m_TexUOffset;
		float vOffset = info.m_TexVOffset;
		float uTile = info.m_TexUTile;
		float vTile = info.m_TexVTile;

		//	If we don't have at least 2 points, then there's nothing to render yet
		if( trailLength < 1 )
			return;

		MeshFilter meshFilter = trail_object.GetComponent<MeshFilter>();
		Mesh mesh = meshFilter.sharedMesh;

		int numVerts = (sections + 1)*2;

		Vector2[] myUVs = new Vector2[numVerts];

		int vertIdx = 0;

		int trailPointIdx = trailHead - 1;
		if( trailPointIdx < 0 )
			trailPointIdx = trailLength;

		int numSpokes = trailLength + 1;

		float texU = uOffset;
		float dTexU = uTile/(float)numSpokes;
		
		for( int spoke = 0; spoke<numSpokes; spoke++ )
		{
			myUVs[vertIdx++] = new Vector2( texU, vOffset );
			myUVs[vertIdx++] = new Vector2( texU, vOffset + vTile );

			texU += dTexU;
		}

		//	Apply the new uvs to the mesh
		mesh.uv = myUVs;
	}
	
	//	Trails need to be reconstructed each frame to reflect the new trail points
	public static void ReconstructMesh( GameObject trail_object )
	{
		PrimitiveTrailInfo info = trail_object.GetComponent<PrimitiveTrailInfo>();
		int sections = info.m_Sections;
		Vector3[] trailPoints = info.m_TrailPoints;
		Vector3[] trailDirections = info.m_TrailDirections;
		Vector3 trailNormal = info.m_TrailNormal;
		
		int trailHead = info.m_TrailHead;
		int trailLength = info.m_TrailLength;
		float fadePoint = info.m_FadePoint;
		float startWidth = info.m_StartWidth;
		float endWidth = info.m_EndWidth;

		//	If we don't have at least 2 points, then there's nothing to render yet
		if( trailLength < 1 )
			return;

		MeshFilter meshFilter = trail_object.GetComponent<MeshFilter>();
		Mesh mesh = meshFilter.sharedMesh;

		int numVerts = (sections + 1)*2;

		Vector3[] myVertices = new Vector3[numVerts];
		Color[] myColours = new Color[numVerts];

		int vertIdx = 0;

		int trailPointIdx = trailHead - 1;
		if( trailPointIdx < 0 )
			trailPointIdx = trailLength;

		int numSpokes = trailLength + 1;

		float proportion = 1.0f - (float)trailLength/(float)sections;
		float dProportion = 1.0f/(float)sections;
		float fadeRatio = 1.0f - fadePoint;

		float halfWidth = (1.0f - proportion)*startWidth + proportion*endWidth;
		float dHalfWidth = (endWidth - startWidth)/(float)sections;

		for( int spoke = 0; spoke<numSpokes; spoke++ )
		{
			float alpha = 1.0f;
			if( proportion > fadePoint )
				alpha -= (proportion - fadePoint)/fadeRatio;

			Vector3 tangent = Vector3.Cross( trailDirections[trailPointIdx], trailNormal );
			
			//	2 verts per spoke
			myVertices[vertIdx] = trailPoints[trailPointIdx];
			myVertices[vertIdx] += tangent*halfWidth;
			myColours[vertIdx++] = new Color( 1.0f, 1.0f, 1.0f, alpha );

			myVertices[vertIdx] = trailPoints[trailPointIdx];
			myVertices[vertIdx] -= tangent*halfWidth;
			myColours[vertIdx++] = new Color( 1.0f, 1.0f, 1.0f, alpha );

			trailPointIdx--;
			if( trailPointIdx < 0 )
				trailPointIdx = sections;

			proportion += dProportion;
			halfWidth += dHalfWidth;
		}

		//	Apply the verts to make the trail
		mesh.vertices = myVertices;
		mesh.colors = myColours;
		mesh.RecalculateBounds();
	}
}
