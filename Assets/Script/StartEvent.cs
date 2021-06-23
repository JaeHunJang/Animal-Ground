using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartEvent : MonoBehaviour
{
    CMainTitle cMainTitle;
    bool isStart=false;

    // Start is called before the first frame update
    void Start()
    {
        cMainTitle = GameObject.Find("NetworkManager").GetComponent<CMainTitle>();
        
    }

    public void StartGame()
    {
        Debug.Log("이즈스타트"+isStart);
        if (!isStart)
        {
            isStart = true;
            cMainTitle.matchStart();
        }

        MainManager.Instance.GameLoadingWindow.SetActive(true);    
    }

    public void CancelGame()

    {       
        if(isStart)
        {
            isStart = false;
            CMainTitle.instance.send(PROTOCOL.ENTER_GAME_ROOM_QUIT);
        }

        MainManager.Instance.GameLoadingWindow.SetActive(false);
    }

}
