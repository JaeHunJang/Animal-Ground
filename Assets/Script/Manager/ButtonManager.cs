using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{

    ProgressManager progressManager;

    private void Start()
    {
        progressManager = ProgressManager.Instance;
    }
    public void callOk()
    {
        progressManager.isDone = true;
    }

    public void callCancle()
    {
        progressManager.StopAllCoroutines();

        switch(progressManager.myState)
        {
            case ProgressManager.State.tileSelect:
                progressManager.myState = ProgressManager.State.cardSelect;
                break;
            default:
                break;

        }

        progressManager.isDone = true;
        progressManager.callCycle();
    }
    
    public void callPass()
    {
        progressManager.activeFalseAll();
        progressManager.timerManager.timer = 0;
    }

    public void callHide()
    {
        switch (progressManager.myState)
        {
            case ProgressManager.State.cardSelect:
                progressManager.selectCardToPlace.SetActive(false);
                break;
            case ProgressManager.State.itemUse:
                progressManager.itemObj.SetActive(false);
                break;
            default:
                break;
        }
        progressManager.twoBtn.SetActive(false);
        progressManager.showBtn.SetActive(true);
    }

    public void callShow()
    {
        switch (progressManager.myState)
        {

            case ProgressManager.State.cardSelect:
                progressManager.selectCardToPlace.SetActive(true);
                break;
            case ProgressManager.State.itemUse:
                progressManager.itemObj.SetActive(true);
                break;
            default:
                break;
        }
        progressManager.showBtn.SetActive(false);
        progressManager.twoBtn.SetActive(true);
    }
}
