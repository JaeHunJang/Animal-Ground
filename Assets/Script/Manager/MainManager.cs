using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;


public class MainManager : MonoBehaviour
{
    //게임 전체적인 데이터를 관리 및 매칭을 담당하는 매니저 클래스
    private static MainManager mainManager = null;

    public GameObject[] characterList = new GameObject[15];
    public GameObject[] bearList = new GameObject[4];
    public GameObject[] rabbitList = new GameObject[4];
    public GameObject[] catList = new GameObject[4];
    public Sprite[] levelImageList = new Sprite[20];
    public Sprite[] rankImageList = new Sprite[4];
    public Sprite[] CheckImage = new Sprite[2];

    public String myID;
    private String setNickName;
    private String setID;
    private String setPW;
    private String ID;
    private String PW;
    public bool check = false;
    public bool NickNameCheck = false;
    public bool PWCheck = false;
    private bool loginCheck;
    public GameObject MainWindow;
    public GameObject nickNameWindow;
    public GameObject LoginWindow;
    public GameObject JoinWindow;
    public GameObject GameLoadingWindow;
    public GameObject MessageWindow;
    public GameObject LoadingWindow;

    private int characterNum;
    public Text txt;

    GameObject character;

    private MySqlDataReader reader;
    public Text myNickName;
    public InputField setNickNameText;
    public Text msgNickName;
    public Text msgJoin;
    public Text msgLogin;
    public Text moneyTxt;
    public Text cashTxt;
    public GameObject characterParent;
    public InputField setIDText;
    public InputField setPWText;
    public InputField setPWCheck;
    public InputField IDText;
    public InputField PWText;
    public Image levelImage;
    public Text levelText;
    public Slider expBar;
    public Text expText;
    public Image IDCheckImage;
    public Image NickNameCheckImage;
    public Image PWCheckImage;
    public Button JoinBtn;
    public Image myRankImage;
    public Text myRankText;

    public Text[] RankNickName = new Text[3];
    public Text[] RankLevel = new Text[3];
    public Image[] RankLevelImage = new Image[3];

    public Text tet;

    private bool isDone { get; set; } = false;
    private bool isJoinDone { get; set; } = false;
    public bool isFirst { get; set; } = true;

    public static MainManager Instance;
    public GameObject NetworkManager;
    public int[] selectedItems = new int[2] { -1, -1 };
    public int[] itemNums = new int[6];

    const int ITEM_NUM = 6;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void setEnd()
    {
        if (PlayerPrefs.HasKey("ID"))
        {
            myID = PlayerPrefs.GetString("ID");
            reader = MySqlConnector.Instance.doQuery("select count(*) from account where account_id = '" + myID + "'");
            reader.Read();

            if (int.Parse(reader[0].ToString()) == 0)
            {
                reader.Close();
                StartCoroutine(Login());
            }
            else
            {
                reader.Close();

                reader = MySqlConnector.Instance.doQuery("select * from account where account_id = '" + myID + "'");
                reader.Read();
                loginCheck = bool.Parse(reader[10].ToString());

                if (loginCheck)
                {
                    MessageWindow.SetActive(true);
                    reader.Close();
                    return;
                }

                reader.Close();

                StartCoroutine(GetData());
            }
        }
        else
        {
            StartCoroutine(Login());
        }
    }

    public void callGetDate()
    {
        StartCoroutine(GetData());
    }

    //로그인
    IEnumerator Login()
    {
        LoginWindow.SetActive(true);
        yield return new WaitUntil(() => isDone);
        isDone = false;       
        myID = ID;

        PlayerPrefs.SetString("ID", ID);
        LoginWindow.SetActive(false);
        StartCoroutine(GetData());
        yield return null;
    }

    //회원가입
    IEnumerator Join()
    {
        LoginWindow.SetActive(false);
        JoinWindow.SetActive(true);
        yield return new WaitUntil(() => isJoinDone);
        isJoinDone = false;
        check = false;
        JoinWindow.SetActive(false);
        LoginWindow.SetActive(true);
        yield return null;
    }

