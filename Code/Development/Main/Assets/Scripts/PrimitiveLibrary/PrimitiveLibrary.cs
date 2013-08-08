//-----------------------------------------------------------------------------
// File: PrimitiveLibrary.cs
//
// Desc:	Singleton class which manages pools and batches of standard 
//			primitives created at runtime for other objects to use as 
//			renderers.
//
// Copyright Echo Peak Ltd 2013
//-----------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

public sealed class PrimitiveLibrary
{
	static readonly PrimitiveLibrary instance = new PrimitiveLibrary();
	public static PrimitiveLibrary Get
	{
		get { return instance; }
	}
	public void Poke() {}

	
	//	Used to construct unique GameObject names to help debugging
	static int m_uniqueIDNumber = 0;

	//	Reset returns pool objects' material to this shader to help asset clearing
	Shader m_DefaultShader = null;
	Texture m_DefaultTexture = null;
	
	//	Objects used to organise the library pools in the scene hierarchy
	Transform m_HierarchyTopLevel;
	Transform m_HierarchyQuadPool;
	Transform m_HierarchyTextPool;
	Transform m_HierarchyQuadBatches;


	//	The quad pool. An array of GameObjects with the quads attached as
	//	MeshFilters. If they are inactive they are available
	const int gPL_NumQuads = 100;
	GameObject[] m_quadPool = new GameObject[gPL_NumQuads];
	int m_quadPoolCursor;


	//	The text pool. Same as the quad pool - soon to be batched
	const int gPL_NumText = 1000;
	GameObject[] m_textPool = new GameObject[gPL_NumText];
	int m_textPoolCursor;

	
	public TextureAtlas[] m_Atlases = new TextureAtlas[(int)TextureAtlas.AtlasID.NUM];

	
	public enum QuadBatch
	{
		FX_BATCH = 0,
		NUMBERS_BATCH,
		FRONTEND_BATCH,
		PLAYER_BATCH,
		ELVIS_BATCH,
		OBSTACLE_BATCH,
		COINS_BATCH,
		PLATFORM_BATCH,
		
		NUM
	}
	const int g_FXBatchSize = 1000;
	const int g_NumbersBatchSize = 70;
	const int g_FrontendBatchSize = 256;
	const int g_PlayerBatchSize = 16;
	const int g_ElvisBatchSize = 16;
	const int g_ObstacleBatchSize = 100;
	const int g_CoinsBatchSize = 256;
	const int g_PlatformBatchSize = 100;
	
