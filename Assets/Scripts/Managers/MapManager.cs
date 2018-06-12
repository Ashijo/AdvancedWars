using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager  {

    #region singleton
    private static MapManager instance;

    private MapManager() { }

    public static MapManager Instance
    {
        get
        {
            if (instance == null)
                instance = new MapManager();

            return instance;
        }
    }
    #endregion singleton

    //Tilemap tilemap;
    GameObject cursor;

    public void Start () {
        //tilemap = GameObject.Find("Grid").GetComponent<Tilemap>();
        cursor = GameObject.Find("Cursor");
    }

    // Update is called once per frame
    public void Update () {
		
	}

   // public void ChangeCase(Vector3Int pos, string tile_name) {
    //    Tile tile = tilemap.GetTile(pos) as Tile;
     //   tile.sprite = null;
   // }

    public Vector3 GetCursorPos() {
        if (cursor != null)
            return cursor.transform.position;
        else
            return new Vector3(0, 0, 0);
    }

    public void SetNewTurn(bool team) {
        GameObject camera = GameObject.Find("MainCamera");

        if (team)
        {
            camera.transform.position = new Vector3(-3, -1, -10);
            cursor.transform.position = new Vector3(-12,-7,0);
        }
        else {
            camera.transform.position = new Vector3(17,5,-10);
            cursor.transform.position = new Vector3(25, 9, 0);
        }

    }

    public void MooveLeft() {
        if (cursor.transform.position.x > -15)
        {
            Vector3 pos = cursor.transform.position;
            pos.x--;
            cursor.transform.position = pos;
        }
        //Debug.Log("Cursor Pos : " + cursor.transform.position);
    }
    public void MooveRight() {
        if (cursor.transform.position.x < 28)
        {
            Vector3 pos = cursor.transform.position;
            pos.x++;
            cursor.transform.position = pos;
          //  Debug.Log("Cursor Pos : " + cursor.transform.position);
        }
    }
    public void MooveUp() {
        if (cursor.transform.position.y < 12)
        {
            Vector3 pos = cursor.transform.position;
            pos.y++;
            cursor.transform.position = pos;
          //  Debug.Log("Cursor Pos : " + cursor.transform.position);
        }
    }
    public void MooveDown() {
        if (cursor.transform.position.y > -9)
        {
            Vector3 pos = cursor.transform.position;
            pos.y--;
            cursor.transform.position = pos;
          //  Debug.Log("Cursor Pos : " + cursor.transform.position);
        }
    }

}
