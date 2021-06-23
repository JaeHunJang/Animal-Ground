using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileSelectManager : MonoBehaviour
{
    public Sprite[] alerts;
    public Image alertImg;

    public void changeAlert()
    {
        StopAllCoroutines();
        StartCoroutine(alertChange());
    }

    IEnumerator alertChange()
    {
        alertImg.sprite = alerts[1];
        yield return new WaitForSeconds(1.0f);
        alertImg.sprite = alerts[0];
    }

}
