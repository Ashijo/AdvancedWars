using System;
using UnityEngine;
using UnityEngine.UI;



//TODO Change the name of this manager for MenuManager
public class UIManager
{

    #region singleton
    private static UIManager instance;

    private UIManager() { }

    public static UIManager Instance
    {
        get
        {
            if (instance == null)
                instance = new UIManager();

            return instance;
        }
    }
    #endregion singleton

    private Text selection;
    string[] MainMenuSelection = { "Game", "Options", "Quit" };
    int iteratorMainMenu = 0;

    // Use this for initialization
    public void Start()
    {
        selection = GameObject.Find("Selection").GetComponent<Text>();
    }

    public void SwitchLeft()
    {
        
        iteratorMainMenu--;
        iteratorMainMenu = (iteratorMainMenu <= -1) ? 2 : iteratorMainMenu;

        selection.text = MainMenuSelection[iteratorMainMenu];
    }

    public void SwitchRight()
    {
        iteratorMainMenu++;
        iteratorMainMenu = (iteratorMainMenu >= 3) ? 0 : iteratorMainMenu;
        selection.text = MainMenuSelection[iteratorMainMenu];
    }



    public void Confirm()
    {

        switch (iteratorMainMenu)
        {
            case 0:
                GameFlowManager.Instance.inMenu = false;
                GameFlowManager.Instance.StartGame();
                break;
            case 1:; break;
            case 2: Application.Quit(); break;
        }

    }

}