	GameObject[] m_quadBatches = new GameObject[(int)QuadBatch.NUM];

	
	//-----------------------------------------------------------------------------
	// Constructor
	//
	// Desc:	Creates the pools of primitives that the library will manage
	PrimitiveLibrary()
	{
		//	Create containing objects for scene tidyness, and to aid debugging.
		GameObject topLevel = new GameObject();
		topLevel.name = "PrimitiveLibrary";		
		topLevel.AddComponent<PrimitiveLibraryUpdater>();
		m_HierarchyTopLevel = topLevel.GetComponent<Transform>();

		GameObject quadPool = new GameObject();
		quadPool.name = "QuadPool";
		m_HierarchyQuadPool = quadPool.GetComponent<Transform>();
		m_HierarchyQuadPool.parent = m_HierarchyTopLevel;

		GameObject textPool = new GameObject();
		textPool.name = "TextPool";
		m_HierarchyTextPool = textPool.GetComponent<Transform>();
		m_HierarchyTextPool.parent = m_HierarchyTopLevel;
		
		GameObject quadBatches = new GameObject( "QuadBatches" );
		m_HierarchyQuadBatches = quadBatches.transform;
		m_HierarchyQuadBatches.parent = m_HierarchyTopLevel;
		
		
		//	Set up the quad pool
		for( int p = 0; p<gPL_NumQuads; ++p )
		{
			m_quadPool[p] = PrimitivePlane.MakePlane( 1.0f, 1.0f, 1, 1 );
			m_quadPool[p].name = "Primitive[QUADPOOL]" + p.ToString();
			m_quadPool[p].SetActive( false );
				
			Transform transform = m_quadPool[p].GetComponent<Transform>();
			transform.parent = m_HierarchyQuadPool;
			transform.localPosition = Vector3.zero;
	
			MeshRenderer renderer = m_quadPool[p].GetComponent<MeshRenderer>();
			renderer.sharedMaterial.color = Color.white;
			
			GameObject.DontDestroyOnLoad( m_quadPool[p] );
		}
		m_quadPoolCursor = 0;


		//	Set up the text pool
		for (int p = 0; p < m_textPool.Length; ++p)
		{
			MakeText(p);
		}
		m_textPoolCursor = 0;

		
		
		//	Set up the texture atlasses for the batched primitives
		InitialiseAtlases();
		
		//	Set up the quad batches
		m_quadBatches[(int)QuadBatch.FX_BATCH] = PrimitiveQuadBatch.MakeQuadBatch( 
			g_FXBatchSize, 
			m_Atlases[(int)TextureAtlas.AtlasID.PARTICLE_FX], 
			(int)TextureAtlas.AtlasID.PARTICLE_FX, 
			true, true, true, 14 
			);
		m_quadBatches[(int)QuadBatch.FX_BATCH].transform.localPosition = Vector3.zero;
		
		m_quadBatches[(int)QuadBatch.NUMBERS_BATCH] = PrimitiveQuadBatch.MakeQuadBatch( 
			g_NumbersBatchSize, 
			m_Atlases[(int)TextureAtlas.AtlasID.NUMBERS], 
			(int)TextureAtlas.AtlasID.NUMBERS, 
			false, true, false, 8 
			);
		m_quadBatches[(int)QuadBatch.NUMBERS_BATCH].transform.localPosition = Vector3.zero;
		
		m_quadBatches[(int)QuadBatch.FRONTEND_BATCH] = PrimitiveQuadBatch.MakeQuadBatch( 
			g_FrontendBatchSize, 
			m_Atlases[(int)TextureAtlas.AtlasID.FRONTEND], 
			(int)TextureAtlas.AtlasID.FRONTEND, 
			false, false, false, 8 
			);
		m_quadBatches[(int)QuadBatch.FRONTEND_BATCH].transform.localPosition = Vector3.zero;

		m_quadBatches[(int)QuadBatch.PLAYER_BATCH] = PrimitiveQuadBatch.MakeQuadBatch( 
			g_PlayerBatchSize, 
			m_Atlases[(int)TextureAtlas.AtlasID.PLAYER],
			(int)TextureAtlas.AtlasID.PLAYER,
			false, true, false, 0 
			);
		m_quadBatches[(int)QuadBatch.PLAYER_BATCH].transform.localPosition = Vector3.zero;

		m_quadBatches[(int)QuadBatch.ELVIS_BATCH] = PrimitiveQuadBatch.MakeQuadBatch( 
			g_ElvisBatchSize, 
			m_Atlases[(int)TextureAtlas.AtlasID.ELVIS],
			(int)TextureAtlas.AtlasID.ELVIS,
			false, true, false, 0 
			);
		m_quadBatches[(int)QuadBatch.PLAYER_BATCH].transform.localPosition = Vector3.zero;

		m_quadBatches[(int)QuadBatch.OBSTACLE_BATCH] = PrimitiveQuadBatch.MakeQuadBatch( 
			g_ObstacleBatchSize, 
			m_Atlases[(int)TextureAtlas.AtlasID.OBSTACLES],
			(int)TextureAtlas.AtlasID.OBSTACLES,
			false, false, false, 0 
			);
		m_quadBatches[(int)QuadBatch.OBSTACLE_BATCH].transform.localPosition = Vector3.zero;

		m_quadBatches[(int)QuadBatch.COINS_BATCH] = PrimitiveQuadBatch.MakeQuadBatch( 
			g_CoinsBatchSize,
			m_Atlases[(int)TextureAtlas.AtlasID.COINS],
			(int)TextureAtlas.AtlasID.COINS,
			false, true, false, 0 
			);
		m_quadBatches[(int)QuadBatch.COINS_BATCH].transform.localPosition = Vector3.zero;

		m_quadBatches[(int)QuadBatch.PLATFORM_BATCH] = PrimitiveQuadBatch.MakeQuadBatch( 
			g_PlatformBatchSize, 
			m_Atlases[(int)TextureAtlas.AtlasID.PLATFORMS],
			(int)TextureAtlas.AtlasID.PLATFORMS,
			false, false, false, 0 
			);
		m_quadBatches[(int)QuadBatch.PLATFORM_BATCH].transform.localPosition = Vector3.zero;
		
		
		for( int q = 0; q<(int)QuadBatch.NUM; q++ )
			m_quadBatches[q].transform.parent = m_HierarchyQuadBatches;
		

		//	Default shader and texture are used to help Unity release assets on scene unloads
		m_DefaultShader = Shader.Find( "EchoShader/AlphaBlended" );
		m_DefaultTexture = (Texture2D)Resources.Load( "WhiteSquare" );
		GameObject.DontDestroyOnLoad( topLevel );
	}
	
	
	//-----------------------------------------------------------------------------
	// Method:	ReclaimAllPrimitives()
	// Desc:	Immediately extracts all primitives back into the library, setting
	//			properties to default for safety
	public void ReclaimAllPrimitives()
	{
		for( int p = 0; p<gPL_NumQuads; ++p )
		{
			if (m_quadPool[p] != null)
			{
				ResetQuad( m_quadPool[p] );
				ReleaseQuad( m_quadPool[p] );
				
				m_quadPool[p].name = "Primitive[QuadPool]" + p.ToString();
			}
		}

		
		for (int p = 0; p < m_textPool.Length; ++p)
		{
			
			if (m_textPool[p] != null)
			{
				Transform transform = m_textPool[p].GetComponent<Transform>();

				if (transform.parent != m_HierarchyTextPool)
				{
					Vector3 location = transform.position;
					transform.parent = m_HierarchyTextPool;
					transform.position = location;
					transform.gameObject.SetActive(false);
				}
			}
		}
		
		
		//	Quad batches always live in the Primitive Library hierarchy
		for( int qb = 0; qb<(int)QuadBatch.NUM; qb++ )
		{
			PrimitiveQuadBatch.ReclaimQuadDefs( m_quadBatches[qb] );
		}		
	}

	


