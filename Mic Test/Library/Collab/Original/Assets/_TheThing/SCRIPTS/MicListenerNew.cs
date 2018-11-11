using UnityEngine;
using System.Collections;
using UnityEngine.Audio; // required for dealing with audiomixers
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using _TheThing.SCRIPTS;

[RequireComponent(typeof(AudioSource))]
public class MicListenerNew : MonoBehaviour
{
    //Written in part by Benjamin Outram

    //option to toggle the microphone listenter on startup or not
    public bool startMicOnStartup = true;

    public string filename = "sampleaudio";
    public bool saveToFile = true;

    //allows start and stop of listener at run time within the unity editor
    public bool stopMicrophoneListener = false;
    public bool startMicrophoneListener = false;

    private bool microphoneListenerOn = false;

    //public to allow temporary listening over the speakers if you want of the mic output
    //but internally it toggles the output sound to the speakers of the audiosource depending
    //on if the microphone listener is on or off
    public bool disableOutputSound = false;

    //an audio source also attached to the same object as this script is
    AudioSource src;

    //make an audio mixer from the "create" menu, then drag it into the public field on this script.
    //double click the audio mixer and next to the "groups" section, click the "+" icon to add a 
    //child to the master group, rename it to "microphone".  Then in the audio source, in the "output" option, 
    //select this child of the master you have just created.
    //go back to the audiomixer inspector window, and click the "microphone" you just created, then in the 
    //inspector window, right click "Volume" and select "Expose Volume (of Microphone)" to script,
    //then back in the audiomixer window, in the corner click "Exposed Parameters", click on the "MyExposedParameter"
    //and rename it to "Volume"
    public AudioMixer masterMixer;

    public SpawnThought spawner;


    float timeSinceRestart = 0;


    void Start()
    {
        //start the microphone listener
        if (startMicOnStartup)
        {
            RestartMicrophoneListener();
            StartMicrophoneListener();
        }
    }

    void Update()
    {
        //can use these variables that appear in the inspector, or can call the public functions directly from other scripts
        if (stopMicrophoneListener)
        {
            //StartCoroutine(StopMicrophoneListener());
            StopMicrophoneListener();
        }

        if (startMicrophoneListener)
        {
            //StartCoroutine(StartMicrophoneListener());
            StartMicrophoneListener();
        }

        //reset paramters to false because only want to execute once
        stopMicrophoneListener = false;
        startMicrophoneListener = false;

        //must run in update otherwise it doesnt seem to work
        MicrophoneIntoAudioSource(microphoneListenerOn);

        //can choose to unmute sound from inspector if desired
        DisableSound(!disableOutputSound);
    }


    //stops everything and returns audioclip to null
    public void StopMicrophoneListener()
    {
        //stop the microphone listener
        microphoneListenerOn = false;
        //reenable the master sound in mixer
        disableOutputSound = false;
        //remove mic from audiosource clip
        src.Stop();

        if (saveToFile)
        {
            SavWav.Save(filename, src.clip);
        }

        src.clip = null;

        Microphone.End(null);

        StartCoroutine(Upload());
        //yield return null;
    }

    IEnumerator Upload()
    {
        string path = "C:/Users/VIPVR/AppData/LocalLow/DefaultCompany/Mic Test/sampleaudio.wav";
        byte[] audioBytes = File.ReadAllBytes(path);
        string audio64 = Convert.ToBase64String(audioBytes);

        BubbleObject bubble = new BubbleObject(audio64);
        Config config = new Config("LINEAR16", 44100, "en-US");
        string apiKey = "AIzaSyB-VFIcarjpbcmEAWljXIdPIzr1ZT35jdM";

        Response resp = new Response(bubble, config);

        string bodyJsonString = JsonUtility.ToJson(resp);

        bodyJsonString = JsonUtility.ToJson(bubble);


        Debug.Log(bodyJsonString);

        string url = "https://patzingo.lib.id/getV2T@dev/";

        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.Send();

        string textResp = request.downloadHandler.text;
        MessageSystem.setMessage(textResp);
        Debug.Log("Response: " + textResp);
        spawner.SpawnTheThought(textResp);

        /*
        url = "https://speech.googleapis.com/v1/speech:recognize?key="+apiKey;
        Debug.Log(bodyJsonString);
        
        Debug.Log(url);
        
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.Send();

        string textResp = request.downloadHandler.text;
        Debug.Log("Response: " + textResp);
        
        url = "https://patzingo.lib.id/getV2T@dev/";
        
        request = new UnityWebRequest(url, "POST");
        bodyRaw = new System.Text.UTF8Encoding().GetBytes(textResp);
        request.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        
        yield return request.Send();
        textResp = request.downloadHandler.text;
        */
    }


    public void StartMicrophoneListener()
    {
        //start the microphone listener
        microphoneListenerOn = true;
        //disable sound output (dont want to hear mic input on the output!)
        disableOutputSound = true;
        //reset the audiosource
        RestartMicrophoneListener();
        //yield return null;
    }


    //controls whether the volume is on or off, use "off" for mic input (dont want to hear your own voice input!) 
    //and "on" for music input
    public void DisableSound(bool SoundOn)
    {
        float volume = 0;

        if (SoundOn)
        {
            volume = 0.0f;
        }
        else
        {
            volume = -80.0f;
        }

        masterMixer.SetFloat("Volume", volume);
    }


    // restart microphone removes the clip from the audiosource
    public void RestartMicrophoneListener()
    {
        src = GetComponent<AudioSource>();

        //remove any soundfile in the audiosource
        src.clip = null;

        timeSinceRestart = Time.time;
    }

    //puts the mic into the audiosource
    void MicrophoneIntoAudioSource(bool MicrophoneListenerOn)
    {
        if (MicrophoneListenerOn)
        {
            //pause a little before setting clip to avoid lag and bugginess
            if (Time.time - timeSinceRestart > 0.5f && !Microphone.IsRecording(null))
            {
                src.clip = Microphone.Start(null, true, 10, 44100);

                //wait until microphone position is found (?)
                while (!(Microphone.GetPosition(null) > 0))
                {
                }

                src.Play(); // Play the audio source
            }
        }
    }
}