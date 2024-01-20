using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Roof_UShape_Control : MonoBehaviour
{
    public GameObject Roof;
    public LayerMask Player;
    private List<GameObject> PlayerCheck = new List<GameObject>();
    // Start is called before the first frame update
    private void OnTriggerStay(Collider other)
    {
        if ((Player & (1 << other.gameObject.layer)) != 0)
        {
            PlayerCheck.Add(other.gameObject);
            //Roof.SetActive(false);
            if (PlayerCheck.Count > 0)
            {
                Roof.SetActive(false);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Roof.SetActive(true);
    }
}
