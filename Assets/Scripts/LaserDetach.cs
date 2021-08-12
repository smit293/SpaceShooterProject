using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDetach : MonoBehaviour
{
    // This was created in order to have the tripleshot laser prefab break apart and be individual lasers this was one solution
    // created to stop all lasers from destroying on impact.
    public Transform[] _myChildObjects;
   
    void OnEnable()
    {
        foreach(Transform t in _myChildObjects)
        {
            t.parent = null;
        }
    }
}
