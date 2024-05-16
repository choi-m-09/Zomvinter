using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Item : MonoBehaviour
{
    /// <summary> ������ �����͸� �ҷ��´� </summary>
    public ItemData Data { get; set; }
    /// <summary> �ҷ��� �����͸� Data�� ���� </summary>
    /// <param name="data"></param>
    public Item(ItemData data) => Data = data;
}