using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{    
    Animator _anim = null;
    protected Animator myAnim
    {
        get
        {
            if (_anim == null)
            {
                _anim = GetComponent<Animator>();
                _anim = GetComponentInChildren<Animator>();
            }
            return _anim;
        }
    }
    
    /*-----------------------------------------------------------------------------------------------*/
    protected void Moving(Transform Base, ref Vector3 pos, float moveSpeed, Transform CamTr)
    {        
        float delta = moveSpeed * Time.deltaTime;
        
        if (pos.magnitude > 1.0f)
        {
            pos.Normalize();
        }
       
        bool IsRun = myAnim.GetBool("isRun");

        Base.Translate(pos * delta, CamTr);

        myAnim.SetFloat("pos.x", pos.x);
        myAnim.SetFloat("pos.z", pos.z);
    }

    protected void AnimMove(Transform Obj, Vector3 pos, float moveSpeed, Transform baseTrans)
    {
        float delta = moveSpeed * Time.deltaTime;

        myAnim.SetFloat("pos.x", pos.x);
        myAnim.SetFloat("pos.z", pos.z);

        if (pos.magnitude > 1.0f)
        {
            pos.Normalize();
        }

        Obj.Translate(pos * delta, Space.Self);
    }
    
    protected void Rotate(Transform RotatePoint, bool Aiming)
    {
        if (!Aiming)
        {
            CameraArm C_Rot = GameObject.Find("CameraArm").GetComponent<CameraArm>();
            RotatePoint.transform.rotation = C_Rot.transform.rotation;
        }
        else
        {
            Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            //���콺 ��ġ�� ī�޶��̸� �̿��� ī�޶󿡼� ��ũ���� ���� ���� ��ȯ
            Plane GroupPlane = new Plane(Vector3.up, Vector3.zero); // ���̾� ����ũ�� ������ ����
                                                                    //������ǥ�� �ϴ� ���⿡ ũ�Ⱑ 1�� ���Ϳ� ������ ����
            float rayLength;
            if (GroupPlane.Raycast(cameraRay, out rayLength)) //���̰� ���� �����ߴ��� �ľ�
            {
                Vector3 pointTolook = cameraRay.GetPoint(rayLength); //���̷����Ÿ��� ��ġ�� ��ȯ
                                                                     //transform.LookAt(new Vector3(pointTolook.x, transform.position.y, pointTolook.z));
                                                                     //������ ���� pointTolook ��ġ���� ĳ���Ͱ� ������
                Vector3 dir = pointTolook - this.transform.position; //���� ���ϱ�, ���� ���Ͱ� = ��ǥ���� - ���ۺ���
                dir.y = 0.0f;
                Quaternion rot = Quaternion.LookRotation(dir.normalized); //������ ���ʹϾ� �� ���ϱ�, ����Ƽ�� �� = ����Ƽ�� ���� ��(���� ����)

                RotatePoint.transform.rotation = rot; //���� ������
                                                      //Debug.DrawRay(cameraRay.origin, cameraRay.direction * 10.0f, Color.red, 0.1f);
                                                      // ?���� �ʱ�ȭ ��ų ���  ?�������� �� ��� ���� ���� �Ұ����� ?������ȯ�� �ε巴�� �ϴ� ���v
                                                      // ��!�ȴ� �ִϸ��̼� �߰�, �ȴ� �ִϸ��̼� ����� �� �̵��ӵ� �����ϱ�? �޸��� �̵��ӵ� �����ϱ�? 
                                                      // ��!��ҿ� �ȴ� �ִϸ��̼����� �Ҵ�, ����Ʈ Ű ������ �޸��� ����, �����߿� �޸��� ��Ȱ��ȭ ��Ű��
            }
        }
    }

    protected void BulletRotate(Transform RotatePoint)
    {
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        //���콺 ��ġ�� ī�޶��̸� �̿��� ī�޶󿡼� ��ũ���� ���� ���� ��ȯ
        Plane GroupPlane = new Plane(Vector3.up, Vector3.zero);
        //������ǥ�� �ϴ� ���⿡ ũ�Ⱑ 1�� ���Ϳ� ������ ����
        if (GroupPlane.Raycast(cameraRay, out float rayLength)) //���̰� ���� �����ߴ��� �ľ�
        {
            Vector3 pointTolook = cameraRay.GetPoint(rayLength); //���̷����Ÿ��� ��ġ�� ��ȯ
            /*transform.LookAt(new Vector3(pointTolook.x, transform.position.y, pointTolook.z));
            //������ ���� pointTolook ��ġ���� ĳ���Ͱ� ������*/
            Vector3 dir = pointTolook - this.transform.position; //���� ���ϱ�, ���� ���Ͱ� = ��ǥ���� - ���ۺ���
            Quaternion rot = Quaternion.LookRotation(dir.normalized); //������ ���ʹϾ� �� ���ϱ�, ����Ƽ�� �� = ����Ƽ�� ���� ��(���� ����)
            
            RotatePoint.transform.rotation = rot; //���� ������
            Debug.DrawRay(cameraRay.origin, cameraRay.direction * 10.0f, Color.red, 0.1f);
        }
    }
}
