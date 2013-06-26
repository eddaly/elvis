using UnityEngine;
using System.Collections;

public class Startup : MonoBehaviour {
	
	BatchedQuadDef play, persistentData;
	Rect playRect, persistentDataRect;
	
	Rect ScreenRect (BatchedQuadDef quad)
	{
		Vector3 wp0 = new Vector3 (quad.m_Position.x + -.5f * quad.m_Scale.x, quad.m_Position.y + -.5f * quad.m_Scale.y, 0);
		Vector3 wp1 = new Vector3 (wp0.x + quad.m_Scale.x, wp0.y + quad.m_Scale.y, 0);
		Vector3 sp0 = Camera.mainCamera.WorldToScreenPoint (wp0);
		Vector3 sp1 = Camera.mainCamera.WorldToScreenPoint (wp1);
		
		return new Rect (sp0.x, sp0.y, sp1.x - sp0.x, sp1.y - sp0.y);
	}

	// Use this for initialization
	void Start () {
	
		play = PrimitiveLibrary.Get.GetQuadDefinition (PrimitiveLibrary.QuadBatch.FRONTEND_BATCH);
		play.m_TextureIdx = (int)TextureAtlas.Frontend.NUM_1;		
		play.m_Colour = Color.red;
		play.m_Position.x = 0f;
		play.m_Position.y = 1f;
		play.m_Position.z = 0f;
		play.m_Scale.x = 2.0f;
		play.m_Scale.y = 1.0f;		
		playRect = ScreenRect (play);
	
		persistentData = PrimitiveLibrary.Get.GetQuadDefinition (PrimitiveLibrary.QuadBatch.FRONTEND_BATCH);
		persistentData.m_TextureIdx = (int)TextureAtlas.Frontend.NUM_2;		
		persistentData.m_Colour = Color.green;
		persistentData.m_Position.x = 0f;
		persistentData.m_Position.y = -1f;
		persistentData.m_Position.z = 0f;
		persistentData.m_Scale.x = 1.0f;
		persistentData.m_Scale.y = 1.0f;
		persistentDataRect = ScreenRect (persistentData);
	}
	
	// Update is called once per frame
	void Update () {
		
		//for (int n = 0; n < Input.touchCount; n++)
		{
			//if (Input.GetTouch(n).phase == TouchPhase.Ended)
			if (Input.GetMouseButtonUp (0))
			{
				Vector2 p = Input.mousePosition; //Input.GetTouch(n).position;//
				if (playRect.Contains (p))
				{
					Debug.Log ("PLAY");
				}
				else if (persistentDataRect.Contains (p))
				{
					Debug.Log ("PERSISTENT DATA");
				}
				//break;
			}
		}
	}
}
