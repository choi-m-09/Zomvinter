using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> ���� ������ - ���� ������ </summary>
public class PotionItem : CountableItem
{
    public PotionItemData PotionData;
    public PotionItem(PotionItemData data, int amount = 1) : base(data, amount) 
    {
        PotionData = data;
    }

    public bool Use()
    {
        // �ӽ� : ���� �ϳ� ����
        Amount--;

        return true;
    }

    /* --------------------------------------------------------------------------------- */

    protected override CountableItem Clone(int amount)
    {
        return new PotionItem(CountableData as PotionItemData, amount);
    }
}
