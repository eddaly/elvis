using UnityEngine;
using System.Collections;

public class AudioUtils : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	// Coroutine to set volume over a given duration
	public IEnumerator SetVolume(AudioSource snd, float startVol, float endVol, float length)
	{
		float delta = 0.0f;
		float step = 0.02f;		//Frequency of volume updates
		
		while (delta < length)
		{
			snd.volume = delta / length * (endVol - startVol) + startVol;
			//Console.WriteLine("Ambience Volume: " + snd.volume);
        	yield return new WaitForSeconds(step);
			delta += step;
		}
		snd.volume = endVol;
		//Console.WriteLine("Ambience Volume: " + snd.volume);
	}
	
	public IEnumerator SetLPF(float startHz, float endHz, float length)
	{
		float delta = 0.0f;
		float step = 0.02f;
		
		while (delta < length)
		{
			GetComponent<AudioLowPassFilter>().cutoffFrequency = delta / length * (endHz - startHz) + startHz;
			//Console.WriteLine("Cutoff Frequency: " + GetComponent<AudioLowPassFilter>().cutoffFrequency);
			yield return new WaitForSeconds(step);
			delta += step;
		}
		GetComponent<AudioLowPassFilter>().cutoffFrequency = endHz;
	}
		
	public IEnumerator SetReverbLevel(float start, float end, float length)
	{
		float delta = 0.0f;
		float step = 0.02f;
		
		while (delta < length)
		{
			GetComponent<AudioReverbFilter>().reverbLevel = delta / length * (end - start) + start;
			yield return new WaitForSeconds(step);
			delta += step;
		}
		GetComponent<AudioReverbFilter>().reverbLevel = end;
	}
}
