using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientItem : CountableItem
{
    public IngredientItemData IngredientData;
    
    public IngredientItem(IngredientItemData data, int amount = 1) : base(data, amount)
    {
        IngredientData = data;
    }

    protected override CountableItem Clone(int amount)
    {
        return new IngredientItem(CountableData as IngredientItemData, amount);
    }
}
