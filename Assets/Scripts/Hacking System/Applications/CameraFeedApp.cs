using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraFeedApp : MonoBehaviour
{

    // public List<CCTVCamera> Cameras = new List<CCTVCamera>();
    // public int CurrentIndex;
    // public Camera PreviewCamera;
    // public RawImage AppFeed;
	// public Text cameraName;

    // RenderTexture rTexture;

    // void Start()
    // {
    //     PreviewCamera = new GameObject("Preview Camera").AddComponent<Camera>();
    //     PreviewCamera.enabled = false;
    //     rTexture = new RenderTexture(512, 512, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
    //     PreviewCamera.targetTexture = rTexture;
    //     AppFeed.texture = rTexture;
	// 	PreviewCamera.fieldOfView = 100;
    // }

    // public void Next()
    // {
    //     CurrentIndex = Mathf.Clamp(CurrentIndex + 1, 0, Cameras.Count - 1);
    // }

    // public void Back()
    // {
    //     CurrentIndex = Mathf.Clamp(CurrentIndex - 1, 0, Cameras.Count - 1);
    // }

    // public void First()
    // {
    //     CurrentIndex = 0;
    // }

    // public void Last()
    // {
    //     CurrentIndex = Cameras.Count - 1;
    // }

    // public void DCCamera()
    // {
    //     Cameras.RemoveAt(CurrentIndex);
    //     CurrentIndex = Mathf.Clamp(CurrentIndex, 0, Cameras.Count - 1);
    // }

    // void Update()
    // {
    //     if (CurrentIndex >= 0 && CurrentIndex < Cameras.Count)
    //     {
    //         if (Cameras[CurrentIndex] != null)
    //         {
    //             if (Cameras[CurrentIndex].enabled)
    //             {
    //                 PreviewCamera.transform.position = Cameras[CurrentIndex].CameraSettings.position;
    //                 PreviewCamera.transform.rotation = Cameras[CurrentIndex].CameraSettings.rotation;
    //                 AppFeed.texture = rTexture;
    //                 PreviewCamera.enabled = true;
	// 				cameraName.text = "Feed: " + Cameras[CurrentIndex].GetDeviceName();
    //             }
    //             else
    //             {
    //                 PreviewCamera.enabled = false;
    //                 AppFeed.texture = null;
    //             }
    //         }
    //         else
    //         {
    //             PreviewCamera.enabled = false;
    //             AppFeed.texture = null;
    //         }
    //     }
    // }

    // internal void AddFeed(CCTVCamera cCTVCamera)
    // {
    //     Cameras.Add(cCTVCamera);
    // }
}
