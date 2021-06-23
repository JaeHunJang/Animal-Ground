using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour
{
    public Sprite[] nums = new Sprite[10];
    ProgressManager progressManager;

    public Image firstNum;
    public Image secondNum;
    bool isTiming=false;

    public int timer;

    private void Start()
    {
        progressManager = ProgressManager.Instance;
    }

    public void setTimer(int time)
    {
        if (!isTiming)
        {
            isTiming = true;
            progressManager.isTImeout = false;
            timer = time;
            StartCoroutine(timing());
        }
    }

    IEnumerator timing()
    {
        firstNum.enabled = true;
        secondNum.enabled = true;
        while (!progressManager.isTImeout)
        {
            if (timer <=0)
            {
                progressManager.StopAllCoroutines();
                switch (progressManager.myState)
                {
                    case ProgressManager.State.cardSelect:
                    case ProgressManager.State.tileSelect:
                        if (progressManager.cardSetNum == 0)
                        {
                            progressManager.cardSetData[1] = 0;
                            progressManager.cardSetData[2] = -1;
                        }
                        progressManager.cardSetData[3] = 0;
                        progressManager.cardSetData[4] = -1;
                        progressManager.myState = ProgressManager.State.betWait;
                        progressManager.isDone = true;
                        progressManager.callCycle();
                        progressManager.sendMsgToServer(progressManager.cardSetData, PROTOCOL.CARD_SET_REQ);
                        break;
                    case ProgressManager.State.itemUse:
                        progressManager.myState = ProgressManager.State.betWait;
                        progressManager.isDone = true;
                        progressManager.callCycle();
                        CMainTitle.instance.send(new int[] { int.MinValue, int.MinValue }, PROTOCOL.ITEM_USE_REQ);
                        break;
                    default:
                        break;

                }

                break;
            }
            if (timer / 10 == 1)
            {
                firstNum.enabled = true;
                firstNum.sprite = nums[1];
            }
            else
            {
                firstNum.enabled = false;
            }
            secondNum.sprite = nums[timer % 10];
            yield return new WaitForSeconds(1.0f);
            timer--;

        }
        isTiming = false;
        firstNum.enabled = false;
        secondNum.enabled = false;
    }
}
