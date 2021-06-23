using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageNormalize : MonoBehaviour
{

    float scrheight;
    float scrwidth;

    [SerializeField]
    float SWIDTH = 1920;
    [SerializeField]
    float SHEIGHT = 1080;

    public RectTransform scrSize;
    float ratio;
    float scaleRatio;
 
    //이미지 크기를 비율에 맞게 설정해주는 스크립트 SWIDTH : SHEIGHT 비율로 설정해준다.
    void Update()
    {
       
        scrwidth = Screen.width; //추후 게임매니저로 관리
        scrheight = Screen.height; //추후 게임매니저로 관리

        ratio = scrwidth / scrheight;

        if (ratio < 1.77f)
        {
            scaleRatio = scrwidth / SWIDTH;
            scrSize.localScale = new Vector3(scaleRatio, scaleRatio, scaleRatio);
            scrSize.localPosition = Vector3.zero;
            scrSize.sizeDelta = new Vector2(SWIDTH, SHEIGHT);
        }else
        {
            scaleRatio = scrheight / SHEIGHT;
            scrSize.localScale = new Vector3(scaleRatio, scaleRatio, scaleRatio);
            scrSize.localPosition = Vector3.zero;
            scrSize.sizeDelta = new Vector2(SWIDTH, SHEIGHT);

        }
    }
}
