using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDeathTimer : MonoBehaviour
{
    
    void Start()
    {
        StartCoroutine(Dying());
    }

    IEnumerator Dying()
    {
        yield return new WaitForSeconds(0.3f);
        Destroy(this.gameObject);
    }
}
