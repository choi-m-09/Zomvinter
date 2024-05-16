using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLootArea : MonoBehaviour
{
    /// <summary> 캔버스 </summary>
    [SerializeField]
    Canvas myCanvas;

    public List<GameObject> LootableItems = new List<GameObject>();

    [SerializeField]
    private LayerMask ItemLayerMask;
    [SerializeField]
    private LayerMask LootableLayerMask;

    /// <summary> 아이템 PickUp UI </summary>
    PickUpUI myPickUpUI = null;
    GameObject InstPickupUI = null;

    public Item _item;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && LootableItems.Count != 0)
        {
            AddInventory(LootableItems[0].GetComponent<Item>());
            LootableItems[0].gameObject.SetActive(false);
            LootableItems.RemoveAt(0);
            Destroy(InstPickupUI);
        }
    }

    public void AddInventory(Item item)
    {
        if (item is EquipmentItem ei) Inventory._inventory.Add(ei);
        else if (item is CountableItem ci) Inventory._inventory.Add(ci);
    }
    /*-----------------------------------------------------------------------------------------------*/
    private void OnTriggerEnter(Collider other)
    {
        if ((ItemLayerMask & (1 << other.gameObject.layer)) != 0)
        {
            LootableItems.Add(other.gameObject);

            if (other.GetComponent<Rigidbody>() != null)
            {
                if (InstPickupUI == null) InstPickupUI = Instantiate(Resources.Load("UI/Popup_Pickup"), GameObject.Find("Canvas").transform) as GameObject;
                myPickUpUI = InstPickupUI.GetComponent<PickUpUI>();
                myPickUpUI.Initialize(other.GetComponent<Transform>().transform, 50.0f);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        LootableItems.Remove(other.gameObject);
        Destroy(InstPickupUI);
    }
}