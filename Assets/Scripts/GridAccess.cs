using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridAccess : MonoBehaviour {


    [SerializeField]
    public List<TilePair> buildings;

    [SerializeField]
    public List<SpritePair> units;

    // Use this for initialization
    void Start() {



    }

    [System.Serializable]
    public class TilePair{

        public string key;
        public Tile value;
    }

    [System.Serializable]
    public class SpritePair
    {

        public string key;
        public Sprite value;
    }

    public Tile GetTile(string tile_name) {
        Tile backer = null;

        foreach (TilePair pair in buildings) 
            if (pair.key == tile_name) backer = pair.value;
        
        return backer;
    }

    public Sprite GetSprite(string sprite_name) {
        Sprite backer = null;

        foreach (SpritePair pair in units)
            if (pair.key == sprite_name) backer = pair.value;

        return backer;
    }
}
