using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterBoxSize : MonoBehaviour
{

    float scrheight;
    float scrwidth;


    public RectTransform leftLetterBox;
    public RectTransform rightLetterBox;
    public RectTransform topLetterBox;
    public RectTransform botLetterBox;


    //추후 const로 수정
    [SerializeField]
    float SWIDTH = 1920;
    [SerializeField]
    float SHEIGHT = 1080;

    float ratio;


    public static LetterBoxSize instance;

    //레터박스를 생성해주는 스크립트. 테스트를 위해 Update문에서 사용하지만 추후 Start부분으로 변경해준다.
    void Start()
    {
        if(instance !=null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        /*
            scrwidth = Screen.width; //추후 게임매니저로 관리
            scrheight = Screen.height; //추후 게임매니저로 관리

            ratio = scrwidth / scrheight;

            if (ratio < 1.77f)
            {
                topLetterBox.gameObject.SetActive(true);
                botLetterBox.gameObject.SetActive(true);
                leftLetterBox.gameObject.SetActive(false);
                rightLetterBox.gameObject.SetActive(false);

                topLetterBox.sizeDelta = new Vector2(scrwidth, scrheight - SHEIGHT * scrwidth / SWIDTH);
                botLetterBox.sizeDelta = new Vector2(scrwidth, scrheight - SHEIGHT * scrwidth / SWIDTH);


                topLetterBox.position = new Vector3(scrwidth / 2, scrheight, 0);
                botLetterBox.position = new Vector3(scrwidth / 2, 0, 0);
            }
            else
            {
                leftLetterBox.gameObject.SetActive(true);
                rightLetterBox.gameObject.SetActive(true);
                topLetterBox.gameObject.SetActive(false);
                botLetterBox.gameObject.SetActive(false);

                leftLetterBox.sizeDelta = new Vector2(scrwidth - SWIDTH * scrheight / SHEIGHT, scrheight);
                rightLetterBox.sizeDelta = new Vector2(scrwidth - SWIDTH * scrheight / SHEIGHT, scrheight);


                leftLetterBox.position = new Vector3(0, scrheight / 2, 0);
                rightLetterBox.position = new Vector3(scrwidth, scrheight / 2, 0);
            }
            */
    }


    void Update()
    {
        scrwidth = Screen.width; //추후 게임매니저로 관리
        scrheight = Screen.height; //추후 게임매니저로 관리

        ratio = scrwidth / scrheight;

        if (ratio < 1.77f)
        {
            topLetterBox.gameObject.SetActive(true);
            botLetterBox.gameObject.SetActive(true);
            leftLetterBox.gameObject.SetActive(false);
            rightLetterBox.gameObject.SetActive(false);

            topLetterBox.sizeDelta = new Vector2(scrwidth, scrheight - SHEIGHT * scrwidth / SWIDTH);
            botLetterBox.sizeDelta = new Vector2(scrwidth, scrheight - SHEIGHT * scrwidth / SWIDTH);

            topLetterBox.position = new Vector3(scrwidth / 2, scrheight, 0);
            botLetterBox.position = new Vector3(scrwidth / 2, 0, 0);
        }
        else
        {
            leftLetterBox.gameObject.SetActive(true);
            rightLetterBox.gameObject.SetActive(true);
            topLetterBox.gameObject.SetActive(false);
            botLetterBox.gameObject.SetActive(false);

            leftLetterBox.sizeDelta = new Vector2(scrwidth - SWIDTH * scrheight / SHEIGHT, scrheight);
            rightLetterBox.sizeDelta = new Vector2(scrwidth - SWIDTH * scrheight / SHEIGHT, scrheight);

            leftLetterBox.position = new Vector3(0, scrheight / 2, 0);
            rightLetterBox.position = new Vector3(scrwidth, scrheight / 2, 0);
        }
    }
}