    //데이터를 불러옴
    IEnumerator GetData()
    {
        reader = MySqlConnector.Instance.doQuery("select * from account where account_id = '" + myID + "'");
        reader.Read();
        myNickName.text = reader[1].ToString();
        moneyTxt.text = StringFormat(reader[2], "{0:#,#}");
        cashTxt.text = StringFormat(reader[4], "{0:#,#}");
        characterNum = int.Parse(reader[8].ToString());
               
        myCharacter = characterNum;
        int level = int.Parse(reader[6].ToString());
        int exp = int.Parse(reader[7].ToString());

        character = Instantiate(characterList[characterNum]);
        character.transform.SetParent(characterParent.transform);
        character.transform.localPosition = new Vector3(0, 0, 0);
        character.transform.Rotate(0, 180, 0);
        character.transform.localScale = new Vector3(1f, 1f, 1f);

        if(characterNum >= 0 && characterNum <=4)
        {
            for (int i = 0; i < 4; i++)
            {
                MainManager.Instance.bearList[i].SetActive(true);
            }
        }
        else if(characterNum >= 5 && characterNum <= 9)
        {
            for (int i = 0; i < 4; i++)
            {
                MainManager.Instance.rabbitList[i].SetActive(true);
            }
        }
        else if(characterNum >= 10 && characterNum <= 14)
        {
            for (int i = 0; i < 4; i++)
            {
                MainManager.Instance.catList[i].SetActive(true);
            }
        }

        if (exp >= 100)
        {
            level++;
            exp -= 100;
        }

        levelImage.GetComponent<Image>().sprite = getLevelImage(level);

        levelText.text = "Lv." + level.ToString();
        expBar.value = (float)exp / 100;

        reader.Close();

        MySqlConnector.Instance.doNonQuery("update account set account_level = '" + level + "' where account_id = '" + myID + "'");
        MySqlConnector.Instance.doNonQuery("update account set account_exp = '" + exp + "' where account_id = '" + myID + "'");

        //랭킹
        //내 랭킹
        reader = MySqlConnector.Instance.doQuery("select count(*) from account where account_level > " + level);
        reader.Read();
        int myRank = int.Parse(reader[0].ToString());

        switch(myRank+1)
        {
            case 1:
                myRankImage.GetComponent<Image>().sprite = rankImageList[0];
                myRankText.text = "";
                break;

            case 2:
                myRankImage.GetComponent<Image>().sprite = rankImageList[1];
                myRankText.text = "";
                break;

            case 3:
                myRankImage.GetComponent<Image>().sprite = rankImageList[2];
                myRankText.text = "";
                break;

            default:
                myRankImage.GetComponent<Image>().sprite = rankImageList[3];
                myRankText.text = (myRank+1).ToString();
                break;
        }
        reader.Close();

        //1,2,3위 랭킹
        reader = MySqlConnector.Instance.doQuery("select * from account order by account_level desc limit 3");

        int j = 0;

        while(reader.Read())
        {
            RankNickName[j].text = reader[1].ToString();
            RankLevel[j].text = "Lv." + reader[6].ToString();
            RankLevelImage[j].GetComponent<Image>().sprite = getLevelImage(int.Parse(reader[6].ToString()));
            j++;
        }

        reader.Close();
        MySqlConnector.Instance.doNonQuery("update account set account_login = 1 where account_id = '" + myID + "'");


        reader = MySqlConnector.Instance.doQuery("select account_item1 , account_item2 from account where account_id = '" + myID + "';");
        reader.Read();

        for (int i = 0; i < selectedItems.Length; i++)
            selectedItems[i] = int.Parse(reader[i].ToString());
        reader.Close();

        reader = MySqlConnector.Instance.doQuery("select * from stored_item where account_id = '" + myID + "';");
        reader.Read();

        //아이템 갯수 할당
        for (int i = 0; i < ITEM_NUM; i++)
        {
            itemNums[i] = int.Parse(reader[i + 1].ToString());
        }
        reader.Close();

        for(int i=0; i<ITEM_NUM; i++)
        {
            if (itemNums[i] == 0)
            {
                if (selectedItems[0] == i)
                {
                    selectedItems[0] = -1;
                    MySqlConnector.Instance.doNonQuery("update account set account_item1 = '-1' where account_id = '" + myID + "'");
                }
                else if (selectedItems[1] == i)
                {
                    selectedItems[1] = -1;
                    MySqlConnector.Instance.doNonQuery("update account set account_item2 = '-1' where account_id = '" + myID + "'");
                }
            }
        }

        yield return null;
    }

