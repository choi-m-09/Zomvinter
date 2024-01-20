using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
//LJM
public class ZMoveController : Character
{
    /* 지역 변수 -----------------------------------------------------------------------------------------------*/

    /* 시작 함수 -----------------------------------------------------------------------------------------------*/
    void Start()
    {
       
    }

    void Update()
    {
       
    }

    /* 싱속 함수 -----------------------------------------------------------------------------------------------*/

    /// <summary> 각 인자의 속성에 맞게 객체를 Transform의 위치로 이동시키는 코루틴을 실행 </summary>
    /// <param name="Target">Transform 값의 위치로 이동</param>
    /// <param name="MoveSpeed"> Transform 객체의 이동 속도</param>
    /// <param name="AttRange"> this 객체의 공격 가능 범위 </param>
    /// <param name="AttDelay"> this 객체의 공격 간격 </param>
    /// <param name="AttSpeed"> 미 사용 인자 </param>
    /// <param name="TurnSpeed">this 객체의 회전 속도 </param>
    protected void MoveToPosition(Transform Target,float MoveSpeed, float AttRange, float AttDelay, float AttSpeed, float TurnSpeed)
    {
        //myAnim.SetBool("IsMoving", true);
        if (MoveRoutine != null) StopCoroutine(MoveRoutine);
        MoveRoutine = StartCoroutine(Chasing(Target.position, MoveSpeed, AttRange, AttDelay, AttSpeed));
        if (RotRoutine != null) StopCoroutine(RotRoutine);
        RotRoutine = StartCoroutine(Rotating(Target.position, TurnSpeed));
    }

    /* 이동 코루틴 -----------------------------------------------------------------------------------------------*/

    Coroutine MoveRoutine = null;
    protected IEnumerator Chasing(Vector3 pos,float MoveSpeed, float AttackRange, float AttackDelay, float AttackSpeed)
    {
        Vector3 Dir = pos - this.transform.position;
        float Dist = Dir.magnitude;
        Dir.Normalize();
        float AttTime = AttackDelay;

        //While 조건문으로 Aggro 해제 조건 삽입
        while (true)
        {
            //공격 거리 유지
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
                        /*if(랜덤 난수)
                         * {
                         * 크리티컬 확률 Anim
                         * }
                         * else 
                         * {
                         * 일반 공격 확률 Anim
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

    /* 회전 코루틴 -----------------------------------------------------------------------------------------------*/

    protected Coroutine RotRoutine = null;
    protected IEnumerator Rotating(Vector3 pos, float TurnSpeed)
    {
        //지점 방향 벡터
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
