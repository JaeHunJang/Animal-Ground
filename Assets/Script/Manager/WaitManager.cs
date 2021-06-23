using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitManager : MonoBehaviour
{
    public RectTransform[] dots;

    private void OnEnable()
    {
        
        StartCoroutine(dotsMove());
    }

    IEnumerator dotsMove()
    {
        int i = 3;
        while (true)
        {
            if(i > 2)
            {
                i = 0;
                for (int j = 0; j < 3; j++)
                    dots[j].gameObject.SetActive(false);
                yield return new WaitForSeconds(0.5f);
            }
            dots[i].gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            i++;
        }
    }

}
