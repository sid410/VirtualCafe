using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRM;

public class AutoBlinkVRM : MonoBehaviour
{

    [SerializeField]
    float blinkTime = 0.1f;//目を瞑っている時間
    [SerializeField]
    float blinkInterval = 1.0f;//瞬きと瞬きの間の時間

    bool blinkEnabled = true;
    bool blinking = false;//true: 瞬き中,false: 瞬きしていないとき
    BlendShapePreset currentFace;
    VRMBlendShapeProxy proxy;
    float deltaTime;

    void Start()
    {
        proxy = GetComponent<VRMBlendShapeProxy>();

        //デフォルトの表情をセット
        currentFace = BlendShapePreset.Neutral;
        proxy.AccumulateValue(currentFace, 1);
        //Debug.Log("Test: Start");

        deltaTime = 0.0f;
    }


    void FixedUpdate()
    {
        //AutoBlink
        //ここから
        if (blinking) {//瞬きしている
            if (deltaTime > blinkTime)
            {
                deltaTime = 0.0f;
                blinking = false;
                //Debug.Log("She Blinked!");
            }
            else
            {
                proxy.AccumulateValue(BlendShapePreset.Blink, 1);
            }
        }
        else//瞬きしていない
        {
            if (deltaTime > blinkInterval)
            {
                deltaTime = 0.0f;
                blinking = true;
                blinkInterval = Random.Range(2, 6);//2-5秒の間でランダムで瞬き
            }
            else
            {
                proxy.AccumulateValue(BlendShapePreset.Blink, 0);
            }
        }
        deltaTime+= Time.deltaTime;
        //ここまで

        proxy.Apply();
    }
   
    public void Angry()
    {
        proxy.AccumulateValue(BlendShapePreset.Fun, 0);
        proxy.AccumulateValue(BlendShapePreset.Angry, 1);
        proxy.Apply();
    }
    public void Smile()
    {
        proxy.AccumulateValue(BlendShapePreset.Angry, 0);
        proxy.AccumulateValue(BlendShapePreset.Fun, 1);
        proxy.Apply();
    }
    public void FaceReset()
    {
        proxy.AccumulateValue(BlendShapePreset.Angry, 0);
        proxy.AccumulateValue(BlendShapePreset.Fun, 0);
        proxy.Apply();
    }

    //public void ChangeFace(BlendShapePreset preset = BlendShapePreset.Neutral, bool blink = false)
    //{
    //    blinkEnabled = blink;

    //    if (!blink)
    //    {
    //        StopCoroutine("AutoBlink"); //文字列で指定しないと止まらないので注意
    //        blinking = false;
    //        proxy.AccumulateValue(BlendShapePreset.Blink, 0);
    //    }

    //    proxy.AccumulateValue(currentFace, 0);  //今の表情を無効化する
    //    proxy.AccumulateValue(preset, 1);    //新しい表情をセットする

    //    currentFace = preset;
    //}

}