using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainEntry : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        DontDestroyOnLoad(gameObject);

        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }

        GameFlowManager.Instance.Start();
	}
	
	// Update is called once per frame
	void Update () {
        GameFlowManager.Instance.Update();
    }
}
