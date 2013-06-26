//-----------------------------------------------------------------------------
// File: PrimitivePlane.cs
//
// Desc:	Custom plane mesh creation and modification.
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

//-----------------------------------------------------------------------------a
// Class:	PrimitivePlaneInfo
// Desc:	Data class attached to each GameObject returned by
//			PrimitivePlane.MakePlane()
//			Provides permanent state information needed by the modification
//			functions.
public class PrimitivePlaneInfo : MonoBehaviour
{
	public float m_ScaleX;
	public float m_ScaleY;
	
	public int m_SectionsX;
	public int m_SectionsY;
	
	public float m_TexUTile;
	public float m_TexVTile;
	
	public float m_TexUOffset;
	public float m_TexVOffset;
}

public class PrimitivePlane 
{ 
	static int m_uniquePlaneID = 0;
	
	public static GameObject MakePlane(float scale_x, float scale_y, int sections_x, int sections_y)
	{
		GameObject gameObject = new GameObject();
		gameObject.name = "Primitive" + m_uniquePlaneID.ToString() + " [PLANE]";
		m_uniquePlaneID++;

		PrimitivePlaneInfo info = gameObject.AddComponent<PrimitivePlaneInfo>();
		info.m_ScaleX = scale_x;
		info.m_ScaleY = scale_y;
		info.m_SectionsX = sections_x;
		info.m_SectionsY = sections_y;
		info.m_TexUTile = 1.0f;
		info.m_TexVTile = 1.0f;
		info.m_TexUOffset = 0.0f;
		info.m_TexVOffset = 0.0f;

		MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
		MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
		Mesh mesh = meshFilter.mesh;

		mesh.MarkDynamic();
		mesh.Clear();

		int numVertsX = sections_x + 1;
		int numVertsY = sections_y + 1;

		int numVerts = numVertsX * numVertsY;
		int numIndices = sections_x * sections_y * 6;

		Vector3[] myVertices = new Vector3[numVerts];
		Vector2[] myUVs = new Vector2[numVerts];
		Color[] myColours = new Color[numVerts];
		int[] myTris = new int[numIndices];

		float xPos = -scale_x * 0.5f;
		float yPos = -scale_y * 0.5f;
		float dX = scale_x / (float)sections_x;
		float dY = scale_y / (float)sections_y;

		float uPos = 0.0f;
		float vPos = 0.0f;
		float dU = 1.0f / (float)sections_x;
		float dV = 1.0f / (float)sections_y;

		int vertIdx = 0;
		int triIdx = 0;

		for (int yv = 0; yv < numVertsY; ++yv)
		{
			for (int xv = 0; xv < numVertsX; ++xv)
			{
				myVertices[vertIdx] = new Vector3(xPos, yPos, 0.0f);
				myUVs[vertIdx] = new Vector2(uPos, vPos);
				myColours[vertIdx] = new Color(1.0f, 1.0f, 1.0f, 1.0f);

				xPos += dX;
				uPos += dU;

				if (xv < sections_x && yv < sections_y)
				{
					myTris[triIdx++] = vertIdx;
					myTris[triIdx++] = vertIdx + numVertsX;
					myTris[triIdx++] = vertIdx + 1;

					myTris[triIdx++] = vertIdx + 1;
					myTris[triIdx++] = vertIdx + numVertsX;
					myTris[triIdx++] = vertIdx + numVertsX + 1;
				}

				vertIdx++;
			}

			xPos = -scale_x * 0.5f;
			uPos = 0.0f;

			vPos += dV;
			yPos += dY;
		}

		//	Apply the verts to make the plane
		mesh.vertices = myVertices;
		mesh.uv = myUVs;
		mesh.colors = myColours;
		mesh.triangles = myTris;

		mesh.RecalculateBounds();

		//	Create a unique material with the alpha shader
		Material newMaterial = new Material( Shader.Find("EchoShader/AlphaBlended") );
		meshRenderer.sharedMaterial = newMaterial;
		meshRenderer.sharedMaterial.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

		return gameObject;
	}



