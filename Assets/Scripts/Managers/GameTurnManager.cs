using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTurnManager {

    #region singleton
    private static GameTurnManager instance;

    private GameTurnManager() { }

    public static GameTurnManager Instance
    {
        get
        {
            if (instance == null)
                instance = new GameTurnManager();

            return instance;
        }
    }
    #endregion singleton


    public int moneyPlayer1 = 6000;
    public int moneyPlayer2 = 6000;

    public bool playerOnePhase = true;
    public int Turn { get; private set; }

    bool moveSelectionPhase = false;
    bool attackSelectionPhase = false;

    int _SPEED = 7;

    private bool playable = true;
    private UnitManager.Unit selectedOne = null;
    BuildingManager.Building selectedBuilding = null;

    Timer.toCall popUpClose;
    Timer closePopUpTimer;

    Option move;
    Option attack;
    Option capture;
    Option infantery;
    Option tank;
    Option bazooka;

    public void Start() {
        Turn = 1;
        popUpClose = GameUI.Instance.ClosePopUp;
        popUpClose += SetPlayable;
        closePopUpTimer = new Timer(2f, popUpClose, false);

        Option.toCall moveHandler = new Option.toCall(Move);
        move = new Option("Move", moveHandler);

        Option.toCall attackHandler = new Option.toCall(Attack);
        attack = new Option("Attack", attackHandler);

        Option.toCall captureHandler = new Option.toCall(Capture);
        capture = new Option("Capture", captureHandler);

        Option.toCall tankHandler = new Option.toCall(MakeTank);
        tank = new Option("Tank", tankHandler);

        Option.toCall infanteryHandler = new Option.toCall(MakeInfantery);
        infantery = new Option("Infantery", infanteryHandler);

        Option.toCall bazookaHandler = new Option.toCall(MakeBazooka);
        bazooka = new Option("Bazooka", bazookaHandler);

        GameUI.Instance.UpdateMoney(moneyPlayer1);
        MapManager.Instance.SetNewTurn(playerOnePhase);

    }


    private void BeginNewTurn()
    {

        playerOnePhase = true;
        selectedOne = null;
        selectedBuilding = null;
        Turn++;
        GameUI.Instance.SendPopUp("Player 1\r\nTurn " + Turn);
        TimerManager.Instance.AddTimer(this, closePopUpTimer);
        moneyPlayer1 += BuildingManager.Instance.BeginOfPhase(playerOnePhase) * 500;
        GameUI.Instance.UpdateMoney(moneyPlayer1);
    }

    public void EndPhase()
    {
        playable = false;

        GameUI.Instance.CloseGameMenu();

        if (playerOnePhase)
        {
            playerOnePhase = false;
            GameUI.Instance.SendPopUp("Player 2\r\nTurn " + Turn);
            TimerManager.Instance.AddTimer(this, closePopUpTimer);
            moneyPlayer2 += BuildingManager.Instance.BeginOfPhase(playerOnePhase) * 500;
            GameUI.Instance.UpdateMoney(moneyPlayer2);
        }
        else
        {
            BeginNewTurn();
        }

        MapManager.Instance.SetNewTurn(playerOnePhase);
        UnitManager.Instance.NewPhase(playerOnePhase);
    }


    private void SetPlayable() {
        playable = true;
    }

    #region UnitsOptions

    private void Move() {
        GameUI.Instance.CloseActionMenu();
        selectedOne = UnitManager.Instance.Selected(MapManager.Instance.GetCursorPos());
        moveSelectionPhase = true;
    }
    private void Attack() {
        GameUI.Instance.CloseActionMenu();
        selectedOne = UnitManager.Instance.Selected(MapManager.Instance.GetCursorPos());

        attackSelectionPhase = true;
    }
    private void Capture() {
        GameUI.Instance.CloseActionMenu();
        selectedOne.Capture(selectedBuilding);
    }
    #endregion

    #region MakeUnit
    private void MakeTank() {

        
        if (playerOnePhase && moneyPlayer1 >= 2000)
        {
            moneyPlayer1 -= 2000;
            GameUI.Instance.UpdateMoney(moneyPlayer1);
            UnitManager.Instance.Make(playerOnePhase, UnitManager.UnitType.Tank, MapManager.Instance.GetCursorPos());
            GameUI.Instance.CloseActionMenu();
        }
        else if (!playerOnePhase && moneyPlayer2 >= 2000) {
            moneyPlayer2 -= 2000;
            GameUI.Instance.UpdateMoney(moneyPlayer2);
            UnitManager.Instance.Make(playerOnePhase, UnitManager.UnitType.Tank, MapManager.Instance.GetCursorPos());
            GameUI.Instance.CloseActionMenu();
        }
        
    }
    private void MakeInfantery() {
        if (playerOnePhase && moneyPlayer1 >= 2000)
        {
            moneyPlayer1 -= 2000;
            GameUI.Instance.UpdateMoney(moneyPlayer1);
            UnitManager.Instance.Make(playerOnePhase, UnitManager.UnitType.Infantery, MapManager.Instance.GetCursorPos());
            GameUI.Instance.CloseActionMenu();
        }
        else if (!playerOnePhase && moneyPlayer2 >= 2000)
        {
            moneyPlayer2 -= 2000;
            GameUI.Instance.UpdateMoney(moneyPlayer2);
            UnitManager.Instance.Make(playerOnePhase, UnitManager.UnitType.Infantery, MapManager.Instance.GetCursorPos());
            GameUI.Instance.CloseActionMenu();
        }
    }
    private void MakeBazooka() {
        if (playerOnePhase && moneyPlayer1 >= 2000)
        {
            moneyPlayer1 -= 2000;
            GameUI.Instance.UpdateMoney(moneyPlayer1);
            UnitManager.Instance.Make(playerOnePhase, UnitManager.UnitType.Bazooka, MapManager.Instance.GetCursorPos());
            GameUI.Instance.CloseActionMenu();
        }
        else if (!playerOnePhase && moneyPlayer2 >= 2000)
        {
            moneyPlayer2 -= 2000;
            GameUI.Instance.UpdateMoney(moneyPlayer2);
            UnitManager.Instance.Make(playerOnePhase, UnitManager.UnitType.Bazooka, MapManager.Instance.GetCursorPos());
            GameUI.Instance.CloseActionMenu();
        }
    }

    #endregion

    #region inputs

    public void PressEscape() {
        if (playable)
        {
            if (!GameUI.Instance.inGameMenu)
            {
                if (GameUI.Instance.inActionMenu) GameUI.Instance.CloseActionMenu();
                else GameUI.Instance.CallGameMenu();   
            }
            else if (moveSelectionPhase || attackSelectionPhase) {
                moveSelectionPhase = false;
                attackSelectionPhase = false;
                selectedOne = null;
            }
            else
            {
                GameUI.Instance.CloseGameMenu();
            }
        }
    }
    public void PressSpace(){
        Vector3 cursorPos = MapManager.Instance.GetCursorPos();
        Vector3Int pos = new Vector3Int((int) cursorPos.x, (int) cursorPos.y, 0);

        if (playable)
        {
            if (!GameUI.Instance.inGameMenu  && !GameUI.Instance.inActionMenu && !moveSelectionPhase && !attackSelectionPhase) {
                selectedOne = UnitManager.Instance.Selected(pos);
                selectedBuilding = BuildingManager.Instance.Selected(pos);


                if (selectedOne != null) {
                    if (selectedOne.team == playerOnePhase) {

                        List<Option> unitActions = new List<Option>();
                        if (selectedOne.CanMove) {
                            unitActions.Add(move);
                        }
                        if (selectedOne.CanAct) {
                            unitActions.Add(attack);
                            if (selectedBuilding != null) {
                                if(selectedBuilding.team != playerOnePhase) unitActions.Add(capture);
                            }
                        }

                        if (unitActions.Count > 0) {
                            GameUI.Instance.CallActionMenu(unitActions);
                        }
                    }
                }
                else if (selectedBuilding != null) { 
                    if (selectedBuilding.nature == false && selectedBuilding.team == playerOnePhase)
                        LaunchStoreMenu();
                }

            }
            else if (moveSelectionPhase) {
                moveSelectionPhase = false;
                selectedOne.Move(cursorPos);
            }
            else if (attackSelectionPhase) {
                attackSelectionPhase = false;
                selectedOne.Attack(UnitManager.Instance.Selected(cursorPos));
            }
            else {
                GameUI.Instance.PressSpace();
            }

        }
    }

    public void PressLeft() {
        if (playable)
        {
            Vector3 cursorPos = MapManager.Instance.GetCursorPos();

            if (!GameUI.Instance.inGameMenu && !GameUI.Instance.inActionMenu && !attackSelectionPhase && !moveSelectionPhase) {
                MapManager.Instance.MooveLeft();
            }
            else if (attackSelectionPhase) {
                if (cursorPos.x >= selectedOne.Position.x) {
                    MapManager.Instance.MooveLeft();
                }
            }
            else if (moveSelectionPhase) {
                int moveDist = 0;

                moveDist += (int) (cursorPos.x - selectedOne.Position.x);
                moveDist -= cursorPos.y - selectedOne.Position.y < 0 ? (int)-(cursorPos.y - selectedOne.Position.y) : (int)(cursorPos.y - selectedOne.Position.y);

                if (moveDist > -_SPEED) {
                    MapManager.Instance.MooveLeft();
                }
            }
        }
    }
    public void PressRight() {
        if (playable)
        {
            Vector3 cursorPos = MapManager.Instance.GetCursorPos();

            if (!GameUI.Instance.inGameMenu && !GameUI.Instance.inActionMenu && !attackSelectionPhase && !moveSelectionPhase) {
                MapManager.Instance.MooveRight();
            }
            else if (attackSelectionPhase) {
                if (cursorPos.x <= selectedOne.Position.x) {
                    MapManager.Instance.MooveRight();
                }
            }
            else if (moveSelectionPhase) {
                int moveDist = 0;

                moveDist += (int)(cursorPos.x - selectedOne.Position.x);
                moveDist += cursorPos.y - selectedOne.Position.y < 0 ? (int)-(cursorPos.y - selectedOne.Position.y) : (int)(cursorPos.y - selectedOne.Position.y);

                if (moveDist < _SPEED) {
                    MapManager.Instance.MooveRight();
                }
            }
        }
    }
    public void PressUp() {
        if (playable)
        {
            Vector3 cursorPos = MapManager.Instance.GetCursorPos();

            if (!GameUI.Instance.inGameMenu && !GameUI.Instance.inActionMenu && !attackSelectionPhase && !moveSelectionPhase) {
                MapManager.Instance.MooveUp();
            }
            else if (attackSelectionPhase) {
                if (cursorPos.y <= selectedOne.Position.y) {
                    MapManager.Instance.MooveUp();
                }
            }
            else if (moveSelectionPhase) {
                int moveDist = 0;

                moveDist += (int)(cursorPos.y - selectedOne.Position.y);
                moveDist += cursorPos.x - selectedOne.Position.x < 0 ? (int)-(cursorPos.x - selectedOne.Position.x) : (int)(cursorPos.x - selectedOne.Position.x);

                if (moveDist < _SPEED) {
                    MapManager.Instance.MooveUp();
                }
            }
            else
            {
                GameUI.Instance.PressUp();
            }
        }
    }
    public void PressDown() {
        if (playable)
        {
            Vector3 cursorPos = MapManager.Instance.GetCursorPos();

            if (!GameUI.Instance.inGameMenu && !GameUI.Instance.inActionMenu && !attackSelectionPhase && !moveSelectionPhase) {
                MapManager.Instance.MooveDown();
            }
            else if (attackSelectionPhase) {
                if (cursorPos.y >= selectedOne.Position.y) {
                    MapManager.Instance.MooveDown();
                }
            }
            else if (moveSelectionPhase) {
                int moveDist = 0;

                moveDist += (int)(cursorPos.y - selectedOne.Position.y);
                moveDist -= cursorPos.x - selectedOne.Position.x < 0 ? (int)-(cursorPos.x - selectedOne.Position.x) : (int)(cursorPos.x - selectedOne.Position.x);

                if (moveDist > -_SPEED) {
                    MapManager.Instance.MooveDown();
                }
            }
            else
            {
                GameUI.Instance.PressDown();
            }
        }
    }

    #endregion inputs

    private void LaunchStoreMenu() {
        List<Option> list = new List<Option>();

        list.Add(infantery);
        list.Add(bazooka);
        list.Add(tank);

        GameUI.Instance.CallActionMenu(list);
    }


    public void LaunchEndGame(bool winner) {
        playable = false;

        popUpClose = GameUI.Instance.ClosePopUp;
        popUpClose -= SetPlayable;
        popUpClose += EndGame;
        closePopUpTimer.ChangeTimeToWait(3f);

        string win = winner ? "red" : "blue";
        string victoryMSG = string.Format("Player {0}\r\n WIN", win);

        GameUI.Instance.SendPopUp(victoryMSG);
        TimerManager.Instance.AddTimer(this, closePopUpTimer);
    }

    private void EndGame() {
        Application.Quit();
    }
}
