using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class _UICrosshair : MonoBehaviour
{
    [SerializeField]
    Sprite AimedPoint = null;
    [SerializeField]
    Sprite Point = null;

    bool isAimed = false;

    Vector2 MousePos = Vector2.zero;

    [SerializeField]
    Transform CrossHair;

    /***********************************************************************
    *                               Unity Events
    ***********************************************************************/
    #region 유니티 이벤트
    void Start()
    {
        CrossHair = this.GetComponentInChildren<Image>().transform;
        // Cursor.visible = false; // 마우스 커서 없애기
        if(isAimed)
        {
            this.GetComponent<Image>().sprite = AimedPoint;
        }
        else
        {
            this.GetComponent<Image>().sprite = Point;
        }
    }

    // Update is called once per frame
    void Update()
    {
        MousePos = Input.mousePosition;
        this.GetComponent<RectTransform>().position = MousePos;
        if (Input.GetMouseButtonDown(1))
        {
            isAimed = true;
        }
        if (Input.GetMouseButtonUp(1))
        {
            isAimed = false;
        }

        if (isAimed)
        {
            this.GetComponentInChildren<Image>().sprite = AimedPoint;
            CrossHair.Rotate(Vector3.forward, Space.Self);
        }
        else
        {
            this.GetComponentInChildren<Image>().sprite = Point;
        }
    }
    #endregion
}
