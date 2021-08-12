using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _missilePrefab;
    private bool _bossLevel;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = 0.0f;
    [SerializeField]
    private int _lives = 3;
    private int _maxLives;
    [SerializeField]
    private SpawnManager _spawnManager;
    [SerializeField]
    private UIManager _uiManager;
    [SerializeField]
    private GameManager _gameManager;
    private bool _isTripleShotActive;
    [SerializeField]
    private int _score;
    [SerializeField]
    private GameObject _leftEngine, _rightEngine;
    private bool _isShieldActive = false;
    private bool _speedPowerupActive = false;
    [SerializeField]
    private GameObject _playerShield;
    [SerializeField]
    private SpriteRenderer _shieldVisualizerRenderer;
    private bool _hasAmmo = true;
    [SerializeField]
    private int _ammoCount = 25;
    [SerializeField]
    private Transform _cameraTransform;
    private Vector3 _originalCameraPos;
    private float _shakeAmount = 0.09f;
    private float _boostedMultiplier = 1f;
    private float _maxBoost = 100f;
    private float _currentBoost;
    private bool _isBoosting;
    private bool _isBoostReady = true;
    private int _shieldStr = 3;
    private bool _hazardIsActive;
    [SerializeField]
    private GameObject _enemyReference;
    [SerializeField]
    private bool _missileActive;
    private int _missleCount;
    [SerializeField]
    private float _missleFireRate = 0.8f;

    void Start()
    {

        if (_shieldVisualizerRenderer == null)
        {
            Debug.LogError("Shield Sprite Renderer Null on player");
        }
        if (_uiManager == null)
        {
            Debug.LogError("UI Manager is null on player");
        }
        if (_gameManager == null)
        {
            Debug.LogError("Game manager is null on player");
        }
        if (_enemyReference == null)
        {
            Debug.LogError("Enemy is null on player");
        }
        _uiManager.AmmoCount(_ammoCount);
        _uiManager.MaxSliderValue(_maxBoost);
        _maxLives = _lives;
        transform.position = new Vector3(0, 0, 0);

    }

    void Update()
    {
        Fire();
        Movement();
        Boost();
        _uiManager.AmmoCount(_ammoCount);

    }

    private void Boost()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && _isBoostReady == true)
        {
            _isBoosting = true;
            _currentBoost = _maxBoost;
            Boosting();
        }
    }
    private void Movement()
    {
        float _horizontalInput = Input.GetAxis("Horizontal");
        float _verticalInput = Input.GetAxis("Vertical");

        if (_hazardIsActive == false)
        {
            transform.Translate(_boostedMultiplier * _horizontalInput * _speed * Time.deltaTime * Vector3.right);
            transform.Translate(_boostedMultiplier * _speed * _verticalInput * Time.deltaTime * Vector3.up);
        }
        else if (_hazardIsActive == true)
        {
            transform.Translate(_boostedMultiplier * -_horizontalInput * _speed * Time.deltaTime * Vector3.right);
            transform.Translate(_boostedMultiplier * _speed * -_verticalInput * Time.deltaTime * Vector3.up);
            StartCoroutine(HazardCooldown());
        }
        if (transform.position.y >= 3.5f)
        {
            transform.position = new Vector3(transform.position.x, 3.5f, 0);
        }
        else if (transform.position.y <= -3.8f)
        {
            transform.position = new Vector3(transform.position.x, -3.8f, 0);
        }
        if (transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }


    }
    public void HazardPickup()
    {
        _hazardIsActive = true;
    }
    IEnumerator HazardCooldown()
    {
        yield return new WaitForSeconds(5f);
        _hazardIsActive = false;
    }

    void Boosting()
    {
        _boostedMultiplier = 2f;
        if (_currentBoost > 0 && _isBoosting == true && _isBoostReady == true)
        {


            _isBoostReady = false;
            StartCoroutine(StartBoosting());
        }
    }
    IEnumerator StartBoosting()
    {

        while (_currentBoost > 0 && _isBoosting == true)
        {
            yield return new WaitForSeconds(0.1f);
            _currentBoost -= 5;
            _uiManager.BoostSlider(_currentBoost);

        }
        if (_currentBoost <= 0 && _isBoosting == true)
        {
            _isBoosting = false;
            _boostedMultiplier = 1f;
            StartCoroutine(BoostCooldown());
        }
        yield return new WaitForSeconds(0.2f);
        StopCoroutine(StartBoosting());

    }
    IEnumerator BoostCooldown()
    {

        while (_currentBoost < _maxBoost && _isBoosting == false)
        {
            yield return new WaitForSeconds(0.1f);
            _currentBoost += 5;
            _uiManager.BoostSlider(_currentBoost);

        }
        if (_currentBoost == _maxBoost && _isBoosting == false)
        {
            _isBoostReady = true;
        }

        yield return new WaitForSeconds(0.2f);
        StopCoroutine(BoostCooldown());
    }
   
    void Fire()
    {
        if (Time.time > _canFire)
        {
            if ((Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0)))
            {
                if (_hasAmmo == true && _missileActive == false && _bossLevel == false)
                {

                    _canFire = Time.time + _fireRate;


                    if (_ammoCount > 0)
                    {
                        
                        if (_isTripleShotActive == true)
                        {
                            Instantiate(_tripleShotPrefab, transform.position + new Vector3(0, 0.85f, 0), Quaternion.identity);
                        }
                        else
                        {
                            Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.85f, 0), Quaternion.identity);
                        }
                    }
                    _ammoCount--;
                    if (_ammoCount == 0)
                    {
                        _hasAmmo = false;
                    }

                }
                else if (_missileActive == true)
                {

                    _canFire = Time.time + _missleFireRate;


                    if (_missleCount > 0)
                    {
                        Instantiate(_missilePrefab, transform.position, Quaternion.identity);
                    }
                    _missleCount--;

                    if (_missleCount == 0)
                    {
                        _missileActive = false;
                    }

                }
                else if (_bossLevel == true)
                {
                    _canFire = Time.time + _fireRate;

                    Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.85f, 0), Quaternion.identity);

                }
            }
        }
    }

    public void IsBossLevel()
    {
        _bossLevel = true;
    }
    public void MissilePower()
    {
        _missileActive = true;
        _missleCount = 3;
    }
    public void AmmoReload()
    {
        _ammoCount = 25;
        _hasAmmo = true;

    }
    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotCooldown());
    }
    public void SpeedPowerupActive()
    {
        StartCoroutine(SpeedPowerupCooldown());
    }
    public void ShieldIsActive()
    {
        if (_isShieldActive == true)
        {
            _shieldStr = 3;
            _shieldVisualizerRenderer.color = Color.white;
        }


        _playerShield.SetActive(true);
        _isShieldActive = true;

    }
    public void LifeRefill()
    {
        if (_lives < _maxLives)
        {
            _lives += 1;
            _uiManager.UpdateLives(_lives);
            if (_lives == 3)
            {
                _leftEngine.SetActive(false);
                _rightEngine.SetActive(false);
            }

            if (_lives == 2)
            {
                _leftEngine.SetActive(true);
                _rightEngine.SetActive(false);
            }
            if (_lives == 1)
            {
                _rightEngine.SetActive(true);
            }
        }
    }

    private IEnumerator SpeedPowerupCooldown()
    {
        if (_speedPowerupActive == false)
        {
            _speedPowerupActive = true;
            _speed += 5f;
            yield return new WaitForSeconds(5.0f);
            _speedPowerupActive = false;
            _speed -= 5f;
        }

    }
    private IEnumerator TripleShotCooldown()
    {
        yield return new WaitForSeconds(5f);
        _isTripleShotActive = false;

    }
    public void Damage()
    {
        if (_isShieldActive == true)
        {
            if (_shieldStr == 3)
            {
                _shieldStr = 2;
            }
            if (_shieldStr == 2)
            {
                _shieldVisualizerRenderer.color = Color.green;
                _shieldStr = 1;
                return;
            }
            if (_shieldStr == 1)
            {
                _shieldVisualizerRenderer.color = Color.red;
                _shieldStr = 0;
                return;
            }
            if (_shieldStr == 0)
            {
                _isShieldActive = false;
                _playerShield.SetActive(false);
                _shieldVisualizerRenderer.color = Color.white;
                _shieldStr = 3;
            }
            return;
        }

        _lives--;
        AddScore(-5);
        CameraShake();
        _uiManager.UpdateLives(_lives);
        if (_lives == 3)
        {
            _leftEngine.SetActive(false);
            _rightEngine.SetActive(false);
        }

        if (_lives == 2)
        {
            _leftEngine.SetActive(true);
            _rightEngine.SetActive(false);
        }
        if (_lives == 1)
        {
            _rightEngine.SetActive(true);
        }

        if (_lives == 0)
        {
            _leftEngine.SetActive(false);
            _rightEngine.SetActive(false);
            _gameManager.PlayerIsDead();
            _spawnManager.PlayerIsDead();
            _uiManager.GameOver();

            Destroy(this.gameObject);
        }
    }
    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    private void CameraShake()
    {
        _originalCameraPos = _cameraTransform.position;

        StartCoroutine(CameraShaking());

    }
    IEnumerator CameraShaking()
    {
        yield return new WaitForSeconds(0.05f);
        _cameraTransform.position = _originalCameraPos + Random.insideUnitSphere * _shakeAmount;
        yield return new WaitForSeconds(0.05f);
        _cameraTransform.position = _originalCameraPos + Random.insideUnitSphere * _shakeAmount;
        yield return new WaitForSeconds(0.05f);
        _cameraTransform.position = _originalCameraPos + Random.insideUnitSphere * _shakeAmount;
        yield return new WaitForSeconds(0.05f);
        _cameraTransform.position = _originalCameraPos + Random.insideUnitSphere * _shakeAmount;
        yield return new WaitForSeconds(0.05f);
        _cameraTransform.position = _originalCameraPos;
    }


}