using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using TMPro.Examples;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _scoreText;
    [SerializeField]
    private Image _livesDisplay;
    [SerializeField]
    private Sprite[] _liveSprites;

    [Header("Game Over Effects")]
    [SerializeField]
    private TMP_Text _gameOverText;
    [SerializeField]
    private TMP_Text _restartText;
    [SerializeField]
    private TMP_Text _mainMenuText;
    [SerializeField]
    private float _letterDisplayDelay, _flashDelay, restartDelay;
    [SerializeField]
    private int _flashCount;

    [Header("Thruster Gauge Settings")]
    [SerializeField]
    private Image _thrusterImage;
    [SerializeField]
    private float _drainRate, _fillRate, _blinkRate;
    [SerializeField]
    private bool _isFullyDrained = false;
    [SerializeField]
    private bool _isDraining = false;
    [SerializeField]
    private bool _isFilling = false;
    [SerializeField]
    private Color _colorChanger;

    private WaitForSeconds _pauseForLetter;
    private WaitForSeconds _pauseForFlash;
    private WaitForSeconds _pauseForRestart;

    private WaitForSeconds _drainDelay;
    private WaitForSeconds _fillDelay;
    private WaitForSeconds _blinkDelay;

    private GameManager _gameManager;

    private void Start()
    {
        UpdateLives(3);

        _pauseForLetter = new WaitForSeconds(_letterDisplayDelay);
        _pauseForFlash = new WaitForSeconds(_flashDelay);
        _pauseForRestart = new WaitForSeconds(restartDelay);

        _drainDelay = new WaitForSeconds(_drainRate);
        _fillDelay = new WaitForSeconds(_fillRate);
        _blinkDelay = new WaitForSeconds(_blinkRate);

        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if (_gameManager == null) Debug.LogError("Game Manager not found!");
    }

    public void UpdateScore(int value)
    {
        _scoreText.text = ("SCORE: " + value.ToString());
    }

    public void UpdateLives(int livesLeft)
    {
        if (livesLeft < 0 ||  livesLeft >= _liveSprites.Length)
        {
            Debug.LogError("Incorrect lives value supplied!");
            return;
        }

        _livesDisplay.sprite = _liveSprites[livesLeft];
    }

    [ContextMenu("Test Game Over")]
    public void GameOver()
    {
        _gameOverText.enabled = true;

        string msg = _gameOverText.text;
        _gameOverText.text = null;
        StartCoroutine(GameOverRoutine(msg));
    }

    private IEnumerator GameOverRoutine(string msg)
    {
        for (int i = 0; i < msg.Length; i++)
        {
            _gameOverText.text += msg[i];
            yield return _pauseForLetter;
        }

        bool flashGameOver = true;
        int flashCount = 0;

        while (flashGameOver)
        {
            yield return _pauseForFlash;
            _gameOverText.enabled = false;
            yield return _pauseForFlash;
            _gameOverText.enabled = true;

            flashCount++;
            if (flashCount >= _flashCount)
            {
                flashGameOver = false;
                yield return _pauseForRestart;
                _restartText.enabled = true;
                _mainMenuText.enabled = true;
                _gameManager.SetGameOver();
            }
        }        
    }

    public void StartDrainingGauge()
    {
        _isDraining = true;
        _isFilling = false;
        StartCoroutine(DrainGauge());
    }

    public void StopDrainingGauge()
    {
        _isDraining = false;
        _isFilling = true;
        StartCoroutine(FillGauge());
    }

    private IEnumerator DrainGauge()
    {
        _colorChanger = _thrusterImage.color;

        while (_isDraining)
        {
            yield return _drainDelay;
            _thrusterImage.fillAmount -= _drainRate;

            if (_colorChanger.r <= 1) _colorChanger.r += _drainRate * 2;
            if (_colorChanger.r >= 1) _colorChanger.g -= _drainRate * 2;

            _thrusterImage.color = _colorChanger;

            if (_thrusterImage.fillAmount <= 0)
            {
                _isFullyDrained = true;
                _isDraining = false;
                _isFilling = true;
                StartCoroutine(BlinkGauge());
                StartCoroutine(FillGauge());
            }
        }
    }

    private IEnumerator FillGauge()
    {
        _colorChanger = _thrusterImage.color;

        while (_isFilling)
        {
            yield return _fillDelay;
            _thrusterImage.fillAmount += _fillRate;

            if (_colorChanger.g <= 1) _colorChanger.g += _fillRate * 2;
            if (_colorChanger.g >= 1) _colorChanger.r -= _fillRate * 2;

            _thrusterImage.color = _colorChanger;

            if (_thrusterImage.fillAmount >= 1)
            {
                _colorChanger = new Color(0, 255, 0, 255);
                _thrusterImage.color = _colorChanger;
                _isFullyDrained = false;
                _isFilling = false;
            }
        }
    }

    private IEnumerator BlinkGauge()
    {
        while (_isFullyDrained)
        {
            _colorChanger.a = 0f;
            yield return _blinkDelay;
            _colorChanger.a = 1f;
            yield return _blinkDelay;
        }
    }

    public bool QueryFullDrain()
    {
        return _isFullyDrained;
    }
}
