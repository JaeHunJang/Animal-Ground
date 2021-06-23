using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCardRotate : MonoBehaviour
{
    public int cardNum;

    public Sprite[] itemImages;

    public GameObject[] items;

    float speed = 300.0f;

    public void OnDisable()
    {
        cardInit();
    }

    public void rotate(int itemNum , ItemCardShowManager icsm)
    {

        StartCoroutine(rotateCard(itemNum,icsm));
    }

    public void cardInit()
    {
        GetComponent<SpriteRenderer>().sprite = itemImages[0];
    }

    IEnumerator rotateCard(int itemNum , ItemCardShowManager icsm)
    {
        GameObject temp = null;

        while (transform.localEulerAngles.y < 89.9f)
        {
            transform.Rotate(Vector3.up * Time.deltaTime * speed);
            yield return null;
        }

        GetComponent<SpriteRenderer>().sprite = itemImages[cardNum];

        if(itemNum != -1)
        {
            temp = Instantiate(items[itemNum]) as GameObject;
            temp.transform.SetParent(gameObject.transform);
            temp.transform.localPosition = Vector3.zero;
            temp.transform.localScale = new Vector3(3f,3f, 3f);
            temp.transform.localRotation = Quaternion.identity;
        }

        yield return null;

        while (transform.localEulerAngles.y < 180.0f)
        {
            transform.Rotate(Vector3.down * Time.deltaTime * speed);
            yield return null;
        }

        transform.localRotation = Quaternion.Euler(Vector3.zero);

        yield return new WaitForSeconds(2.0f);
        if(temp!=null)
            Destroy(temp);
        icsm.done++;
    }
}
