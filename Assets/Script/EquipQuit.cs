using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipQuit : MonoBehaviour
{


    public void callEquiptQuit()
    {
        MySqlConnector.Instance.doNonQuery("update account set account_item1 = " + MainManager.Instance.selectedItems[0] + " , account_item2 = " + MainManager.Instance.selectedItems[1] + " where account_id = '" + MainManager.Instance.myID + "'");
        gameObject.SetActive(false);
    }
}
