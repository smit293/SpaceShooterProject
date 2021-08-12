using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PowerupDetector : MonoBehaviour
{

  
    private void OnTriggerEnter2D(Collider2D other)
    {
        
      if (other.CompareTag("PowerUp"))
        {
            transform.parent.gameObject.GetComponent<Enemy>().PowerupDetected();
        }
      
     
    }
}
