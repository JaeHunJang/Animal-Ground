using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterManager : MonoBehaviour
{
    public GameObject[] itemObjs = new GameObject[10];
    public int myPosition { get; set; } = 0;
    public int myPos4Rank { get; set; } = 0;
    public int myNumber { get; set; } = 0;
    public int goalNum = 0;
    public int goal = 1;

    public bool isShield = false;
    public bool isLightning = false;
    public bool isBoost = false;
    public bool isMark = false;
    public bool isStepBanana = false;

    public int[] bananaPos = new int[4] { -1, -1, -1, -1 };

    public int tileNum;
    GameObject[] itemList = new GameObject[5];
    public GameObject shieldObj;
    public GameObject shockObj;
    public GameObject boostObj;
    public GameObject markObj;
    public GameObject magnetObj;
    Animator animator;

    enum Anim
    {
        idle = 1,
        victory = 3,
        sad = 6,
        happy = 9,
        move = 15
    }

    public TileManager tileManager;

    private void Start()
    {
        tileManager = FindObjectOfType<TileManager>();
        animator = GetComponent<Animator>();
        tileNum = ProgressManager.Instance.tileNum;
        itemObjs = ProgressManager.Instance.itemObjs;
        initItems();

    }

    public void initItems()
    {
        shieldObj = Instantiate(itemObjs[0]) as GameObject;
        shockObj = Instantiate(itemObjs[8]) as GameObject;
        boostObj = Instantiate(itemObjs[3]) as GameObject;
        markObj = Instantiate(itemObjs[5]) as GameObject;
        magnetObj = Instantiate(itemObjs[2]) as GameObject;

        itemList[0] = shieldObj;
        itemList[1] = shockObj;
        itemList[2] = boostObj;
        itemList[3] = markObj;
        itemList[4] = magnetObj;

        for (int i = 0; i < itemList.Length; i++)
        {
            itemSetting(itemList[i]);
        }
    }

    public void itemSetting(GameObject temp)
    {
        temp.transform.SetParent(gameObject.transform);
        temp.transform.localPosition = Vector3.zero + new Vector3(0, 0.5f, 0.5f);
        temp.transform.localRotation = Quaternion.Euler(new Vector3(0, 180, 0));
        temp.transform.localScale = new Vector3(1, 1, 1);
        temp.SetActive(false);
    }



    public void move(int moveNum)
    {
        int mark;
        if (moveNum > 0)
            mark = 1;
        else
            mark = -1;
        StartCoroutine(moveChar(moveNum, mark));
    }

    public void cMove()
    {
        StartCoroutine(cardMove());
    }

    public void itemFollow()
    {
        for(int i=0; i<itemList.Length;i++)
        {
            itemList[i].transform.position = gameObject.transform.position + new Vector3(0, 0.5f, -0.5f);
            itemList[i].transform.rotation = Quaternion.identity;
        }
    }

    public void itemsSetParent()
    {
        for (int i = 0; i < itemList.Length; i++)
        {
            itemList[i].transform.SetParent(gameObject.transform);
            itemList[i].transform.localRotation = Quaternion.identity;
            itemList[i].transform.localPosition = Vector3.zero + new Vector3(0, 0.5f, 0.5f);
        }
    }

    public void romoveParent()
    {
        for(int i=0; i<itemList.Length; i++)
        {
            itemList[i].transform.SetParent(null);
        }
    }

    IEnumerator moveChar(int moveNum, int mark)
    {
        isStepBanana = false;
        Transform target;
        Transform moveChar = gameObject.transform;
        float speed = 3.0f;
        int startPosition = myPosition;

        if (isBoost)
        {
            moveNum *= 2;
        }
        if(isMark)
        {
            mark = 1;
        }
        
        for (int index = 0; index < moveNum; index++)
        {

            //번개처리
            if (isLightning)
            {
                isLightning = false;
                shockObj.SetActive(false);
                moveNum = index;
                break;
            }

            for (int j = 0; j < bananaPos.Length; j++)
            {
                if (bananaPos[j] != -1)
                {
                    tileManager.tiles[bananaPos[j]].isBanana = false;
                    bananaPos[j] = -1;
                }


            }

            //한명이라도 도착시
            if (ProgressManager.Instance.isArrive)
            {
                moveNum = index;
                break;
            }

            startPosition += mark;

            //0번째타일 예외처리
            if (startPosition < 0)
            {
                myPosition = 0;
                moveNum = 0;
                break;
            }else if(startPosition > tileNum -1)
            {
                goalNum++;
                startPosition = 0;
            }

            target = tileManager.tiles[startPosition].holders[myNumber].transform;

            Vector3 targetPos = target.position;
            targetPos += new Vector3(0, 0.2f, 0);

            moveChar.LookAt(targetPos);
            romoveParent();
            while (Vector3.Distance(moveChar.position, targetPos) > 0.01f)
            {
                SetInt("animation," + (int)Anim.move);
                moveChar.position = Vector3.MoveTowards(moveChar.position, targetPos, Time.deltaTime * speed);
                itemFollow();
                yield return null;
            }

            if (goalNum == goal)
            {
                ProgressManager.Instance.isArrive = true;
                ProgressManager.Instance.arriveNum.Add(myNumber);
                ProgressManager.Instance.myState = ProgressManager.State.gameEnd;
                myPosition = 0;
                moveNum = 0;
                break;
            }
            

            //바나나 처리
            if (tileManager.tiles[startPosition].isBanana)
            {
                bananaPos[myNumber] = startPosition;
                if (isShield)
                {
                    isShield = false;
                    shieldObj.SetActive(false);
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < ProgressManager.Instance.bananas.Count; j++)
                        {
                            if (ProgressManager.Instance.bananas[j].transform.IsChildOf(tileManager.tiles[startPosition].positionHolder))
                            {
                                ProgressManager.Instance.bananas[j].gameObject.SetActive(false);
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < ProgressManager.Instance.bananas.Count; j++)
                        {
                            if (ProgressManager.Instance.bananas[j].transform.IsChildOf(tileManager.tiles[startPosition].positionHolder))
                            {
                                ProgressManager.Instance.bananas[j].gameObject.SetActive(false);
                            }
                        }

                    }
                    moveNum = index + 1;
                    //바나나애니메이션
                    break;
                }
            }

            
            itemsSetParent();
            
            //moveChar.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            moveChar.position = target.position + new Vector3(0, 0.2f, 0);
            moveChar.SetParent(target);

            if(isBoost)
            {
                isBoost = false;
                boostObj.SetActive(false);
            }
            yield return null;
        }
        SetInt("animation," + (int)Anim.idle);

        moveChar.rotation = Quaternion.Euler(new Vector3(0, 180, 0));

        myPosition += (moveNum * mark);
        if (myPosition > tileNum-1)
            myPosition -= tileNum;
        myPos4Rank = myPosition + (goalNum * 24);
        ProgressManager.Instance.isDone = true;//마그넷용
        ProgressManager.Instance.moveEndNum++;
    }




    IEnumerator cardMove()
    {
        if (tileManager.tiles[myPosition].isTiled)
        {
            int moveNum , markNum;
            moveNum = tileManager.tiles[myPosition].card.cardNumber;

            if (moveNum > 0 || isMark)
            {
                markNum = 1;
                SetInt("animation," + (int)Anim.happy);
            }
            else if (moveNum < 0)
            {
                markNum = -1;
                SetInt("animation," + (int)Anim.sad);

            }
            else
                markNum = -1;

            moveNum = Mathf.Abs(moveNum);

            yield return new WaitForSeconds(1.3f);

            SetInt("animation," + (int)Anim.idle);

            yield return StartCoroutine(moveChar(moveNum, markNum));
        
        }else
        {
            ProgressManager.Instance.moveEndNum++;
        }
        if (isMark)
        {
            isMark = false;
            markObj.SetActive(false);
        }
        yield return null;
    }

    public bool checkMove()
    {
        for (int i = 0; i < ProgressManager.Instance.moveOne.Length; i++)
        {
            if (!ProgressManager.Instance.moveOne[i])
                return false;
        }
        return true;
    }

    public void SetInt(string parameter = "key,value")
    {
        char[] separator = { ',', ';' };
        string[] param = parameter.Split(separator);

        string name = param[0];
        int value = Convert.ToInt32(param[1]);

        animator.SetInteger(name, value);
        
    }

}
