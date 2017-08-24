
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    bool shakeOn = false;
    float shakeAmount = 0.1f;
    float shakeDuration = 0.1f;
    float stime = 0;
    Vector3 currentPosition;
    private static CameraShake instance;

    private void Awake()
    {
        GetCameraShakeSystem();
        currentPosition = Camera.main.transform.position;
        stime = 0;
    }

    public static CameraShake GetCameraShakeSystem()
    {
        if (!instance)
        {
            instance = GameObject.FindObjectOfType(typeof(CameraShake)) as CameraShake;
            if (!instance)
                Debug.LogError("No active CameraShake in the scene");
        }
        return instance;
    }

    public void CamShakeOn(float shakeAmount, float shakeDuration)
    {
        this.shakeAmount = shakeAmount;
        this.shakeDuration = shakeDuration;
        shakeOn = true;
        stime = 0;
    }

    public void OnCamShake()
    {
        if (shakeOn == true)
        {
            stime += Time.deltaTime;

            if (stime <= shakeDuration)
            {
                Camera.main.transform.position = currentPosition + Random.insideUnitSphere * shakeAmount;
            }
            else
            {
                stime = 0;
                shakeOn = false;
                Camera.main.transform.position = currentPosition;
            }
        }
    }

    private void Update()
    {
        OnCamShake();
    }
}
