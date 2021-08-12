using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{

    private bool _isPlayerDead = false;
    public bool _wave1;
    public bool _wave2;
    public bool _wave3;
    private bool _wave2Ready;
    private bool _wave3Ready;
    private bool _bossReady;
    private int _maxEnemy;
    private UIManager _uiManager;
    private int _currentEnemy;
    private SpawnManager _spawnManager;
    private Player _player;




    public void Start()
    {
        Cursor.visible = false;
        _player = FindObjectOfType<Player>();
        if (_player == null)
        {
            Debug.Log("Player is null on GM");
        }
        _uiManager = FindObjectOfType<UIManager>();
        if (_uiManager == null)
        {
            Debug.Log("UI Manager is null on Game Manager");
        }
        _spawnManager = FindObjectOfType<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.Log("Spawn Manager is null on game manager");
        }

        _wave1 = true;
        _wave2 = false;
        _wave3 = false;

    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.R) && _isPlayerDead == true)
        {
            _isPlayerDead = false;
            SceneManager.LoadScene("Game");

        }

    }
    public void GameStart()
    {
        _spawnManager.OtherSpawnEnabled();
        _spawnManager.SpawnEnabled();
        WaveStart();
    }

    public void WaveStart()
    {

        if (_wave1 == true)
        {

            _uiManager.Wave(1);
            _wave2Ready = true;
            _wave1 = false;
            _maxEnemy = 10;
            _currentEnemy = _maxEnemy;
            _uiManager.EnemyCount(_currentEnemy, _maxEnemy);

        }

        if (_wave2 == true && _wave2Ready == true)
        {
            _uiManager.Wave(2);
            _wave2Ready = false;
            _wave3Ready = true;
            _maxEnemy = 20;
            _currentEnemy = _maxEnemy;
            _uiManager.EnemyCount(_currentEnemy, _maxEnemy);

        }
        if (_wave3 == true && _wave3Ready == true)
        {

            _wave3Ready = false;
            _uiManager.Wave(3);
            _maxEnemy = 30;
            _currentEnemy = _maxEnemy;
            _uiManager.EnemyCount(_currentEnemy, _maxEnemy);

        }

    }
    public void WaveCount()
    {
        _currentEnemy--;
        _uiManager.EnemyCount(_currentEnemy, _maxEnemy);
              
        if (_currentEnemy < 1 || _currentEnemy == 0)
        {
            if (_wave3Ready == false && _wave3 == true)
            {
                _bossReady = true;
            }            
            _spawnManager.StopSpawning();
            NextWave();
        }
        if (_bossReady == true && _wave3Ready == false)
        {
           
            DeleteAllEnemies();
            _player.IsBossLevel();
            _spawnManager.SpawnBoss();
            _uiManager.BossLevel();

        }

    }
    public void DeleteAllEnemies()
    {
        GameObject[] Enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in Enemies)
            GameObject.Destroy(enemy);

    }
    public void DeleteAllPowerUps()
    {
        GameObject[] PowerUps = GameObject.FindGameObjectsWithTag("PowerUp");
        foreach (GameObject powerup in PowerUps)
            GameObject.Destroy(powerup);

    }
    private void NextWave()

    {
        if (_wave2Ready == true)
        {
            _wave2 = true;
            _wave1 = false;

        }
        if (_wave3Ready == true)
        {
            _wave2 = false;
            _wave3 = true;


        }
        WaveStart();

    }


    public void PlayerIsDead()
    {
        FindObjectOfType<BackgroundMusic>().TurnOff();
        _isPlayerDead = true;
        Destroy(FindObjectOfType<Enemy>());
        _spawnManager.NoSpawn();
    }

}

    

