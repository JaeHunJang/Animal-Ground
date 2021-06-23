using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public GameObject[] itemList = new GameObject[6];

    public Transform[] itemObjs = new Transform[2];

    private void Start()
    {
        for (int i = 0; i < itemObjs.Length; i++)
        {
            if (ProgressManager.Instance.itemNums[i] == -1)
                continue;
            GameObject temp = Instantiate(itemList[ProgressManager.Instance.itemNums[i]]) as GameObject;
            temp.transform.SetParent(itemObjs[i]);
            temp.transform.localPosition = Vector3.zero;
            temp.transform.localRotation = Quaternion.identity;
            temp.transform.localScale = Vector3.one;
        }
    }

}
