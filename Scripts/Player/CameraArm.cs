using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraArm : MonoBehaviour
{
    [SerializeField]
    private float CameraMoveSpeed = 20.0f;

    public Transform Target;
    public Transform _playerCamera;

    #region ȸ�� ����
    /// <summary> Camera Arm ȸ�� ���� </summary>
    Quaternion TargetRot;
    /// <summary> Camera Arm ȸ�� �ӵ� </summary>
    public float RotSpeed;
    Coroutine RotRoutine = null;
    #endregion

    #region �� ����
    [SerializeField]
    /// <summary> ���� �� �Ÿ� </summary>
    private float ZoomDist = -5.0f;
    [SerializeField]
    /// <summary> �ּ�, �ִ� �� �Ÿ� </summary>
    private Vector2 ZoomRange;
    /// <summary> ��ǥ �� �Ÿ� </summary>
    private float TargetZoomDist = 0.0f;
    /// <summary> �� ��/�ƿ� �ӵ� </summary>
    public float ZoomSpeed = 10.0f;
    #endregion

    #region ���� ����
    /// <summary> �ð��� ���� ���� �� �Ÿ� </summary>
    private float LightZoomDist = 0.0f;
    /// <summary> �ð��� ���� ��ǥ �� �Ÿ� </summary>
    private float TargetLightZoomDist = 0.0f;
    #endregion

    /***********************************************************************
    *                               Unity Events
    ***********************************************************************/
    #region ����Ƽ �̺�Ʈ
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

        //ī�޶� �ܿ� ���� ���� �þ� ���ξƿ�
        // TargetLightZoomDist += -Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed;
        // TargetLightZoomDist = Mathf.Clamp(TargetLightZoomDist, -6.0f, Mathf.Epsilon);
        // LightZoomDist = Mathf.Lerp(LightZoomDist, TargetLightZoomDist, Time.deltaTime * ZoomSpeed);

        //���� �þ� ���ξƿ�
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
    #region Input �Լ�
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
        ///<summary> Zoom �Ÿ� �ּ�, �ִ밪 Clamp </summary>
        TargetZoomDist = Mathf.Clamp(TargetZoomDist, ZoomRange.x, ZoomRange.y);
        ///<summary> ���콺 �� ��ǲ </summary>
        TargetZoomDist += Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed;
        ///<summary> �� �Ÿ� ���� ���� Update </summary>
        ZoomDist = Mathf.Lerp(ZoomDist, TargetZoomDist, Time.deltaTime * ZoomSpeed);
        ///<summary> ī�޶� �Ÿ� ���� - Zoom </summary>
        _playerCamera.localPosition = new Vector3(0.0f, -ZoomDist, -0.5f);
    }
#endregion

/***********************************************************************
*                               Private Methods
***********************************************************************/
#region Private �Լ�
private void InitValue()
    {
        /// <summary> ���� �� �Ÿ� </summary>
        ZoomDist = -5.0f;
        /// <summary> �ּ�, �ִ� �� �Ÿ� </summary>
        ZoomRange.x = -10.0f;
        ZoomRange.y = -4.0f;
        /// <summary> ��ǥ �� �Ÿ� </summary>
        TargetZoomDist = 0.0f;
        /// <summary> �� ��/�ƿ� �ӵ� </summary>
        ZoomSpeed = 10.0f;

        /// <summary> ī�޶� ȸ�� �ӵ� </summary>
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
