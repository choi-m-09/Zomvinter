using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
//LJM
public abstract class ZMoveController : Character
{
    ZSensor _sensor = null;

    public ZSensor mySensor
    {
        get
        {
            if (_sensor == null)
            {
                _sensor = this.GetComponentInChildren<ZSensor>();
            }
            return _sensor;
        }
    }

    /// <summary> 좀비 상태 </summary>
    protected enum STATE
    {
        NONE, CREATE, ROAM, BATTLE, RUSH, DEAD
    }

    protected abstract void OnAttack();

    /// <param name="Target"> 목적지 위치 </param>
    /// <param name="MoveSpeed"> 이동 속도 </param>
    /// <param name="AttRange"> 공격 거리 </param>
    /// <param name="AttDelay"> 공격 딜레이 </param>
    /// <param name="AttSpeed"> 공격 속도 </param>
    /// <param name="TurnSpeed"> 회전 속도 </param>
    
    Coroutine MoveRoutine = null;
    Coroutine RoamRoutine;
    protected void MoveToPosition(Transform Target,float MoveSpeed, float AttRange, float AttDelay, float AttSpeed, float TurnSpeed)
    {
        //myAnim.SetBool("IsMoving", true);
        if (MoveRoutine != null) StopCoroutine(MoveRoutine);
        MoveRoutine = StartCoroutine(Chasing(Target.position, MoveSpeed, AttRange, AttDelay, AttSpeed));
        if (RotRoutine != null) StopCoroutine(RotRoutine);
        RotRoutine = StartCoroutine(Rotating(Target.position, TurnSpeed));
    }


    protected IEnumerator Chasing(Vector3 pos,float MoveSpeed, float AttackRange, float AttackDelay, float AttackSpeed)
    {
        Vector3 Dir = pos - this.transform.position;
        float Dist = Dir.magnitude;
        Dir.Normalize();
        float AttTime = AttackDelay;
        while (true)
        {
            // 공격 사거리 도달 전
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

    protected Coroutine RotRoutine = null;
    protected IEnumerator Rotating(Vector3 pos, float TurnSpeed)
    {
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

    /// <summary> 플레이어 미발견 시 로밍 텀을 두기 위한 코루틴 </summary>
    protected IEnumerator Waitting(float t, UnityAction done)
    {
        yield return new WaitForSeconds(t);
        done?.Invoke();
    }
    /*-----------------------------------------------------------------------------------------------*/
}
