using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NameCardManager : MonoBehaviour
{
    public NameCardInfo[] nameCardInfo_temp;
    public NameCardInfo[] nameCardInfo;
    public RectTransform[] cardLocate;
    public RectTransform[] nameCards;
    public Sprite[] nameCardImage;
    public string[] nickNames;
    int myNum;

    private void OnEnable()
    {
        myNum = ProgressManager.Instance.myNum;

        int tempNum = myNum;

        for (int i = 0; i < 4; i++)
        {
            nameCardInfo[tempNum % 4] = nameCardInfo_temp[i];
            nameCardInfo_temp[i].cardImage.sprite = nameCardImage[tempNum % 4];
            nameCardInfo[tempNum % 4].userNum = tempNum % 4;
            tempNum++;
        }

    }

    public void setNickNames(string[] names)
    {
        nickNames = names;
        for (int i = 0; i < 4; i++)
        {
            nameCardInfo[i].userName.text = nickNames[i];
        }
    }
    public void setCharacters(GameObject[] obj)
    {
        for(int i=0; i<4; i++)
        {
            GameObject temp = Instantiate(obj[i]) as GameObject;
            temp.transform.SetParent(nameCardInfo[i].userCharacter);
            temp.transform.localPosition = Vector3.zero;
            temp.transform.localRotation = Quaternion.identity;
            temp.transform.localScale = new Vector3(1, 1, 1);
        }

    }

}
