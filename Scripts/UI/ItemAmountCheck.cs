using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemAmountCheck : MonoBehaviour
{
    TMP_Text amountText;
    
    int Amount;

    [SerializeField] int needAmount;
    
    enum TYPE
    {
        Iron, Copper
    }

    [SerializeField] TYPE type;
    // Start is called before the first frame update
    void Start()
    {
        amountText = GetComponentInChildren<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        Set_Text();
    }

    void Set_Text()
    {
        switch(type)
        {
            case TYPE.Iron:
                amountText.text = CraftManual.Total_IronAmount.ToString() + " / " + needAmount.ToString();
                Amount = CraftManual.Total_IronAmount;
                break;
            case TYPE.Copper:
                amountText.text = CraftManual.Total_CopperAmount.ToString() + " / " + needAmount.ToString();
                Amount = CraftManual.Total_CopperAmount;
                break;
        }
        Change_Color();
    }
    void Change_Color()
    {
        if (Amount >= needAmount)
        {
            if (amountText.color != Color.green)
            {
                amountText.color = Color.green;
            }
            else return;
        }
        else
        {
            if (amountText.color != Color.red)
            {
                amountText.color = Color.red;
            }
            else return;
        }
    }
}
