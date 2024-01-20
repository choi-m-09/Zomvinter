using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MovableHeaderUI : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    /// <summary> 클릭 이벤트를 받을 UI </summary>
    [SerializeField]
    private Transform _Target; // 

    /// <summary> 클릭 시작 지점 </summary>
    private Vector2 _beginPoint;
    /// <summary> UI 위치 </summary>
    private Vector2 _moveBegin;

    /// <summary> 이동 대상 UI를 지정하지 않은 경우, 자동으로 부모로 초기화 </summary>
    private void Awake()
    {
        if (_Target == null)
            _Target = transform.parent;
    }

    /// <summary> 클릭 한 경우 마우스 클릭 지점과 UI위치값을 반환 </summary>
    // 드래그 시작 위치 지정
    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        _beginPoint = _Target.position;
        _moveBegin = eventData.position;
    }

    /// <summary> 마우스 커서 위치로 이동 </summary>
    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        _Target.position = _beginPoint + (eventData.position - _moveBegin);
    }
}
