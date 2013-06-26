using UnityEngine;
using System.Collections;

public class LevelAmbience : MonoBehaviour {

	public AudioSource m_LevelAmbience;
	public float m_IntroVolume;
	public float m_MainVolume;
	public float m_FadeInTime;
	public float m_FadeOutTime;
	public float m_IntroEnd;
	public float m_IntroFadeTime;
	public float m_LevelEnd;
	public float m_LevelEndTime;
	
	AudioUtils audioUtils;
	
	float m_lastUpdate = -0.1f;
	
	// Use this for initialization
	void Start () {
		audioUtils = GetComponent<AudioUtils>();
		
		// Start the ambience loop
		StartCoroutine(audioUtils.SetVolume(m_LevelAmbience, 0.0f, m_IntroVolume, m_FadeInTime));
		m_LevelAmbience.Play();
	}
	
	// Update is called once per frame
	void Update () {
		
		float now = GlobalData.Get.m_GlobalTime;
		
		// Fade the ambience from IntroVolume to MainVolume
		if (m_IntroEnd > m_lastUpdate && m_IntroEnd <= now && now - m_lastUpdate < 1.0f)
			{
				//Console.WriteLine("Intro Fade");
				StartCoroutine(audioUtils.SetVolume(m_LevelAmbience, m_IntroVolume, m_MainVolume, m_IntroFadeTime));
			}
		//*/
		
		// Fade the ambience out at the end, then stop it
		if (m_LevelEnd > m_lastUpdate && m_LevelEnd <= now && now - m_lastUpdate < 1.0f)
			{
				//Console.WriteLine("Ending Fade");
				StartCoroutine(audioUtils.SetVolume(m_LevelAmbience, m_MainVolume, 0.0f, m_LevelEndTime));
			}
		if (m_LevelEnd + m_LevelEndTime > m_lastUpdate && m_LevelEnd + m_LevelEndTime <= now && now - m_lastUpdate < 1.0f)
			{
				m_LevelAmbience.Stop();
			}
		
		
		m_lastUpdate = now;
	}
}
