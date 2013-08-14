//-----------------------------------------------------------------------------
// File: Collisions.cs
//
// Desc:	Shows the number of times the player has fatally collided with
//			something
//
// Copyright Echo Peak Ltd 2013
//-----------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

public class Collisions : MonoBehaviour 
{
	FontNumber m_fontNumber;
	
	int m_collisions;
	bool m_oldColliding;
	
	CriticallyDampedFloatSpring m_coins = new CriticallyDampedFloatSpring(); 
	
	void Start() 
	{
		m_fontNumber = gameObject.AddComponent<FontNumber>();
		
		m_fontNumber.m_Number = 0;
		m_fontNumber.m_DisplayLeadingDigits = false;
		m_fontNumber.m_NumDigits = 5;
		m_fontNumber.m_ScaleX = 100.0f;
		m_fontNumber.m_ScaleY = 100.0f;
		m_fontNumber.m_Spacing = 64.0f;
		m_fontNumber.m_Layer = gameObject.layer;
		m_fontNumber.m_Colour = new Color( 1.0f, 0.0f, 0.0f, 1.0f );
		
		m_fontNumber.InitialiseDigits();
		
		
		m_collisions = 0;
		m_oldColliding = false;
		
		m_coins.SetAll( 0.0f, 0.0f, 0.0f, 50.0f );
	}
	
	void Update() 
	{		
		bool newColliding = RL.m_Player.m_Colliding;
		
		if( newColliding && !m_oldColliding )
		{
			m_collisions++;
			m_fontNumber.m_Number = m_collisions;
		}
			
		m_oldColliding = newColliding;
		
		
		//	Using it for coins now
		m_coins.m_Target = RL.m_Player.m_Coins;
		m_coins.Update( GlobalData.Get.m_GlobalDTime );
		m_coins.Snap( 5.0f );
		
		m_fontNumber.m_Number = (int)(m_coins.m_Pos);
		
		float springDiff = m_coins.m_Target - m_coins.m_Pos;

		float fontScale = 100.0f + springDiff/2.0f;
		m_fontNumber.m_ScaleX = fontScale;
		m_fontNumber.m_ScaleY = fontScale;
	}
	
	void OnDestroy()
	{
		m_fontNumber.Release();
	}
}
