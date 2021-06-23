using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//scriptableObject인 card를 생성하는 스크립트
[CreateAssetMenu(fileName ="New Card",menuName ="Card")]
public class Card : ScriptableObject
{
    public int cardNumber;
    public Sprite cardImage;
    public Sprite cardBackImage;
}
