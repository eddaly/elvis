//-----------------------------------------------------------------------------
// File: EntryScreen.cs
//
// Desc: 	Transitioning screen for the in-game to hide level starts and add
//			a click-through event to replays
//
// Copyright Echo Peak Ltd 2013
//-----------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

public class EntryScreen : MonoBehaviour 
{
	GameObject m_curtains;
	
	GameObject m_backPlane;
	GameObject m_elvis;
	GameObject m_ribbons;
	
	GameObject m_tap;
	GameObject m_tapGlow;
	
	GameObject[] m_letters = new GameObject[5];
	GameObject[] m_letterGlows = new GameObject[5];
	
	bool m_transitioningIn = false;
	
	float m_transitionValue = 1.0f;
	float m_oldTransitionValue = 1.0f;
	public float m_TransitionTarget = 1.0f;
	public float m_TransitionSpeed = 0.5f;
	
	float m_backPlaneY = -8000.0f;
	
	float m_cycleTime = 0.0f;
	
	void Start()
	{
		Transform foundTransform;		
		
		foundTransform = transform.FindChild( "Curtains" );
		if( foundTransform != null )
			m_curtains = foundTransform.gameObject;

		
		foundTransform = transform.FindChild( "Background" );
		if( foundTransform != null )
			m_backPlane = foundTransform.gameObject;
		
		foundTransform = transform.FindChild( "Elvis" );
		if( foundTransform != null )
			m_elvis = foundTransform.gameObject;
		
		foundTransform = transform.FindChild( "Ribbons" );
		if( foundTransform != null )
			m_ribbons = foundTransform.gameObject;

		
		foundTransform = transform.FindChild( "Tap" );
		if( foundTransform != null )
			m_tap = foundTransform.gameObject;
		
		foundTransform = transform.FindChild( "TapGlow" );
		if( foundTransform != null )
			m_tapGlow = foundTransform.gameObject;

		
		foundTransform = transform.FindChild( "LetterE" );
		if( foundTransform != null )
			m_letters[0] = foundTransform.gameObject;
		
		foundTransform = transform.FindChild( "LetterEGlow" );
		if( foundTransform != null )
			m_letterGlows[0] = foundTransform.gameObject;
		
		foundTransform = transform.FindChild( "LetterL" );
		if( foundTransform != null )
			m_letters[1] = foundTransform.gameObject;
		
		foundTransform = transform.FindChild( "LetterLGlow" );
		if( foundTransform != null )
			m_letterGlows[1] = foundTransform.gameObject;
		
		foundTransform = transform.FindChild( "LetterV" );
		if( foundTransform != null )
			m_letters[2] = foundTransform.gameObject;
		
		foundTransform = transform.FindChild( "LetterVGlow" );
		if( foundTransform != null )
			m_letterGlows[2] = foundTransform.gameObject;
		
		foundTransform = transform.FindChild( "LetterI" );
		if( foundTransform != null )
			m_letters[3] = foundTransform.gameObject;
		
		foundTransform = transform.FindChild( "LetterIGlow" );
		if( foundTransform != null )
			m_letterGlows[3] = foundTransform.gameObject;
		
		foundTransform = transform.FindChild( "LetterS" );
		if( foundTransform != null )
			m_letters[4] = foundTransform.gameObject;
		
		foundTransform = transform.FindChild( "LetterSGlow" );
		if( foundTransform != null )
			m_letterGlows[4] = foundTransform.gameObject;
		
		
		m_backPlaneY = -8000.0f;
		m_cycleTime = 0.0f;
		
		m_transitionValue = 0.0f;
		m_oldTransitionValue = 0.0f;
		m_TransitionTarget = 1.0f;
	}
	
	void Update() 
	{
		if( m_backPlane == null )
			return;

		if( m_transitionValue < m_TransitionTarget )
		{
			m_transitioningIn = true;
			
			m_transitionValue += GlobalData.Get.m_GlobalDTime*m_TransitionSpeed;
			if( m_transitionValue > m_TransitionTarget )
				m_transitionValue = m_TransitionTarget;
		}
		
		if( m_transitionValue > m_TransitionTarget )
		{
			m_transitioningIn = false;
			
			if( m_curtains.activeSelf )
				m_curtains.SetActive( false );
			
			m_transitionValue -= GlobalData.Get.m_GlobalDTime*m_TransitionSpeed;
			if( m_transitionValue < m_TransitionTarget )
				m_transitionValue = m_TransitionTarget;
		}
				
		
		//	See if we're transitioning, or if we've reached out target value
		float diff = Mathf.Abs( m_TransitionTarget - m_transitionValue );
		if( diff < 0.001f )
		{
			//	So we're not transitioning, see if we're off the screen or on it
			if( m_transitionValue < 0.01f )
			{
				//	We're off, see if we need to deactivate the renderers
				if( m_backPlane.activeSelf )
					deactivateAll();
			}
		}
		else
		{
			//	We are transitioning, see if we're trying to get on the screen
			if( m_TransitionTarget > 0.95f )
			{
				//	We're on, so see if we need to activate the renderers
				if( !m_backPlane.activeSelf )
					activateAll();
			}
		}
		
		//	If we're inactive then skip the updates
		if( m_backPlane.activeSelf )
		{
			if( m_transitioningIn )
				setTransitionIn();
			else
				setTransitionOut();
		}
		
		//	If we're active, then do the waiting animations
		if( m_transitionValue > 0.95f )
		{
			//	If we're just entering (or re-entering) the cycle phase, reset cycle time
			if( m_oldTransitionValue <= 0.95f )
				m_cycleTime = 0.0f;
			
			cycleAnimation();
		}
		
		m_oldTransitionValue = m_transitionValue;
	}
	
