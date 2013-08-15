using UnityEngine;
using System.Collections;

public class EPSoundEvent : MonoBehaviour {

		
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public virtual void Play() {}
	
	public virtual void Play ( float volume, float pitch ) {}
	
	public virtual void PlayScheduled ( double delay ) {}
	
	public virtual void PlayScheduled ( float volume, float pitch, double delay ) {}
	
	public virtual void Play ( float volume, float pitch, double delay ) {}
	
	public virtual void Stop() {}
	
	public virtual void Pause() {}
	
	public virtual void Resume() {}
	
	public virtual void SetVolume( float volume ) {}
	
	public virtual void SetPitch( float pitch ) {}
	
	public virtual float GetVolume()
	{
		return 0.0f;
	}
	
	public virtual float GetEventVolume()
	{
		return 0.0f;
	}
	
	public virtual float GetPitch()
	{
		return 0.0f;
	}
	
	public virtual void SetFade( float endVol, float endPitch, double duration, bool isFadeOut ) {}
	
	public virtual void ApplyFade() {}
	
	public virtual bool IsPlaying()
	{
		return false;
	}
	
	public virtual double time()
	{
		return 0.0f;
	}
	
	public virtual void time( double time ) {}
}
