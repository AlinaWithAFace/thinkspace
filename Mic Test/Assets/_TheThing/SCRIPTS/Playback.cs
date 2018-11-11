using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class Playback : MonoBehaviour
{
	public AudioClip clip;
	public AudioSource src;
	
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void PlayClip()
	{
		if (clip.loadState == AudioDataLoadState.Loaded)
		{
			src.Play();
		}
	}
}
