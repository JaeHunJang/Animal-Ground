using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScene : MonoBehaviour
{
    //씬 로딩시 화면을 가리는 오브젝트 관리용 스크립트.
    float m_time;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        m_time += Time.deltaTime;
        if (m_time >= 3)
            gameObject.SetActive(false);
    }
}
