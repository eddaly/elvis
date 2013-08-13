//-----------------------------------------------------------------------------
// File: PrimitiveQuadBatch.cs
//
// Desc:	Quad buffer creation and modification.
//
//			Static functions to create and modify a vertex buffer composed
//			of a number of quads.
//
//			Users get a reference to a BatchedQuadDef in the list held in the 
//			info object. This is used to fill the vertex buffer according
//			to it's properties.
//
// Note: 	The GameObject must be managed by the calling object
//
// Copyright Echo Peak Ltd 2013
//-----------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

//-----------------------------------------------------------------------------
// Class:	BatchedQuadDef
// Def:		Data defining a quad that will be used by the QuadBatch primitive
//			to add a quad into it's vertex buffer
public class BatchedQuadDef
{
	public bool m_Active;
	
	public int m_TextureIdx;
	
	public Vector3 m_Position;
	public Vector2 m_Scale;
	
	public float m_Rotation;
	public Color m_Colour;
}

//-----------------------------------------------------------------------------
// Class:	PrimitiveQuadBatchInfo
// Desc:	Data class attached to each GameObject returned by
//			PrimitiveQuadBatch.MakeVB()
//			Provides permanent state information needed by the modification
//			functions.
public class PrimitiveQuadBatchInfo : MonoBehaviour
{
	public int m_NumQuads;
	public int m_QuadSearchCursor;
	
	public int m_AltasIdx;
	
	public bool m_Rotating;
	public bool m_UniformScale;
	
	public Vector3[] m_Vertices;
	public Vector2[] m_UVs;
	public Color[] m_Colours;
	
	public BatchedQuadDef[] m_QuadDefs;
}

public class PrimitiveQuadBatch 
{ 
	static int m_uniqueVBID = 0;
	
	//	This is actually sqrt(2) - 1, used to calculate the hypotenuse of a uniformly
	//	scaled right angle triangle without a sqrt
	const float k_uniformHypCoeff = 0.41421356f;
	
