using UnityEngine;
using UnityEngine.InputSystem;

public class PauseButtonSkript : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;

    private bool isPaused; //будет кнопка звука потом

    void Update()
    {
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            PauseGame();
        }

        if (Keyboard.current.cKey.wasPressedThisFrame)
        {
            ContinueGame();
        }
    }

    public void PauseGame()
    {
        if (isPaused)
        {
            return;
        }

        isPaused = true;
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ContinueGame()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }
}
