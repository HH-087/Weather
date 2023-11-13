using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour
{
    //�]�w�Ψ��o���󪺨���
    public float Angle { get { return this.transform.eulerAngles.z; } set { this.transform.eulerAngles = new Vector3(0, 0, value); } }

    private bool isDrag; //�O�_�즲��
    private float preAngle; //�}�l�즲�e������
    private Vector3 mousePos; //�}�l�즲�e���ƹ���m

    void Update()
    {
        if (isDrag)
        {
            //�򥻤T���I
            Vector2 ringWorldPos = new Vector2(this.transform.position.x, this.transform.position.y);
            Vector2 startWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            //�p�⧨��
            float angle = Vector2.Angle(startWorldPos - ringWorldPos, mouseWorldPos - ringWorldPos);
            Vector3 cross = Vector3.Cross(startWorldPos - ringWorldPos, mouseWorldPos - ringWorldPos);
            if (cross.z > 0) angle = 360 - angle;

            //�վ�Ϥ�����
            this.transform.eulerAngles = new Vector3(0, 0, preAngle - angle);
        }
    }

    void OnMouseDown()
    {
        if (!this.gameObject.activeSelf) return; //�p�G����S����ܴN���ʧ@

        isDrag = true;
        preAngle = this.transform.eulerAngles.z; //�����}�l�즲�e������
        mousePos = Input.mousePosition; //�����}�l�즲�e���ƹ���m
    }

    void OnMouseUp()
    {
        if (!this.gameObject.activeSelf) return; //�p�G����S����ܴN���ʧ@
        isDrag = false;
    }

    //���ιϤ��������/����
    public void ActiveRing(bool active)
    {
        this.gameObject.SetActive(active);
        if (active) this.transform.eulerAngles = new Vector3(0, 0, Random.Range(0, 360)); //�üƽվ㨤��
    }
}

