using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MySql.Data.MySqlClient;

public class EquipManager : MonoBehaviour
{
    public int[] selectedItems = new int[2];
    public ShopItems[] shopItems = new ShopItems[6];
    public GameObject[] checkIcons = new GameObject[6];
    public Image[] numObjs = new Image[12];
    public Sprite[] itemDetails = new Sprite[6];
    public Sprite[] numbers = new Sprite[10];
    public Image itemExplainImg;

    int[] itemNums = new int[6];

    private MySqlDataReader reader;

    int checkNum = 0;

    bool isDone = false;

    private void Awake()
    {
        selectedItems = MainManager.Instance.selectedItems;
        itemNums = MainManager.Instance.itemNums;
    }

    private void Start()
    {
        for (int i = 0; i < shopItems.Length; i++)
            shopItems[i].setMyIndex(i);
    }

    private void OnEnable()
    {
        for(int i= 0; i< itemNums.Length; i++)
        {
            if (itemNums[i] == 0)
            {
                shopItems[i].gameObject.GetComponent<Button>().interactable = false;
            }
            else
                shopItems[i].gameObject.GetComponent<Button>().interactable = true;
        }

        Debug.Log("아임콜링");
        //아이템갯수 이미지
        int index = 0;
        for (int i = 0; i < numObjs.Length; i = i + 2)
        {
            numObjs[i].sprite = numbers[itemNums[index] / 10];
            numObjs[i + 1].sprite = numbers[itemNums[index] % 10];
            index++;
        }

        setCheckIcons();
    }

    public void setSelectedItems(int index)
    {
        isDone = false;
        int num;
        for (int i = 0; i < selectedItems.Length; i++)
        {
            if (selectedItems[i] == index)
            {
                selectedItems[i] = -1;
                num = i + 1;
                MySqlConnector.Instance.doNonQuery("update account set account_item"+num+" = -1 where account_id = '" + MainManager.Instance.myID + "'");
                checkNum++;
                isDone = true;
                break;
            }
        }

        for (int i = 0; i < selectedItems.Length; i++)
        {
            if (!isDone && selectedItems[i] == -1)
            {
                selectedItems[i] = index;
                num = i + 1;
                MySqlConnector.Instance.doNonQuery("update account set account_item" + num + " = " + index + " where account_id = '" + MainManager.Instance.myID + "'");
                checkNum = i+1;
                isDone = true;
                break;
            }
        }
        if (!isDone)
        {
            selectedItems[checkNum%2] = index;
            num = checkNum % 2 + 1;
            MySqlConnector.Instance.doNonQuery("update account set account_item" + num + " = " + index + " where account_id = '" + MainManager.Instance.myID + "'");
            checkNum++;
        }

        setCheckIcons();
    }

    private void setCheckIcons()
    {
        for (int i = 0; i < checkIcons.Length; i++)
            checkIcons[i].SetActive(false);
        
        
        if(selectedItems[0] != -1 && itemNums[selectedItems[0]] >0)
            checkIcons[selectedItems[0]].SetActive(true);

        if (selectedItems[1] != -1 && itemNums[selectedItems[1]] > 0)
            checkIcons[selectedItems[1]].SetActive(true);
    }

}
