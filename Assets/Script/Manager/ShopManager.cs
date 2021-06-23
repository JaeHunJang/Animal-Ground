using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MySql.Data.MySqlClient;

public class ShopManager : MonoBehaviour
{
    public int selectedItemIndex = 0;
    public ShopItems[] shopItems = new ShopItems[6];
    public Sprite[] itemDetails = new Sprite[6];
    public int[] itemCost = new int[] { 100, 100, 100, 100, 100, 100 };
    public Image itemExplainImg;
    public Button buyBtn;

    string[] itemName = { "shield", "lightning", "magnet", "boost", "banana", "mark" };

    private MySqlDataReader reader;

    private void Start()
    {
        for (int i = 0; i < shopItems.Length; i++)
            shopItems[i].setMyIndex(i);
    }

    public void buyItem()
    {
        
        MySqlConnector.Instance.doNonQuery("update stored_item set " + itemName[selectedItemIndex] + "=" + itemName[selectedItemIndex] + "+1 where account_id = '" + MainManager.Instance.myID + "';");
        MySqlConnector.Instance.doNonQuery("update account set account_money_now = account_money_now - "+itemCost[selectedItemIndex]+" where account_id = '" + MainManager.Instance.myID + "';");
        MainManager.Instance.itemNums[selectedItemIndex]++;
        int money = int.Parse(MainManager.Instance.StringReplace(MainManager.Instance.moneyTxt.text,",",""));
        int result = money - itemCost[selectedItemIndex];
        MainManager.Instance.moneyTxt.text = MainManager.Instance.StringFormat(result, "{0:#,#}");
    }
}
