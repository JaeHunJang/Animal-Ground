using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FocusChange : MonoBehaviour
{
    public List<InputField> inputFields;
    private int index;

    private void Start()
    {
        index = 0;
        inputFields[index].Select();
    }

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            
            StartCoroutine(focusing());
        }

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            //if (index != (inputFields.Count-1))
            //{
             //   StartCoroutine(focusing());
            //}
            //else
            //{
                if (inputFields[index].gameObject.tag == "Login")
                    MainManager.Instance.CheckID();
                else if (inputFields[index].gameObject.tag == "Join")
                {
                    if (MainManager.Instance.check == true && MainManager.Instance.NickNameCheck == true && MainManager.Instance.PWCheck == true)
                    {
                        MainManager.Instance.JoinOkBtn();
                    }
                }
           // }
        }
        
    }

    IEnumerator focusing()
    {

        if (inputFields[index].isFocused)
        {
            if (index == inputFields.Count - 1)
                index = 0;
            else
                index++;
            inputFields[index].Select();
        }
        else
        {
            for (int i = 0; i < inputFields.Count; i++)
            {
                if (inputFields[i].isFocused)
                {
                    index = i;
                    StartCoroutine(focusing());
                    break;
                }
            }
        }
        yield return null;
    }
}