	float[] phaseStart = new float[5];
	float[] phaseEnd = new float[5];
	float[] phaseNormal = new float[5];
	
	void setTransitionOut()
	{
		setTransitionIn();
		
		//	Fade Ribbons out without sweep
		Vector3 scale = m_ribbons.transform.localScale;
		scale.x = 240.0f;
		m_ribbons.transform.localScale = scale;
		
		Vector3 localPosition = m_ribbons.transform.localPosition;
		localPosition.x = 1024.0f;
		m_ribbons.transform.localPosition = localPosition;
		
		Material ribbonMaterial = m_ribbons.renderer.material;
		ribbonMaterial.mainTextureScale = new Vector2( 1.0f, 1.0f );
		ribbonMaterial.mainTextureOffset = new Vector2( 0.0f, 0.0f );
		
		Material ribbonsMaterial = m_ribbons.renderer.sharedMaterial;
		ribbonsMaterial.color = new Color( 1.0f, 1.0f, 1.0f, phaseNormal[1] );
	}
	
	void setTransitionIn()
	{		
		//	Fade background in
		phaseStart[0] = 0.0f;
		phaseEnd[0] = 0.2f;
		
		//	Sweep ribbons left
		phaseStart[1] = 0.15f;
		phaseEnd[1] = 0.35f;
		
		//	Stab Elvis in up right
		phaseStart[2] = 0.3f;
		phaseEnd[2] = 0.5f;
		
		//	Shine letters in left to right
		phaseStart[3] = 0.4f;
		phaseEnd[3] = 0.7f;
		
		//	Stamp tap message in
		phaseStart[4] = 0.7f;
		phaseEnd[4] = 1.0f;
		
		
		for( int p = 0; p<5; p++ )
		{
			float normal = m_transitionValue - phaseStart[p];
			normal /= (phaseEnd[p] - phaseStart[p]);
			
			if( normal < 0.0f )		normal = 0.0f;
			if( normal > 1.0f )		normal = 1.0f;
			
			phaseNormal[p] = normal;
		}
		
		
		//	Fade background in
		Material backgroundMaterial = m_backPlane.renderer.sharedMaterial;
		backgroundMaterial.color = new Color( 1.0f, 1.0f, 1.0f, phaseNormal[0] );
		
		
		//	Sweep ribbons left
		Vector3 scale = m_ribbons.transform.localScale;
		scale.x = 240.0f*phaseNormal[1];
		m_ribbons.transform.localScale = scale;
		
		Vector3 localPosition = m_ribbons.transform.localPosition;
		localPosition.x = 2224.0f - 1200.0f*phaseNormal[1];
		m_ribbons.transform.localPosition = localPosition;
		
		Material ribbonMaterial = m_ribbons.renderer.material;
		ribbonMaterial.mainTextureScale = new Vector2( phaseNormal[1], 1.0f );
		ribbonMaterial.mainTextureOffset = new Vector2( -phaseNormal[1], 0.0f );
		
		Material ribbonsMaterial = m_ribbons.renderer.sharedMaterial;
		ribbonsMaterial.color = new Color( 1.0f, 1.0f, 1.0f, 1.0f );
		
		//	Stab Elvis in up right
		float stabNormal = phaseNormal[2];
		stabNormal = 1.0f - stabNormal;
		stabNormal *= stabNormal*stabNormal;
		stabNormal = 1.0f - stabNormal;
		
		localPosition = m_elvis.transform.localPosition;
		localPosition.x = -600.0f + 1280.0f*stabNormal;
		localPosition.y = -100.0f + 995.0f*stabNormal;
		m_elvis.transform.localPosition = localPosition;
		
		
		//	Shine letters in left to right
		for( int l = 0; l<5; l++ )
		{
			//	Stagger the normals
			float letterNormal = phaseNormal[3] - ((float)l)*0.15f;
			letterNormal /= 0.4f;
			
			if( letterNormal < 0.0f )	letterNormal = 0.0f;
			if( letterNormal > 1.0f )	letterNormal = 1.0f;
			
			Material letterMaterial = m_letters[l].renderer.sharedMaterial;
			letterMaterial.color = new Color( 1.0f, 1.0f, 1.0f, letterNormal );
			
			Material letterGlowMaterial = m_letterGlows[l].renderer.sharedMaterial;
			letterGlowMaterial.color = new Color( letterNormal, letterNormal, letterNormal, 1.0f );
		}
		
		
		//	Stamp tap message in
		stabNormal = phaseNormal[4]*1.5f;
		if( stabNormal > 1.0f )	stabNormal = 1.0f;
		
		stabNormal = 1.0f - stabNormal;
		stabNormal *= stabNormal*stabNormal;
		stabNormal = 1.0f - stabNormal;

		Material tapMaterial = m_tap.renderer.sharedMaterial;
		tapMaterial.color = new Color( 1.0f, 1.0f, 1.0f, stabNormal );
		
		float scaleNormal = stabNormal*1.5f;
		if( scaleNormal > 1.25f )	scaleNormal = 2.5f - scaleNormal;
		
		scale = m_tap.transform.localScale;
		scale.x = 200.0f - 100.0f*scaleNormal;
		scale.z = 30.0f - 15.0f*scaleNormal;
		m_tap.transform.localScale = scale;
		m_tapGlow.transform.localScale = scale;
				
		tapMaterial = m_tapGlow.renderer.sharedMaterial;
		tapMaterial.color = new Color( 0.0f, 0.0f, 0.0f, 1.0f );
	}
	
