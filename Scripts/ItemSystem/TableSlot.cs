using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class TableSlot : MonoBehaviour, IPointerClickHandler
{ 
    public Item item;
    ItemData ItemData;
    public TMP_Text amount_text;

    Inventory _inventory;

    void Start()
    {
        if (item == null)
        {
            Destroy(this.gameObject);
            return;
        }
        ItemData = SetItemData(item);
        this.GetComponent<Image>().sprite = ItemData.ItemImage;

        if (item is CountableItem ci)
        {
            amount_text.text = ci.Amount.ToString();
        }
        else Destroy(amount_text.gameObject);
        _inventory = Canvas.FindObjectOfType<Inventory>();
    }

    public ItemData SetItemData(Item item)
    {
        if (item is GunItem gi) return gi.GunData;
        else if (item is AmmoItem am) return am.AmmoData;
        else if (item is ArmorItem ai) return ai.ArmorData;
        else if (item is PotionItem pi) return pi.PotionData;
        else if (item is IngredientItem in_i) return in_i.IngredientData;
        return null;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (item != null)
            {
                {
                    _inventory.Add(item);
                    Destroy(this.gameObject);
                }
            }
            
        }
    }
}