	//-----------------------------------------------------------------------------
	//	Will be killed soon
	void MakeText(int p)
	{
		m_textPool[p] = PrimitivePlane.MakePlane(1.0f, 1.0f, 1, 1);
		GameObject.DontDestroyOnLoad(m_textPool[p]);
		m_textPool[p].SetActive(false);

		//	Force the name to QUADPOOL for debugging
		m_textPool[p].name = "Primitive" + (m_uniqueIDNumber++).ToString() + " [TEXTPOOL]";

		Transform transform = m_textPool[p].GetComponent<Transform>();
		transform.parent = m_HierarchyTextPool;
		Vector3 newPos = new Vector3(0.0f, 0.0f, 0.0f);
		transform.localPosition = newPos;

		MeshRenderer renderer = m_textPool[p].GetComponent<MeshRenderer>();
		renderer.sharedMaterial.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);


//		m_textPool[p].AddComponent<DestructionObserver>();
	}


	
	//-----------------------------------------------------------------------------
	// Method:	GetQuad()
	// Desc:	Find an inactive quad in the pool and return it.
	// Note:	The quads shouldn't have their geometry altered - if you need to
	//			do that then create your own quad with PrimitivePlane::MakePlane()
	//			Use the transform to alter position and size.
	//
	// Returns:	A GameObject with the quad attached as a MeshFilter, or null if
	//			there are no inactive quads	left in the pool.
	public GameObject GetQuad()
	{
		int currentCursor = m_quadPoolCursor++;
		if( m_quadPoolCursor == gPL_NumQuads )
			m_quadPoolCursor = 0;
			
		while( m_quadPool[m_quadPoolCursor].activeSelf && m_quadPoolCursor != currentCursor )
		{
			m_quadPoolCursor++;
			
			if( m_quadPoolCursor == gPL_NumQuads )
				m_quadPoolCursor = 0;			
		}

		if (currentCursor == m_quadPoolCursor)
		{
			return null;
		}
		else
		{
			m_quadPool[m_quadPoolCursor].SetActive(true);
			
			return m_quadPool[m_quadPoolCursor];
		}
	}
	
	//-----------------------------------------------------------------------------
	// Method:	GetQuad()
	// Desc:	Overloaded versions to encapsulate common setup
	public GameObject GetQuad( Texture texture )
	{
		return GetQuad( texture, new Vector3( 0.0f, 0.0f, 0.0f ) );
	}
	public GameObject GetQuad( Texture texture, Vector3 position )
	{
		return GetQuad( texture, position, new Color( 1.0f, 1.0f, 1.0f, 1.0f ) );
	}
	public GameObject GetQuad( Texture texture, Vector3 position, Color colour )
	{
		return GetQuad( texture, position, colour, new Vector3( 1.0f, 1.0f, 1.0f ) );
	}
	public GameObject GetQuad( Texture texture, Vector3 position, Color colour, Vector3 scale )
	{
		GameObject quad = PrimitiveLibrary.Get.GetQuad();		

		MeshRenderer renderer = quad.GetComponent<MeshRenderer>();			
		renderer.sharedMaterial.mainTexture = texture;
		renderer.sharedMaterial.SetColor( "_Color", colour );
		
		quad.transform.localPosition = position;
		quad.transform.localScale = scale;
		
		return quad;
	}
	
	//-----------------------------------------------------------------------------
	// Method:	ReleaseQuad()
	// Desc:	Returns the quad back to the library pool
	// Note:	The calling object should set their own reference to null to avoid
	//			conflicts
	public void ReleaseQuad( GameObject quad_object )
	{
		if( quad_object == null )
		{
			Console.WriteLine( "!** Trying to release null quad" );
			return;
		}
			
		quad_object.SetActive(false);
		quad_object.layer = 0;
		
		//	Put the quad object back in our hierarchy
		Transform quadTransform = quad_object.GetComponent<Transform>();
		quadTransform.parent = m_HierarchyQuadPool;

		//	Reset simple properties
		Vector3 zeroRotation = new Vector3( 0.0f, 0.0f, 0.0f );
		quadTransform.localEulerAngles = zeroRotation;
		
		quadTransform.localScale = new Vector3( 1.0f, 1.0f, 1.0f );
	}

	//-----------------------------------------------------------------------------
	// Method:	ResetQuad()
	// Desc:	Sets various properties to default
	// Note:	Quad owners should reset special properties themselves before
	//			returning a quad. ResetQuad should be avoided if possible at 
	//			runtime as it does a lot of processing.
	public void ResetQuad( GameObject quad_object )
	{
		if( quad_object == null )
		{
			Console.WriteLine( "!** Trying to reset null quad" );
			return;
		}
			
		Material quadMaterial = quad_object.GetComponent<MeshRenderer>().material;
		quadMaterial.SetColor( "_Emissive", new Color( 0.0f, 0.0f, 0.0f, 0.0f ) );
		quadMaterial.shader = m_DefaultShader;
		quadMaterial.mainTexture = m_DefaultTexture;
		quadMaterial.renderQueue = 3000;

		PrimitivePlaneInfo info = quad_object.GetComponent<PrimitivePlaneInfo>();
		info.m_TexUTile = 1.0f;
		info.m_TexVTile = 1.0f;
		info.m_TexUOffset = 0.0f;
		info.m_TexVOffset = 0.0f;

		PrimitivePlane.ModifyVerts(quad_object, 1.0f, 1.0f);				
	}
	


	//	Will be killed
	public GameObject GetText()
	{
		int currentCursor = m_textPoolCursor++;
		if (m_textPoolCursor == m_textPool.Length)
			m_textPoolCursor = 0;

		while (m_textPool[m_textPoolCursor] == null || (m_textPool[m_textPoolCursor].activeSelf && m_textPoolCursor != currentCursor))
		{
			m_textPoolCursor++;

			if (m_textPoolCursor == m_textPool.Length)
				m_textPoolCursor = 0;
		}

		if (currentCursor == m_textPoolCursor)
		{
			//Console.WriteLine("GetQuad() -> NULL");
			return null;
		}
		else
		{
			m_textPool[m_textPoolCursor].SetActive(true);
			PrimitivePlane.ModifyUVs(m_textPool[m_textPoolCursor], 0.0f, 0.0f, 1.0f, 1.0f);
			return m_textPool[m_textPoolCursor];
		}
	}

	public void ReleaseText(GameObject quad_object)
	{
		if (quad_object == null)
		{
			Console.WriteLine("!** Trying to release null Text");
			return;
		}

		quad_object.SetActive(false);

		//	Put the quad object back in our hierarchy
		Vector3 hideQuad = new Vector3(-1000.0f, 0.0f, 0.0f);
		Transform transform = quad_object.GetComponent<Transform>();
		transform.parent = m_HierarchyTextPool;
		transform.localPosition = hideQuad;

		Vector3 zeroRotation = new Vector3(0.0f, 0.0f, 0.0f);
		transform.localEulerAngles = zeroRotation;

		Vector3 unitScale = new Vector3(1.0f, 1.0f, 1.0f);
		transform.localScale = unitScale;

		MeshRenderer quadRenderer = quad_object.GetComponent<MeshRenderer>();
		Color glowColour = new Color(0.0f, 0.0f, 0.0f, 0.0f);
		quadRenderer.sharedMaterial.SetColor("_Emissive", glowColour);
		quadRenderer.sharedMaterial.shader = m_DefaultShader;


		PrimitivePlaneInfo info = quad_object.GetComponent<PrimitivePlaneInfo>();
		info.m_TexUTile = 1.0f;
		info.m_TexVTile = 1.0f;
		info.m_TexUOffset = 0.0f;
		info.m_TexVOffset = 0.0f;

		quad_object.layer = 0;
	}
	
	
	
	//-----------------------------------------------------------------------------
	// Method:	GetQuadDefinition()
	// Desc:	Returns a reference to a QuadDefinition object from the specified
	//			quad batch, or null if none are left
	public BatchedQuadDef GetQuadDefinition( QuadBatch batch )
	{
		return PrimitiveQuadBatch.GetQuadDef( m_quadBatches[(int)batch] );
	}

	//-----------------------------------------------------------------------------
	// Method:	ReleaseQuadDefinition()
	// Desc:	Hands the quad definition back to the quad batch
	public void ReleaseQuadDefinition( BatchedQuadDef def )
	{
		PrimitiveQuadBatch.ReleaseQuadDef( def );
	}
	
	//-----------------------------------------------------------------------------
	// Method:	UpdateBatches()
	public void UpdateBatches()
	{
		for( int qb = 0; qb<(int)QuadBatch.NUM; qb++ )
		{
			PrimitiveQuadBatch.SetQuadVB( m_quadBatches[qb] );
		}
	}

	//-----------------------------------------------------------------------------
	// Method:	InitialiseAtlases
	// Desc:	Sets all the atlas texture data
	public void InitialiseAtlases()
	{
		//	Create them all
		for( int a = 0; a<(int)TextureAtlas.AtlasID.NUM; a++ )
			m_Atlases[a] = new TextureAtlas();
		
		
		//	Load the textures
		m_Atlases[(int)TextureAtlas.AtlasID.PARTICLE_FX].m_TexturePage =
			Resources.Load( "FX_Atlas" ) as Texture;
		m_Atlases[(int)TextureAtlas.AtlasID.NUMBERS].m_TexturePage =
			Resources.Load( "Numbers_Atlas" ) as Texture;
		
		if( FrontEnd.instance != null )
		{
			m_Atlases[(int)TextureAtlas.AtlasID.FRONTEND].m_TexturePage =
				Resources.Load( FrontEnd.instance.m_AtlasFile ) as Texture;
		}
		else
		{
			m_Atlases[(int)TextureAtlas.AtlasID.FRONTEND].m_TexturePage =
				Resources.Load( "Numbers_Atlas" ) as Texture;
		}
		
		m_Atlases[(int)TextureAtlas.AtlasID.PLAYER].m_TexturePage =
			Resources.Load( "Player_Atlas" ) as Texture;
		m_Atlases[(int)TextureAtlas.AtlasID.ELVIS].m_TexturePage =
			Resources.Load( "Elvis_Atlas" ) as Texture;
		m_Atlases[(int)TextureAtlas.AtlasID.OBSTACLES].m_TexturePage =
			Resources.Load( "Obstacles_Atlas" ) as Texture;
		m_Atlases[(int)TextureAtlas.AtlasID.COINS].m_TexturePage =
			Resources.Load( "Coins_Atlas" ) as Texture;
		m_Atlases[(int)TextureAtlas.AtlasID.PLATFORMS].m_TexturePage =
			Resources.Load( "Platforms_Atlas" ) as Texture;
		
		//	Set up the FX atlas
		TextureAtlas fxAtlas = m_Atlases[(int)TextureAtlas.AtlasID.PARTICLE_FX];		
		fxAtlas.m_NumElements = (int)TextureAtlas.ParticleFX.NUM;		
		fxAtlas.m_UVSet = new Vector4[fxAtlas.m_NumElements];
		
		fxAtlas.m_UVSet[(int)TextureAtlas.ParticleFX.SHINE] = new Vector4( 0.0f, 0.5f, 0.5f, 1.0f );
		fxAtlas.m_UVSet[(int)TextureAtlas.ParticleFX.BANG_PIECES] = new Vector4( 0.5f, 0.5f, 1.0f, 1.0f );
		fxAtlas.m_UVSet[(int)TextureAtlas.ParticleFX.GLOW_RING] = new Vector4( 0.0f, 0.0f, 0.5f, 0.5f );
		fxAtlas.m_UVSet[(int)TextureAtlas.ParticleFX.GLOW] = new Vector4( 0.5f, 0.0f, 1.0f, 0.5f );
		
		
		//	And the numbers
		TextureAtlas numbersAtlas = m_Atlases[(int)TextureAtlas.AtlasID.NUMBERS];		
		numbersAtlas.m_NumElements = (int)TextureAtlas.Numbers.NUM;		
		numbersAtlas.m_UVSet = new Vector4[numbersAtlas.m_NumElements];

		for( int n = 0; n<10; n++ )
		{
			int column = n%4;
			int row = 3 - n/4;
			
			float uStart = (float)column*0.25f;
			float vStart = (float)row*0.25f;
			
			numbersAtlas.m_UVSet[n] = new Vector4( uStart, vStart, uStart + 0.25f, vStart + 0.25f );
		}
		
		numbersAtlas.m_UVSet[(int)TextureAtlas.Numbers.NUM_COLON] = new Vector4( 0.0f, 0.0f, 0.25f, 0.25f );
		numbersAtlas.m_UVSet[(int)TextureAtlas.Numbers.NUM_PERCENT] = new Vector4( 0.75f, 0.25f, 1.0f, 0.5f );
		numbersAtlas.m_UVSet[(int)TextureAtlas.Numbers.NUM_TIMES] = new Vector4( 0.5f, 0.25f, 0.75f, 0.5f );		

		// And the front-end
		TextureAtlas frontendAtlas = m_Atlases[(int)TextureAtlas.AtlasID.FRONTEND];		
		frontendAtlas.m_NumElements = TextureAtlas.maxFrontendElements;		
		frontendAtlas.m_UVSet = new Vector4[frontendAtlas.m_NumElements]; // Setup dynamically by FastGUIElement
		
		
		//	The player atlas - just break it up in pieces for now to leave it open for animation
		TextureAtlas playerAtlas = m_Atlases[(int)TextureAtlas.AtlasID.PLAYER];
		playerAtlas.m_NumElements = 64;
		playerAtlas.m_UVSet = new Vector4[playerAtlas.m_NumElements];
		
		for( int p = 0; p<64; p++ )
		{
			float uTex = ((float)(p%8))*0.125f;
			float vTex = (1.0f - 0.125f) - ((float)(p/8))*0.125f;
			
			playerAtlas.m_UVSet[p] = new Vector4( uTex, vTex, uTex + 0.125f, vTex + 0.125f );
		}
		
		//	Same for Elvis
		TextureAtlas elvisAtlas = m_Atlases[(int)TextureAtlas.AtlasID.ELVIS];
		elvisAtlas.m_NumElements = 64;
		elvisAtlas.m_UVSet = new Vector4[elvisAtlas.m_NumElements];
		
		for( int e = 0; e<64; e++ )
		{
			float uTex = ((float)(e%8))*0.125f;
			float vTex = ((float)(e/8))*0.125f;
			
			elvisAtlas.m_UVSet[e] = new Vector4( uTex, vTex, uTex + 0.125f, vTex + 0.125f );
		}
		
		
		//	Obstacle atlas
		TextureAtlas obstacleAtlas = m_Atlases[(int)TextureAtlas.AtlasID.OBSTACLES];		
		obstacleAtlas.m_NumElements = (int)TextureAtlas.Obstacles.NUM;		
		obstacleAtlas.m_UVSet = new Vector4[obstacleAtlas.m_NumElements];
		
		obstacleAtlas.m_UVSet[(int)TextureAtlas.Obstacles.KILLBOX] = new Vector4( 0.0f, 0.5f, 1.0f, 1.0f );
		obstacleAtlas.m_UVSet[(int)TextureAtlas.Obstacles.KILLCIRCLE] = new Vector4( 0.0f, 0.0f, 1.0f, 0.5f );

		
		//	Coins atlas
		TextureAtlas coinsAtlas = m_Atlases[(int)TextureAtlas.AtlasID.COINS];
		coinsAtlas.m_NumElements = 64;
		coinsAtlas.m_UVSet = new Vector4[coinsAtlas.m_NumElements];
		
		for( int e = 0; e<32; e++ )
		{
			float uTex = ((float)(e%4))*0.25f;
			float vTex = ((float)(e/4))*0.125f;
			
			coinsAtlas.m_UVSet[e] = new Vector4( uTex, vTex, uTex + 0.25f, vTex + 0.125f );
		}

		
		//	Platform atlas
		TextureAtlas platformAtlas = m_Atlases[(int)TextureAtlas.AtlasID.PLATFORMS];		
		platformAtlas.m_NumElements = (int)TextureAtlas.Platforms.NUM;
		platformAtlas.m_UVSet = new Vector4[platformAtlas.m_NumElements];
		
		platformAtlas.m_UVSet[(int)TextureAtlas.Platforms.SOLID] = new Vector4( 0.0f, 0.5f, 1.0f, 1.0f );
		platformAtlas.m_UVSet[(int)TextureAtlas.Platforms.PASS_THROUGH] = new Vector4( 0.0f, 0.426f, 1.0f, 0.5f );
	}
}

