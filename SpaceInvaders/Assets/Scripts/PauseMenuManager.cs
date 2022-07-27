using System.Collections;
using UnityEngine;

[ExecuteAlways]
public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;

    [SerializeField] public bool gamePaused = false;

    void Update()
    {
        CheckInput();
    }

    void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gamePaused = !gamePaused;

            if(gamePaused)
            {
                Time.timeScale = 0.0f;
            }
            else
            {
                Time.timeScale = 1.0f;
            }

            PauseMenuActivation();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            Application.Quit();
        }
    }

    void PauseMenuActivation()
    {
        pauseMenu.SetActive(gamePaused);
    }
}
