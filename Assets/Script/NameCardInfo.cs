using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameCardInfo : MonoBehaviour
{
    public Image cardImage;
    public Text userName;
    public Image[] userItem = new Image[2];
    int itemIndex = 0;
    public Image userRankImg;

    public Sprite[] userColor;

    public Sprite[] rankImages;
    public Sprite[] pointImages;
    public GameObject[] items = new GameObject[6];


    public int userNum;
    public RectTransform userCharacter;
    public int userRank;

    private void Update()
    {
        userRankImg.sprite = rankImages[userRank];
    }

    public void itemUse(int itemNum)
    {
        if (itemIndex < 2)
        {
            userItem[itemIndex].sprite = userColor[userNum];
            if (itemNum != 6)
            {
                GameObject temp = Instantiate(items[itemNum]) as GameObject;
                temp.transform.SetParent(userItem[itemIndex].transform);
                temp.transform.localPosition = Vector3.zero;
                temp.transform.localScale = new Vector3(300f, 300f, 300f);
                temp.transform.localRotation = Quaternion.identity;
            }
            itemIndex++;
        }
    }


}
