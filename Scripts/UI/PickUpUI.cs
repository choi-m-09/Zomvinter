using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpUI : MonoBehaviour
{
    /// <summary> UI�� Root�� Height ���̸�ŭ ���� �ǵ��� �ڷ�ƾ�� ���ڸ� ���� </summary>
    /// <param name="Root"> ���� �� ������Ʈ�� Transform�� </param>
    /// <param name="Height"> ���� �� ������Ʈ���� UI�� ��� �� ���� </param>
    public void Initialize(Transform Root, float Height)
    {
        StartCoroutine(Following(Root, Height));
    }

    /// <summary> Root�� ���� �Ǹ� UI�� ��ġ�� Root�� Height��ġ�� ����ٴϵ��� �ݺ� ���� </summary>
    IEnumerator Following(Transform Root, float Height)
    {
        while (Root != null)
        {
            Vector3 pos = Camera.main.WorldToScreenPoint(Root.position);
            pos.y += Height;
            this.GetComponent<RectTransform>().position = pos;
            yield return null;
        }
    }
}