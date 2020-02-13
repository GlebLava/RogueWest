using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject InventoryUIs_ItemSlotContainer;
    public GameObject PauseMenuUI;
    public GameObject UI_JoyStickHolder;


    // PauseButton cant be under Pause Menu because on Resume() PauseMenu gets disabled, so the pause Button becomes unresponsive 
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            } else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        InventoryUIs_ItemSlotContainer.SetActive(false);
        PauseMenuUI.SetActive(false);
        UI_JoyStickHolder.SetActive(true);

        Time.timeScale = 1f;

        GameIsPaused = false;
    }

   void Pause()
    {
        InventoryUIs_ItemSlotContainer.SetActive(true);
        PauseMenuUI.SetActive(true);
        UI_JoyStickHolder.SetActive(false);

        Time.timeScale = 0f;

        GameIsPaused = true;
    }

    public void PauseButtonUpdate()
    {
        if (GameIsPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }
}
