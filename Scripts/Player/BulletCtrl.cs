using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : Character, BattleSystem
{
    public LayerMask collisionMask;
    public GameObject bullet;
    public float bulletSpeed;
    public float damage = 5.0f;
    public float RotRange = 0.001f;
    // Start is called before the first frame update
    void Start()
    {
        //GetComponent<Rigidbody>().AddForce(bullet.transform.forward * bulletSpeed);
        //StartCoroutine(bulletDestroy(3.0f));
        bulletSpeed = 500.0f;

        GetComponent<Transform>().parent = null;
    }
    void Update()
    {
        float moveDist = bulletSpeed * Time.deltaTime;
        CheckCollision(moveDist);
        FireStart(moveDist);
    }

    void FireStart(float moveDist)
    {
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //if (Physics.Raycast(ray, out RaycastHit hit, 9999.0f))
        //{
        //    Vector3 targetPos = new Vector3(hit.point.x - transform.position.x, 0f, hit.point.z - transform.position.z); //방향 구하기
        //    dir = targetPos.normalized; //방향 설정
        //}
        //transform.Translate(moveDist * dir, Space.World); //이동

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        //float rnd = Random.Range(-RotRange, RotRange);
        if (Physics.Raycast(ray, out hit, 9999, collisionMask))
        {

            Vector3 dir = new Vector3(hit.point.x - transform.position.x,
                hit.point.y - transform.position.y, hit.point.z - transform.position.z); //방향 구하기
            Debug.Log(dir);
            transform.rotation = Quaternion.LookRotation(dir); //방향 설정
                                                               //transform.Translate(rnd)

        }
        transform.Translate(Vector3.forward * Time.deltaTime);

    }
    void CheckCollision(float moveDist)
    {
        Ray ray = new Ray(transform.position, transform.forward);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, moveDist, collisionMask, QueryTriggerInteraction.Collide))
        {
            Debug.DrawRay(ray.origin, ray.direction * 10.0f, Color.blue, 0.1f);
            Debug.Log("hit");
            BattleSystem bs = hit.collider.GetComponent<BattleSystem>();
            Destroy(gameObject);
            if (bs != null) bs.OnDamage(damage);
        }
    }

    public void OnDamage(float Damage)
    {

    }
    public void OnCritDamage(float CritDamage)
    {

    }
    public bool IsLive()
    {
        return true;
    }

    IEnumerator bulletDestroy(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(bullet);
    }
}
