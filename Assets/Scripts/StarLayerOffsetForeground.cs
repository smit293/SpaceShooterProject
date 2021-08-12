using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarLayerOffsetForeground : MonoBehaviour
{
    [SerializeField]
    private float _scrollSpeed = 0.005f;
    private MeshRenderer _mr;

    void Start()
    {
        _mr = GetComponent<MeshRenderer>();
        _mr.sortingLayerName = "Foreground";
        _mr.sortingOrder = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 _offset = new Vector2(_scrollSpeed * Time.time,0);
        _mr.material.mainTextureOffset = _offset;
    }
}
