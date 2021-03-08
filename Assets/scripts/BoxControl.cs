using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoxControl : MonoBehaviour
{
    // Start is called before the first frame update

    public float TotalDistance;//ドラッグできる距離
    private float Ratio=8.0f;//（最高距離）

    public Image BoxCtrl;

    public bool boxflag;//boxの有無判別flag（距離0ならtrue）

    void Start()
    {
        BoxCtrl = GetComponent<Image>();

        TotalDistance = Ratio;
        BoxCtrl.fillAmount = 1;

        BoxCtrl.enabled = false;
        boxflag = false;
    }

    public void BoxControlRst()
    {
        TotalDistance = Ratio;
        BoxCtrl.fillAmount = 1;

        BoxCtrl.enabled = false;
        boxflag = false;
    }

    public void DistanceDecrease(float distance)//ゲージが減る
    {
        BoxCtrl.enabled = true;
        TotalDistance -= distance;
        if (TotalDistance > 0)
        {
            BoxCtrl.fillAmount = TotalDistance / Ratio;
        }
        else
        {
            BoxCtrl.fillAmount = 0;
            boxflag = true;
        }
    }

    public void DistanceIncrease()//ゲージが増える
    {
        if (TotalDistance < Ratio)
        {
            TotalDistance += Time.deltaTime*Ratio;
            BoxCtrl.fillAmount = TotalDistance / Ratio;
        }
        else
        {
            BoxCtrl.fillAmount = 1;
            TotalDistance = Ratio;
            BoxCtrl.enabled = false;
            boxflag = false;
        }
    }

    public void JumpDistance()
    {
        if (TotalDistance < Ratio)
        {
            if (TotalDistance< 0.8f*Ratio)
            {
                TotalDistance = 0.8f*Ratio;
                BoxCtrl.fillAmount = TotalDistance/Ratio;
            }
        }
        else
        {
            BoxCtrl.fillAmount = 1;
            TotalDistance = Ratio;
            BoxCtrl.enabled = false;
            boxflag = false;
        }
    }
}
