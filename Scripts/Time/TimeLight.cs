using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLight : MonoBehaviour
{
    public float SunRotSpeed; // 태양 회전량
    public GameObject NightLight; // 저녁 시야 오브젝트
    CameraArm ZoomDist;


    private float nightFogDensity = 0.13f; // 저녁 안개 수치 제한량
    public float FogDensity;
    public float currentFogDensity; // 현재 안개 수치
    public float Intensity; // 저녁 시야 밝기
    public float ZoomIntensity; // 카메라 줌에 따른 밝기 조정


    [SerializeField]
    private bool IsNight = default; // 밤 낮 구분
    // Start is called before the first frame update
    void Start()
    {
        currentFogDensity = Mathf.Epsilon;
        Intensity = 0.0f;
    }

    // Update is called once per frame
    void Update()
    { 
        Intensity = Mathf.Clamp(Intensity, 0.0f, 25.0f);
        NightLight.GetComponentInChildren<Light>().intensity = Intensity;
        this.transform.Rotate(Vector3.right, SunRotSpeed * Time.deltaTime);

        //this.transform.Rotate(Vector3.right, TimeScale * Time.deltaTime);

        /*if (time.Gametime >= 6.0f || time.Gametime < 20.0f)
        {
            Debug.Log("Moning");
            IsNight = false;
            this.gameObject.SetActive(true);
        }
        if(time.Gametime >= 20.0f || time.Gametime < 6.0f)
        {
            Debug.Log("Night");
            IsNight = true;
            this.gameObject.SetActive(false);
            this.transform.eulerAngles = new Vector3(-10.0f,-30.0f,0.0f);
        }
        */
        if(this.transform.eulerAngles.x >= 170.0f)
        {
            IsNight = true;
        }
        else if(this.transform.eulerAngles.x <= 2.0f)
        {
            IsNight = false;
            time.day++;
        }

        
        if(IsNight && Intensity < 35.0f)
        {
            Intensity += 2 * Time.deltaTime;
        }
        else if(!IsNight && Intensity > Mathf.Epsilon)
        {
            Intensity -= 2 * Time.deltaTime;
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
