using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossExplosion : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Dying()); 
    }

    IEnumerator Dying()
    {
        yield return new WaitForSeconds(1.0f);
        Destroy(this.gameObject);
    }
}
