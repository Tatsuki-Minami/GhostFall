using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeControl : MonoBehaviour
{

    private float fadespeed = 0.05f;//暗転する時間
    private bool isFadeout;//フェードアウト,インの処理発動flag
    public bool isFadein;

    private float r, g, b, alpha;
    private Image FadeImage;

    void Start()
    {

        FadeImage = GetComponent<Image>();
        FadeImage.enabled = false;

        //FadeImageの初期設定
        r = FadeImage.color.r;
        g = FadeImage.color.g;
        b = FadeImage.color.b;
        alpha = FadeImage.color.a;

        //flag初期設定
        isFadein = false;
        isFadeout = false;
    }
    

    private void Update()
    {
        if (isFadeout)
        {
            StartFadeOut();
        }
        else if (isFadein)
        {
            StartFadeIn();
        }
    }

    public void FadeControlRst()
    {
        isFadeout = true;
    }

    void StartFadeIn()
    {
        FadeImage.enabled = true;
        alpha -= fadespeed;//alpha値を引く
        SetAlpha();
        if (alpha <= 0)
        {
            isFadein = false;
            FadeImage.enabled = false;
        }
    }

    void StartFadeOut()
    {
        FadeImage.enabled = true;
        alpha += fadespeed;//alpha値に足していく
        SetAlpha();
        if (alpha >= 1)
        {
            isFadeout = false;
            isFadein = true;
        }
    }

    void SetAlpha()
    {
        FadeImage.color = new Color(r, g, b, alpha);
    }
}
