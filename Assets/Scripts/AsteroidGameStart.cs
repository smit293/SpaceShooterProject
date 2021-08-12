using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AsteroidGameStart : MonoBehaviour
{
    private GameManager _gameManager;
    private UIManager _uiManager;
    public float degrees = 10f;

    private void Start()
    {
        _uiManager = FindObjectOfType<UIManager>();
        _gameManager = FindObjectOfType<GameManager>();
        if(_gameManager == null)
        {
            Debug.LogError("GameManager is Null on Asteroid");
        }
    }
    private void Update()
    {
        transform.Rotate(Vector3.forward,degrees * Time.deltaTime);
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Laser"))
        {
            _gameManager.GameStart();
            _uiManager.GameStartText();
            Destroy(this.gameObject);
        }
    }
}
