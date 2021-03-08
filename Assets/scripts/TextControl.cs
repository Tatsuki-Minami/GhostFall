using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextControl : MonoBehaviour
{
    private List<string> Status = new List<string>();
    private float fadespeed=0.01f;
    private float time;
    private int beforenum=-1;
    private bool invisibleflag;

    private float r, g, b, alpha;
    [SerializeField]
    private TextMeshProUGUI PlayerStatus;

    Transform StatusTransform;
    // Start is called before the first frame update
    void Start()
    {
        invisibleflag = false;
        PlayerStatus.enabled = false;
        time = 0f;

        r = PlayerStatus.color.r;
        g = PlayerStatus.color.g;
        b = PlayerStatus.color.b;
        alpha = PlayerStatus.color.a;

        //statusの文字設定
        Status.Add("Catch");//num0
        Status.Add("Release");//num1
        Status.Add("Invisible");//num2
        Status.Add("Jump");//num3

        StatusTransform = GameObject.Find("StatusText").GetComponent<Transform>();
    }
    
    void Update()
    {
        if (PlayerStatus.enabled)
        {
            time += Time.deltaTime;
            //時間が1秒経過した時
            if (time > 0.5f)
            {
                StartFadeOut();
            }
        }
    }

    public void SetStatus(int num)
    {
        //透明→Release防止
        if (num == 1 && invisibleflag)
        {
            return;
        }
        else if(num==2)
        {
            invisibleflag = true;
        }
        else
        {
            invisibleflag = false;  
        }

        if (beforenum != num)
        {
            PlayerStatus.enabled = true;
            PlayerStatus.text = Status[num];

            time = 0f;
            beforenum = num;
        } 
    }

    public void ReverceText()
    {
        StatusTransform.localScale = new Vector3(StatusTransform.localScale.x * -1, StatusTransform.localScale.y, StatusTransform.localScale.z);
    }

    public void TextControlRst()
    {
        PlayerStatus.enabled = false;
        alpha = 1f;
        SetAlpha();
        time = 0f;

        if (StatusTransform.localScale.x < 0)
        {
            ReverceText();
        }
    }

    private void StartFadeOut()
    {
        alpha -= fadespeed;//alpha値を引く
        SetAlpha();
        if (alpha < 0)
        {
            PlayerStatus.enabled = false;
            alpha = 1f;
            SetAlpha();
            time = 0f;
        }
    }

    private void SetAlpha()
    {
        PlayerStatus.color = new Color(r, g, b, alpha);
    }
}
