using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeSelect : MonoBehaviour
{
    public GameObject[] checkImg = new GameObject[2];
    
    private void OnEnable()
    {
        if (CMainTitle.instance.tileNum == 24)
            click24();
        else
            click16();
    }

    public void click24()
    {
        checkImg[0].SetActive(false);
        checkImg[1].SetActive(true);
        CMainTitle.instance.tileNum = 24;
        
    }

    public void click16()
    {
        checkImg[0].SetActive(true);
        checkImg[1].SetActive(false);
        CMainTitle.instance.tileNum = 16;
    }


}
