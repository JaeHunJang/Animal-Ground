using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemAction : MonoBehaviour
{
    int myNum;

    int[] data = new int[2];

    bool isSelected = false;

    private void Start()
    {
        myNum = ProgressManager.Instance.myNum;
        data[0] = myNum;
    }

    private void OnEnable()
    {
        isSelected = false;
    }

    public void itemShield()
    {
        if (!isSelected)
        {
            isSelected = true;
            sendMessage(0);
        }
    }
    public void itemLightning()
    {
        if (!isSelected)
        {
            isSelected = true;
            sendMessage(1);
        }
    }
    public void itemMagnet()
    {
        if (!isSelected)
        {
            isSelected = true;
            sendMessage(2);
        }
    }
    public void itemBooster()
    {
        if (!isSelected)
        {
            isSelected = true;
            sendMessage(3);
        }
    }
    public void itemBanana()
    {
        if (!isSelected)
        {
            isSelected = true;
            sendMessage(4);
        }
    }
    public void itemMark()
    {
        if (!isSelected)
        {
            isSelected = true;
            sendMessage(5);
        }
    }

    void sendMessage(int itemNum)
    {
        data[1] = itemNum;
        CMainTitle.instance.send(data, PROTOCOL.ITEM_USE_REQ);
        gameObject.GetComponent<Button>().interactable = false;
        ProgressManager.Instance.itemUseCount--;
        ProgressManager.Instance.myState = ProgressManager.State.betWait;
        ProgressManager.Instance.isDone = true;
    }     


}
