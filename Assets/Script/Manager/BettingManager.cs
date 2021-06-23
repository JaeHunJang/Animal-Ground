using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BettingManager : MonoBehaviour
{
    public GameObject posObj;

    private void OnEnable()
    {
        GameObject character = Instantiate(ProgressManager.Instance.myBetChar) as GameObject;
        character.transform.SetParent(posObj.transform);
        character.transform.localPosition = Vector3.zero;
        character.transform.localRotation = Quaternion.identity;
        character.transform.localScale = new Vector3(1, 1, 1);
        StartCoroutine(wait());
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(5.0f);
        ProgressManager.Instance.myState = ProgressManager.State.alertShow;
        ProgressManager.Instance.isDone = true;
        this.gameObject.SetActive(false);
    }

}
