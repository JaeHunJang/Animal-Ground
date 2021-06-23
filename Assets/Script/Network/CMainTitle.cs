using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FreeNet;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CMainTitle : MonoBehaviour, IMessageReceiver
{

    enum USER_STATE
    {
        NOT_CONNECTED,
        CONNECTED,
        WAITING_MATCHING
    }

    public int player_index { get; set; }

    public int tileNum = 24;
    public string[] nickName = new string[4];
    public int[] selectedChar = new int[4];
    public int[,] setCard;
    public int[,] moveNum = new int[4, 3];
    public int[] itemNums;

    string[] itemNames = new string[6]{"Shield","Lightning","Magnet","Boost","Banana","Mark"};

    [SerializeField]
    CNetworkManager network_manager;
    USER_STATE user_state;

    /*
    public Text text;//test용 로그 확인 객체
    public Text log;
    public Text index;
    */
    

    public static CMainTitle instance;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
    }


    // Use this for initialization
    void Start()
    {

        instance = this;
        DontDestroyOnLoad(gameObject);
        
        this.user_state = USER_STATE.NOT_CONNECTED;

        this.network_manager = GetComponent<CNetworkManager>();
        
    }

    public void enter()
    {
        StopCoroutine("after_connected");

        this.network_manager.message_receiver = this;

        if (!this.network_manager.is_connected())
        {
            this.user_state = USER_STATE.CONNECTED;
            this.network_manager.connect();
        }
        else
        {
            on_connected();
        }
    }

    /// <summary>
    /// 서버에 직접 메시지를 보내기 위한 메소드
    /// </summary>
    //public void send()
    //{
    //    CPacket msg = CPacket.create((short)PROTOCOL.MOVING_REQ);
    //    //msg.push(text.text.ToString());
    //    this.network_manager.send(msg);
    //}

    public void send(PROTOCOL protocol)
    {
        CPacket msg = CPacket.create((short)protocol);
        this.network_manager.send(msg);
    }

    public void send(int[] data ,PROTOCOL protocol)
    {
        CPacket msg = CPacket.create((short)protocol);
        for (int i = 0; i < data.Length; i++)
            msg.push(data[i]);
        this.network_manager.send(msg);
    }
    public void send(CPacket msg)
    {
        this.network_manager.send(msg);
    }

    public void matchStart()
    {
        enter();
        setCard = new int[tileNum, 3];
    }


    /// <summary>
    /// 서버에 접속된 이후에 처리할 루프.
    /// </summary>
    IEnumerator after_connected()
    {
        yield return new WaitForEndOfFrame();

        while (true)
        {
            Debug.Log("running...");
            if (this.user_state == USER_STATE.CONNECTED)
            {
                this.user_state = USER_STATE.WAITING_MATCHING;
                CPacket msg;
                if (tileNum == 24)
                { 
                    msg = CPacket.create((short)PROTOCOL.ENTER_GAME_ROOM_REQ);
                }else
                {
                    msg = CPacket.create((short)PROTOCOL.ENTER_GAME_ROOM_REQ2);
                }
                this.network_manager.send(msg);

                StopCoroutine("after_connected");
            }
           
            yield return 0;
        }
    }


    /// <summary>
    /// 서버에 접속이 완료되면 호출됨.
    /// </summary>
    public void on_connected()
    {
        this.user_state = USER_STATE.CONNECTED;

        StartCoroutine("after_connected");
    }


    /// <summary>
    /// 패킷을 수신 했을 때 호출됨.
    /// </summary>
    void IMessageReceiver.on_recv(CPacket msg)
    {
        // 제일 먼저 프로토콜 아이디를 꺼내온다.
        Debug.Log("Recv / " + (PROTOCOL)msg.protocol_id);
        PROTOCOL protocol_id = (PROTOCOL)msg.pop_protocol_id();
        switch (protocol_id)
        {
            /// <summary> 매칭이 완료되었을시 기본정보를 서버로 보내고 다음 씬으로 변경
            case PROTOCOL.START_LOADING:
                {
                    this.player_index = msg.pop_int32(); //GameRoom에서 설정해준 플레이어의 Index값을 받아온다.
                    if (tileNum == 24)
                    {
                        SceneManager.LoadScene(1);
                    }
                    else if (tileNum == 16)
                    {
                        SceneManager.LoadScene(2);
                    }

                    itemNums = MainManager.Instance.selectedItems;

                    for (int i = 0; i < itemNums.Length; i++)
                    {
                        if(itemNums[i] !=-1)
                        MySqlConnector.Instance.doNonQuery("update stored_item set " + itemNames[itemNums[i]] + "=" + itemNames[itemNums[i]] + "-1 where account_id = '" + MainManager.Instance.myID + "';");
                    }
                    CPacket resMsg = CPacket.create((short)PROTOCOL.LOADING_COMPLETED);
                    resMsg.push(MainManager.Instance.myNickName.text);
                    resMsg.push(MainManager.Instance.myCharacter);
                    this.network_manager.send(resMsg);

                   
                }
                break;
            /// <summary> 서버로부터 기본정보를 받아 각 데이터들 초기화
            case PROTOCOL.GAME_START:
                {
                    for (int i = 0; i < 4; i++)
                    {
                        int userIndex = msg.pop_int32();
                        ProgressManager.Instance.nicknames[userIndex] = msg.pop_string();
                        int charNum = msg.pop_int32();
                        ProgressManager.Instance.setCharacter(charNum, userIndex);
                    }

                    ProgressManager.Instance.setPivot();
                    ProgressManager.Instance.allPlayerReady();
                    CPacket resMsg2 = CPacket.create((short)PROTOCOL.GAME_INIT);
                    resMsg2.push(tileNum);
                    resMsg2.push(3);
                    this.network_manager.send(resMsg2);
                }
                break;
            /// <summary> 캐릭터 이동 턴에서 각 캐릭터가 몇칸 이동해야하는지 서버로부터 받음
            case PROTOCOL.UNIT_MOVE:
                {
                    for (int i = 0; i < moveNum.GetLength(0); i++)
                        for (int j = 0; j < moveNum.GetLength(1); j++)
                            ProgressManager.Instance.moveNum[i, j] = msg.pop_int32();
                    ProgressManager.Instance.myState = ProgressManager.State.alertShow;
                    ProgressManager.Instance.isDone = true;
                }
                break;
            /// <summary> 각 유저들이 카드를 세팅한 정보를 서버로 전송
            case PROTOCOL.CARD_SET:
                {
                    //타일수 24개, 3개의 파라미터를 받으므로 24,3 배열에 집어넣기
                    for (int i =0; i<setCard.GetLength(0); i++)
                    {
                        for(int j=0; j<setCard.GetLength(1); j++)
                        {
                            setCard[i, j] = msg.pop_int32();
                        }
                    }

                    for(int i=0; i<setCard.GetLength(0); i++)
                    {
                        ProgressManager.Instance.setCards(setCard[i, 0], setCard[i, 1], setCard[i, 2]);
                    }

                    ProgressManager.Instance.cardSetComplete();
                }
                break;
            /// <summary> 서버에 전송된 각 유저들이 사용한 아이템 정보를 가져옴
            case PROTOCOL.ITEM_USE:
                {
                    int userNum;
                    int itemNum;
                    string itemName = "";
                    for(int i=0; i<4; i++)
                    {
                        userNum = msg.pop_int32();
                        itemNum = msg.pop_int32();

                        if (itemNum >= 0 && itemNum < 6)
                            itemName = itemNames[itemNum];
                        else
                            itemName = "";
                        if (!itemName.Equals(""))
                        {
                            ProgressManager.Instance.useItemList[userNum] = itemName;
                        }
                    }
                    ProgressManager.Instance.myState = ProgressManager.State.itemAction;
                    ProgressManager.Instance.isDone = true;
                }
                break;
        }

    }


}
