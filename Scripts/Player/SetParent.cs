using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetParent : MonoBehaviour
{
    public Transform parent;
    // Start is called before the first frame update
    void Start()
    {
        this.transform.parent = parent;
    }

    // Update is called once per frame
    void Update()
    {
         
    }
}
