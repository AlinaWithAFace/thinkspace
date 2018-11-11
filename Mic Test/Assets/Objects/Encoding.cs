using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;


public class Encoding : MonoBehaviour {
    public AudioClip source;
   
	// Use this for initialization
	void Start () {
        StartCoroutine(Upload());
        //Debug.Log(audio64);
    }

    IEnumerator Upload()
    {
		string path = "C:/Users/alina/AppData/LocalLow/DefaultCompany/Mic Test/sampleaudio.wav";
        byte[] audioBytes = File.ReadAllBytes(path);
        string audio64 = Convert.ToBase64String(audioBytes);
		
		BubbleObject bubble = new BubbleObject(audio64);
		Config config = new Config("LINEAR16", 44100, "en-US");
		string apiKey = "AIzaSyB-VFIcarjpbcmEAWljXIdPIzr1ZT35jdM";
		
		Response resp = new Response(bubble,config);
		
		string bodyJsonString = JsonUtility.ToJson(resp);
		
		
        string url = "https://patzingo.lib.id/getV2T@dev/";
		url = "https://speech.googleapis.com/v1/speech:recognize?key="+apiKey;
		Debug.Log(bodyJsonString);
		
		Debug.Log(url);
		
		var request = new UnityWebRequest(url, "POST");
		byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(bodyJsonString);
		request.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
		request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
		request.SetRequestHeader("Content-Type", "application/json");

		yield return request.Send();

		Debug.Log("Response: " + request.downloadHandler.text);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
