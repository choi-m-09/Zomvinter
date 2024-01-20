using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MovableHeaderUI : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    /// <summary> Ŭ�� �̺�Ʈ�� ���� UI </summary>
    [SerializeField]
    private Transform _Target; // 

    /// <summary> Ŭ�� ���� ���� </summary>
    private Vector2 _beginPoint;
    /// <summary> UI ��ġ </summary>
    private Vector2 _moveBegin;

    /// <summary> �̵� ��� UI�� �������� ���� ���, �ڵ����� �θ�� �ʱ�ȭ </summary>
    private void Awake()
    {
        if (_Target == null)
            _Target = transform.parent;
    }

    /// <summary> Ŭ�� �� ��� ���콺 Ŭ�� ������ UI��ġ���� ��ȯ </summary>
    // �巡�� ���� ��ġ ����
    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        _beginPoint = _Target.position;
        _moveBegin = eventData.position;
    }

    /// <summary> ���콺 Ŀ�� ��ġ�� �̵� </summary>
    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        _Target.position = _beginPoint + (eventData.position - _moveBegin);
    }
}
