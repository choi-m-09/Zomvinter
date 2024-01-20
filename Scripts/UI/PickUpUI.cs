using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpUI : MonoBehaviour
{
    /// <summary> UI가 Root의 Height 높이만큼 고정 되도록 코루틴에 인자를 전달 </summary>
    /// <param name="Root"> 고정 할 오브젝트의 Transform값 </param>
    /// <param name="Height"> 고정 할 오브젝트에서 UI가 출력 될 높이 </param>
    public void Initialize(Transform Root, float Height)
    {
        StartCoroutine(Following(Root, Height));
    }

    /// <summary> Root가 전달 되면 UI의 위치를 Root의 Height위치에 따라다니도록 반복 실행 </summary>
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