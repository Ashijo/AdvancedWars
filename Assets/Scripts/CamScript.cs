using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamScript : MonoBehaviour {

    private Vector3 cursorPos;
    private Vector3 camPos;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        cursorPos = MapManager.Instance.GetCursorPos();
        camPos = gameObject.transform.position;


        if (cursorPos.x - camPos.x < -5 && camPos.x > - 3) camPos.x--;
        if (cursorPos.x - camPos.x > 5 && camPos.x < 17) camPos.x++;
        if (cursorPos.y - camPos.y < -4 && camPos.y > -1) camPos.y--;
        if (cursorPos.y - camPos.y > 4 && camPos.y < 5) camPos.y++;
        

        gameObject.transform.position = camPos;
    }
}
