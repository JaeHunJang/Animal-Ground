using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ForTestScene : MonoBehaviour
{
    int userNum = 0;
    string itemName = "Shield";
    public void Start()
    {

    }

    public void next()
    {
        SceneManager.LoadScene(0);
    }
    
}
