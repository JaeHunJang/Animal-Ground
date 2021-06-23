using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectPlaceCard : MonoBehaviour
{
    public Card card;
    public CardToPlaceManager cardToPlaceManager;

    public void selectPlaceCard()
    {
        if(!cardToPlaceManager.isSelect)
        {
            cardToPlaceManager.isSelect = true;
            ProgressManager.Instance.myState = ProgressManager.State.tileSelect;
            ProgressManager.Instance.card = card;
            ProgressManager.Instance.isDone = true;
            cardToPlaceManager.isSelect = false;

        }
    }

    private void OnEnable()
    {
        if (card.cardNumber == 2)
        {
            cardToPlaceManager.plus2.sprite = cardToPlaceManager.images[ProgressManager.Instance.ptwoNum];
            if (ProgressManager.Instance.ptwoNum == 0)
                gameObject.GetComponent<Button>().interactable = false;
            else
                gameObject.GetComponent<Button>().interactable = true;
        }
        if (card.cardNumber == -2)
        {
            cardToPlaceManager.minus2.sprite = cardToPlaceManager.images[ProgressManager.Instance.mtwoNum];
            if (ProgressManager.Instance.mtwoNum == 0)
                gameObject.GetComponent<Button>().interactable = false;
            else
                gameObject.GetComponent<Button>().interactable = true;
        }
        if (card.cardNumber == 0)
        {
            cardToPlaceManager.zero.sprite = cardToPlaceManager.images[ProgressManager.Instance.zeroNum];
            if (ProgressManager.Instance.zeroNum == 0)
                gameObject.GetComponent<Button>().interactable = false;
            else
                gameObject.GetComponent<Button>().interactable = true;
        }





    }


}
