using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _powerUps; //0 = triple shot, 1 = speed, 2 = shield, 3 = hazard 4 = homing missile
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemy2Prefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject _bossPrefab;
    private bool _isPlayerDead;
    [SerializeField]
    private GameObject _lifeRefill;
    [SerializeField]
    private GameObject _ammoRefill;
    private GameManager _gameManager;

    private float _enemySpawnRate;


    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        if (_gameManager == null)
        {
            Debug.Log("Game Manager is null on Spawn Manager");
        }
    }
    private void EnemyWaveOneSpawn()
    {

        _enemySpawnRate = 4.0f;
        StartCoroutine(EnemySpawnRoutine(_enemySpawnRate));

    }
    private void EnemyWaveTwoSpawn()
    {
        _enemySpawnRate = 3.8f;
        StartCoroutine(EnemySpawnRoutine(_enemySpawnRate));
    }
    private void EnemyWaveThreeSpawn()
    {
        _enemySpawnRate = 3.5f;
        StartCoroutine(EnemySpawnRoutine(_enemySpawnRate));
    }
    IEnumerator EnemySpawnRoutine(float _enemySpawnRate)
    {
        int _randomEnemy = Random.Range(1, 11);
        float randomX = Random.Range(-9.5f, 9.5f);
        Vector3 _posToSpawn = new Vector3(randomX, 7.5f, 0);
        if (_randomEnemy < 7)
        {

            GameObject _newEnemy = Instantiate(_enemyPrefab, _posToSpawn, Quaternion.identity);
            _newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(_enemySpawnRate);
            StartCoroutine(EnemySpawnRoutine(_enemySpawnRate));

        }
        if (_randomEnemy >= 7)
        {

            GameObject _newEnemy = Instantiate(_enemy2Prefab, _posToSpawn, Quaternion.identity);
            _newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(_enemySpawnRate);
            StartCoroutine(EnemySpawnRoutine(_enemySpawnRate));


        }

    }



    IEnumerator PowerupSpawnRoutine()
    {
        while (_isPlayerDead == false)
        {
            float randomX = Random.Range(-9.5f, 9.5f);
            Vector3 _posToSpawn = new Vector3(randomX, 7.5f, 0);
            GameObject _powerUp = _powerUps[Random.Range(0, 5)];
            Instantiate(_powerUp, _posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(6.0f);
        }
    }
    IEnumerator AmmoSpawnRoutine()
    {
        while (_isPlayerDead == false)
        {
            float randomX = Random.Range(-9.5f, 9.5f);
            Vector3 _posToSpawn = new Vector3(randomX, 7.5f, 0);
            Instantiate(_ammoRefill, _posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(10.0f);
        }
    }
    IEnumerator LifeRefill()
    {
        while (_isPlayerDead == false)
        {
            float randomX = Random.Range(-9.5f, 9.5f);
            Vector3 _postToSpawn = new Vector3(randomX, 7.5f, 0);
            Instantiate(_lifeRefill, _postToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(12f);
        }
    }
    public void SpawnEnabled()
    {

        StartCoroutine(SpawnRateControlRoutine());

    }
    IEnumerator SpawnRateControlRoutine()
    {

        yield return new WaitForSeconds(3.0f);
        Spawning();
        StopCoroutine(SpawnRateControlRoutine());
    }
    private void Spawning()
    {

        if (_gameManager._wave2 == false && _gameManager._wave3 == false)
        {
            EnemyWaveOneSpawn();
        }
        if (_gameManager._wave2 == true)
        {
            EnemyWaveTwoSpawn();
        }
        if (_gameManager._wave3 == true)
        {
            EnemyWaveThreeSpawn();
        }
    }
    public void OtherSpawnEnabled()
    {
        StartCoroutine(LifeRefill());
        StartCoroutine(PowerupSpawnRoutine());
        StartCoroutine(AmmoSpawnRoutine());
    }
    public void StopSpawning()
    {
        _gameManager.DeleteAllEnemies();
        StopCoroutine(EnemySpawnRoutine(0));
        StartCoroutine(SpawnResetRoutine());
    }
    public void NoSpawn()
    {
        _gameManager.DeleteAllEnemies();
        _gameManager.DeleteAllPowerUps();
        StopAllCoroutines();
    }
    public void SpawnBoss()
    {
        StopAllCoroutines();
        Instantiate(_bossPrefab, new Vector3(0, 9.85f, 0), Quaternion.identity);
    }
    IEnumerator SpawnResetRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        SpawnEnabled();
    }
    public void PlayerIsDead()
    {
        _isPlayerDead = true;
    }

}
