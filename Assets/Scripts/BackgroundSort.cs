using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSort : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<Renderer>().sortingLayerName = "Background";
        this.gameObject.GetComponent<Renderer>().sortingOrder = -10;
    }

   
}
