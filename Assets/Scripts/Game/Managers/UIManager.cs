using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

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

    private WaitForSeconds _pauseForLetter;
    private WaitForSeconds _pauseForFlash;
    private WaitForSeconds _pauseForRestart;

    private GameManager _gameManager;

    private void Start()
    {
        UpdateLives(3);

        _pauseForLetter = new WaitForSeconds(_letterDisplayDelay);
        _pauseForFlash = new WaitForSeconds(_flashDelay);
        _pauseForRestart = new WaitForSeconds(restartDelay);

        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if (_gameManager == null) Debug.LogError("Game Manager not found!");
    }

    public void UpdateScore(int value)
    {
        _scoreText.text = ("SCORE: " + value.ToString());
    }

    public void UpdateLives(int livesLeft)
    {
        switch (livesLeft)
        {
            case 0:
                _livesDisplay.sprite = _liveSprites[0];
                break;
            case 1:
                _livesDisplay.sprite = _liveSprites[1];
                break;
            case 2:
                _livesDisplay.sprite = _liveSprites[2];
                break;
            case 3:
                _livesDisplay.sprite = _liveSprites[3];
                break;
            default:
                Debug.LogError("Incorrect lives value supplied!");
                break;
        }
    }

    public void GameOver()
    {
        _gameOverText.enabled = true;

        string msg = _gameOverText.text;
        _gameOverText.text = null;
        StartCoroutine(GameOverRoutine(msg));
    }

    private IEnumerator GameOverRoutine(string msg)
    {
        for (int i=0; i<msg.Length; i++)
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
}
