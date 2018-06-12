using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameFlowManager
{

    #region singleton
    private static GameFlowManager instance;

    private GameFlowManager() { }

    public static GameFlowManager Instance
    {
        get
        {
            if (instance == null)
                instance = new GameFlowManager();

            return instance;
        }
    }
    #endregion singleton


    public bool inMenu;

    public void Start()
    {
        Cursor.visible = false;
        TimerManager.Instance.Start();
        InputManager.Instance.Start();
        UIManager.Instance.Start();
        SceneManager.Instance.Start();
        inMenu = true;
    }

    public void StartGame()
    {
        SceneManager.Instance.LoadScene("MainBattle");
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += StartGameLoaded;
    }

    public void MainMenu()
    {
        inMenu = true;
        SceneManager.Instance.LoadScene("MainMenu");
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnMenuLoaded;

    }

    private void OnMenuLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        InputManager.Instance.LaunchDelayer();
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnMenuLoaded;

    }

    private void StartGameLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        if (SceneManager.Instance.CurrentScene == "MainBattle")
        {
            GameUI.Instance.Start();
            UnitManager.Instance.Start();
            MapManager.Instance.Start();

            GameTurnManager.Instance.Start();
            BuildingManager.Instance.Start();
        }
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= StartGameLoaded;

    }

    private void UpdateGame() { }

    public void EndGame() { }

    // Update is called once per frame
    public void Update()
    {
        TimerManager.Instance.Update();
        InputManager.Instance.Update();
        if (!inMenu)
        {
            UpdateGame();
        }

    }

    public void RightPressed()
    {
        if (inMenu) UIManager.Instance.SwitchRight();
        else GameTurnManager.Instance.PressRight();
    }
    public void LeftPressed()
    {
        if (inMenu) UIManager.Instance.SwitchLeft();
        else GameTurnManager.Instance.PressLeft();

    }
    public void UpPressed()
    {
        if (!inMenu) GameTurnManager.Instance.PressUp();

    }
    public void DownPressed()
    {
        if (!inMenu) GameTurnManager.Instance.PressDown();
    }


    public void SpacePressed()
    {
        if (inMenu) UIManager.Instance.Confirm();
        else GameTurnManager.Instance.PressSpace();
    }


    public void EscapePressed()
    {
        if (!inMenu) GameTurnManager.Instance.PressEscape();
    }


}
