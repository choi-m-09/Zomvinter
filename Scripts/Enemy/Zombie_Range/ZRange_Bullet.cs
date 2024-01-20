using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZRange_Bullet : MonoBehaviour
{ 
    public LayerMask layerMask;
    float BMoveSpeed;

    private void Start()
    {
        BMoveSpeed = 15.0f;
    }

    private void Update()
    {
        CheckCollision();
    }

    void CheckCollision()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, BMoveSpeed, layerMask, QueryTriggerInteraction.Collide))
        {
            Debug.Log("hit");
            BattleSystem bs = hit.collider.GetComponent<BattleSystem>();
            Destroy(this.gameObject);
            if (bs != null) bs.OnDamage(10.0f);
        }
    }
}
