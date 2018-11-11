using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SendText : MonoBehaviour {
	public String message = "hi";

	public void pressButton () {
		StartCoroutine(Upload());
	}

	IEnumerator Upload () {
		string url = "https://agusakov.lib.id/sendtext@dev/";

		var request = new UnityWebRequest(url, "POST");
		byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(message);
		request.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
		request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();

		yield return request.Send();
	}
}
