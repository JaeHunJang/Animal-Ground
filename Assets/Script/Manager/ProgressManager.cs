using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ProgressManager : MonoBehaviour
{

    public GameObject[] characterList = new GameObject[15];
    public string[] nicknames = new string[4];
    public GameObject[] chars = new GameObject[4];
    public GameObject[] chars2 = new GameObject[4]; 
    public GameObject[] pivotList = new GameObject[4];
    public GameObject[] pivots = new GameObject[4];
    public GameObject[] itemObjs = new GameObject[10];
    public GameObject tiledBanana;
    public GameObject myBetChar;

    public GameObject betObj;
    public GameObject selectCardToPlace;
    public GameObject betWaitObj;
    public GameObject cardShowObj;
    public GameObject tileSelectObj;
    public GameObject alertObj;
    public GameObject resultObj;
    public GameObject itemObj;
    public GameObject itemShowObj;

    public GameObject twoBtn;
    public GameObject cancelBtn;
    public GameObject showBtn;

    public TileManager tileManager;
    public NameCardManager nameCardManager;
    public CardToPlaceManager cardToPlaceManager;
    public TimerManager timerManager;

    CharacterManager[] characterManagers = new CharacterManager[4];

    public Card[] cardList = new Card[5];
    public Card card;
    public GameObject cardPrefab;
    public GameObject nameCardManagerObj;

    public bool isDone { get; set; } = false;
    public bool isEnd { get; set; } = false;
    public bool isArrive { get; set; } = false;
    public bool isTImeout { get; set; } = true;
    public bool[] moveOne = new bool[4] { false,false,false,false };

    public int myNum { get; set; }
    public int cardSetNum { get; set; } = 0;
    public int moveEndNum { get; set; } = 0;
    public int charMoveNum { get; set; } = 0;
    public int tileNum { get; set; }
    

    public int[,] moveNum = new int[4,3];
    public int[] cardSetData = new int[5];
    public int[] itemNums = new int[2];

    public List<int> arriveNum = new List<int>();
    public List<GameObject> bananas = new List<GameObject>();

    public int itemUseCount = 2;
    public int ptwoNum = 1;
    public int mtwoNum = 1;
    public int zeroNum = 2;

    enum Items
    {
        Shiled,
        Lightning,
        Magnet,
        Booster,
        Banana,
        Mark
    }

    public string[] useItemList = new string[4] { "", "", "", "" };

    public int[] resultRank = new int[4];

    public enum State
    {
        gameStart,
        alertShow,
        cardSelect,
        tileSelect,
        betWait,
        itemUse,
        itemAction,
        moveNumShow,
        charMove,
        cardStepWait,
        cardStep,
        moveEnd,
        gameEnd
    }

    public State myState;

    private static ProgressManager progressManager = null;

    public static ProgressManager Instance
    {
        get { return progressManager; }
    }

    private void Awake()
    {
        if (progressManager == null)
            progressManager = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        myState = State.gameStart;
        myNum = CMainTitle.instance.player_index;
        tileNum = CMainTitle.instance.tileNum;
        itemNums = CMainTitle.instance.itemNums;

        for(int i=0; i<itemNums.Length; i++)
        {
            if (itemNums[i] == -1)
                itemUseCount--;
        }

        switch(myNum)
        {
            case 0:
                cardSetData[0] = 1;
                break;
            case 1:
                cardSetData[0] = 2;
                break;
            case 2:
                cardSetData[0] = 4;
                break;
            case 3:
                cardSetData[0] = 8;
                break;
        }
        //sendMsgToServer(PROTOCOL.GAME_START);

        nameCardManagerObj.SetActive(true);

        StartCoroutine(cycle());
    }

    //네명의 플레이어가 전부 준비 완료되었을 경우 호출.
    public void allPlayerReady()
    {
        tileManager.tiles[0].nowColor = 16;
        tileManager.tiles[0].changeMat(16);
        myBetChar = chars2[myNum];

        nameCardManager.setNickNames(nicknames);
        nameCardManager.setCharacters(chars2);

        isDone = true;
        
    }

    //각 유저의 인덱스넘버에 따라 캐릭터를 생성함.
    public void setCharacter(int charNumber, int userNum)
    {
        chars[userNum] = Instantiate(characterList[charNumber]);
        chars2[userNum] = Instantiate(characterList[charNumber]);
        chars[userNum].transform.SetParent(tileManager.tiles[0].holders[userNum].transform);
        chars[userNum].transform.localPosition = Vector3.zero + new Vector3(0, 0.2f, 0);
        chars[userNum].transform.Rotate(0, 180, 0);
        chars[userNum].transform.localScale = new Vector3(2, 2, 2);
        chars[userNum].AddComponent<CharacterManager>();
        characterManagers[userNum] = chars[userNum].GetComponent<CharacterManager>();
        characterManagers[userNum].myNumber = userNum;
    }

    //각 캐릭터에 피봇을 달아줌.
    public void setPivot()
    {
        for(int i=0; i<chars.Length; i++)
        {
            pivots[i] = Instantiate(pivotList[i]);
            pivots[i].transform.SetParent(chars[i].transform);
            pivots[i].transform.localScale = new Vector3(0.2f, 0.2f,1);
            pivots[i].transform.localPosition = Vector3.zero + new Vector3(0, 0.9f, 0);
        }
    }

    //랭크를 갱신함.
    public void setRank()
    {
        for (int i = 0; i <4; i++)
        {
            int rank = 0;
            for (int j = 0; j < 4; j++)
            {
                if (characterManagers[i].myPos4Rank < characterManagers[j].myPos4Rank)
                    rank++;
            }
            nameCardManager.nameCardInfo[i].userRank = rank;
        }
    }

    //내 다음등수의 유저의 인덱스넘버를 가져옴.
    public int getNextRank(int num)
    {
        num--;
        for (int i = 0; i < 4; i++)
        {
            if (nameCardManager.nameCardInfo[i].userRank == num)
            {
                return i;
            }
        }
        return num;
    }

    //특정 등수인 유저들의 인덱스넘버를 배열로 가져옴.
    public List<int> getRank(int num)
    {
        List<int> userRank = new List<int>();
        for(int i=0; i<4; i++)
        {
            if (nameCardManager.nameCardInfo[i].userRank == num)
            {
                userRank.Add(i);
            }
        }
        return userRank;
    }

    //서버에 데이터가 필요없는 패킷을 보냄.
    public void sendMsgToServer(PROTOCOL protocol)
    {
        CMainTitle.instance.send(protocol);
    }

    //서버에 데이터가 필요한 패킷을 보냄.
    public void sendMsgToServer(int[] msg,PROTOCOL protocol)
    {
        CMainTitle.instance.send(msg , protocol);
    }

    //타일에 생성된 카드오브젝트를 배치함.
    public void setCards(int userNum, int tileNum, int cardNum)
    {
        if (cardNum > 4 || cardNum <0)
            return;

        card = cardList[cardNum];

        TileInfo target;
        target = tileManager.tiles[tileNum].GetComponent<TileInfo>();
        createCard(target);
        changeMat(target, userNum);
        target.isTiled = true;
        target.card = card;
    }

    //카드 오브젝트를 생성함.
    public void createCard(TileInfo target)
    {
        GameObject cardObj = Instantiate(cardPrefab, target.cardHolder);
        cardObj.GetComponent<RectTransform>().sizeDelta = new Vector2(42f, 64f);
        cardObj.GetComponent<Image>().sprite = card.cardImage;
    }

    //타일의 머티리얼을 바꿔줌.
    public void changeMat(TileInfo target, int userNum)
    {
        target.changeMat(userNum);
    }

    //타일에 카드생성이 끝난것을 서버에 알려줌.
    public void cardSetComplete()
    {
        sendMsgToServer(PROTOCOL.CARD_SETTED);
        myState = State.alertShow;
        isDone = true;
    }
    
    //타겟의 유저 인덱스넘버를 가져옴.
    public int getTargetNum(TileInfo target)
    {
        for (int i = 0; i < tileManager.tiles.Length; i++)
        {
            if (target.name.Equals(tileManager.tiles[i].name))
                return i;
        }
        return -1;
    }
    
    //모든 알림창들을 비활성화함(인터럽트 발생시 호출)
    public void activeFalseAll()
    {
        selectCardToPlace.SetActive(false);
        tileSelectObj.SetActive(false);
        cancelBtn.SetActive(false);
        showBtn.SetActive(false);
        twoBtn.SetActive(false);
        itemObj.SetActive(false);
    }

    //사이클 코루틴을 다시 호출함(인터럽트 발생시 호출)
    public void callCycle()
    {
        activeFalseAll();
        StartCoroutine(cycle());
    }

    //자석과 타겟용 아이템생성
    public GameObject itemEffect(int userNum, int itemNum)
    {
        GameObject temp = Instantiate(itemObjs[itemNum]) as GameObject;
        temp.transform.SetParent(chars[userNum].transform);
        temp.transform.localPosition = Vector3.zero + new Vector3(0, 1, 1);
        temp.transform.localRotation = Quaternion.identity;
        temp.transform.localScale = new Vector3(1,1,1);
        return temp;
    }

    //게임 결과에 필요한 최종 랭크를 가져옴.
    public int[] resultRankSet()
    {
        for (int i = 0; i < 4; i++)
        {
            int rank = 0;
            for (int j = 0; j < 4; j++)
            {
                if (characterManagers[i].myPos4Rank < characterManagers[j].myPos4Rank)
                    rank++;
            }
            resultRank[i] = rank;
        }
        return resultRank;
    }

   
    //스테이트에 해당하는 코루틴을 불러주는 코루틴. 게임이 종료될때까지 반복.
    IEnumerator cycle()
    {
        yield return new WaitUntil(() => isDone);
        while (!isEnd)
        {
            isDone = false;
            yield return StartCoroutine(myState.ToString());
        }
        yield return null;
    }

    //게임이 시작되었을때 내가 1등으로 만들어야할 캐릭터를 보여주는 betObj 오브젝트 활성화.
    IEnumerator gameStart()
    {
        betObj.SetActive(true);
        yield return new WaitUntil(() => isDone);
        betObj.SetActive(false);
    }

    //카드배치, 아이템사용, 캐릭터이동 턴이라는 것을 보여주는 alertObj 오브젝트 활성화.
    IEnumerator alertShow()
    {
        alertObj.SetActive(true);
        yield return new WaitUntil(() => isDone);
        alertObj.SetActive(false);
    }

    //카드배치할 수 있는 selectCardToPlace 오브젝트 활성화.
    IEnumerator cardSelect()
    {
        selectCardToPlace.SetActive(true);
        twoBtn.SetActive(true);
        timerManager.setTimer(15);
        yield return new WaitUntil(() => isDone);
        selectCardToPlace.SetActive(false);
        twoBtn.SetActive(false);
    }

    //cardSelect에서 카드를 선택 후 카드를 배치할 타일을 보여주는 코루틴.
    IEnumerator tileSelect()
    {
        tileSelectObj.SetActive(true);
        cancelBtn.SetActive(true);
        TileInfo target = null;
        int targetNum = 0;

        while (!isDone)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Physics.Raycast(ray, out hit);

                if (hit.collider != null && hit.collider.tag.Equals("Tile"))
                {
                    //타일확인여부
                    target = hit.transform.GetComponent<TileInfo>();

                    if (target.isTiled)
                    {
                        int tot = target.card.cardNumber + card.cardNumber;
                        if (tot > 2 || tot < -2)
                        {
                            tileSelectObj.SendMessage("changeAlert", SendMessageOptions.DontRequireReceiver);
                        }
                        else
                        {
                            targetNum = getTargetNum(target);
                            target.card = cardList[tot + 2];
                            isDone = true;
                        }
                    }
                    else
                    {
                        targetNum = getTargetNum(target);
                        target.card = cardList[card.cardNumber+2];
                        isDone = true;
                    }
                }
            }
            yield return null;
        }
        cancelBtn.SetActive(false);

        if (card.cardNumber == 2)
            ptwoNum--;
        else if (card.cardNumber == -2)
            mtwoNum--;
        else if (card.cardNumber == 0)
            zeroNum--;

        int cardListNum = card.cardNumber + 2;
       
        cardSetData[cardSetNum*2 + 1] = targetNum;
        cardSetData[cardSetNum*2 + 2] = cardListNum;

        setCards(cardSetData[0], cardSetData[cardSetNum * 2 + 1], cardSetData[cardSetNum * 2 + 2]);
        cardSetNum++;
        tileSelectObj.SetActive(false);

        if (cardSetNum < 2)
            myState = State.cardSelect;
        else
        {
            sendMsgToServer(cardSetData, PROTOCOL.CARD_SET_REQ);
            myState = State.betWait;
        }
    }

    //다른 유저들이 작업을 완료할때까지 대기하라는 것을 보여주는 betWaitObj 오브젝트 활성화.
    IEnumerator betWait()
    {
        isTImeout = true;
        betWaitObj.SetActive(true);
        yield return new WaitUntil(() => isDone);
        betWaitObj.SetActive(false);
    }

    //아이템사용 턴일때 사용할 아이템을 선택하는 itemObj 오브젝트 활성화.
    IEnumerator itemUse()
    {
        itemObj.SetActive(true);
        twoBtn.SetActive(true);
        if (itemUseCount == 0)
        {
            timerManager.setTimer(0);
        }
        else
            timerManager.setTimer(10);
        yield return new WaitUntil(() => isDone);
        itemObj.SetActive(false);
        twoBtn.SetActive(false);
    }

    //캐릭터 이동 전 사용된 아이템들을 사용하여 효과를 적용하는 코루틴.
    IEnumerator itemAction()
    {
        itemShowObj.SetActive(true);
        yield return new WaitUntil(() => isDone);
        isDone = false;
        itemShowObj.SetActive(false);

        for (int i = 0; i < useItemList.Length; i++)
        {
            if(useItemList[i] == "Shield")
            {
                characterManagers[i].isShield = true;
                nameCardManager.nameCardInfo[i].itemUse(0);
                characterManagers[i].shieldObj.SetActive(true);
                yield return new WaitForSeconds(1.0f);
            }
        }

        for(int i=0; i< useItemList.Length; i++)
        {
            if(useItemList[i] == "Lightning")
            {
                nameCardManager.nameCardInfo[i].itemUse(1);
                bool isTwo = false;
                Destroy(itemEffect(i, 1),1.0f);
                yield return new WaitForSeconds(1.0f);

                foreach (int list in getRank(0))
                {
                    if (!isTwo)
                    {
                        isTwo = true;
                        GameObject temp = Instantiate(itemObjs[7]) as GameObject;
                        temp.transform.SetParent(tileManager.tiles[characterManagers[list].myPosition].transform);
                        temp.transform.localPosition = Vector3.zero + new Vector3(0, 3, 0);
                        temp.transform.localRotation = Quaternion.Euler(new Vector3(0, -45, 0));
                        temp.transform.localScale = new Vector3(10, 8, 1);
                        yield return new WaitForSeconds(1.0f);
                        Destroy(temp);
                    }
                    if (characterManagers[list].isShield)
                    {
                        characterManagers[list].isLightning = false;
                        characterManagers[list].isShield = false;
                        characterManagers[list].shieldObj.SetActive(false);
                        Destroy(itemEffect(list, 6),1.0f);
                    }
                    else
                    {
                        characterManagers[list].shockObj.SetActive(true);
                        characterManagers[list].isLightning = true;
                    }
                }
                yield return new WaitForSeconds(1.0f);
            }
        }
        for (int i = 0; i < useItemList.Length; i++)
        {
            if (useItemList[i] == "Magnet")
            {
                setRank();
                int myRank = nameCardManager.nameCardInfo[i].userRank;
                int target = getNextRank(myRank);
                nameCardManager.nameCardInfo[i].itemUse(2);

                if (!characterManagers[i].isLightning && target!=-1)
                {
                    //GameObject temp1 = itemEffect(i, 2);
                    characterManagers[i].magnetObj.SetActive(true);
                    GameObject temp2 = itemEffect(target, 9);

                    if (characterManagers[target].isShield)
                    {
                        //Destroy(temp1, 0.5f);
                        Destroy(temp2, 0.5f);
                        yield return new WaitForSeconds(0.5f);
                        characterManagers[target].isShield = false;
                        characterManagers[target].shieldObj.SetActive(false);
                        Destroy(itemEffect(target, 6), 1.0f);
                        characterManagers[i].magnetObj.SetActive(false);
                        yield return new WaitForSeconds(1.0f);
                    }
                    else
                    {
                        yield return new WaitForSeconds(1.0f);
                        int moveNum = characterManagers[target].myPosition - characterManagers[i].myPosition;
                        chars[i].SendMessage("move", moveNum, SendMessageOptions.DontRequireReceiver);
                        yield return new WaitUntil(() => isDone);
                        //Destroy(temp1, 0.5f);
                        characterManagers[i].magnetObj.SetActive(false);
                        Destroy(temp2, 0.5f);
                    }
                }

            }
        }
        for (int i = 0; i < useItemList.Length; i++)
        {
            if (useItemList[i] == "Boost" && !characterManagers[i].isLightning)
            {
                nameCardManager.nameCardInfo[i].itemUse(3);
                characterManagers[i].isBoost = true;
                characterManagers[i].boostObj.SetActive(true);

                yield return new WaitForSeconds(1.0f);
            }
        }
        for (int i = 0; i < useItemList.Length; i++)
        {
            if (useItemList[i] == "Banana")
            {
                nameCardManager.nameCardInfo[i].itemUse(4);
                Destroy(itemEffect(i, 4),1.0f);
                yield return new WaitForSeconds(1.0f);
                if (!tileManager.tiles[characterManagers[i].myPosition].isBanana)
                {
                    tileManager.tiles[characterManagers[i].myPosition].isBanana = true;
                    GameObject banana = Instantiate(tiledBanana) as GameObject;
                    bananas.Add(banana);
                    banana.transform.SetParent(tileManager.tiles[characterManagers[i].myPosition].positionHolder);
                    banana.transform.localPosition = Vector3.zero + new Vector3(0,1.0f,0);
                    banana.transform.localRotation = Quaternion.Euler(0, -45, 0);
                    banana.GetComponent<SpriteRenderer>().enabled = true;
                }
            }
        }
        for (int i = 0; i < useItemList.Length; i++)
        {
            if (useItemList[i] == "Mark" && !characterManagers[i].isLightning)
            {
                nameCardManager.nameCardInfo[i].itemUse(5);
                characterManagers[i].isMark = true;
                characterManagers[i].markObj.SetActive(true);
                yield return new WaitForSeconds(1.0f);
            }
        }
        
        moveEndNum = 0;
        sendMsgToServer(PROTOCOL.ITEM_USED);

        for (int i = 0; i < 4; i++)
            useItemList[i] = string.Empty;

        yield return new WaitForSeconds(1.0f);
        myState = State.alertShow;
    }
    
    //각 캐릭터가 이동할 숫자를 애니메이션으로 보여주는 코루틴.
    IEnumerator moveNumShow()
    {
        cardShowObj.SetActive(true);
        yield return new WaitUntil(()=>isDone);
        cardShowObj.SetActive(false);
    }

    //각 캐릭터에게 이동할 칸수를 보내주는 코루틴.
    IEnumerator charMove()
    {
        for (int i=0; i<chars.Length; i++)
        {
            chars[i].SendMessage("move",moveNum[i,charMoveNum],SendMessageOptions.DontRequireReceiver);
        }
        charMoveNum++;
        yield return new WaitUntil(()=>moveEndNum >3);
        setRank();
        isDone = false;
        moveEndNum = 0;
        if (isArrive)
            myState = State.gameEnd;
        else
            myState = State.cardStepWait;
    }

    //캐릭터 이동이 끝날때까지 3초의 여유를 주는 코루틴.
    IEnumerator cardStepWait()
    {
        yield return new WaitForSeconds(3.0f);
        myState = State.cardStep;
    }

    //캐릭터가 한번 이동한후 카드를 밟았을 시 그에 따른 이동을 처리하는 코루틴.
    IEnumerator cardStep()
    {
        for (int i = 0; i < chars.Length; i++)
        {
            chars[i].SendMessage("cMove", SendMessageOptions.DontRequireReceiver);
        }
        yield return new WaitUntil(()=>moveEndNum >3);
        setRank();
        isDone = false;
        moveEndNum = 0;
        if (isArrive)
            myState = State.gameEnd;
        else if (charMoveNum > 2)
            myState = State.moveEnd;
        else
        {
            myState = State.moveNumShow;
            yield return new WaitForSeconds(0.5f);
        }
    }

    //정해진 횟수만큼 이동이 끝났을때 값들을 초기화하고 카드배치로 넘겨주는 코루틴.
    IEnumerator moveEnd()
    {
        cardSetNum = 0;
        charMoveNum = 0;
        for(int i=0; i<4; i++)
        {
            characterManagers[i].shieldObj.SetActive(false);
        }
        yield return new WaitForSeconds(3.0f);
        //각 말들 위치
        sendMsgToServer(PROTOCOL.UNIT_MOVED);
        myState = State.alertShow;
        yield return null;
    }

    //한명이 정해진 횟수만큼 시작지점을 지났을때 결과화면을 띄워주는 코루틴.
    IEnumerator gameEnd()
    {
        resultObj.SetActive(true);
        yield return new WaitForSeconds(8.0f);
        sendMsgToServer(PROTOCOL.GAME_OVER);
        resultObj.SetActive(false);
        SceneManager.LoadScene(0);

    }
   
}
