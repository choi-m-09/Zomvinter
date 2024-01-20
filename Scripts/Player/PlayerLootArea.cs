using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLootArea : MonoBehaviour
{
    /// <summary> UI를 불러오기 위한 변수  </summary>
    [SerializeField]
    Canvas myCanvas;
    /// <summary> 내 인벤토리UI 스크립트 변수 </summary>
    [SerializeField]
    Inventory myInventory;
    InventoryUI _inventoryUI;

    public List<GameObject> LootableItems = new List<GameObject>();

    [SerializeField]
    private LayerMask ItemLayerMask;
    [SerializeField]
    private LayerMask LootableLayerMask;

    /// <summary> PickUp 팝업 UI </summary>
    PickUpUI myPickUpUI = null;
    GameObject InstPickupUI = null;

    /// <summary> Loot 팝업 UI </summary>

    /// <summary> 서칭 팝업 UI </summary>

    void Start()
    {
        myInventory = myCanvas.GetComponentInChildren<Inventory>();
        _inventoryUI = myCanvas.GetComponentInChildren<InventoryUI>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && LootableItems.Count != 0)
        {

            ItemData item = null;
            if (LootableItems[0].GetComponent<WeaponItem>() is WeaponItem)
            {
                item = LootableItems[0].GetComponent<WeaponItem>().WeaponData;
            }
            else if (LootableItems[0].GetComponent<ArmorItem>() is ArmorItem)
            {
                item = LootableItems[0].GetComponent<ArmorItem>().ArmorData;
            }
            else if (LootableItems[0].GetComponent<PotionItem>() is PotionItem)
            {
                item = LootableItems[0].GetComponent<PotionItem>().PotionData;
            }
            else if (LootableItems[0].GetComponent<IngredientItem>() is IngredientItem)
            {
                item = LootableItems[0].GetComponent<IngredientItem>().IngredientData;
            }
            int Index = myInventory.FindEmptySlotIndex(myInventory.Items, myInventory.Items.Count);
            if (Index != -1)
            {
                myInventory.Items[Index] = item;
                Destroy(LootableItems[0]);
                LootableItems.RemoveAt(0);
                Destroy(InstPickupUI);
            }
        }
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

    //IEnumerator SearchingObject(GameObject UI)
    //{
    //    while (UI.gameObject.GetComponent<Slider>().value < 1.0f) 
    //    {
    //        UI.gameObject.GetComponent<Slider>().value += Time.deltaTime;

    //        yield return null;
    //    }
    //    Destroy(UI);
    //    SearchingObj = null;
    //    ObjectUI = Instantiate(Resources.Load("UI/ItemTableUi"), GameObject.Find("Canvas").transform) as GameObject;
    //    yield return null;
    //}
}