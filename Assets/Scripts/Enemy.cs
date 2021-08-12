using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Animator _animator;
    private Player _player;
    [SerializeField]
    private GameObject _laserPrefab;
    private float _speed = 4f;
    private bool _isDead = false;
    private float _fireRate = 3.0f;
    private float _canFire = -1;
    private bool _isAlive = true;
    private bool _swayMovementActive;
    private bool _isSwayActive = false;
    private float _swayOffset;
    [SerializeField]
    private bool _shieldActive;
    [SerializeField]
    private GameObject _enemyShield;
    private bool _movingTowards;
    [SerializeField]
    private float _maxRangeToPlayer;
    [SerializeField]
    private GameObject _explosion;
    [SerializeField]
    private GameObject _deathSound;

    private GameManager _gameManager;
    private bool _isEnemy2;

    void Start()
    {

        _gameManager = GameObject.FindObjectOfType<GameManager>();
        if (_gameManager == null)
        {
            Debug.Log("Game Manager on Enemy is null");
        }
        if (this.gameObject.name == "Enemy2(Clone)") // This checks to see if the object spawned is actually the second enemy
        {
            _isEnemy2 = true;

        }

        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("Player is null on enemy");
        }
        _animator = GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.LogError("Animator is null on enemy");
        }



        if (_isEnemy2 == true && _gameManager._wave2 == true) // Allows for sway movememnt during second wave
        {
            _swayMovementActive = true;

        }


        int _shieldEnemy = Random.Range(0, 10); // Random number to help with assigning random shield on enemies
        if (_shieldEnemy > 7 && _gameManager._wave2 == true) // gives the enemies a 30% chance of having a shield
        {
            _shieldActive = true;
            _enemyShield.SetActive(true);

        }

    }


    void Update()
    {
        if (_player != null)
        {
            PlayerPosition();
        }
        CalculateMovement();



    }

    private void PlayerPosition() // Used to decide where the player is so enemies can shoot backwards
    {
        if (Time.time > _canFire && _isAlive == true)
        {
            if (_player.transform.position.y > gameObject.transform.position.y && _gameManager._wave2 == true)
            {

                ReverseFiring();
            }
            else if (_player.transform.position.y < gameObject.transform.position.y)
            {
                ForwardsFire();

            }
        }
    }

    private void ReverseFiring()
    {
        _fireRate = Random.Range(2f, 3f);
        _canFire = Time.time + _fireRate;
        Vector3 _offSet = new Vector3(0, 3.6f, 0);
        GameObject enemyLaser = Instantiate(_laserPrefab, transform.position + _offSet, Quaternion.identity);
        Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();


        for (int i = 0; i < lasers.Length; i++)
        {
            lasers[i].EnemyReverseFiring();
            lasers[i].AssignEnemyLaser();

        }
    }
    private void ForwardsFire()
    {
        _fireRate = Random.Range(2f, 3f);
        _canFire = Time.time + _fireRate;
        GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
        Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

        for (int i = 0; i < lasers.Length; i++)
        {
            lasers[i].AssignEnemyLaser();
        }
    }
    public void PowerupDetected() //Used to detect if a powerup is in front of the enemy so they can fire at it during wave 3
    {
        if (_gameManager._wave3 == true)
        {
            ForwardsFire();
        }
    }



    void CalculateMovement()
    {
        if (_isDead == false)
        {
            if (_swayMovementActive == false && _isSwayActive == false && _movingTowards == false)
            {
                transform.Translate(_speed * Time.deltaTime * Vector3.down);
            }
            if (_swayMovementActive == true && _isSwayActive == false && _movingTowards == false)
            {
                StartCoroutine(SwayMove());
            }
            if (_swayMovementActive == true && _isSwayActive == true && _movingTowards == false)
            {
                transform.Translate(_swayOffset, -1 * _speed * Time.deltaTime, 0);
            }
            if (_player != null)
            {
                float _distanceToPlayer = Vector3.Distance(_player.transform.position, transform.position);

                _maxRangeToPlayer = 6f;
                if (_distanceToPlayer < _maxRangeToPlayer)
                {
                    _movingTowards = true;
                }
            }
            if (_movingTowards == true && _player != null)
            {
                float step = _speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, step);

            }
            if (transform.position.y <= -7.5f)
            {
                int _number = Random.Range(1, 3);
                _player.AddScore(-_number);
                float _randomX = Random.Range(-9.5f, 9.5f);
                transform.position = new Vector3(_randomX, 7.5f, 0);
            }
        }
    }


    private IEnumerator SwayMove()
    {

        {
            _isSwayActive = true;
            float xVel = 0.0f;
            _swayOffset *= Time.deltaTime;

            while (_swayMovementActive == true)
            {
                _swayOffset = Mathf.SmoothDamp(0, -0.03f, ref xVel, 0.005f);
                yield return new WaitForSeconds(0.5f);

                _swayOffset = Mathf.SmoothDamp(0, 0.03f, ref xVel, 0.005f);
                yield return new WaitForSeconds(0.5f);

            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Laser") && _isDead == false || other.CompareTag("HomingMissile") && _isDead == false)
        {
            Destroy(other.gameObject);
            if (_shieldActive == true)
            {
                _shieldActive = false;
                _enemyShield.SetActive(false);
            }

            else
            {
                if (_player != null && _shieldActive == false)
                {
                    _player.AddScore(10);

                }

                StartCoroutine(IsDying());
                this.gameObject.SetActive(false);
            }
        }
        if (other.CompareTag("Player") && _isDead == false)
        {

            other.transform.GetComponent<Player>()
            .Damage();
            StartCoroutine(IsDying());
        }


    }

    private IEnumerator IsDying()
    {
        Instantiate(_explosion, transform.position, Quaternion.identity);
        Instantiate(_deathSound, transform.position, Quaternion.identity);
        _gameManager.WaveCount();
        _shieldActive = false;
        _enemyShield.SetActive(false);
        _isAlive = false;
        _isDead = true;
        yield return new WaitForSeconds(0.01f);
        Destroy(this.gameObject);

    }
}
