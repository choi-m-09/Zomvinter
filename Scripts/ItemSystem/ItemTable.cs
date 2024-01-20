using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemTable : MonoBehaviour
{
    [SerializeField]
    List<GameObject> LootableObject = new List<GameObject>();
    
    [SerializeField]
    List<ItemData> ItemDatas;

    public ItemData[] items;

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

    void Start()
    {
        items = new ItemData[4];
        for (int i = 0; i < 4; ++i)
        {
            SpawnItem(i);
        }
        Itemtable = Instantiate(Resources.Load("UI/ItemtableUI"), GameObject.Find("Canvas").transform) as GameObject;
        tableSlot = Itemtable.GetComponentsInChildren<TableSlot>();
        SetTableSlot();
        Itemtable.SetActive(false);
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
                    {
                        if (InstLootUI != null)
                        {
                            Destroy(InstLootUI);
                        }
                        if (SearchingObj == null && LootableObject != null)
                        {
                            SearchingObj = Instantiate(Resources.Load("UI/SearchingObj"), GameObject.Find("Canvas").transform) as GameObject;
                            StartCoroutine(SearchingObject(SearchingObj));
                        }
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

        switch(random)
        {
            case 0:
                items[i] = ItemDatas[0];
                break;
            case 1:
                items[i] = ItemDatas[1];
                break;
            case 2:
                items[i] = ItemDatas[2];
                break;
            case 3:
                items[i] = ItemDatas[3];
                break;
        }
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

    void SetTableSlot()
    {
        for(int i = 0; i < 4; ++i)
        {
            tableSlot[i].Data = items[i];
        }
    }
}
