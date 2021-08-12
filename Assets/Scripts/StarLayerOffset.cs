using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarLayerOffset : MonoBehaviour
{
    [SerializeField]
    private float _scrollSpeed = 0.005f;
    private MeshRenderer _mr;
    
    void Start()
    {
        _mr = GetComponent<MeshRenderer>();
        _mr.sortingOrder = -5;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 _offset = new Vector2(0, _scrollSpeed * Time.time);
        _mr.material.mainTextureOffset = _offset;
    }
}