//-----------------------------------------------------------------------------
// Class:	TextureAtlas
// Def:		A list of uv co-ordinates mapping out a shared texture 
public class TextureAtlas
{
	public int m_NumElements;
	public Vector4[] m_UVSet;
	public Texture m_TexturePage;

	public enum AtlasID
	{
		PARTICLE_FX = 0,
		NUMBERS,
		FRONTEND,
		PLAYER,
		ELVIS,
		OBSTACLES,
		COINS,
		PLATFORMS,
		
		NUM
	}
	
	public enum ParticleFX
	{
		SHINE = 0,
		BANG_PIECES,
		GLOW_RING,
		GLOW,
		
		NUM
	}
	
	public enum Numbers
	{
		NUM_0 = 0,
		NUM_1,
		NUM_2,
		NUM_3,
		NUM_4,
		NUM_5,
		NUM_6,
		NUM_7,
		NUM_8,
		NUM_9,
		NUM_PERCENT,
		NUM_COLON,
		NUM_TIMES,
		
		NUM
	}	
	
	static public int maxFrontendElements = 256;
	
	public enum Obstacles
	{
		KILLBOX,
		KILLCIRCLE,
		
		NUM
	}
	
	public enum Platforms
	{
		SOLID,
		PASS_THROUGH,
		
		NUM
	}
}
