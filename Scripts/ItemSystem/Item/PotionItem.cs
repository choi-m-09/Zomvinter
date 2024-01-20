using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 수량 아이템 - 포션 아이템 </summary>
public class PotionItem : CountableItem
{
    public PotionItemData PotionData;
    public PotionItem(PotionItemData data, int amount = 1) : base(data, amount) 
    {
        PotionData = data;
    }

    public bool Use()
    {
        // 임시 : 개수 하나 감소
        Amount--;

        return true;
    }

    /* --------------------------------------------------------------------------------- */

    protected override CountableItem Clone(int amount)
    {
        return new PotionItem(CountableData as PotionItemData, amount);
    }
}