	//-----------------------------------------------------------------------------
	// Method:	MakeQuadVB()
	// Desc:	Creates a new quad buffer of maximum size and creates the material
	public static GameObject MakeQuadBatch( int num_quads, TextureAtlas atlas, int atlas_idx, 
		bool rotate, bool uniform_scale, bool additive, int layer )
	{
		GameObject gameObject = new GameObject();
		gameObject.layer = layer;
		
		gameObject.name = "Primitive" + m_uniqueVBID.ToString() + " [QUADBATCH]";
		m_uniqueVBID++;
		
		PrimitiveQuadBatchInfo info = gameObject.AddComponent<PrimitiveQuadBatchInfo>();
		info.m_NumQuads = num_quads;
		info.m_QuadSearchCursor = 0;
		
		info.m_AltasIdx = atlas_idx;
		
		info.m_Rotating = rotate;
		info.m_UniformScale = uniform_scale;
		
		info.m_QuadDefs = new BatchedQuadDef[num_quads];
		
		int numVerts = num_quads*4;
		int numIndices = num_quads*6;
		
		MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
		MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();

		meshFilter.sharedMesh = new Mesh();
		Mesh mesh = meshFilter.sharedMesh;
		
		mesh.MarkDynamic();		// Unity4 only
		mesh.Clear();
		
		info.m_Vertices = new Vector3[numVerts];
		info.m_UVs = new Vector2[numVerts];
		info.m_Colours = new Color[numVerts];
		int[] myTris = new int[numIndices];

		for( int v = 0; v<numVerts; v++ )
		{
			info.m_Colours[v] = new Color( 1.0f, 1.0f, 1.0f, 1.0f );
		}

		int triIdx = 0;
		for( int q = 0; q<num_quads; q++ )
		{
			info.m_QuadDefs[q] = new BatchedQuadDef();
			
			info.m_QuadDefs[q].m_TextureIdx = 0;
			
			info.m_QuadDefs[q].m_Position = new Vector3( -1000.0f, -1000.0f, -1000.0f );
			info.m_QuadDefs[q].m_Scale = Vector2.one;
			
			info.m_QuadDefs[q].m_Rotation = 0.0f;
			info.m_QuadDefs[q].m_Colour = new Color( 1.0f, 1.0f, 1.0f, 1.0f );
			info.m_QuadDefs[q].m_Active = false;

			info.m_Vertices[q*4 + 0] = new Vector3( 1100.0f, 100.0f, 0.0f );
			info.m_UVs[q*4 + 0] = new Vector2( 0.0f, 0.0f );
			info.m_Vertices[q*4 + 1] = new Vector3( 1100.0f, 200.0f, 0.0f );
			info.m_UVs[q*4 + 1] = new Vector2( 0.0f, 1.0f );
			info.m_Vertices[q*4 + 2] = new Vector3( 1200.0f, 200.0f, 0.0f );
			info.m_UVs[q*4 + 2] = new Vector2( 1.0f, 1.0f );
			info.m_Vertices[q*4 + 3] = new Vector3( 1200.0f, 100.0f, 0.0f );
			info.m_UVs[q*4 + 3] = new Vector2( 1.0f, 0.0f );
			
			
			int baseIdx = q*4;
			myTris[triIdx++] = baseIdx;			
			myTris[triIdx++] = baseIdx + 1;			
			myTris[triIdx++] = baseIdx + 2;			
			
			myTris[triIdx++] = baseIdx;			
			myTris[triIdx++] = baseIdx + 2;			
			myTris[triIdx++] = baseIdx + 3;			
		}
		
		//	Apply the verts to make the plane
		mesh.vertices = info.m_Vertices;
		mesh.uv = info.m_UVs;
		mesh.colors = info.m_Colours;
		mesh.triangles = myTris;
		
		mesh.RecalculateBounds();
//		mesh.RecalculateNormals();
 
		//	Create a unique material with the alpha shader
		string shader = "EchoShader/AlphaBlended";
		if( additive )
			shader = "EchoShader/AlphaAdditive";
		
		Material newMaterial = new Material( Shader.Find( shader ) );
//		Material newMaterial = new Material( Shader.Find( "WOTD/UnlitAlpha" ) );
		meshRenderer.sharedMaterial = newMaterial;
		
		meshRenderer.sharedMaterial.mainTexture = atlas.m_TexturePage;
		meshRenderer.sharedMaterial.color = new Color( 1.0f, 1.0f, 1.0f, 1.0f );
		
		meshRenderer.sharedMaterial.renderQueue = 3500;
		
		return gameObject;
	}	
	
	//-----------------------------------------------------------------------------
	// Method:	GetQuadDef()
	// Desc:	Finds an inactive quad definition from the master list
	public static BatchedQuadDef GetQuadDef( GameObject quad_batch )
	{
		PrimitiveQuadBatchInfo info = quad_batch.GetComponent<PrimitiveQuadBatchInfo>();
		
		int searchPoint = info.m_QuadSearchCursor + 1;
		if( searchPoint >= info.m_NumQuads )
			searchPoint = 0;

		while( searchPoint != info.m_QuadSearchCursor )
		{
			if( !info.m_QuadDefs[searchPoint].m_Active )
			{
				info.m_QuadSearchCursor = searchPoint;
				info.m_QuadDefs[searchPoint].m_Active = true;
				
				return info.m_QuadDefs[searchPoint];
			}
			
			searchPoint++;
			if( searchPoint >= info.m_NumQuads )
				searchPoint = 0;			
		}
		
		//	There aren't any available
		return null;
	}
	
	//-----------------------------------------------------------------------------
	// Method:	ReleaseQuadDef()
	// Desc:	Gives the definition back to this batch
	public static void ReleaseQuadDef( BatchedQuadDef definition )
	{
		definition.m_Active = false;
	}

	//-----------------------------------------------------------------------------
	// Method:	ReclaimQuadDefs()
	// Desc:	Forcibly releases all quad definitions
	public static void ReclaimQuadDefs( GameObject quad_batch )
	{
		PrimitiveQuadBatchInfo info = quad_batch.GetComponent<PrimitiveQuadBatchInfo>();
		
		int maxQuads = info.m_NumQuads;
		for( int q = 0; q<maxQuads; q++ )
		{
			if( info.m_QuadDefs[q].m_Active )
				info.m_QuadDefs[q].m_Active = false;
		}
	}
	
