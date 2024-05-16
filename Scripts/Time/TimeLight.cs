using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLight : MonoBehaviour
{
    private float nightFogDensity = 0.18f;

    [SerializeField] Light playerHeadLight;

    public float SunRotSpeed; // Direction Light 회전 속도

    public float FogDensity;
    public float currentFogDensity; // 현재 Fog 값

    public bool IsNight = false; // 저녁 체크

    // Start is called before the first frame update
    void Start()
    {
        currentFogDensity = Mathf.Epsilon;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentFogDensity > 0.1f) playerHeadLight.enabled = true;
        else playerHeadLight.enabled = false;

        this.transform.Rotate(Vector3.right, SunRotSpeed * Time.deltaTime);

        if(this.transform.eulerAngles.x >= 170.0f)
        {
            IsNight = true;
        }
        else if(this.transform.eulerAngles.x <= 2.0f)
        {
            IsNight = false;
            time.day++;
        }

        
        if (IsNight)
        {
            if (currentFogDensity <= nightFogDensity)
            {
                currentFogDensity += 0.01f * Time.deltaTime;

                RenderSettings.fogDensity = currentFogDensity;
            }
        }
        else
        {
            if (currentFogDensity >= Mathf.Epsilon)
            {
                currentFogDensity -= 0.01f * Time.deltaTime;
                RenderSettings.fogDensity = currentFogDensity;
            }
        }
        
        
    }
}
