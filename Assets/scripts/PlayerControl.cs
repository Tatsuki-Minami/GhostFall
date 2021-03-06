using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    //Moveで使うVector3
    private Vector3 screenPoint;
    private Vector3 offset;//オフセット（中点とのズレ）
    private Vector3 beforePosition;
    private Vector3 currentPosition;

    //List（position）
    private List<Vector3> PlayerPosition = new List<Vector3>();
    private List<Vector3> NextInPlayerPosition = new List<Vector3>();
    private List<Vector3> NextOutPlayerPosition = new List<Vector3>();

    //各種flag
    private bool objectflag;//床に触れているか検出するflag
    private bool slidflag, moveflag;

    //Moveで使うfloat
    private float xtox, ytoy;//移動前、後のx,ｙの距離
    private float revercecount;//反転判定
    private float distance;//どれだけ移動したかの距離

    //RGBA
    private float r, g, b, alpha;

    private float ReleaseTime = 1f;//透明になれる時間

    private SpriteRenderer PlayerImage;
    private Rigidbody2D rb2d;
    private Collider2D slidcollider2d;

    //Moveplayer関数
    public bool nextflag;//playerのnextstage時flag
    public bool Inflag;//true=In,false=Out
    //子スクリプト
    private CircleControl CircleControl;
    private BoxControl BoxControl;
    private ParentManager ParentManager;
    private TextControl TextControl;

    // Start is called before the first frame update
    void Start()
    {
        PlayerImage = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
        slidcollider2d = GameObject.Find("SlidObject").GetComponent<Collider2D>();

        //子スクリプトと連携
        CircleControl = GameObject.Find("Circle").GetComponent<CircleControl>();
        BoxControl = GameObject.Find("Box").GetComponent<BoxControl>();
        ParentManager = GameObject.Find("ParentManager").GetComponent<ParentManager>();
        TextControl = GameObject.Find("TextScript").GetComponent<TextControl>();
        //RGBA初期設定
        r = PlayerImage.color.r;
        g = PlayerImage.color.g;
        b = PlayerImage.color.b;
        alpha = PlayerImage.color.a;

        //各種数値初期設定
        revercecount = 1;
        AddPositionList();
        AddNextPositionList();
        //flag初期設定
        objectflag = false;
        slidflag = false;
        moveflag = false;
        transform.position = PlayerPosition[ParentManager.stagenum];

    }
    // Update is called once per frame
    void Update()
    {//右クリックをした瞬間の処理
        if (Input.GetMouseButtonDown(1))
        {
            if (slidflag == false)
            {
                if (moveflag)//透明ON
                {
                    TextControl.SetStatus(2);
                    slidcollider2d.isTrigger = true;
                    alpha = 0.5f;
                    SetColor();
                    slidflag = true;
                }
            }
            else
            {
                TextControl.SetStatus(1);

                slidcollider2d.isTrigger = false;
                alpha = 1f;
                SetColor();
                ReleaseTime = 1f;
                slidflag = false;
            }
        }

        //画面外へ行った時の不自然なワープ解決
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction, 10f, 1);

        if (hit.collider&&JudgeAction())
        {
            if (hit.collider.name == "Wall"||hit.collider.name=="Object")
            {
                NotMove();
            }
        }
        {

        }

        //object(floor)に接しているかの判定
        if (objectflag)
        {
            CircleControl.TimeIncrease();
            BoxControl.DistanceIncrease();
        }

        //stage遷移時の処理
        if (nextflag)
        {
            NextMovePlayer();
        }

        if (slidflag)
        {
            ReleaseTime -= Time.deltaTime;
            if (ReleaseTime < 0)
            {
                TextControl.SetStatus(1);

                slidcollider2d.isTrigger = false;
                alpha = 1f;
                SetColor();
                ReleaseTime = 1f;
                slidflag = false;
            }
        }
    }

    //初期位置をリストでまとめている
    //未来的には外部ファイル（excelやメモ帳等）で入力できたら楽そう！
    //csvでできそう.forで最大添字文回して、、、できる（確信）時間あったら作る
    private void AddPositionList()
    {
        PlayerPosition.Add(new Vector3(-125f, -6f, 0));
        PlayerPosition.Add(new Vector3(-95f, -6f, 0));
        PlayerPosition.Add(new Vector3(-97f, 8.5f, 0));
    }

    //ステージ遷移のpostion格納
    //偶数番は戻る時の数字
    //奇数番は入った時の数字
    private void AddNextPositionList()
    {
        NextInPlayerPosition.Add(new Vector3(0, 0, 0));
        NextInPlayerPosition.Add(new Vector3(-99.6f, -6.1f, 0));
        NextInPlayerPosition.Add(new Vector3(-97f, 8.5f, 0));

        NextOutPlayerPosition.Add(new Vector3(-101f, -6.2f, 0));
        NextOutPlayerPosition.Add(new Vector3(-97f, 6.1f, 0));
    }

    //rst関数
    public void PlayerControlRst()
    {
        revercecount = 1;
        alpha = 1f;
        r = 1f;
        SetColor();
        NotMove();
        objectflag = false;
        transform.position = PlayerPosition[ParentManager.stagenum];
        if (transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }

        BoxControl.BoxControlRst();
        CircleControl.CircleControlRst();
    }

    //移動できない場合の処理
    private void NotMove()
    {
        if (slidflag == false)
        {
        TextControl.SetStatus(1);
        }

        r = 1f;
        SetColor();

        Debug.Log("hogenass");
        moveflag = false;
        Cursor.visible = true;
    }

    private bool JudgeAction()//MouseDownの条件が多くなったので、関数化して可視化しやすく
    {
        if (moveflag && CircleControl.circleflag == false && BoxControl.boxflag == false && nextflag == false && nextflag == false && slidflag == false)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //stage遷移時の処理
    private void NextMovePlayer()
    {
        moveflag = false;
        Cursor.lockState = CursorLockMode.Locked;
        if (Inflag)
        {
            transform.position = NextInPlayerPosition[ParentManager.stagenum];
        }
        else
        {
            transform.position = NextOutPlayerPosition[ParentManager.stagenum];
        }
        nextflag = false;
    }

    //player移動処理
    private void MovePlayer()
    {
        TextControl.SetStatus(0);

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
                TextControl.ReverceText();
                revercecount++;
            }
        }
        else if (xtox < 0)
        {
            if (Math.Abs(xtox) < 5 && revercecount % 2 != 0)//とんでもない飛び方防止
            {
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
                TextControl.ReverceText();
                revercecount++;
            }
        }
        Vector3 MovePosition = new Vector3(transform.position.x + xtox, transform.position.y + ytoy, transform.position.z);
        rb2d.MovePosition(currentPosition);//今回の最適解発見！ただし、相手もRb2Dを持ってる時はバグの元かも？敵を作る時はRb2dを持たせない設計に
                                           //rb2d.velocity = currentPosition;//代替案...ワープではなく、力を加える
                                           //transform.position = currentPosition;//壁貫通する（transformは数値の直接入力で移動をしているため？）
        beforePosition = currentPosition;
    }

    private void SetColor()
    {
        PlayerImage.color = new Color(g, r, b, alpha);
    }

    private void OnMouseDown()
    {
        if (moveflag == false)
        {
            moveflag = true;
            CircleControl.circleflag = false;
            BoxControl.boxflag = false;

            r = 0.5f;
            SetColor();
            this.screenPoint = Camera.main.WorldToScreenPoint(transform.position);
            this.offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
            //マウスと中心座標の距離を求めズレを無くす処理
            beforePosition = screenPoint + this.offset;
        }
        else
        {
            NotMove();
        }
    }

    private void OnMouseDrag()
    {
        if (JudgeAction())
        {
            MovePlayer();
        }
        else
        {
            NotMove();
        }
    }

    private void OnMouseUp()
    {
        if (JudgeAction())
        {
            NotMove();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Object")
        {
            objectflag = true;
        }
        else if (collision.gameObject.tag == "Jump")
        {
            NotMove();
            TextControl.SetStatus(3);
            CircleControl.JumpTime();
            BoxControl.JumpDistance();
        }
        else if (collision.gameObject.tag == "Slid")
        {
            objectflag = true;
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
        else if (collider.gameObject.tag == "OddNext")//stage遷移の処理
        {
            ParentManager.OddNextStage();
        }
        else if (collider.gameObject.tag == "EvenNext")
        {
            ParentManager.EvenNextStage();
        }
    }//istriggerに当たった瞬間の処理

    private void OnTriggerExit2D(Collider2D collider)
    {

    }
}

//メモ書き
//objectのtag検索で大文字小文字の間違いでミスをしてしまった。個人で作る上では意識しなくていいが、
//多人数で作る場合は意識して一字ずつ書くべき(どちらかに統一すべき）
//別の提案ができて、バックアップでスクリプトを保持して、別スクリプトを造ったとき、結合が面倒だった
//代案：テストスクリプトで実験して、成功したら本スクリプトに書く？
//flagのtrue,falseの向きが分かりにくい・・・
//チーム単位で迷惑をかける書き方は避けたい
//次からソースコードはpythonを意識して書く（ルールがあるから共通化しやすい）