using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
//LJM
public class ZMoveController : Character
{
    /* ���� ���� -----------------------------------------------------------------------------------------------*/

    /* ���� �Լ� -----------------------------------------------------------------------------------------------*/
    void Start()
    {
       
    }

    void Update()
    {
       
    }

    /* �̼� �Լ� -----------------------------------------------------------------------------------------------*/

    /// <summary> �� ������ �Ӽ��� �°� ��ü�� Transform�� ��ġ�� �̵���Ű�� �ڷ�ƾ�� ���� </summary>
    /// <param name="Target">Transform ���� ��ġ�� �̵�</param>
    /// <param name="MoveSpeed"> Transform ��ü�� �̵� �ӵ�</param>
    /// <param name="AttRange"> this ��ü�� ���� ���� ���� </param>
    /// <param name="AttDelay"> this ��ü�� ���� ���� </param>
    /// <param name="AttSpeed"> �� ��� ���� </param>
    /// <param name="TurnSpeed">this ��ü�� ȸ�� �ӵ� </param>
    protected void MoveToPosition(Transform Target,float MoveSpeed, float AttRange, float AttDelay, float AttSpeed, float TurnSpeed)
    {
        //myAnim.SetBool("IsMoving", true);
        if (MoveRoutine != null) StopCoroutine(MoveRoutine);
        MoveRoutine = StartCoroutine(Chasing(Target.position, MoveSpeed, AttRange, AttDelay, AttSpeed));
        if (RotRoutine != null) StopCoroutine(RotRoutine);
        RotRoutine = StartCoroutine(Rotating(Target.position, TurnSpeed));
    }

    /* �̵� �ڷ�ƾ -----------------------------------------------------------------------------------------------*/

    Coroutine MoveRoutine = null;
    protected IEnumerator Chasing(Vector3 pos,float MoveSpeed, float AttackRange, float AttackDelay, float AttackSpeed)
    {
        Vector3 Dir = pos - this.transform.position;
        float Dist = Dir.magnitude;
        Dir.Normalize();
        float AttTime = AttackDelay;

        //While ���ǹ����� Aggro ���� ���� ����
        while (true)
        {
            //���� �Ÿ� ����
            if (Dist > AttackRange)
            {
                if (myAnim.GetBool("IsAttacking") == false)
                {
                    myAnim.SetBool("isMoving", true);
                    float delta = MoveSpeed * Time.deltaTime;
                    if (Dist < delta)
                    {
                        delta = Dist;
                    }
                    this.transform.Translate(Dir * delta, Space.World);
                    Dist -= delta;
                }
            }
            else
            {
                myAnim.SetBool("isMoving", false);
                if (myAnim.GetBool("IsAttacking") == false)
                {
                    AttTime += Time.deltaTime;
                    if (AttackDelay <= AttTime)
                    {
                        //Debug.Log(AttackTime);
                        /*if(���� ����)
                         * {
                         * ũ��Ƽ�� Ȯ�� Anim
                         * }
                         * else 
                         * {
                         * �Ϲ� ���� Ȯ�� Anim
                         * }
                         */

                        myAnim.SetTrigger("Attack");
                        AttTime = 0.0f;
                    }
                }
            }
            yield return null;
        }
    }

    protected void RoamToPosition(Vector3 pos, float MoveSpeed, float TurnSpeed, UnityAction done = null)
    {
        if (RoamRoutine != null) StopCoroutine(RoamRoutine);
        RoamRoutine = StartCoroutine(Roaming(pos, MoveSpeed, done));
        if (RotRoutine != null) StopCoroutine(RotRoutine);
        RotRoutine = StartCoroutine(Rotating(pos, TurnSpeed));
    }

    Coroutine RoamRoutine;

    protected IEnumerator Roaming(Vector3 pos, float MoveSpeed, UnityAction done)
    {
        Vector3 Dir = pos - this.transform.position;
        float Dist = Dir.magnitude;
        Dir.Normalize();
        myAnim.SetBool("isMoving", true);
        while(Dist > Mathf.Epsilon)
        {
            float delta = MoveSpeed * Time.deltaTime;
            if (Dist < delta)
            {
                delta = Dist;
            }
            this.transform.Translate(Dir * delta, Space.World);
            Dist -= delta;
            yield return null;
        }
        myAnim.SetBool("isMoving", false);
        RoamRoutine = null;
        done?.Invoke();
        yield return null;
    }

    /* ȸ�� �ڷ�ƾ -----------------------------------------------------------------------------------------------*/

    protected Coroutine RotRoutine = null;
    protected IEnumerator Rotating(Vector3 pos, float TurnSpeed)
    {
        //���� ���� ����
        Vector3 _Dir = (pos - this.transform.position).normalized;
        CalcAngle(this.transform.forward, _Dir, this.transform.right, out ROTATEDATA data);

        while (data.Angle > Mathf.Epsilon)
        {
            float delta = TurnSpeed * Time.deltaTime;
            delta = delta > data.Angle ? data.Angle : delta;

            this.transform.Rotate(Vector3.up * delta * data.Dir);

            data.Angle -= delta;
            yield return null;
        }
        RotRoutine = null;
    }
    /*-----------------------------------------------------------------------------------------------*/
}
