using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingManager
{

    #region singleton
    private static BuildingManager instance;

    private BuildingManager() { }

    public static BuildingManager Instance
    {
        get
        {
            if (instance == null)
                instance = new BuildingManager();

            return instance;
        }
    }
    #endregion singleton


    List<Building> buildings;
    Tilemap tilemap = null;
    GridAccess gridAccess = null;

    //city red = Tileset_326
    //factory red = Tileset_325
    // QG red = Tileset_324


    // factory blue = Tileset_330
    // city blue = Tileset_331
    // QG blue = Tileset 329

    // factory neutral = Tileset 320
    // city neutral = Tileset 321


    public void Start()
    {
        tilemap = GameObject.Find("Buildings").GetComponent<Tilemap>();
        gridAccess = GameObject.Find("Grid").GetComponent<GridAccess>();
        buildings = new List<Building>();

        Building qgBlue = new Building(false, null, new Vector3Int(25, 9, 0));
        Building ct1Blue = new Building(false, true, new Vector3Int(22, 10, 0));
        Building ct2Blue = new Building(false, true, new Vector3Int(21, 6, 0));
        Building ct3Blue = new Building(false, true, new Vector3Int(24, 5, 0));
        Building ft1Blue = new Building(false, false, new Vector3Int(20, 8, 0));
        Building ft2Blue = new Building(false, false, new Vector3Int(23, 4, 0));

        Building qgRed = new Building(true, null, new Vector3Int(-12, -7, 0));
        Building ct1Red = new Building(true, true, new Vector3Int(-12, -5, 0));
        Building ct2Red = new Building(true, true, new Vector3Int(-9, -6, 0));
        Building ct3Red = new Building(true, true, new Vector3Int(-10, -8, 0));
        Building ft1Red = new Building(true, false, new Vector3Int(-7, -7, 0));
        Building ft2Red = new Building(true, false, new Vector3Int(-10, -3, 0));

        Building c1w = new Building(null, true, new Vector3Int(-12, 11, 0));
        Building c2w = new Building(null, true, new Vector3Int(-13, 10, 0));
        Building c3w = new Building(null, true, new Vector3Int(-12, 9, 0));
        Building c4w = new Building(null, true, new Vector3Int(-11, 10, 0));
        Building c5w = new Building(null, true, new Vector3Int(-1, 1, 0));
        Building c6w = new Building(null, true, new Vector3Int(6, 1, 0));
        Building c7w = new Building(null, true, new Vector3Int(5, -3, 0));
        Building c8w = new Building(null, true, new Vector3Int(10, -5, 0));
        Building c9w = new Building(null, true, new Vector3Int(11, 3, 0));
        Building c10w = new Building(null, true, new Vector3Int(17, -1, 0));
        Building c11w = new Building(null, true, new Vector3Int(19, -8, 0));
        Building c12w = new Building(null, true, new Vector3Int(20, -6, 0));
        Building c13w = new Building(null, true, new Vector3Int(22, -8, 0));
        Building c14w = new Building(null, true, new Vector3Int(22, -4, 0));

        Building f1w = new Building(null, false, new Vector3Int(13, 10, 0));
        Building f2w = new Building(null, false, new Vector3Int(10, 0, 0));
        Building f3w = new Building(null, false, new Vector3Int(2, 4, 0));
        Building f4w = new Building(null, false, new Vector3Int(2, -4, 0));


        buildings.Add(qgBlue);
        buildings.Add(ct1Blue);
        buildings.Add(ct2Blue);
        buildings.Add(ct3Blue);
        buildings.Add(ft1Blue);
        buildings.Add(ft2Blue);

        buildings.Add(qgRed);
        buildings.Add(ct1Red);
        buildings.Add(ct2Red);
        buildings.Add(ct3Red);
        buildings.Add(ft1Red);
        buildings.Add(ft2Red);

        buildings.Add(c1w);
        buildings.Add(c2w);
        buildings.Add(c3w);
        buildings.Add(c4w);
        buildings.Add(c5w);
        buildings.Add(c6w);
        buildings.Add(c7w);
        buildings.Add(c8w);
        buildings.Add(c9w);
        buildings.Add(c10w);
        buildings.Add(c11w);
        buildings.Add(c12w);
        buildings.Add(c13w);
        buildings.Add(c14w);

        buildings.Add(f1w);
        buildings.Add(f2w);
        buildings.Add(f3w);
        buildings.Add(f4w);

    }

    public Building Selected(Vector3Int pos) {
        Building building = null;

        foreach (Building b in buildings)
        {
            if (b.pos == pos) {
               // b.Present();
                building = b;
            }
        }

        return building;
    }


    public int BeginOfPhase(bool team)
    {
        //return the number of buildings for each team

        int nb_building = 0;

        foreach (Building b in buildings)
            if (b.team == team) nb_building++;

        return nb_building;
    }


    public void EndOfPhase() { }


    public class Building
    {
        public bool? team;
        // true => red
        // null => neutral
        // false => blue

        public readonly Vector3Int pos;
        public bool? nature;
        //null => QG
        //true => city
        //false => Factory

        public Building(bool? team, bool? nature, Vector3Int pos)
        {
            this.team = team;
            this.nature = nature;
            this.pos = pos;
        }

        public void Present() {

            string type;

            if (team == true)
                type = "red ";
            else if (team == null)
                type = "neutral ";
            else
                type = "blue ";

            if (nature == true)
                type += "city";
            else if (nature == null)
                type += "QG";
            else
                type = "factory";


            Debug.Log(string.Format("You selected a {0} from ({1},{2})", type, pos.x, pos.y));
        }



        public void Captured(bool team)
        {
            if (this.team != team)
            {
                this.team = team;
                string tile_to_switch = "";

                if (nature == true)  tile_to_switch = team ? "RedCity" : "BlueCity";
                else if (nature == false) tile_to_switch = team ? "RedFactory" : "BlueFactory";
                else tile_to_switch = team ? "RedQG" : "BlueQG";

                Tile tile = Instance.gridAccess.GetTile(tile_to_switch);


                if (tile != null)
                {
                    Instance.tilemap.SetTile(pos, tile);
                    if (nature != false)
                    {
                        Vector3Int s_pos = new Vector3Int(pos.x, pos.y + 1, 0);
                        Instance.tilemap.SetTile(s_pos, Instance.gridAccess.GetTile(tile_to_switch + "Top"));
                    }
                }
                else Debug.Log("ERROR : UNEXISTING TILE");


                if (nature == null) GameTurnManager.Instance.LaunchEndGame(team);
            }
        }


    }

}

