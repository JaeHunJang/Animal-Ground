using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCardShowManager : MonoBehaviour
{
    public ItemCardRotate[] cards = new ItemCardRotate[4];

    public int done = 0;

    private void Start()
    {
        for (int i = 0; i < cards.Length; i++)
            cards[i].cardNum = i + 1;
    }

    private void OnEnable()
    {
        StartCoroutine(cardRotate());
    }


    IEnumerator cardRotate()
    {
        int itemNum;
        for (int i = 0; i < cards.Length; i++)
        {
            switch (ProgressManager.Instance.useItemList[i])
            {
                case "Shield":
                    itemNum = 0;
                    break;
                case "Lightning":
                    itemNum = 1;
                    break;
                case "Magnet":
                    itemNum = 2;
                    break;
                case "Boost":
                    itemNum = 3;
                    break;
                case "Banana":
                    itemNum = 4;
                    break;
                case "Mark":
                    itemNum = 5;
                    break;
                default:
                    itemNum = -1;
                    break;
            }
            cards[i].rotate(itemNum,this);
        }
        yield return new WaitUntil(() => done > 3);
        done = 0;
        ProgressManager.Instance.isDone = true;
    }
}
