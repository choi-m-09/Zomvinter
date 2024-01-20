using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraArm : MonoBehaviour
{
    [SerializeField]
    private float CameraMoveSpeed = 20.0f;

    public Transform Target;
    public Transform _playerCamera;
    public Transform _nightLight;

    #region 회전 영역
    /// <summary> Camera Arm 회전 벡터 </summary>
    Quaternion TargetRot;
    /// <summary> Camera Arm 회전 속도 </summary>
    public float RotSpeed;
    Coroutine RotRoutine = null;
    #endregion

    #region 줌 영역
    [SerializeField]
    /// <summary> 현재 줌 거리 </summary>
    private float ZoomDist = -5.0f;
    [SerializeField]
    /// <summary> 최소, 최대 줌 거리 </summary>
    private Vector2 ZoomRange;
    /// <summary> 목표 줌 거리 </summary>
    private float TargetZoomDist = 0.0f;
    /// <summary> 줌 인/아웃 속도 </summary>
    public float ZoomSpeed = 10.0f;
    #endregion

    #region 광원 영역
    /// <summary> 시간에 따른 현재 줌 거리 </summary>
    private float LightZoomDist = 0.0f;
    /// <summary> 시간에 따른 목표 줌 거리 </summary>
    private float TargetLightZoomDist = 0.0f;
    #endregion

    /***********************************************************************
    *                               Unity Events
    ***********************************************************************/
    #region 유니티 이벤트
    private void OnValidate()
    {
        InitComponent();
        InitValue();
    }

    private void Awake()
    {
        InitComponent();
        InitValue();
        this.transform.parent = null;
    }
    // Update is called once per frame
    void Update()
    {
        InputMethods();
        ZoomCamera();

        //카메라 줌에 따른 저녁 시야 줌인아웃
        // TargetLightZoomDist += -Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed;
        // TargetLightZoomDist = Mathf.Clamp(TargetLightZoomDist, -6.0f, Mathf.Epsilon);
        // LightZoomDist = Mathf.Lerp(LightZoomDist, TargetLightZoomDist, Time.deltaTime * ZoomSpeed);

        //저녁 시야 줌인아웃
        // _nightLight.localPosition = new Vector3(0.0f, 3.0f, LightZoomDist);
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, Target.position, Time.deltaTime * CameraMoveSpeed);
    }
    #endregion

    /***********************************************************************
    *                               Input Methods
    ***********************************************************************/
    #region Input 함수
    void InputMethods()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            //if (RotRoutine != null) StopCoroutine("Rotate");
            //RotRoutine = StartCoroutine(Rotate(-10.0f));
            RotateArm(-20.0f);
        }
        if (Input.GetKey(KeyCode.E))
        {
            //if (RotRoutine != null) StopCoroutine("Rotate");
            //RotRoutine = StartCoroutine(Rotate(10.0f));
            RotateArm(20.0f);
        }
    }

    void ZoomCamera()
    {
        ///<summary> Zoom 거리 최소, 최대값 Clamp </summary>
        TargetZoomDist = Mathf.Clamp(TargetZoomDist, ZoomRange.x, ZoomRange.y);
        ///<summary> 마우스 휠 인풋 </summary>
        TargetZoomDist += Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed;
        ///<summary> 줌 거리 선형 보간 Update </summary>
        ZoomDist = Mathf.Lerp(ZoomDist, TargetZoomDist, Time.deltaTime * ZoomSpeed);
        ///<summary> 카메라 거리 조절 - Zoom </summary>
        _playerCamera.localPosition = new Vector3(0.0f, -ZoomDist, -0.5f);
    }
#endregion

/***********************************************************************
*                               Private Methods
***********************************************************************/
#region Private 함수
private void InitValue()
    {
        /// <summary> 현재 줌 거리 </summary>
        ZoomDist = -5.0f;
        /// <summary> 최소, 최대 줌 거리 </summary>
        ZoomRange.x = -10.0f;
        ZoomRange.y = -4.0f;
        /// <summary> 목표 줌 거리 </summary>
        TargetZoomDist = 0.0f;
        /// <summary> 줌 인/아웃 속도 </summary>
        ZoomSpeed = 10.0f;

        /// <summary> 카메라 회전 속도 </summary>
        RotSpeed = 0.1f;
}
    private void InitComponent()
    {
        _playerCamera = GetComponentInChildren<Camera>().transform;
    }

    private void RotateArm(float Angle)
    {
        float delta = RotSpeed + Time.deltaTime;
        TargetRot = Quaternion.Slerp(transform.rotation, transform.rotation * Quaternion.Euler(0.0f, Angle, 0.0f), delta);
        this.transform.rotation = TargetRot;
    }
    #endregion
}
