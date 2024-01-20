using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Item : MonoBehaviour
{
    /// <summary> 아이템 데이터를 불러온다 </summary>
    public ItemData Data { get; private set; }
    /// <summary> 불러온 데이터를 Data에 대입 </summary>
    /// <param name="data"></param>
    public Item(ItemData data) => Data = data;
}