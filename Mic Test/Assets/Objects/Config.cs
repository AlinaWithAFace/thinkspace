using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Config {
	public string encoding;
	public int sampleRateHertz;
	public string languageCode;
	
	public Config(string encoding, int sampleRateHertz, string languageCode){
		this.encoding = encoding;
		this.sampleRateHertz = sampleRateHertz;
		this.languageCode = languageCode;
	}
}
