using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossExplosionSound : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Dying());
    }

    IEnumerator Dying()
    {
        yield return new WaitForSeconds(3.5f);
        Destroy(this.gameObject);
    }
}
