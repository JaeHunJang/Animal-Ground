using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public TileInfo[] tiles;

    //각 타일들을 저장하는 스크립트
    void Start()
    {
        tiles = GetComponentsInChildren<TileInfo>();
    }
}
