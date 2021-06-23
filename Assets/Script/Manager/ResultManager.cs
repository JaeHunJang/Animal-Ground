using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using MySql.Data.MySqlClient;

public class ResultManager : MonoBehaviour
{
    public Transform[] charPos;
    public GameObject[] characters = new GameObject[4];

    public Image[] coinObj = new Image[4];
    public Image[] expObj = new Image[4];

    public Sprite[] coins = new Sprite[4];
    public Sprite[] exps = new Sprite[4];

    Animator[] animator = new Animator[4];
    
    const int charNum = 4;

    public int[] resultRank = new int[4];

    enum Anim
    {
        idle = 1,
        victory = 3,
        sad = 6,
        happy = 9,
        move = 15
    }

    private void OnEnable()
    {
        for(int i=0; i<charNum; i++)
        {
            characters[i] = Instantiate(ProgressManager.Instance.chars2[i]);
            characters[i].transform.SetParent(charPos[i]);
            characters[i].transform.localPosition = Vector3.zero;
            characters[i].transform.localRotation = Quaternion.identity;
            characters[i].transform.localScale = new Vector3(1, 1, 1);
            animator[i] = characters[i].GetComponent<Animator>();
        }

        StartCoroutine(playAnim());
    }

    IEnumerator playAnim()
    {
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < charNum; i++)
        {
            if(ProgressManager.Instance.arriveNum.Contains(i))
                SetInt(i, "animation," + (int)Anim.victory);
            else
                SetInt(i, "animation," + (int)Anim.sad);
        }

        resultRank = ProgressManager.Instance.resultRankSet();

        for(int i=0; i<4; i++)
        {
            switch(resultRank[i])
            {
                case 0:
                    showResult(i, 0, 25);
                    break;
                case 1:
                    showResult(i, 1, 20);
                    break;
                case 2:
                    showResult(i, 2, 15);
                    break;
                case 3:
                    showResult(i, 3, 10);
                    break;
            }
        }
        
        yield return new WaitForSeconds(8.0f);
        ProgressManager.Instance.isDone = true;
    }

    private void showResult(int userNum ,int index ,int value)
    {
        string id;
        id = ProgressManager.Instance.nicknames[userNum];

        expObj[userNum].sprite = exps[index];
        coinObj[userNum].sprite = coins[index];

        expObj[userNum].enabled = true;
        coinObj[userNum].enabled = true;
        
        MySqlConnector.Instance.doNonQuery("update account set account_money_now = account_money_now + "+value+ ", account_money_all = account_money_all  + " + value + ", account_exp = account_exp  + " + value + " where account_nickname = '" + id + "'");
    }







    public void SetInt(int num, string parameter = "key,value")
    {
        char[] separator = { ',', ';' };
        string[] param = parameter.Split(separator);

        string name = param[0];
        int value = Convert.ToInt32(param[1]);

        Debug.Log(name + " " + value);

        animator[num].SetInteger(name, value);

    }

}
