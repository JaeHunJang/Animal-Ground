using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    int contextIndex = 0;
    public void clickContext()
    {
        contextIndex++;
        Debug.Log("현재 인덱스 : " + contextIndex);

    }

}
