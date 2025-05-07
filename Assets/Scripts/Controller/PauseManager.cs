using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance;

    [Header("UI References")]
    public GameObject pausePanel;
    public GameObject pauseButton;

    private void Awake()
    {
        Instance = this;
        pausePanel.SetActive(false); 
    }

    public void OnPausePressed()
    {
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
        pauseButton.SetActive(false); 
    }

    public void OnContinuePressed()
    {
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
        pauseButton.SetActive(true);
    }

    public void OnExitPressed()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}
