using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class AmmoItem : CountableItem
{
    public AmmoItemData AmmoData;

    public int c_amount;

    public AmmoItem(AmmoItemData data, int amount = 1) : base(data, amount) { }

    private void Awake()
    {
        base.CountableData = AmmoData;
    }

    private void Update()
    {
        c_amount = Amount;
    }
}
