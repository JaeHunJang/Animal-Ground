using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MySql.Data.MySqlClient;
public class ShopItems : MonoBehaviour
{
    public int myIndex;
    public ShopManager shopManager;
    public EquipManager equipManager;
    public Image myImg;
    public Image itemImg;

    bool isSelected = false;

    private MySqlDataReader reader;

    public void setMyIndex(int index)
    {
        myIndex = index;
    }

    public void setShopSelectedIndex()
    {
        itemImg.sprite = myImg.sprite;
        shopManager.itemExplainImg.sprite = shopManager.itemDetails[myIndex];
        shopManager.selectedItemIndex = myIndex;

        reader = MySqlConnector.Instance.doQuery("select account_money_now from account where account_id = '" + MainManager.Instance.myID + "';");
        reader.Read();
        if (int.Parse(reader[0].ToString()) < shopManager.itemCost[myIndex])
        {
            shopManager.buyBtn.interactable = false;
        }else
        {
            shopManager.buyBtn.interactable = true;
        }
        reader.Close();

    }

    public void setEquipSelectedIndex()
    {
        equipManager.setSelectedItems(myIndex);
        itemImg.sprite = myImg.sprite;
        equipManager.itemExplainImg.sprite = equipManager.itemDetails[myIndex];
    }

}
