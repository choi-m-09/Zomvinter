using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientItem : CountableItem
{
    public IngredientItemData IngredientData;

    public int c_Amount;
    public IngredientItem(IngredientItemData data, int amount = 1) : base(data, amount) { }

    private void Awake()
    {
        base.CountableData = IngredientData;
        Amount += c_Amount;
    }
}
