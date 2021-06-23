using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToMainScene : MonoBehaviour
{
    //씬 이동 스크립트
    public void Click()
    {
        SceneManager.LoadScene(1);
    }
}