    public Sprite getLevelImage(int level)
    {

        if (level-1 < 20)
            return levelImageList[level];
        else
            return levelImageList[19];
    }

    //닉네임 생성
    /*
    IEnumerator MakeNickName()
    {
        nickNameWindow.SetActive(true);
        yield return new WaitUntil(() => isDone);
        nickNameWindow.SetActive(false);
        MySqlConnector.Instance.doNonQuery("update account set account_nickname = '" + setNickName + "' where account_id = '" + myID + "'");
        StartCoroutine(GetData());
        yield return null;
    }
    */

    //로그인시 아이디 비밀번호 확인
    public void CheckID()
    {
        ID = IDText.text;
        PW = PWText.text;
        reader = MySqlConnector.Instance.doQuery("select count(*) from account where account_id = '" + ID + "'" + " and account_pw = '" + PW + "'");
        reader.Read();

        if (int.Parse(reader[0].ToString()) == 1)
        {
            reader.Close();
            reader = MySqlConnector.Instance.doQuery("select * from account where account_id = '" + ID + "'");
            reader.Read();
            int level = int.Parse(reader[6].ToString());
            loginCheck = bool.Parse(reader[10].ToString());

            if (loginCheck)
            {
                LoginWindow.SetActive(false);
                MessageWindow.SetActive(true);
                reader.Close();

                return;
            }
            isDone = true;

            if(level == 0 )
            {
                reader.Close();
                //튜토리얼

            }
        }
        else
        {
            msgLogin.text = "ID/PW가 틀립니다.";
        }
        reader.Close();
    }

    //회원가입 버튼
    public void JoinBtnEvent()
    {
        StartCoroutine(Join());
    }

    //회원가입시 아이디 중복 확인
    public void CheckJoinID()
    {
        setID = setIDText.text;
        reader = MySqlConnector.Instance.doQuery("select count(*) from account where account_id = '" + setID + "'");
        reader.Read();
        if (int.Parse(reader[0].ToString()) == 1)
        {
            IDCheckImage.GetComponent<Image>().sprite = CheckImage[1];
            check = false;
        }
        else
        {
            IDCheckImage.GetComponent<Image>().sprite = CheckImage[0];
            check = true;
        }
        reader.Close();
        BlankCheck();
    }

    //닉네임 생성시 중복 확인
    public void CheckNickName()
    {
        setNickName = setNickNameText.text;

        reader = MySqlConnector.Instance.doQuery("select count(*) from account where account_nickname = '" + setNickName + "'");
        reader.Read();

        if (int.Parse(reader[0].ToString()) == 1)
        {
            NickNameCheckImage.GetComponent<Image>().sprite = CheckImage[1];
            NickNameCheck = false;
        }
        else
        {
            NickNameCheckImage.GetComponent<Image>().sprite = CheckImage[0];
            NickNameCheck = true;
        }
        reader.Close();
        BlankCheck();
    }

    //비밀번호, 비밀번호 확인 칸이 같은지 체크
    public void CheckPassword()
    {
        if(setPWText.text != setPWCheck.text)
        {
            PWCheckImage.GetComponent<Image>().sprite = CheckImage[1];
            PWCheck = false;
        }
        else
        {
            PWCheckImage.GetComponent<Image>().sprite = CheckImage[0];
            PWCheck = true;
        }
        BlankCheck();
    }

    //회원가입시 체크
    public void BlankCheck()
    {
        if (check == true && NickNameCheck == true && PWCheck == true)
        {
            JoinBtn.GetComponent<Button>().interactable = true;
        }
        else
        {
            JoinBtn.GetComponent<Button>().interactable = false;
        }
    }

