using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager {

    #region singleton
    private static InputManager instance;

    private InputManager() { }

    public static InputManager Instance
    {
        get
        {
            if (instance == null)
                instance = new InputManager();

            return instance;
        }
    }
    #endregion singleton


    bool inputDelayer = true;
    Timer.toCall delayerHandler;
    Timer delayer;

    // Use this for initialization
    public void Start () {
        delayerHandler = StopDelayer;
        delayer = new Timer(.2f, delayerHandler, false);
    }

    void StopDelayer() { 
        inputDelayer = true;
    }

    public void LaunchDelayer() {

        inputDelayer = false;
        TimerManager.Instance.AddTimer(this, delayer);
    }

    // Update is called once per frame
    public void Update () {

        if (inputDelayer)
        {
            if (Input.GetAxis("Right") > 0.7)
            {
                GameFlowManager.Instance.RightPressed();
                LaunchDelayer();
            }
            if (Input.GetAxis("Left") > 0.7)
            {
                GameFlowManager.Instance.LeftPressed();
                LaunchDelayer();
            }
            if (Input.GetAxis("Up") > 0.7)
            {
                GameFlowManager.Instance.UpPressed();
                LaunchDelayer();
            }
            if (Input.GetAxis("Down") > 0.7)
            {
                GameFlowManager.Instance.DownPressed();
                LaunchDelayer();
            }
            if (Input.GetAxis("Confirm") > 0.7)
            {
                GameFlowManager.Instance.SpacePressed();
                LaunchDelayer();
            }
            if (Input.GetAxis("Cancel") > 0.7)
            {
                GameFlowManager.Instance.EscapePressed();
                LaunchDelayer();
            }
        }
    }
}
