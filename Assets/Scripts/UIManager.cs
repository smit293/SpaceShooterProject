using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Text _restartText;
    [SerializeField]
    private Text _gameOverText;
    private bool _gameOver;
    [SerializeField]
    private Text _ammoText;
    [SerializeField]
    private Text _infinityText;
    [SerializeField]
    private Text _waveText;
    [SerializeField]
    private Text _enemiesRemainingText;
    [SerializeField]
    private Text _gameStartText;
    [SerializeField]
    private Text _youWinText, _finalScoreText;
    [SerializeField]
    private Image _livesImg;
    [SerializeField]
    private Sprite[] _livesSprites;
    [SerializeField]
    private Slider _thrustSlider;
    [SerializeField]
    private Slider _bossHealthSlider;
    private bool _isBossLevel;
    private int _finalScore;
    private int _currentScore;



    void Start()
    {
        _gameOver = false;
        _gameStartText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(false);
        _gameOverText.gameObject.SetActive(false);
        _bossHealthSlider.gameObject.SetActive(false);
        _infinityText.gameObject.SetActive(false);
        _scoreText.text = "Score: " + 0;
        _waveText.text = "";

    }


    public void GameStartText()
    {
        _gameStartText.gameObject.SetActive(false);
    }
    public void ShowBossHealth()
    {
        _bossHealthSlider.gameObject.SetActive(true);
    }
    public void BossHealth(float _bossHealth)
    {

        _bossHealthSlider.value = _bossHealth;
    }
    public void BoostSlider(float _sliderAmount)
    {
        _thrustSlider.value = _sliderAmount;
    }
    public void MaxSliderValue(float _maxValue)
    {
        _thrustSlider.maxValue = _maxValue;
        _thrustSlider.value = _maxValue;
    }
    public void AmmoCount(int ammoCount)
    {
        if (_isBossLevel == false)
        {
            _ammoText.text = "Ammo: " + ammoCount;
        }
        else if (_isBossLevel == true)
        {
            _ammoText.text = "Ammo: ";
            _infinityText.fontSize = 30;
            _infinityText.gameObject.SetActive(true);
            _infinityText.text = "\u221E";
        }
    }
    public void UpdateScore(int newScore)
    {
        _scoreText.text = "Score: " + newScore;
        _currentScore = newScore;
    }
    public void TotalScore()
    {

        _scoreText.gameObject.SetActive(false);
        _finalScore = _currentScore;
        _youWinText.gameObject.SetActive(true);
        _finalScoreText.text = "Final Score: " + _finalScore;
        _finalScoreText.gameObject.SetActive(true);

    }

    public void BossLevel()
    {
        _isBossLevel = true;
        BossLevelUI();
    }
    private void BossLevelUI()
    {
        _enemiesRemainingText.gameObject.SetActive(false);
    }

    public void UpdateLives(int currentLives)
    {
        if (currentLives < 0)
        {
            _livesImg.sprite = _livesSprites[0];
        }
        else if (currentLives >= 0)
        {
            _livesImg.sprite = _livesSprites[currentLives];
        }
    }

    public void GameOver()
    {
        _gameOver = true;
        _restartText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlicker());

    }
    public void EnemyCount(int _currentEnemy, int _maxEnemy)
    {
        _enemiesRemainingText.text = "Enemies Remaining: " + _currentEnemy + " / " + _maxEnemy;
    }
    public void Wave(int _wave)
    {
        StartCoroutine(WaveVisible(_wave));
    }

    IEnumerator WaveVisible(int _wave)
    {
        yield return new WaitForSeconds(0.1f);
        if (_wave == 1)
        {            
            _waveText.gameObject.SetActive(true);
            _waveText.text = "WAVE ONE";
        }
        if (_wave == 2)
        {
            _waveText.gameObject.SetActive(true);
            _waveText.text = "WAVE TWO";
        }
        if (_wave == 3)
        {
            _waveText.gameObject.SetActive(true);
            _waveText.text = "WAVE THREE";
        }
        yield return new WaitForSeconds(2.5f);
        _waveText.gameObject.SetActive(false);
        _waveText.text = "";

    }

    IEnumerator GameOverFlicker()
    {
        yield return new WaitForSeconds(0.1f);
        while (_gameOver == true)
        {
            _gameOverText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.4f);
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.4f);
        }
    }
}