	//-----------------------------------------------------------------------------
	// Method:	SetQuadVB()
	// Desc:	Goes through all the quad definitions and sets up the vertex
	//			buffer ready for rendering
	public static void SetQuadVB( GameObject quad_batch )
	{
		PrimitiveQuadBatchInfo info = quad_batch.GetComponent<PrimitiveQuadBatchInfo>();
		
		//	Find out how many active quads there are
		int maxQuads = info.m_NumQuads;
		int numQuads = 0;
		for( int q = 0; q<maxQuads; q++ )
		{
			if( info.m_QuadDefs[q].m_Active )
				numQuads++;
		}

		MeshFilter meshFilter = quad_batch.GetComponent<MeshFilter>();
		Mesh mesh = meshFilter.sharedMesh;
		
		//	Do the uvs and colours first
		int vert = 0;
		TextureAtlas atlas = PrimitiveLibrary.Get.m_Atlases[info.m_AltasIdx];
		for( int q = 0; q<maxQuads; q++ )
		{
			BatchedQuadDef quadDef = info.m_QuadDefs[q];
			if( quadDef.m_Active )
			{
				//	Needed to avoid overflowing texels
				const float tempHackFudge = 0;//.002f;
				
//				Debug.Log( quadDef.m_TextureIdx.ToString() + " is the number" );
				
				info.m_UVs[vert] = new Vector2
					( atlas.m_UVSet[quadDef.m_TextureIdx].x + tempHackFudge, atlas.m_UVSet[quadDef.m_TextureIdx].y + tempHackFudge );
				info.m_Colours[vert] = quadDef.m_Colour;
				vert++;
				
				info.m_UVs[vert] = new Vector2
					( atlas.m_UVSet[quadDef.m_TextureIdx].x + tempHackFudge, atlas.m_UVSet[quadDef.m_TextureIdx].w - tempHackFudge );
				info.m_Colours[vert] = quadDef.m_Colour;
				vert++;
				
				info.m_UVs[vert] = new Vector2
					( atlas.m_UVSet[quadDef.m_TextureIdx].z - tempHackFudge, atlas.m_UVSet[quadDef.m_TextureIdx].w - tempHackFudge );
				info.m_Colours[vert] = quadDef.m_Colour;
				vert++;
				
				info.m_UVs[vert] = new Vector2
					( atlas.m_UVSet[quadDef.m_TextureIdx].z - tempHackFudge, atlas.m_UVSet[quadDef.m_TextureIdx].y + tempHackFudge );
				info.m_Colours[vert] = quadDef.m_Colour;
				vert++;				
			}
		}
		
		vert = 0;

		//	Now the vert positions, depending on what functions are available in this batch		
		if( info.m_Rotating )
		{
			if( info.m_UniformScale )
			{
				//	Calculate verts for rotating, uniform scaling quads
				for( int q = 0; q<maxQuads; q++ )
				{
					BatchedQuadDef quadDef = info.m_QuadDefs[q];
					if( quadDef.m_Active )
					{
						//	Have to get the hypotenuse so the sides, rather than the
						//	corners, are m_Scale.x distance from the local origin
						float halfScale = quadDef.m_Scale.x*0.5f;
						float hyp = k_uniformHypCoeff*halfScale + halfScale;
						
						//	And adjust the angle slightly so that 0 means it's vertically
						//	aligned.
						float angle = quadDef.m_Rotation + Mathf.PI*0.25f;
						
						float xRot = hyp*Mathf.Cos( angle );
						float yRot = hyp*Mathf.Sin( angle );

						info.m_Vertices[vert++] = quadDef.m_Position + new Vector3
							( -xRot, -yRot, 0.0f );						
						info.m_Vertices[vert++] = quadDef.m_Position + new Vector3
							( -yRot, xRot, 0.0f );						
						info.m_Vertices[vert++] = quadDef.m_Position + new Vector3
							( xRot, yRot, 0.0f );						
						info.m_Vertices[vert++] = quadDef.m_Position + new Vector3
							( yRot, -xRot, 0.0f );						
					}
				}
			}
			else
			{
				//	Calculate verts for rotating, non-uniform scaling quads (worst-case scenario)
				for( int q = 0; q<maxQuads; q++ )
				{
					BatchedQuadDef quadDef = info.m_QuadDefs[q];
					if( quadDef.m_Active )
					{
						float angle = -quadDef.m_Rotation;
						
						float angCos = Mathf.Cos( angle );
						float angSin = Mathf.Sin( angle );
						float xCos = quadDef.m_Scale.x*0.5f*angCos;
						float xSin = quadDef.m_Scale.x*0.5f*angSin;
						float yCos = quadDef.m_Scale.y*0.5f*angCos;
						float ySin = quadDef.m_Scale.y*0.5f*angSin;
						
						info.m_Vertices[vert++] = quadDef.m_Position + new Vector3
							( -xCos - ySin, -yCos + xSin, 0.0f );						
						info.m_Vertices[vert++] = quadDef.m_Position + new Vector3
							( -xCos + ySin, yCos + xSin, 0.0f );						
						info.m_Vertices[vert++] = quadDef.m_Position + new Vector3
							( xCos + ySin, yCos - xSin, 0.0f );						
						info.m_Vertices[vert++] = quadDef.m_Position + new Vector3
							( xCos - ySin, -yCos - xSin, 0.0f );	
					}
				}
			}
		}
		else
		{
			if( info.m_UniformScale )
			{
				//	Calculate verts for non-rotating, uniform scaling quads (best-case scenario)
				for( int q = 0; q<maxQuads; q++ )
				{
					BatchedQuadDef quadDef = info.m_QuadDefs[q];
					if( quadDef.m_Active )
					{
						float side = quadDef.m_Scale.x*0.5f;
						
						info.m_Vertices[vert++] = quadDef.m_Position + new Vector3
							( -side, -side, 0.0f );						
						info.m_Vertices[vert++] = quadDef.m_Position + new Vector3
							( -side, side, 0.0f );						
						info.m_Vertices[vert++] = quadDef.m_Position + new Vector3
							( side, side, 0.0f );						
						info.m_Vertices[vert++] = quadDef.m_Position + new Vector3
							( side, -side, 0.0f );		
					}
				}
			}
			else
			{
				//	Calculate verts for non-rotating, non-uniform scaling quads
				for( int q = 0; q<maxQuads; q++ )
				{
					BatchedQuadDef quadDef = info.m_QuadDefs[q];
					if( quadDef.m_Active )
					{
						float x_side = quadDef.m_Scale.x*0.5f;
						float y_side = quadDef.m_Scale.y*0.5f;
						
						info.m_Vertices[vert++] = quadDef.m_Position + new Vector3
							( -x_side, -y_side, 0.0f );						
						info.m_Vertices[vert++] = quadDef.m_Position + new Vector3
							( -x_side, y_side, 0.0f );						
						info.m_Vertices[vert++] = quadDef.m_Position + new Vector3
							( x_side, y_side, 0.0f );						
						info.m_Vertices[vert++] = quadDef.m_Position + new Vector3
							( x_side, -y_side, 0.0f );						
					}
				}
			}
		}

		//	Might need to add a fake invisible quad if we didn't entirely fill the buffer
		if( numQuads != maxQuads )
		{
			Color invisible = new Color( 0.0f, 0.0f, 0.0f, 0.0f );
			
			for( int q = numQuads; q<maxQuads; q++ )
			{
				info.m_Colours[vert] = invisible;
				info.m_Vertices[vert++] = Vector3.one;
				info.m_Colours[vert] = invisible;
				info.m_Vertices[vert++] = Vector3.one;
				info.m_Colours[vert] = invisible;
				info.m_Vertices[vert++] = Vector3.one;
				info.m_Colours[vert] = invisible;
				info.m_Vertices[vert++] = Vector3.one;
			}
		}
		
		//	Apply the verts to make the plane
		mesh.vertices = info.m_Vertices;
		mesh.uv = info.m_UVs;
		mesh.colors = info.m_Colours;
		
		mesh.RecalculateBounds();
	}	
}


