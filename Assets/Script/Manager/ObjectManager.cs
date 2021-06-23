using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ObjectManager : MonoBehaviour
{

    //메인창
    public Text Main_________;
    public Text myNickName;
    public Text moneyTxt;
    public Text cashTxt;
    public GameObject characterParent;
    public Image levelImage;
    public Text levelText;
    public Slider expBar;
    public Text expText;

    public GameObject mainWindow;
    public GameObject nickNameWindow;
    public GameObject loginWindow;
    public GameObject joinWindow;
    public GameObject gameLoadingWindow;
    public GameObject messageWindow;
    public GameObject loadingWindow;

    public GameObject modeSelect;
    
    //랭킹
    public Text Ranking______________;
    public Sprite[] rankImageList = new Sprite[4];
    public Image myRankImage;
    public Text myRankText;

    public Text First________________;
    public Text FirstNickName;
    public Text FirstLevel;
    public Image FirstLevelImage;

    public Text Second________________;
    public Text SecondNickName;
    public Text SecondLevel;
    public Image SecondLevelImage;

    public Text Third________________;
    public Text ThirdNickName;
    public Text ThirdLevel;
    public Image ThirdLevelImage;

    //닉네임
    public Text NickName_________;
    public InputField setNickNameText;
    public Text msgNickName;

    //로그인창
    public Text Login_________;
    public Text msgLogin;
    public InputField IDText;
    public InputField PWText;

    //회원가입 창
    public Text Join_________;
    public Text msgJoin;
    public InputField setIDText;
    public InputField setPWText;
    public InputField setPWCheck;
    public Image IDCheckImage;
    public Image NickNameCheckImage;
    public Image PWCheckImage;
    public Button JoinBtn;

    //캐릭
    public GameObject[] bearList = new GameObject[4];
    public GameObject[] rabbitList = new GameObject[4];
    public GameObject[] catList = new GameObject[4];

    public GameObject NetworkManager;
    // Start is called before the first frame update
    void Start()
    {
        MainManager.Instance.NetworkManager = NetworkManager;
        //메인창
        MainManager.Instance.myNickName = myNickName;
        MainManager.Instance.moneyTxt = moneyTxt;
        MainManager.Instance.cashTxt = cashTxt;
        MainManager.Instance.characterParent = characterParent;
        MainManager.Instance.levelImage = levelImage;
        MainManager.Instance.levelText = levelText;
        MainManager.Instance.expBar = expBar;
        MainManager.Instance.expText = expText;

        MainManager.Instance.MainWindow = mainWindow;
        MainManager.Instance.nickNameWindow = nickNameWindow;
        MainManager.Instance.LoginWindow = loginWindow;
        MainManager.Instance.JoinWindow = joinWindow;
        MainManager.Instance.GameLoadingWindow = gameLoadingWindow;
        MainManager.Instance.MessageWindow = messageWindow;
        MainManager.Instance.LoadingWindow = loadingWindow;

        //랭킹
        MainManager.Instance.myRankImage = myRankImage;
        MainManager.Instance.myRankText = myRankText;
        MainManager.Instance.rankImageList[0] = rankImageList[0];
        MainManager.Instance.rankImageList[1] = rankImageList[1];
        MainManager.Instance.rankImageList[2] = rankImageList[2];
        MainManager.Instance.rankImageList[3] = rankImageList[3];

        MainManager.Instance.RankNickName[0] = FirstNickName;
        MainManager.Instance.RankNickName[1] = SecondNickName;
        MainManager.Instance.RankNickName[2] = ThirdNickName;

        MainManager.Instance.RankLevel[0] = FirstLevel;
        MainManager.Instance.RankLevel[1] = SecondLevel;
        MainManager.Instance.RankLevel[2] = ThirdLevel;

        MainManager.Instance.RankLevelImage[0] = FirstLevelImage;
        MainManager.Instance.RankLevelImage[1] = SecondLevelImage;
        MainManager.Instance.RankLevelImage[2] = ThirdLevelImage;

        //닉네임
        MainManager.Instance.setNickNameText = setNickNameText;
        MainManager.Instance.msgNickName = msgNickName;

        //로그인창
        MainManager.Instance.IDText = IDText;
        MainManager.Instance.PWText = PWText;
        MainManager.Instance.msgLogin = msgLogin;

        //회원가입 창
        MainManager.Instance.setIDText = setIDText;
        MainManager.Instance.setPWText = setPWText;
        MainManager.Instance.setPWCheck = setPWCheck;
        MainManager.Instance.msgJoin = msgJoin;
        MainManager.Instance.IDCheckImage = IDCheckImage;
        MainManager.Instance.NickNameCheckImage = NickNameCheckImage;
        MainManager.Instance.PWCheckImage = PWCheckImage;
        MainManager.Instance.JoinBtn = JoinBtn;


        //캐릭
        MainManager.Instance.bearList[0] = bearList[0];
        MainManager.Instance.bearList[1] = bearList[1];
        MainManager.Instance.bearList[2] = bearList[2];
        MainManager.Instance.bearList[3] = bearList[3];
        MainManager.Instance.rabbitList[0] = rabbitList[0];
        MainManager.Instance.rabbitList[1] = rabbitList[1];
        MainManager.Instance.rabbitList[2] = rabbitList[2];
        MainManager.Instance.rabbitList[3] = rabbitList[3];
        MainManager.Instance.catList[0] = catList[0];
        MainManager.Instance.catList[1] = catList[1];
        MainManager.Instance.catList[2] = catList[2];
        MainManager.Instance.catList[3] = catList[3];

        if (MainManager.Instance.isFirst)
        {
            MainManager.Instance.isFirst = false;
            MainManager.Instance.setEnd();
        }
        else
            MainManager.Instance.callGetDate();

        StartCoroutine(wait());
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(1.0f);
        modeSelect.SetActive(true);
    }

    public void logout()
    {
        MainManager.Instance.LogoutBtn();
    }


    public void withdraw()
    {
        MainManager.Instance.WithdrawBtn();
    }

}
