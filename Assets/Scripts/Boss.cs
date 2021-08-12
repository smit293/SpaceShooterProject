using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    private Animator _animator;
    [SerializeField]
    private GameObject _bossAttackOne;
    [SerializeField]
    private GameObject _bossAttackTwo;
    [SerializeField]
    private GameObject _damagedState;
    [SerializeField]
    private GameObject _damagedMoreState;
    [SerializeField]
    private GameObject _deathExplosion;
    [SerializeField]
    private GameObject _bossExplosion;
    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip[] _audioClips;
    private bool _attackable;
    private float _bossHealth;
    private GameObject _backgroundMusic;
    private UIManager _uiManager;
    private SpriteRenderer _spriteRenderer;
    private float _speed;
    private Vector3 _endPos;
    private bool _stageTwo;
    private bool _stageThree;
    private bool _started;
    private float xPos;
    private Vector2 _moveDirection = Vector2.left;
    [SerializeField]
    private Vector3[] _movePoints;
    int i;
    void Start()
    {

        _uiManager = FindObjectOfType<UIManager>();
        _backgroundMusic = GameObject.FindGameObjectWithTag("BackgroundMusic");
        _bossHealth = 100f;
        _uiManager.BossHealth(_bossHealth);
        _animator = GetComponent<Animator>();
        _damagedState.SetActive(false);
        _damagedMoreState.SetActive(false);
        _audioSource = GetComponent<AudioSource>();
        _endPos = new Vector3(0, 3.7f, 0);
        _speed = 10.0f;
        StartCoroutine(MoveToStart());
        _spriteRenderer = GetComponent<SpriteRenderer>();


    }
    private void Update()
    {
        //Debug.Log(_bossHealth + "Boss Health");
        if (transform.position == _endPos && _started == false)

        {
            _started = true;

            StopCoroutine(MoveToStart());
            _uiManager.ShowBossHealth();
            StageOne();

        }
        xPos = transform.position.x;


        if (_bossHealth == 65f && _stageTwo == false && _stageThree == false)
        {
            _stageTwo = true;
            _damagedState.SetActive(true);
            StageTwo();
        }
        if (_bossHealth == 35f && _stageTwo == true && _stageThree == false)
        {
            _stageTwo = false;
            _stageThree = true;
            StartCoroutine(DamageFlashing());
            _damagedMoreState.SetActive(true);
            StageThree();
        }
        if (_stageThree == true && _bossHealth <= 0)
        {
            _stageThree = false;
            _animator.SetBool("Dying", true);
            _spriteRenderer.color = Color.white;
            RemoveAttacks();
            StopAllCoroutines();
            StartCoroutine(Dying());
            
        }


    }
    private void RemoveAttacks()
    {
        GameObject[] _enemyLaser = GameObject.FindGameObjectsWithTag("EnemyLaser");
        foreach (GameObject go in _enemyLaser)
        {
            Destroy(go);
        }
    }
    private void StageOne()
    {
        _backgroundMusic.GetComponent<BackgroundMusic>().TurnOff();
        StartCoroutine(AttackOne());
        StartCoroutine(StageOneMoving());
        _audioSource.clip = _audioClips[0];
        _audioSource.Play();
        _attackable = true;
      
    }


    private void StageTwo()
    {
        StopCoroutine(StageOneMoving());
        StartCoroutine(StageTwoMoving());
        _audioSource.clip = _audioClips[1];
        _audioSource.Play();
        StartCoroutine(AttackTwo());
      
    }
    private void StageThree()
    {
        StopCoroutine(StageTwoMoving());
        StartCoroutine(StageThreeMoving());
        _audioSource.clip = _audioClips[2];
        _audioSource.Play();

    }
    IEnumerator StageOneMoving()

    {
        _animator.SetBool("Moving", true);
        yield return new WaitForSeconds(0.1f);
        _speed = 14f;
        while (_stageTwo == false && _stageThree == false)
        {
            if (xPos <= -7.44f)
            {
                _moveDirection = Vector2.right;
            }
            if (xPos >= 7.44f)
            {
                _moveDirection = Vector2.left;
            }
            transform.Translate(_speed * Time.deltaTime * _moveDirection);
            yield return new WaitForSeconds(0.1f);
        }
    }
    IEnumerator StageTwoMoving()
    {

        yield return new WaitForSeconds(0.1f);
        _speed = 5f;
        i = Random.Range(0, _movePoints.Length);
        while (_stageTwo == true && _stageThree == false)
        {

            if (transform.position == _movePoints[i])
            {
                i = Random.Range(0, _movePoints.Length);

            }
            else if (transform.position != _movePoints[i])
            {

            }
            transform.position = Vector2.MoveTowards(transform.position, _movePoints[i], _speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

    }


    IEnumerator StageThreeMoving()
    {

        yield return new WaitForSeconds(0.1f);
        _speed = 8f;
        i = Random.Range(0, _movePoints.Length);
        Debug.Log("Stage Three Move Started current direction should be: " + _movePoints[i]);
        while (_stageTwo == false && _stageThree == true)
        {

            if (transform.position == _movePoints[i])
            {
                i = Random.Range(0, _movePoints.Length);

            }
            else if (transform.position != _movePoints[i])
            {

            }
            Debug.Log("Should be moving to :" + _movePoints[i]);
            transform.position = Vector2.MoveTowards(transform.position, _movePoints[i], _speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator MoveToStart()
    {
        yield return new WaitForSeconds(0.2f);
        _speed = 25f;
        while (transform.position != _endPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, _endPos, _speed * Time.deltaTime);
            yield return new WaitForSeconds(0.1f);
        }
    }
    IEnumerator DamageFlashing()
    {
        yield return new WaitForEndOfFrame();
        while (_stageThree == true)
        {
            _spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.5f);
            _spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(0.5f);
        }
    }


    public void Damaged()
    {

        if (_bossHealth > 0 && _attackable == true)
        {

            _bossHealth -= 1f;
            _uiManager.BossHealth(_bossHealth);

        }
    }

    IEnumerator AttackOne()
    {
        while (true)
        {
            yield return new WaitForSeconds(2.0f);
            Instantiate(_bossAttackOne, transform.position, Quaternion.identity);
            _animator.SetBool("Fire1", true);
            yield return new WaitForSeconds(0.1f);
            _animator.SetBool("Fire1", false);
        }
    }
    IEnumerator AttackTwo()
    {

        while (true)
        {


            yield return new WaitForSeconds(11.0f);
            _animator.SetBool("Fire2", true);
            Vector3 _offSet = new Vector3(0, -0.55f, 0);
            Instantiate(_bossAttackTwo, transform.position + _offSet, Quaternion.identity);
            yield return new WaitForSeconds(0.8f);
            _animator.SetBool("Fire2", false);


        }
    }
    IEnumerator Dying()
    {
        yield return new WaitForSeconds(2.0f);
        _uiManager.TotalScore();
        Instantiate(_bossExplosion, transform.position, Quaternion.identity);
        Instantiate(_deathExplosion, transform.position, Quaternion.identity);
        Destroy(this.gameObject);

    }
}