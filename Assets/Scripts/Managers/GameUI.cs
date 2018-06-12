using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI {

    #region singleton
    private static GameUI instance;

    private GameUI() { }

    public static GameUI Instance
    {
        get
        {
            if (instance == null)
                instance = new GameUI();

            return instance;
        }
    }
    #endregion singleton

    GameObject money;
    GameObject actionPanel;
    GameObject menuPanel;
    GameObject popUpPanel;
    GameObject arrow;
    GameObject[] ActionOptions;

    public bool inActionMenu = false;
    public bool inGameMenu = false;

    private int iterator;

    Vector3 neutralPosArrow;
    List<Option> actionList;

    private int nbOptions = 0;

    public void Start() {
        money = GameObject.Find("Cash");
        actionPanel = GameObject.Find("GameActions");
        menuPanel = GameObject.Find("GameMenu");
        popUpPanel = GameObject.Find("GamePopUp");
        arrow = GameObject.Find("Arrow");

        neutralPosArrow = new Vector3(50, 0, 0);

        iterator = 0;


        ActionOptions = new GameObject[8];

        ActionOptions[0] = GameObject.Find("Option1");
        ActionOptions[1] = GameObject.Find("Option2");
        ActionOptions[2] = GameObject.Find("Option3");
        ActionOptions[3] = GameObject.Find("Option4");

        ActionOptions[4] = GameObject.Find("Option5");
        ActionOptions[5] = GameObject.Find("Option6");
        ActionOptions[6] = GameObject.Find("Option7");
        ActionOptions[7] = GameObject.Find("Option8");


        menuPanel.SetActive(false);
        actionPanel.SetActive(false);
        popUpPanel.SetActive(false);

    }

    public void PressDown() {
        iterator++;
        CheckIterator();
        SetCursor();
        
    }

    public void PressUp() {
        iterator--;
        CheckIterator();

        SetCursor();
        
    }

    #region CanvasControler
    public void SendPopUp(string message) {
        popUpPanel.GetComponentInChildren<Text>().text = message;
        popUpPanel.SetActive(true);
    }

    public void ClosePopUp()
    {
        popUpPanel.SetActive(false);
    }

    public void CallGameMenu() {
        iterator = 4;
        inGameMenu = true;
        menuPanel.SetActive(true);
        SetCursor();
    }

    public void CloseGameMenu() {
        iterator = 4;
        inGameMenu = false;
        menuPanel.SetActive(false);
    }
    public void CallActionMenu(List<Option> list) {
        iterator = 0;
        nbOptions = list.Count;
        inActionMenu = true;
        actionPanel.SetActive(true);
        SetCursor();
        actionList = list;

        for (int i = 0; i < nbOptions; i++) {
            ActionOptions[i].SetActive(true);
            ActionOptions[i].GetComponentInChildren<Text>().text = list[i].Message;
        }
        for (int i = nbOptions; i < 4; i++) {
            ActionOptions[i].SetActive(false);
        }

    }

    public void CloseActionMenu() {
        iterator = 4;
        inActionMenu = false;
        actionPanel.SetActive(false);
    }
    #endregion CanvaControler


    public void PressSpace() {
            switch (iterator)
            {
                case 0: actionList[0].Selected(); break;
                case 1: actionList[1].Selected(); break;
                case 2: actionList[2].Selected(); break;
                case 3: actionList[3].Selected(); break;
                case 4: Option5(); break;
                case 5: Option6(); break;
                case 6: Option7(); break;
                case 7: Option8(); break;
            }
    }

    public void UpdateMoney(int money) {
        this.money.GetComponent<Text>().text = money + " $";

    }

   
    public void Option5()
    {
        //Debug.Log("Option 5 selected");
        inGameMenu = false;
        inActionMenu = false;
        CloseGameMenu();
    }
    public void Option6()
    {
        //Debug.Log("Option 6 selected");

    }
    public void Option7()
    {
        //Debug.Log("Option 7 selected");
        GameTurnManager.Instance.EndPhase();
    }
    public void Option8()
    {
        CloseGameMenu();
        actionPanel.SetActive(true);
        menuPanel.SetActive(true);
        GameFlowManager.Instance.MainMenu();
    }

    private void CheckIterator() {
        if (inActionMenu)
        {
            iterator = iterator > nbOptions -1 ? 0 : iterator;
            iterator = iterator < 0 ? nbOptions -1 : iterator;
        }
        else {
            iterator = iterator > 7 ? 4 : iterator;
            iterator = iterator < 4 ? 7 : iterator;

        }
    }

    private void SetCursor() {

        arrow.transform.SetParent(ActionOptions[iterator].transform);
        arrow.transform.localPosition = neutralPosArrow;
    }



}

public class Option{

    public string Message { get; private set; }
    toCall handler;
    public delegate void toCall();

    public Option(string message, toCall handler) {
        Message = message;
        this.handler = handler;
    }

    public void Selected() {
        handler.Invoke();
    }

}