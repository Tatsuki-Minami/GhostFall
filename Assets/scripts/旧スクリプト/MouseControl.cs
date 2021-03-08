using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MouseControl : MonoBehaviour
{
    private Vector3 screenPoint;
    private Vector3 offset;//オフセット（中点とのズレ）
    private Vector3 beforePosition;
    private Vector3 currentPosition;

    private List<Vector3> PlayerPosition = new List<Vector3>();

    private bool objectflag;//床に触れているか検出するflag
    private bool slidflag;

    private float xtox, ytoy;//移動前、後のx,ｙの距離
    private float revercecount;//反転判定
    private float distance;//どれだけ移動したかの距離


    //MoveCamera関数
    public bool MovePlayerflag;//playerのnextstage時の位置


    private CircleControl CircleControl;
    private BoxControl BoxControl;
    private ParentManager ParentManager;
    private Rigidbody2D rb2d;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        //子スクリプトと連携
        CircleControl = GameObject.Find("Circle").GetComponent<CircleControl>();
        BoxControl = GameObject.Find("Box").GetComponent<BoxControl>();
        ParentManager = GameObject.Find("ParentManager").GetComponent<ParentManager>();

        revercecount = 1;
        AddPositionList();
        //flag初期設定
        objectflag = false;
        slidflag = false;
        transform.position = PlayerPosition[ParentManager.stagenum];

    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            slidflag = true;  
        }
        else if (Input.GetMouseButtonUp(1))
        {
            slidflag = false;
        }
        if (objectflag)
        {
            CircleControl.TimeIncrease();
            BoxControl.DistanceIncrease();
        }
        if (MovePlayerflag)
        {
            NextMovePlayer();
        }
    }

    //初期位置をリストでまとめている
    //未来的には外部ファイル（excelやメモ帳等）で入力できたら楽そう！
    //csvでできそう.forで最大添字文回して、、、できる（確信）時間あったら作る
    private void AddPositionList()
    {
        PlayerPosition.Add(new Vector3(-126f, -6.3f,0));
        PlayerPosition.Add(new Vector3(-95f, -6f, 0));
        PlayerPosition.Add(new Vector3(-97f, 8.5f, 0));
    }

    public void MouseControlRst()
    {
        revercecount = 1;
        objectflag = false;
        transform.position = PlayerPosition[ParentManager.stagenum];
        if (transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }

        BoxControl.BoxControlRst();
        CircleControl.CircleControlRst();
    }

    private bool JudgeAction()//MouseDownの条件が多くなったので、関数化して可視化しやすく
    {
        if(CircleControl.circleflag == false && BoxControl.boxflag == false && MovePlayerflag == false&&MovePlayerflag==false&&slidflag==false)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void NextMovePlayer()
    {
        Debug.Log("hogee");
        transform.position = PlayerPosition[ParentManager.stagenum];
            MovePlayerflag = false;
    }//ステージ遷移関数

    void OnMouseDown()
    {
        {
            CircleControl.circleflag = false;
            BoxControl.boxflag = false;

            this.screenPoint = Camera.main.WorldToScreenPoint(transform.position);
            this.offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
            //マウスと中心座標の距離を求めズレを無くす処理
            beforePosition = screenPoint + this.offset;
        }
    }//マウスを押した瞬間の処理

    void OnMouseDrag()//マウスをドラッグしている間の処理
    {
        if (JudgeAction())
        {
            if (objectflag == false)
            {
                CircleControl.TimeDecrease();
            }

            Vector3 currentScreenPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
            currentPosition = currentScreenPoint + this.offset;//ここでオフセットを使う

            xtox = currentPosition.x - beforePosition.x;//前のx座標から今のx座標の差　距離ではなく、方向に重点を置く
            ytoy = currentPosition.y - beforePosition.y;

            distance = (float)Math.Sqrt(xtox * xtox + ytoy * ytoy);

            // ここは自動でキャラクターの向きを変える仕掛け
            //現在と未来のx軸正負で大体の向きは予測できる！
            //ちょっと長くて見にくい・・・unityは関数で制御しづらい側面が多い気がする？（使いこなせていないだけか・・・）
            //追記：local変数が少ないから制御しづらいのかもしれない。ついでにMain関数もない
            if (distance < 1 && objectflag == false)//とんでもない飛び防止
            {
                BoxControl.DistanceDecrease(distance);
            }

            if (xtox > 0)//差が0以上ということは右に進んでいる
            {
                if (Math.Abs(xtox) < 5 && revercecount % 2 == 0)//とんでもない飛び方防止
                {
                    transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
                    revercecount++;
                }
            }
            else if (xtox < 0)
            {
                if (Math.Abs(xtox) < 5 && revercecount % 2 != 0)//とんでもない飛び方防止
                {
                    transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
                    revercecount++;
                }
            }
            //rb2d.velocity = currentPosition;//代替案...ワープではなく、力を加える
            rb2d.MovePosition(currentPosition);//今回の最適解発見！ただし、相手もRb2Dを持ってる時はバグの元かも？敵を作る時はRb2dを持たせない設計に
            //transform.position = currentPosition;//壁貫通する（transformは数値の直接入力で移動をしているため？）
            beforePosition = currentPosition;
        }
    }

    void OnMouseUp()
    {
        CircleControl.circleflag = true;
        BoxControl.boxflag = true;
    }//マウスを上げた瞬間の処理

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Object")
        {
            objectflag = true;
        }
        else if (collision.gameObject.tag == "Jump")
        {
            CircleControl.JumpTime();
            BoxControl.JumpDistance();
        }
        else if (collision.gameObject.tag == "Slid")
        {
            objectflag = true;

            if (slidflag)
            {
                Debug.Log("hoge");
                collision.collider.isTrigger = true;
            }
        }
    }//当たった瞬間の処理

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Object")
        {
            objectflag = false;
        }
        else if (collision.gameObject.tag == "Slid")
        {
            objectflag = false;
        }
    }//当たり判定から抜ける瞬間の処理

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "OutObject")//playerが当たってはいけないモノの処理
        {
            ParentManager.StageReset();
        }
        else if (collider.gameObject.tag == "Next")//stage遷移の処理
        {
            Destroy(collider.gameObject);
            //ParentManager.NextStage();
        }
    }//istriggerに当たった瞬間の処理

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (slidflag)
        {
            collider.isTrigger = false;
        }
     }
}
//メモ書き
//objectのtag検索で大文字小文字の間違いでミスをしてしまった。個人で作る上では意識しなくていいが、
//多人数で作る場合は意識して一字ずつ書くべき(どちらかに統一すべき）
