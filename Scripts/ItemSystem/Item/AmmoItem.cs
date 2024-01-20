using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoItem : CountableItem
{
    public AmmoItemData AmmoData;
    public AmmoItem(AmmoItemData data, int amount = 1) : base(data, amount)
    {
        AmmoData = data;
    }
    protected override CountableItem Clone(int amount)
    {
        return new AmmoItem(CountableData as AmmoItemData, amount);
    }


    private Vector3 fireVector = Vector3.zero;


    private void OnEnable()
    {
        fireVector = this.transform.position;
    }

    public void Fire(float WeaponAP, float PlayerAP)
    {
        StartCoroutine(FireBullet(WeaponAP, PlayerAP));
    }


    /***********************************************************************
    *                               Corutine
    ***********************************************************************/
    #region �ڷ�ƾ
    public IEnumerator FireBullet(float WeaponAP, float PlayerAP)
    {
        while (true)
        {
            if (Physics.Raycast(this.transform.position, this.transform.forward, out RaycastHit hit, AmmoData.BulletSpeed * Time.deltaTime))
            {
                Vector3 pos = hit.transform.position - fireVector;

                // 1. Monster ���̾��� ������Ʈ�� �浹 �� ���
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Monster"))
                {
                    Instantiate(Resources.Load("Effect/WFX_BImpact SoftBody"), hit.point, Quaternion.LookRotation(-pos));
                    hit.transform.GetComponent<BattleSystem>().OnDamage(AmmoData.AmmoDamage);
                }
                // 2. Wood ���̾��� ������Ʈ�� �浹 �� ���
                else if(hit.transform.gameObject.layer == LayerMask.NameToLayer("Wood"))
                {
                    Instantiate(Resources.Load("Effect/WFX_BImpact Wood"), hit.point, Quaternion.LookRotation(-pos));
                }
                // 3. Concrete ���̾��� ������Ʈ�� �浹 �� ���
                else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Concrete"))
                {
                    Instantiate(Resources.Load("Effect/WFX_BImpact Concrete"), hit.point, Quaternion.LookRotation(-pos));
                }
                // 4. Metal ���̾��� ������Ʈ�� �浹 �� ���
                else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Metal"))
                {
                    Instantiate(Resources.Load("Effect/WFX_BImpact Metal"), hit.point, Quaternion.LookRotation(-pos));
                }
                // 5. Sand ���̾��� ������Ʈ�� �浹 �� ���
                else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Sand"))
                {
                    Instantiate(Resources.Load("Effect/WFX_BImpact Sand"), hit.point, Quaternion.LookRotation(-pos));
                }
                // 6. Dirt ���̾��� ������Ʈ�� �浹 �� ���
                else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Dirt"))
                {
                    Instantiate(Resources.Load("Effect/WFX_BImpact Dirt"), hit.point, Quaternion.LookRotation(-pos));
                }
                // 7. SoftBody ���̾��� ������Ʈ�� �浹 �� ���
                else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("SoftBody"))
                {
                    Instantiate(Resources.Load("Effect/WFX_BImpact SoftBody"), hit.point, Quaternion.LookRotation(-pos));
                }

                Destroy(this.gameObject);
                break;
            }

            this.transform.Translate(Vector3.forward * AmmoData.BulletSpeed * Time.deltaTime);

            yield return null;
        }
    }
    #endregion
}
