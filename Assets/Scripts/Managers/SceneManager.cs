using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager {


    #region singleton
    private static SceneManager instance;

    private SceneManager() { }

    public static SceneManager Instance {
        get {
            if (instance == null)
                instance = new SceneManager();

            return instance;
        }
    }
    #endregion singleton

    public string CurrentScene { get; private set;}

    public void Start() {
        CurrentScene = "MainMenu";
    }
    
    public void LoadScene(string sceneName) {
        CurrentScene = sceneName;
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

}
