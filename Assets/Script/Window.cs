using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour
{
    public GameObject window;

    //특정 캔버스를 on/off 시키는 스크립트
    public void CloseWindow()
    {
        gameObject.SetActive(false);
    }

    public void OpenWindow()
    {
        window.SetActive(true);
    }
}
