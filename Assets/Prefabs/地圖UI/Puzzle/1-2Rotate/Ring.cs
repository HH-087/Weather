using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour
{
    //設定或取得物件的角度
    public float Angle { get { return this.transform.eulerAngles.z; } set { this.transform.eulerAngles = new Vector3(0, 0, value); } }

    private bool isDrag; //是否拖曳中
    private float preAngle; //開始拖曳前的角度
    private Vector3 mousePos; //開始拖曳前的滑鼠位置

    void Update()
    {
        if (isDrag)
        {
            //基本三角點
            Vector2 ringWorldPos = new Vector2(this.transform.position.x, this.transform.position.y);
            Vector2 startWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            //計算夾角
            float angle = Vector2.Angle(startWorldPos - ringWorldPos, mouseWorldPos - ringWorldPos);
            Vector3 cross = Vector3.Cross(startWorldPos - ringWorldPos, mouseWorldPos - ringWorldPos);
            if (cross.z > 0) angle = 360 - angle;

            //調整圖片角度
            this.transform.eulerAngles = new Vector3(0, 0, preAngle - angle);
        }
    }

    void OnMouseDown()
    {
        if (!this.gameObject.activeSelf) return; //如果物件沒有顯示就不動作

        isDrag = true;
        preAngle = this.transform.eulerAngles.z; //紀錄開始拖曳前的角度
        mousePos = Input.mousePosition; //紀錄開始拖曳前的滑鼠位置
    }

    void OnMouseUp()
    {
        if (!this.gameObject.activeSelf) return; //如果物件沒有顯示就不動作
        isDrag = false;
    }

    //環形圖片物件的顯示/關閉
    public void ActiveRing(bool active)
    {
        this.gameObject.SetActive(active);
        if (active) this.transform.eulerAngles = new Vector3(0, 0, Random.Range(0, 360)); //亂數調整角度
    }
}

