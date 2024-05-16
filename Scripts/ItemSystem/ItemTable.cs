using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ItemTable : MonoBehaviour
{
    [SerializeField]
    List<GameObject> LootableObject = new List<GameObject>();

    public Item[] items;
    public GameObject[] item_list;

    [SerializeField]
    private LayerMask LootableLayerMask;

    [SerializeField]
    TableSlot[] tableSlot;

    PickUpUI myLootUI = null;

    GameObject InstLootUI = null;

    GameObject SearchingObj = null;

    GameObject Itemtable = null;

    GameObject ObjectUI = null;


    // Start is called before the first frame update

    void Awake()
    {
        Itemtable = Instantiate(Resources.Load("UI/ItemTableUI"), GameObject.Find("Canvas").transform) as GameObject;
        tableSlot = Itemtable.GetComponentsInChildren<TableSlot>();
        Itemtable.SetActive(false);
        int ItemCount = Random.Range(1, 4);
        items = new Item[ItemCount];
        for (int i = 0; i < ItemCount; i++)
        {
            SpawnItem(i);
            if(items[i] != null) SetTableSlot(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && LootableObject.Count != 0)
        {
            if (Itemtable.activeSelf == false)
            {
                if (InstLootUI != null) Destroy(InstLootUI);
                if (SearchingObj == null && LootableObject != null)
                    if (ObjectUI == null)
                        if (InstLootUI != null)
                        {
                            Destroy(InstLootUI);
                        }
                        if (SearchingObj == null && LootableObject != null)
                        {
                            SearchingObj = Instantiate(Resources.Load("UI/SearchingObj"), GameObject.Find("Canvas").transform) as GameObject;
                            StartCoroutine(SearchingObject(SearchingObj));
                        }
                Destroy(ObjectUI);
                ObjectUI = null;
            }
            else
            {
                Itemtable.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((LootableLayerMask & (1 << other.gameObject.layer)) != 0)
        {
            LootableObject.Add(other.gameObject);

            if (InstLootUI == null) InstLootUI = Instantiate(Resources.Load("UI/Popup_Loot"), GameObject.Find("Canvas").transform) as GameObject;
            myLootUI = InstLootUI.GetComponent<PickUpUI>();
            myLootUI.Initialize(this.transform, 50.0f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        LootableObject.Remove(other.gameObject);
        Destroy(InstLootUI);
        Destroy(SearchingObj);
        InstLootUI = null;
        SearchingObj = null;
        Itemtable.SetActive(false);
    }

    void SpawnItem(int i)
    {
            int random = Random.Range(0, 3);

            switch (random)
            {
                case 0:
                    if (item_list[0] != null) items[i] = Instantiate(item_list[0].GetComponent<Item>());
                    break;
                case 1:
                    if (item_list[1] != null) items[i] = Instantiate(item_list[1].GetComponent<Item>());
                    break;
                case 2:
                    if (item_list[2] != null) items[i] = Instantiate(item_list[2].GetComponent<Item>());
                    break;
                case 3:
                    if (item_list[3] != null) items[i] = Instantiate(item_list[3].GetComponent<Item>());
                    break;
            }
            if (items[i] != null) items[i].gameObject.SetActive(false);
        
    }
    IEnumerator SearchingObject(GameObject obj)
    {
        /* �ִϸ��̼� ���� �� */
        while (obj.gameObject.GetComponent<Slider>().value < 1.0f)
        {
            obj.gameObject.GetComponent<Slider>().value += Time.deltaTime;
            yield return null;
        }
        Destroy(obj);
        SearchingObj = null;
        Itemtable.SetActive(true);
        Destroy(ObjectUI);
    }

    void SetTableSlot(int index)
    {
        if (items[index] is CountableItem ci)
        {
            ci.Amount = Random.Range(5, 30);
        }
        tableSlot[index].item = items[index];
    }
}
