using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : MonoBehaviour
{

    Vector3 lastEuler;
    Light spotlight;
    public float Noise;

    Vector3 noiseVec;

    void Start()
    {
        lastEuler = transform.eulerAngles;
        spotlight = GetComponentInChildren<Light>();
        spotlight.transform.SetParent(null);
        noiseVec = Vector3.zero;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
        spotlight.transform.position = transform.position;
        float noise = Mathf.PerlinNoise(Time.time, Time.time) * 10;
        Vector3 combinedNoise = (transform.right * noise) + (transform.up * noise);

        noiseVec = Vector3.Lerp(noiseVec, combinedNoise * Time.deltaTime, 3f * Time.deltaTime);

        spotlight.transform.forward = Vector3.Lerp(spotlight.transform.forward, transform.forward + noiseVec, 10f * Time.deltaTime);
        }
    }
}