	public static void AddEdgeFade( GameObject plane_object, bool fade_x, bool fade_z )
	{
		PrimitivePlaneInfo info = plane_object.GetComponent<PrimitivePlaneInfo>();		
		int numVertsX = info.m_SectionsX + 1;
		int numVertsY = info.m_SectionsY + 1;		
		int numVerts = numVertsX*numVertsY;
		
		Color[] myColours = new Color[numVerts];
		
		int vertIdx = 0;
		
		for( int yv = 0; yv<numVertsY; ++yv )
		{
			for( int xv = 0; xv<numVertsX; ++xv )
			{
				if( fade_x && (xv < 1 || xv == numVertsX - 1) )
					myColours[vertIdx++] = new Color( 1.0f, 1.0f, 1.0f, 0.0f );
				else if( fade_z && (yv < 1 || yv == numVertsY - 1) )
					myColours[vertIdx++] = new Color( 1.0f, 1.0f, 1.0f, 0.0f );
				else
					myColours[vertIdx++] = new Color( 1.0f, 1.0f, 1.0f, 1.0f );
			}
		}

		//	Apply the verts to modify the plane
		MeshFilter meshFilter = plane_object.GetComponent<MeshFilter>();
		Mesh mesh = meshFilter.sharedMesh;
		mesh.colors = myColours;
	}
	
	public static void ModifyVerts( GameObject plane_object, float scale_x, float scale_y )
	{
		if (plane_object == null)
		{
			EchoDebugLog.LogWarning("PrimitivePlane: ModifyVerts: plane_object == null, early out");
			return;
		}
		
		PrimitivePlaneInfo info = plane_object.GetComponent<PrimitivePlaneInfo>();
		if (info.m_ScaleX != scale_x || info.m_ScaleY != scale_y)
		{
			info.m_ScaleX = scale_x;
			info.m_ScaleY = scale_y;

			int numVertsX = info.m_SectionsX + 1;
			int numVertsY = info.m_SectionsY + 1;
			int numVerts = numVertsX * numVertsY;

			Vector3[] myVertices = new Vector3[numVerts];

			float xPos = -scale_x * 0.5f;
			float yPos = -scale_y * 0.5f;
			float dX = scale_x / (float)(numVertsX - 1);
			float dY = scale_y / (float)(numVertsY - 1);

			int vertIdx = 0;

			for (int yv = 0; yv < numVertsY; ++yv)
			{
				for (int xv = 0; xv < numVertsX; ++xv)
				{
					myVertices[vertIdx++] = new Vector3(xPos, yPos, 0.0f);
					xPos += dX;
				}

				xPos = -scale_x * 0.5f;
				yPos += dY;
			}

			//	Apply the verts to modify the plane
			MeshFilter meshFilter = plane_object.GetComponent<MeshFilter>();
			Mesh mesh = meshFilter.sharedMesh;
			mesh.vertices = myVertices;
		}
	}

	public static void ModifyUVs( GameObject plane_object, 
		float tex_u_offset, float tex_v_offset, float tex_u_tile, float tex_v_tile )
	{
		PrimitivePlaneInfo info = plane_object.GetComponent<PrimitivePlaneInfo>();
		info.m_TexUOffset = tex_u_offset;
		info.m_TexVOffset = tex_v_offset;
		info.m_TexUTile = tex_u_tile;
		info.m_TexVTile = tex_v_tile;
		
		int numVertsX = info.m_SectionsX + 1;
		int numVertsY = info.m_SectionsY + 1;		
		int numVerts = numVertsX*numVertsY;
		
		Vector2[] myUVs = new Vector2[numVerts];
		
		float uPos = tex_u_offset;
		float vPos = tex_v_offset;
		float dU = tex_u_tile/(float)(numVertsX - 1);
		float dV = tex_v_tile/(float)(numVertsY - 1);		
		
		int vertIdx = 0;
		
		for( int yv = 0; yv<numVertsY; ++yv )
		{
			for( int xv = 0; xv<numVertsX; ++xv )
			{
				myUVs[vertIdx++] = new Vector2( uPos, vPos );
				uPos += dU;				
			}

			uPos = tex_u_offset;
			vPos += dV;
		}

		//	Apply the verts to modify the plane
		MeshFilter meshFilter = plane_object.GetComponent<MeshFilter>();
		Mesh mesh = meshFilter.sharedMesh;
		mesh.uv = myUVs;
	}	
}
