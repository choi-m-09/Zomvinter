using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    Transform SpawnPos;
    int rnd;


    void Start()
    {
        StartCoroutine(Spawn());
    }
    // Update is called once per frame

    IEnumerator Spawn()
    {
        while(true)
        {
            rnd = Random.Range(0, 2);
            GameObject obj = Resources.Load("Enemy_Zombie_Normal 1") as GameObject;
            obj.GetComponent<ZMonster_Normal>().rnd = rnd;
            Instantiate(obj, this.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(10.0f);
        }
    }
}
