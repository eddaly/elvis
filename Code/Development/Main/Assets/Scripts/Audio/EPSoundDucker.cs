using UnityEngine;
using System.Collections;

public class EPSoundDucker : EPSoundEvent {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void Play ( float volume, float pitch )
	{
		EPSoundController.Get ().m_DuckingAmount = volume;
	}
}
