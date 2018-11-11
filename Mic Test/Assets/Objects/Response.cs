using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Response {
	public BubbleObject audio;
	public Config config;
	public Response(BubbleObject audio, Config config){
		this.audio = audio;
		this.config = config;
	}
	

}
