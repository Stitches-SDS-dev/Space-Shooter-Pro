using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool _isGameOver;
    [SerializeField]
    private GameObject _asteroid;

    private void Start()
    {
        StartWave();
    }

    private void Update()
    {
        if (Screen.fullScreen && Input.GetKeyDown(KeyCode.Escape)) Screen.fullScreen = false;
        if (!Screen.fullScreen && Input.GetKeyDown(KeyCode.Escape)) SceneManager.LoadScene(0);

        if (_isGameOver)
        {
            if (Input.GetKeyDown(KeyCode.M)) MainMenu();
            if (Input.GetKeyDown(KeyCode.R)) RestartGame();
        }
    }

    public void StartWave()
    {
        Instantiate(_asteroid);
    }

    public void SetGameOver()
    {
        _isGameOver = true;
    }

    private void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(1);
    }
}
