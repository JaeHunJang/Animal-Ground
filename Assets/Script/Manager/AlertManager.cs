using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertManager : MonoBehaviour
{
    public Sprite[] alertImg;
    public RectTransform alertObj;

    int index = 0;
    Image alert;

    public RectTransform[] posObj;
    private void Start()
    {
        posObj[0].localPosition = new Vector3(Screen.width + 2000, 0, 0);
        posObj[3].localPosition = new Vector3(-Screen.width - 2000, 0, 0);
    }

    private void OnEnable()
    {
        alert = alertObj.GetComponent<Image>();
        StartCoroutine(moveAnim());
    }

    IEnumerator moveAnim()
    {
        float speed;
        alertObj.position = posObj[0].position;

        yield return new WaitForSeconds(1.0f);

        speed = 50.0f;
        while (Vector3.Distance(alertObj.position, posObj[1].position) > 2.0f)
        {
            alertObj.position = Vector3.MoveTowards(alertObj.position, posObj[1].position, Time.deltaTime * speed);
            yield return null;
        }

        speed = 1.0f;
        while (Vector3.Distance(alertObj.position, posObj[2].position) > 1.0f)
        {
            alertObj.position = Vector3.MoveTowards(alertObj.position, posObj[2].position, Time.deltaTime * speed);
            yield return null;
        }

        speed = 50.0f;
        while (Vector3.Distance(alertObj.position, posObj[3].position) > 5.0f)
        {
            alertObj.position = Vector3.MoveTowards(alertObj.position, posObj[3].position, Time.deltaTime * speed);
            yield return null;
        }

        if (index == 0)
        {
            index =1;
            alert.sprite = alertImg[index];
            ProgressManager.Instance.myState = ProgressManager.State.cardSelect;
        }
        else if(index == 1)
        {
            index =2;
            alert.sprite = alertImg[index];
            ProgressManager.Instance.myState = ProgressManager.State.itemUse;
        }else if(index ==2)
        {
            index = 0;
            alert.sprite = alertImg[index];
            ProgressManager.Instance.myState = ProgressManager.State.moveNumShow;
        }

        yield return new WaitForSeconds(1.0f);

        ProgressManager.Instance.isDone = true;



    }

}
