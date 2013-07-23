using UnityEngine;
using System.Collections;

public class ObstacleKillBox : MonoBehaviour 
{
	public float size = 0.5f;
	
	void Start() 
	{
		// Could do something
	}
	
	void Update() 
	{
		GameObject.DestroyImmediate( gameObject );
	}
}
