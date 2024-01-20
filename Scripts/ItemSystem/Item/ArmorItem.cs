using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 장비 - 방어구 아이템 </summary>
public class ArmorItem : EquipmentItem
{
    /// <summary> ConsumableData로부터 가져온 정보를 Data에 저장 </summary>
    public ArmorItemData ArmorData { get; private set; }

    public ArmorItem(ArmorItemData data) : base(data)
    {
        ArmorData = data;
    }
}
