using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInfo : MonoBehaviour
{
    public GameObject tileTop;
    public GameObject tileBot;
    public Transform cardHolder;
    public Transform positionHolder;
    public Transform[] holders;
    public Material[] topMt;
    public Material[] botMt;
    public Collider tileCollider;
    public bool isTiled = false;
    public bool isBanana = false;
    public Card card;



    public int nowColor;
    
    Renderer topMs;
    Renderer botMs;

    //각 타일들의 정보를 가지고있는 스크립트
    private void Start()
    {
        topMs = tileTop.GetComponent<Renderer>();
        botMs = tileBot.GetComponent<Renderer>();
    }

    public Vector3 getTileposition(int index)
    {
        return holders[index].position;
    }

    public void changeMat(int index)
    {
        topMs.material = topMt[index];
        botMs.material = botMt[index];
        nowColor = index;
    }
}
