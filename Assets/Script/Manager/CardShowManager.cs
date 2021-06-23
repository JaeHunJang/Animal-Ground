using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardShowManager : MonoBehaviour
{
    public GameObject[] cardObj = new GameObject[4];
    public RectTransform cardInit;
    public NameCardManager nameCardManager;

    public Sprite[] cardNumberImg;
    public bool isRotate;
    private void OnEnable()
    {
        StartCoroutine(cardAnim());
    }

    IEnumerator cardAnim()
    {
        float speed = 50.0f;
        isRotate = false;
        for (int i = 0; i < 4; i++)
        {
            cardObj[i].transform.position = cardInit.transform.position;
            cardObj[i].GetComponent<CardRotate>().cardEnable();
        }
        for (int i=0; i<4; i++)
        {
            while(Vector3.Distance(cardObj[i].transform.position, nameCardManager.cardLocate[i].position) > 0.01f)
            {
                cardObj[i].transform.position = Vector3.MoveTowards(cardObj[i].transform.position, nameCardManager.cardLocate[i].position, Time.deltaTime * speed);
                yield return null;
            }
        }

        int myNum = ProgressManager.Instance.myNum;
        for(int i=0; i<4;i++)
        {
            cardObj[i].GetComponent<CardRotate>().rotate(ProgressManager.Instance.moveNum[(i+myNum)%4,ProgressManager.Instance.charMoveNum],this);
        }

        yield return new WaitUntil(() => isRotate);

        for(int i =0; i<4; i++)
            cardObj[i].GetComponent<CardRotate>().cardInit();

        ProgressManager.Instance.myState = ProgressManager.State.charMove;
        ProgressManager.Instance.isDone = true;
    }

}
