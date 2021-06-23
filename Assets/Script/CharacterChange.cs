using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//캐릭터를 변경하는 스크립트
public class CharacterChange : MonoBehaviour
{
    public GameObject parent;
    GameObject character;

    public int i;


    public void change()
    {
        if (i == 0 || i == 5 || i == 10)
        {
            switch (i)
            {
                case 0:
                    for(int i = 0; i < 4; i++)
                    {
                        MainManager.Instance.bearList[i].SetActive(true);
                        MainManager.Instance.rabbitList[i].SetActive(false);
                        MainManager.Instance.catList[i].SetActive(false);
                    }
                    break;

                case 5:
                    for (int i = 0; i < 4; i++)
                    {
                        MainManager.Instance.bearList[i].SetActive(false);
                        MainManager.Instance.rabbitList[i].SetActive(true);
                        MainManager.Instance.catList[i].SetActive(false);
                    }
                    break;

                case 10:
                    for (int i = 0; i < 4; i++)
                    {
                        MainManager.Instance.bearList[i].SetActive(false);
                        MainManager.Instance.rabbitList[i].SetActive(false);
                        MainManager.Instance.catList[i].SetActive(true);
                    }
                    break;
            }
        }

        try
        {
            character = parent.transform.GetChild(0).gameObject;
            Destroy(character);
        }
        catch(UnityException e)
        {
        }
        finally
        {
            character = Instantiate(MainManager.Instance.characterList[i]);
            MainManager.Instance.myCharacter = i;
            character.transform.SetParent(parent.transform); 
            character.transform.localPosition = new Vector3(0,0,0);
            character.transform.Rotate(0, 180, 0);
            character.transform.localScale = new Vector3(1f, 1f, 1f);

            MySqlConnector.Instance.doNonQuery("update account set account_character = "+ i +" where account_id = '" + MainManager.Instance.myID + "'");
        }
    }
}
