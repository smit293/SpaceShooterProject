using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackOne : MonoBehaviour
{
    private readonly float _speed = 5f;
                  
        void Update()
        {
            transform.Translate(_speed * Time.deltaTime * Vector3.down);

            if (transform.position.y < -4f)
            {
                Destroy(this.gameObject);
            }
        }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>().Damage();
            Destroy(this.gameObject);
        }    
    }
}
