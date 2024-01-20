using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLight : MonoBehaviour
{
    public float SunRotSpeed; // �¾� ȸ����
    public GameObject NightLight; // ���� �þ� ������Ʈ
    CameraArm ZoomDist;


    private float nightFogDensity = 0.13f; // ���� �Ȱ� ��ġ ���ѷ�
    public float FogDensity;
    public float currentFogDensity; // ���� �Ȱ� ��ġ
    public float Intensity; // ���� �þ� ���
    public float ZoomIntensity; // ī�޶� �ܿ� ���� ��� ����


    [SerializeField]
    private bool IsNight = default; // �� �� ����
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
