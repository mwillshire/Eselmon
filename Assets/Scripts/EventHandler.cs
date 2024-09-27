using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class EventHandler : MonoBehaviour
{
    [Header("Inscribed")]
    public GameObject pauseMenu;
    public GameObject inventoryMenu;
    public GameObject idleUI;
    public GameObject combatUI;

    [Header("Dynamic")]
    public bool isPaused = false;
    public bool isInventoryOpen = false;
    public bool inCombat = false;

    private bool escPressed = false;

    private SceneManagement sM;
    private AudioHandler aH;

    private void Awake()
    {
        sM = GetComponent<SceneManagement>();
        aH = FindFirstObjectByType<AudioHandler>();

        pauseMenu.SetActive(false);
    }

    private void Start()
    {
        EnterFreeMode();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.P) && !isPaused)
        {
            PauseGame();
            aH.Play("InterfaceClick");
        }
        if (Input.GetKey(KeyCode.E) && !isPaused && !isInventoryOpen)
        {
            OpenInventory();
            aH.Play("InterfaceClick");
        }
        if (StaticVariables.combatMode && !inCombat && !isPaused) EnterCombatMode();

        if (isPaused)
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !escPressed)
            {
                UnPauseGame();
                escPressed = true;
                aH.Play("InterfaceBack");
            }
            return;
        }

        if (isInventoryOpen)
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !isPaused && !escPressed)
            {
                CloseInventory();
                escPressed = true;
                aH.Play("InterfaceBack");
            }
        }

        if (inCombat)
        {
            if (!StaticVariables.combatMode && !isPaused) EnterFreeMode();
        }

        escPressed = false;
    }

    public void TransDeathScreen()
    {
        sM.LoadScene(9);
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        isPaused = true;
    }

    private void UnPauseGame()
    {
        if (!isInventoryOpen) Time.timeScale = 1;
        pauseMenu.SetActive(false);
        isPaused = false;
    }

    private void OpenInventory()
    {
        inventoryMenu.SetActive(true);
        Time.timeScale = 0;
        isInventoryOpen = true;
    }

    private void CloseInventory()
    {
        Time.timeScale = 1;
        inventoryMenu.SetActive(false);
        isInventoryOpen = false;
    }

    private void EnterCombatMode()
    {
        idleUI.SetActive(false);
        combatUI.SetActive(true);
        inCombat = true;
    }

    private void EnterFreeMode()
    {
        idleUI.SetActive(true);
        combatUI.SetActive(false);
        inCombat = false;
    }
}