    public void OkBtn()
    {
        if(check == true)
        {
            isDone = true;
        }
    }

    //회원가입 완료 버튼
    public void JoinOkBtn()
    {
        isJoinDone = true;
        setID = setIDText.text;
        setPW = setPWText.text;
        setNickName = setNickNameText.text;
        
        MySqlConnector.Instance.doNonQuery("insert into account values ('" + setID + "','" + setNickName + "',1000,1000,100,100,1,0,0,'" + setPW + "',0,-1,-1)");
        // 아이디, 닉네임,현재 돈, 누적 돈, 현재 캐쉬, 누적 캐쉬, 현재 포인트, 최고 포인트, 레벨,경험치, 캐릭터, 비밀번호
        MySqlConnector.Instance.doNonQuery("insert into stored_item(account_id) values ('" + setID + "')");

        setIDText.text = "";
        setPWText.text = "";
        setNickNameText.text = "";
        setPWCheck.text = "";

    }

    public void JoinCancelBtn()
    {
        isJoinDone = true;
    }
    
    //게스트 로그인 버튼
    public void GuestBtn()
    {
        ID = SystemInfo.deviceUniqueIdentifier;
        reader = MySqlConnector.Instance.doQuery("select count(*) from account where account_ID = '"+ ID +"'");
        reader.Read();

        if (int.Parse(reader[0].ToString()) == 1)
        {
            reader.Close();
            GetData();
            isDone = true;
            return;
        }
        reader.Close();
        reader = MySqlConnector.Instance.doQuery("select count(*) from account where account_nickname like 'guest%'");
        reader.Read();
        int cnt = int.Parse(reader[0].ToString());
        reader.Close();
        MySqlConnector.Instance.doNonQuery("insert into account values ('" + ID + "','guest" + cnt + "',1000,1000,100,100,1,0,0,'',0,-1,-1)");
        MySqlConnector.Instance.doNonQuery("insert into stored_item(account_id) values ('" + ID + "')");
                                                                             // 아이디, 닉네임,현재 돈, 누적 돈, 현재 캐쉬, 누적 캐쉬, 현재 포인트, 최고 포인트, 레벨,경험치, 캐릭터, 비밀번호

        isDone = true;
    }

    //계정 로그아웃버튼
    public void LogoutBtn()
    {
        MySqlConnector.Instance.doNonQuery("update account set account_login = 0 where account_id = '" + myID + "'");
        myID = "";
        PlayerPrefs.DeleteKey("ID");
        Destroy(gameObject);
        Destroy(NetworkManager);
        SceneManager.LoadScene(0);
    }

    //회원탈퇴 버튼
    public void WithdrawBtn()
    {
        PlayerPrefs.DeleteKey("ID");
        MySqlConnector.Instance.doNonQuery("delete from stored_item where account_id = '" + myID + "'");
        MySqlConnector.Instance.doNonQuery("delete from account where account_id = '" + myID + "'");
        myID = "";
        Destroy(gameObject);
        Destroy(NetworkManager);
        SceneManager.LoadScene(0);

    }

    //메세지 확인 버튼
    public void MessageOKBtn()
    {
        if(PlayerPrefs.HasKey("ID"))
        {
            PlayerPrefs.DeleteKey("ID");
        }
        myID = "";
        //Destroy(gameObject);
        //Destroy(NetworkManager);
        MessageWindow.SetActive(false);
        StartCoroutine(Login());
    }

    public int myCharacter { get; set; } = 0;

    private void OnDisable()
    {
        MySqlConnector.Instance.doNonQuery("update account set account_login = 0 where account_id = '" + myID + "'");
    }
    private void OnApplicationQuit()
    {
        MySqlConnector.Instance.doNonQuery("update account set account_login = 0 where account_id = '" + myID + "'");
    }

    public string StringReplace(string str, string oldchar, string newchar)
    {
        String temp = str;
        return temp.Replace(oldchar, newchar);
    }
    public string StringFormat(object value, string pattern)
    {
        return String.Format(pattern, value);
    }
}
