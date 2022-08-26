using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using System;

public class HandleApiCalls : MonoBehaviour
{
    public TMP_Text text;
    AudioClip ac;
    [SerializeField] AudioSource audios;
    [SerializeField] string Url;

    public void GetState(string state)
    {
        StartCoroutine(GetRequest(state));
    }
    IEnumerator GetAudio(string url)
    {
        using (UnityWebRequest uad = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.WAV))
        {
            yield return uad.SendWebRequest();
            if (uad.result == UnityWebRequest.Result.ConnectionError || uad.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log("Connection error");
            }
            else
            {
                ac = DownloadHandlerAudioClip.GetContent(uad);
                audios.clip = ac;
                audios.Play();
            }

        }
    }
    
    IEnumerator GetRequest(string state)
    {
        WWWForm form = new WWWForm();
        form.AddField("customer_state",state);
        using UnityWebRequest uwr = UnityWebRequest.Post(Url, form);
        uwr.SetRequestHeader("X-I2CE-ENTERPRISE-ID", "dave_expo");
        uwr.SetRequestHeader("X-I2CE-USER-ID", "74710c52-42a5-3e65-b1f0-2dc39ebe42c2");
        uwr.SetRequestHeader("X-I2CE-API-KEY", "NzQ3MTBjNTItNDJhNS0zZTY1LWIxZjAtMmRjMzllYmU0MmMyMTYwNzIyMDY2NiAzNw__");
        uwr.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log("Connection error");
        }
        else
        {
           Root root = JsonUtility.FromJson<Root>(uwr.downloadHandler.text);
            text.text = root.placeholder;
            StartCoroutine(GetAudio(root.response_channels.voice));
        }
    }

}
