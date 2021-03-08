using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleControl : MonoBehaviour
{
    private float TotalTime;//ドラッグできる時間
    public Image GaugeCtrl;
    private float Ratio = 2.0f;//（最高時間）
    public bool circleflag;//circleの有無判別flag（時間0ならtrue）

    void Start()
    {
        GaugeCtrl = GetComponent<Image>();

        TotalTime = Ratio;
        GaugeCtrl.fillAmount = 1;

        GaugeCtrl.enabled = false;
        circleflag = false;
    }

    public void CircleControlRst()
    {
        TotalTime = Ratio;
        GaugeCtrl.fillAmount = 1;

        GaugeCtrl.enabled = false;
        circleflag = false;
    }
    public void TimeDecrease()//円形ゲージが減る
    {
        GaugeCtrl.enabled = true;
        TotalTime -= Time.deltaTime;

        if (TotalTime >0)
        {
        GaugeCtrl.fillAmount = TotalTime/Ratio;
        }
        else
        {
       GaugeCtrl.fillAmount = 0;
            circleflag = true;
        }
    }

    public void TimeIncrease()//円形ゲージが増える
    {
        if (TotalTime < Ratio)
        {
        TotalTime += Time.deltaTime;
        GaugeCtrl.fillAmount = TotalTime/Ratio;
        }
        else
        {
            GaugeCtrl.fillAmount = 1;
            TotalTime = Ratio;
            GaugeCtrl.enabled = false;
            circleflag = false;
        }
    }

    public void JumpTime()
    {
        if (TotalTime < Ratio)
        {
            if (TotalTime < 0.4f*Ratio)
            {
            TotalTime = 0.4f*Ratio;
            GaugeCtrl.fillAmount = TotalTime/Ratio;
            }
        }
        else
        {
            GaugeCtrl.fillAmount = 1;
            TotalTime = Ratio;
            GaugeCtrl.enabled = false;
            circleflag = false;
        }
    }
}
