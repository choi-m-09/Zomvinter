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

    [SerializeField]
    Player player;

    /***********************************************************************
    *                               Unity Events
    ***********************************************************************/
    #region ����Ƽ �̺�Ʈ
    void Start()
    {
        CrossHair = this.GetComponentInChildren<Image>().transform;
        player = GameObject.Find("Player").GetComponent<Player>();
        // Cursor.visible = false; // ���콺 Ŀ�� ���ֱ�
        if(isAimed)
        {
            this.GetComponentInChildren<Image>().sprite = AimedPoint;
        }
        else
        {
            this.GetComponentInChildren<Image>().sprite = Point;
        }
    }

    // Update is called once per frame
    void Update()
    {
        MousePos = Input.mousePosition;
        this.GetComponent<RectTransform>().position = MousePos;
        if (Input.GetMouseButton(1) && player.AimCheck)
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
            if (player.Reloading) CrossHair.Rotate(Vector3.forward, Space.Self);
        }
        else
        {
            this.GetComponentInChildren<Image>().sprite = Point;
        }
    }
    #endregion
}
