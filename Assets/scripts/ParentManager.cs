using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//実質のGameManger
public class ParentManager : MonoBehaviour
{
    private PlayerControl PlayerControl;
    private FadeControl FadeControl;
    private CameraControl CameraControl;
    private TextControl TextControl;
    private bool onlyflag;
    public int stagenum;
    // Start is called before the first frame update
    void Start()
    {
        CameraControl = GameObject.Find("Main Camera").GetComponent<CameraControl>();
        PlayerControl = GameObject.Find("Player").GetComponent<PlayerControl>();
        FadeControl = GameObject.Find("FadeImage").GetComponent<FadeControl>();
        TextControl=GameObject.Find("TextScript").GetComponent<TextControl>();
        onlyflag = true;//フェードイン時に１回だけ処理してほしいがためのflag
    }

    void Update()
    {
        if (FadeControl.isFadein&&onlyflag)
        {
        PlayerControl.PlayerControlRst();
            onlyflag = false;
        }
        else if (FadeControl.isFadein != true && onlyflag != true)
        {
            onlyflag = true;
        }
    }

    public void StageReset()
    {
        FadeControl.FadeControlRst();
        TextControl.TextControlRst();
    }

    public void OddNextStage()
    {
        //1回だけ起きてほしい処理のため
        if (CameraControl.MoveCameraflag == false&&PlayerControl.nextflag==false)
        {
            if (stagenum % 2 == 1)
            {
                stagenum++;
                CameraControl.MoveCameraflag = true;
                PlayerControl.nextflag = true;
                PlayerControl.Inflag = true;
            }
            else
            {
                stagenum--;
                CameraControl.MoveCameraflag = true;
                PlayerControl.nextflag = true;
                PlayerControl.Inflag = false;
            }
        }
    }

    public void EvenNextStage()
    {
        //1回だけ起きてほしい処理のため
        if (CameraControl.MoveCameraflag == false && PlayerControl.nextflag == false)
        {
            if (stagenum % 2 == 0)
            {
                stagenum++;
                CameraControl.MoveCameraflag = true;
                PlayerControl.nextflag = true;
                PlayerControl.Inflag = true;
            }
            else
            {
                stagenum--;
                Debug.Log("stagenum="+stagenum);
                CameraControl.MoveCameraflag = true;
                PlayerControl.nextflag = true;
                PlayerControl.Inflag = false;
            }
        }
    }
}

//前プロジェクトのソースコードをVSで参照すると、ソリューションの読み込みで色々おかしくなった＼(^o^)／
//unityVer.でも読み込みがおかしくなることが多い（戒め）
//別プロジェクトを参照する時はVSCodeを使う,UnityVerは最新1手前がいい？
//GameManagerがGlobalで既に存在している判定（恐らく別プロジェクトとソリューションが結合？）気をつける・・・
 //1/24追記：SCRIPTフォルダの1階層上にGameManagerがあったああああ＼(^o^)／