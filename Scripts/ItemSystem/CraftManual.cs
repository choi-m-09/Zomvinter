using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[Serializable]
public class DicNeedItem
{
    public GameObject ItemPrefabs;
    public string NeedName;
    public Sprite NeedImage;
    public int NeedAmount;
}

[Serializable]
public class Craft
{
    public string ItemName;
    public GameObject ItemPrefabs;
    public Sprite ItemImage;
    public DicNeedItem[] NeedItem;
}

public class CraftManual : MonoBehaviour
{
    public static int Total_IronAmount;

    public static int Total_CopperAmount;

    public Craft[] CraftList;

    [SerializeField] GameObject Contents;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && Inventory.is_craftWin)
        {
            Inventory.is_craftWin = false;
            GetComponent<RectTransform>().anchoredPosition = new Vector2(700, 2000);
        }
    }

    public void Possible_Craft(int index)
    {
        index -= 1;
        Debug.Log(index);
        if (index > 0) Contents.transform.GetChild(index).gameObject.SetActive(true);
    }


    public void Make_AXE()
    {
        if (Total_IronAmount >= CraftList[0].NeedItem[0].NeedAmount)
        {
            GameObject source = Instantiate(CraftList[0].NeedItem[0].ItemPrefabs);
            source.SetActive(false);
            Inventory._inventory.Count_Ingredient(source.gameObject.GetComponent<IngredientItem>().IngredientData, CraftList[0].NeedItem[0].NeedAmount);
            Destroy(source);
            Item make_go = Instantiate(CraftList[0].ItemPrefabs.GetComponent<Item>());
            Inventory._inventory.Add(make_go);
            make_go.gameObject.SetActive(false);
        }
    }

    public void Make_AKM()
    {
        if (Total_IronAmount >= CraftList[1].NeedItem[0].NeedAmount)
        {
            GameObject source = Instantiate(CraftList[1].NeedItem[0].ItemPrefabs);
            source.SetActive(false);
            Inventory._inventory.Count_Ingredient(source.gameObject.GetComponent<IngredientItem>().IngredientData, CraftList[1].NeedItem[0].NeedAmount);
            Destroy(source);
            Item make_go = Instantiate(CraftList[1].ItemPrefabs.GetComponent<Item>());
            Inventory._inventory.Add(make_go);
            make_go.gameObject.SetActive(false);
        }
    }

    public void Make_7mm()
    {
        if (Total_IronAmount >= CraftList[2].NeedItem[0].NeedAmount && Total_CopperAmount >= CraftList[2].NeedItem[1].NeedAmount)
        {
            GameObject source1 = Instantiate(CraftList[2].NeedItem[0].ItemPrefabs);
            GameObject source2 = Instantiate(CraftList[2].NeedItem[1].ItemPrefabs);
            source1.SetActive(false);
            source2.SetActive(false);
            Inventory._inventory.Count_Ingredient(source1.gameObject.GetComponent<IngredientItem>().IngredientData, CraftList[2].NeedItem[0].NeedAmount);
            Inventory._inventory.Count_Ingredient(source2.gameObject.GetComponent<IngredientItem>().IngredientData, CraftList[2].NeedItem[1].NeedAmount);
            Destroy(source1);
            Destroy(source2);
            Item make_go = Instantiate(CraftList[2].ItemPrefabs.GetComponent<Item>());
            AmmoItem ai = make_go as AmmoItem;
            ai.Amount = 30;
            Inventory._inventory.Add(ai);
            make_go.gameObject.SetActive(false);
        }
    }
}