	void cycleAnimation()
	{			
		//	Shift the background gradient
		m_backPlaneY += 4000.0f*GlobalData.Get.m_GlobalDTime;
		
		if( m_backPlaneY > 2000.0f )
			m_backPlaneY -= 10000.0f;
		
		Vector3 backPos = m_backPlane.transform.localPosition;
		backPos.y = m_backPlaneY;
		m_backPlane.transform.localPosition = backPos;
		
		
		//	Pulse the tap request
		float pulser = Mathf.Sin( m_cycleTime*5.0f );
		pulser = Mathf.Abs( pulser*pulser*pulser );
		
		Vector3 scale = m_tap.transform.localScale;
		scale.x = 100.0f*( 1.0f + pulser*0.15f );
		scale.z = 15.0f*( 1.0f + pulser*0.075f );
		m_tap.transform.localScale = scale;
		m_tapGlow.transform.localScale = scale;
		
		Material tapMaterial = m_tapGlow.renderer.sharedMaterial;
		tapMaterial.color = new Color( pulser, pulser, pulser, 1.0f );
		
		m_cycleTime += GlobalData.Get.m_GlobalDTime;
	}
	
	void deactivateAll()
	{
		m_backPlane.SetActive( false );
		m_elvis.SetActive( false );
		m_ribbons.SetActive( false );
	
		m_tap.SetActive( false );
		m_tapGlow.SetActive( false );
		
		for( int l = 0; l<5; l++ )
		{
			m_letters[l].SetActive( false );
			m_letterGlows[l].SetActive( false );
		}	
	}
	
	void activateAll()
	{
		m_backPlane.SetActive( true );
		m_elvis.SetActive( true );
		m_ribbons.SetActive( true );
	
		m_tap.SetActive( true );
		m_tapGlow.SetActive( true );
		
		for( int l = 0; l<5; l++ )
		{
			m_letters[l].SetActive( true );
			m_letterGlows[l].SetActive( true );
		}	
	}
	
	void onDestroy()
	{
		if( m_backPlane == null )
			return;
		
		//	Set all materials back to default to avoid them continually changing on disk
		m_curtains.renderer.sharedMaterial.color = new Color( 1.0f, 1.0f, 1.0f, 1.0f );
	
		m_backPlane.renderer.sharedMaterial.color = new Color( 1.0f, 1.0f, 1.0f, 1.0f );
		m_elvis.renderer.sharedMaterial.color = new Color( 1.0f, 1.0f, 1.0f, 1.0f );
		m_ribbons.renderer.sharedMaterial.color = new Color( 1.0f, 1.0f, 1.0f, 1.0f );
	
		m_tap.renderer.sharedMaterial.color = new Color( 1.0f, 1.0f, 1.0f, 1.0f );
		m_tapGlow.renderer.sharedMaterial.color = new Color( 1.0f, 1.0f, 1.0f, 1.0f );
	
		for( int l = 0; l<5; l++ )
		{
			m_letters[l].renderer.sharedMaterial.color = new Color( 1.0f, 1.0f, 1.0f, 1.0f );
			m_letterGlows[l].renderer.sharedMaterial.color = new Color( 1.0f, 1.0f, 1.0f, 1.0f );
		}
	}
}
