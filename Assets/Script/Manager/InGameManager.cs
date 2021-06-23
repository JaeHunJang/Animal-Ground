using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameManager : MonoBehaviour 
{

    //매칭이 된후 실제 게임플레이에 필요한 모든 요소를 담당하는 싱글톤패턴 매니저 클래스
    private static InGameManager inGameManager = null;
    MainManager mainManager = MainManager.Instance;

    public TileManager tileManager;
    public NameCardManager nameCardManager;

    
    public BettingManager bettingManager;
    public GameObject betManagerObj;
    public GameObject selectAction;
    public GameObject selectCardToPlace;
    public GameObject selectCardToMove;
    
    public State myState;
    public Card[] cardList = new Card[5];
    public Card card;
    public GameObject cardPrefab;
    public GameObject nameCardManagerObj;


    GameObject[] characters = new GameObject[4];
    CharacterManager[] characterManagers = new CharacterManager[4];


    //List<Participant> participants;


    //코루틴은 대상+동사 , 메소드는 동사 + 대상

    public enum State
    {
        idle,

        cardSelect,
        tileSelect,
        moveSelect,
        characterMove,
        cardStep,

        cardBatch,

        turnEnd,
        roundCheck,

        
    }

    public static InGameManager Instance
    {
        get { return inGameManager; }
    }

    private void Awake()
    {
        if (inGameManager == null)
            inGameManager = this;
    }



    public int firstPlayer { get; set; } = 0;

    public bool isMaster { get; set; } = false;
    public bool isMyTurn { get; set; } = false;
    public bool isDone { get; set; } = false;

    public int myNum { get; set; } = 2;

    public int nextPlayer { get; set; } = 0;

    public int myPosition { get; set; } = 0;
    public int moveNum { get; set; } = 0;
    public int markNum { get; set; } = 0;

    public int currentTurn { get; set; } = 1;
    public int currentRound { get; set; } = 1;

    public int betRound { get; set; } = 1;
    public int betUserNum { get; set; } = 0;
    public int betNum { get; set; } = 0;

    public int roundCycle { get; set; } = 1;
    private void Start()
    {
        StartCoroutine(allPlayerReady());
        //StartCoroutine(tileSelect());
        //StartCoroutine(cardSelect());
        //StartCoroutine(myTurnStart());
    }



    //MainManager 호출용 메소드 시작
    public void setFirst(int first)
    {
        firstPlayer = first;
        isDone = true;
    }

    public void setCharacter(int charNumber , int userNumber)
    {
        characters[userNumber] = Instantiate(MainManager.Instance.characterList[charNumber]);
        characters[userNumber].transform.SetParent(tileManager.tiles[0].holders[userNumber].transform);
        characters[userNumber].transform.localPosition = Vector3.zero + new Vector3(0, 0.2f, 0);
        characters[userNumber].transform.Rotate(0, 180, 0);
        characters[userNumber].transform.localScale = new Vector3(2, 2, 2);
        characters[userNumber].AddComponent<CharacterManager>();
        characterManagers[userNumber] = characters[userNumber].GetComponent<CharacterManager>();
    }

    public void moveCharacter(int userNumber, int startPosition, int moveNumber , int markNumber)
    {
        StartCoroutine(characterTransfer(userNumber, startPosition, moveNumber,markNumber));
    }

    public void setCard(int userNumber, int tileNumber , int cardNumber)
    {
        card = cardList[cardNumber];

        TileInfo target;
        target = tileManager.tiles[tileNumber].GetComponent<TileInfo>();
        createCard(target);
        changeMat(target, userNumber);
        if (card.cardNumber == 0)
            target.isTiled = false;
        else
            target.isTiled = true;
        target.card = card;

    }

    public void createCard(TileInfo target)
    {
        GameObject cardObj = Instantiate(cardPrefab, target.cardHolder);
        cardObj.GetComponent<RectTransform>().sizeDelta = new Vector2(42f, 64f);
        cardObj.GetComponent<Image>().sprite = card.cardImage;
    }

    public void changeMat(TileInfo target , int userNum)
    {
        target.changeMat(userNum);
    }



    public void startMyTurn(int player)
    {
        currentTurn++;
        currentRound = (currentTurn-1) / 4 + 1;
        if (player == myNum)
        {
            StartCoroutine(myTurnStart());
        }
    }


    public void startBet(int betNumber)
    {
        betNum = betNumber;
        betRound += roundCycle;
        StartCoroutine(betStart());
    }

    public void produceBet()
    {
        StartCoroutine(betProduce());
    }

    public void setRank()
    {
        for(int i=0; i<characters.Length; i++)
        {
            int rank = 0;
            for(int j =0; j<characters.Length; j++)
            {
                if (characterManagers[i].myPosition < characterManagers[j].myPosition)
                    rank++;
            }
            nameCardManager.nameCardInfo[i].userRank = rank;
        }
    }

    //MainManager 호출용 메소드 끝

        /*
    public void sendMsgToServer(string msg)
    {
        CMainTitle.instance.send(msg);
    }
    */
    //각 행동별 코루틴 시작
    IEnumerator allPlayerReady()
    {
        yield return new WaitForSeconds(3.0f);
        tileManager.tiles[0].nowColor = 6;
        tileManager.tiles[0].changeMat(6);

        myNum = CMainTitle.instance.player_index;


        /*
        if (myNum != 3)
            nextPlayer = myNum + 1;
        else
            nextPlayer = 0;
        
        
        if (myNum == 0)
            isMaster = true;
        
        */

        /*
        //다른 플레이어들에게 내 캐릭터를 생성하라 알림
        string data = "CharSet" + ":" + MainManager.Instance.myCharacter + ":" + myNum;
        sendMsgToServer(data);
        
        if (isMaster)
            StartCoroutine(firstPlayerSelect());
        */

        nameCardManagerObj.SetActive(true);

        StartCoroutine(gameStart());

    }

    IEnumerator gameStart()
    {
        /*
         * 선택된 캐릭터 알려줌
         */

        yield return null;

    }


    IEnumerator cardBatch()
    {



        yield return null;
    }


    /*
    IEnumerator firstPlayerSelect()
    {
        yield return new WaitForSeconds(1.0f);
        //0~3 중 하나를 뽑아 해당 숫자의 플레이어가 선플레이어임을 알림
        int rnd = Random.Range(0, 3);
        string data = "First" + ":" + rnd.ToString();
        sendMsgToServer(data);

        yield return null;
    }
    */

    /*
    IEnumerator gameStart()
    {
        
        yield return new WaitUntil(() => isDone);
        isDone = false;
        

        
         선 플레이어 선택 애니메이션 효과 추가예정
        


        
        if (myNum == firstPlayer)
            StartCoroutine(myTurnStart());
        
    }
    */

    IEnumerator myTurnStart()
    {
        yield return new WaitForSeconds(2.0f);
        //턴 시작 애니메이션 추가예정
        isMyTurn = true;
        myState = State.roundCheck;

        while (isMyTurn)
        {
            isDone = false;
            yield return null;
            yield return StartCoroutine(myState.ToString());
        }
    }

    IEnumerator idle()
    {
        selectAction.SetActive(true);
        yield return new WaitUntil(() => isDone);
        selectAction.SetActive(false);
    }

    IEnumerator cardSelect()
    {
        selectCardToPlace.SetActive(true);
        yield return new WaitUntil(() => isDone);
        selectCardToPlace.SetActive(false);
    }

    IEnumerator tileSelect()
    {
        int unSelectable = 5;
        //변경전 타일 색상 저장
        int[] tileColors = new int[tileManager.tiles.Length];
        TileInfo target = null;
        int targetNum = 0;

        for (int i = 0; i < tileManager.tiles.Length; i++)
        {
            TileInfo _tile = tileManager.tiles[i];
            tileColors[i] = _tile.nowColor;
            
            //숫자가 0이 아니고 카드가 이미 깔려있다면 선택불가
            if (card.cardNumber != 0 && _tile.isTiled)
            {
                _tile.changeMat(unSelectable);
                _tile.tileCollider.enabled = false;
            }
        }
        while(!isDone)
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

                    for(int i =0; i < tileManager.tiles.Length; i++)
                    {
                        if (target.name.Equals(tileManager.tiles[i].name))
                            targetNum = i;
                    }
                    target.card = card;
                    isDone = true;
                }
            }
            
            yield return null;
        }

        for (int i = 0; i < tileManager.tiles.Length; i++)
        {
            TileInfo _tile = tileManager.tiles[i];
            _tile.changeMat(tileColors[i]);
            _tile.tileCollider.enabled = true;
        }

        int cardListNum = card.cardNumber + 2;

        string data = "CardSet" + ":" + myNum + ":" + targetNum + ":" + cardListNum;

        //sendMsgToServer(data);


        myState = State.moveSelect;

    }

    IEnumerator moveSelect()
    {
        selectCardToMove.SetActive(true);
        yield return new WaitUntil(() => isDone);
        selectCardToMove.SetActive(false);
    }

    IEnumerator characterMove()
    {
        string data = "CharMove" + ":" + myNum + ":" + myPosition + ":" + moveNum + ":" + markNum;
        //sendMsgToServer(data);

        yield return new WaitUntil(() => isDone);
        myPosition += (markNum * moveNum);
        myState = State.cardStep;
    }

    IEnumerator characterTransfer(int userNumber, int startPosition, int moveNumber , int mark)
    {
        Transform target;
        Transform moveChar;
        float speed = 3.0f;

        for (int index = 0; index <moveNumber; index++)
        {
            startPosition += mark;
            if (startPosition < 0)
                break;
            target = tileManager.tiles[startPosition].holders[userNumber].transform;
            Vector3 targetPos = target.position;
            targetPos += new Vector3(0, 0.2f, 0);

            moveChar = characters[userNumber].transform;

            while (Vector3.Distance(moveChar.position, targetPos) > 0.01f)
            {
                moveChar.position = Vector3.MoveTowards(moveChar.position, targetPos, Time.deltaTime * speed);
                yield return null;
            }
            moveChar.position = target.position + new Vector3(0,0.2f,0);
            moveChar.SetParent(target);
        }

        characters[userNumber].GetComponent<CharacterManager>().myPosition += (moveNumber * mark);
        setRank();
        isDone = true;
        yield return null;
    }


    IEnumerator cardStep()
    {

        if(tileManager.tiles[myPosition].isTiled)
        {
            moveNum = tileManager.tiles[myPosition].card.cardNumber;

            if (moveNum > 0)
                markNum = 1;
            else
                markNum = -1;

            moveNum = Mathf.Abs(moveNum);
        
            string data = "CharMove" + ":" + myNum + ":" + myPosition + ":" + moveNum + ":" + markNum;
            //sendMsgToServer(data);


            yield return new WaitUntil(() => isDone);
            myPosition += (markNum * moveNum);
        }
        myState = State.turnEnd;
        yield return null;

    }


    IEnumerator turnEnd()
    {
        string data = "TurnEnd" + ":" + nextPlayer;
       // sendMsgToServer(data);

        isMyTurn = false;
        yield return null;
    }

    IEnumerator roundCheck()
    {
        if (currentRound == betRound)
        {
            if (betRound != roundCycle)
            {
                //결과창팝업 및 배팅결과처리
                string data2 = "BetProduce" + ":" + betNum;
                //sendMsgToServer(data2);

                yield return new WaitUntil(() => isDone);
                isDone = false;
            }


            betNum = Random.Range(0, 3);
            
            
            string data = "BetStart" + ":" + betNum;
            //sendMsgToServer(data);

            yield return new WaitUntil(() => isDone);
            /*
            byte[] message = System.Text.Encoding.Default.GetBytes(data);
            PlayGamesPlatform.Instance.RealTime.SendMessageToAll(true, message);
            */
            //yield return StartCoroutine(betStart());

        }
        else
        {
            myState = State.idle;
        }
        yield return null;
    }


    IEnumerator betStart()
    {
        betManagerObj.SetActive(true);
        //bettingManager.changeNumber(betNum);
        yield return new WaitUntil(() => isDone);
        betManagerObj.SetActive(false);
        Debug.Log("내가선택 :" + betUserNum + "결과:" + betNum);
    }

    IEnumerator betProduce()
    {
        betManagerObj.SetActive(true);
        Debug.Log("내가선택 :" + betUserNum + "결과:" + betNum);
        yield return new WaitForSeconds(3.0f);
        //bettingManager.isResult = false;
        betManagerObj.SetActive(false);
        isDone = true;
    }
    
    //각 행동별 코루틴 끝

}
