using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardRotate : MonoBehaviour
{
    public Card[] card;
    CardShowManager cardShowManager;

    float speed = 300.0f;

    public void cardEnable()
    {
        GetComponent<Image>().enabled = true;
    }

    public void rotate(int moveNum , CardShowManager csm)
    {
        cardShowManager = csm;
        StartCoroutine(rotateCard(moveNum));
    }

    public void cardInit()
    {
        GetComponent<Image>().enabled = false;
        GetComponent<Image>().sprite = card[0].cardBackImage;
    }

    IEnumerator rotateCard(int moveNum)
    {
        Debug.Log(moveNum + "이동숫자");


        while (transform.localEulerAngles.y < 89.9f)
        {
            transform.Rotate(Vector3.up * Time.deltaTime * speed);
            yield return null;
        }


        GetComponent<Image>().sprite = card[moveNum-1].cardImage;
        yield return null;

        while (transform.localEulerAngles.y < 180.0f)
        {
            transform.Rotate(Vector3.down * Time.deltaTime * speed);
            yield return null;
        }

        transform.localRotation = Quaternion.Euler(Vector3.zero);


        yield return new WaitForSeconds(2.0f);

        cardShowManager.isRotate = true;
        
    }
}
