using System.Collections.Generic;
using UnityEngine;

public class UnitManager {

    #region singleton
    private static UnitManager instance;

    private UnitManager() { }

    public static UnitManager Instance {
        get {
            if (instance == null)
                instance = new UnitManager();

            return instance;
        }
    }
    #endregion singleton


    GridAccess gridAccess;
    List<Unit> unitPool;
    int[,] type_table;
    // [Attacker, Defender]

    // Use this for initialization
    public void Start () {
        gridAccess = GameObject.Find("Grid").GetComponent<GridAccess>();
        unitPool = new List<Unit>();
        type_table = new int[3, 3] {{0,1,-1}, {-1,0,1}, {1,-1,0}};
	}

    public void Make(bool team, UnitType type, Vector3 where) {
        bool revived = false;
        foreach (Unit unit in unitPool) {
            if (unit.dead) {
                unit.ReUse(team, where, type);
                revived = true;
                break;
            }
        }
        if (!revived) {
            Unit unit = new Unit(team, where, type);
            unitPool.Add(unit);
        }
    }

    public void NewPhase(bool team) {
        foreach (Unit unit in unitPool) 
            unit.NewTurn();
    }

    public Unit Selected(Vector3 position) {
        Unit back = null;
        foreach (Unit unit in unitPool) {
            if (!unit.dead && unit.Position == position) {
                back = unit;  
            }
        }
        return back;
    }

    public void Remove(Unit toRemove) {

    }

    public void Close() {
    }

    public class Unit {
        public UnitType type;

        public bool dead;
        public bool team;
        //true => red, false => blue
        public Vector3 Position { get; private set; }

        public bool CanMove { get; private set; }
        public bool CanAct { get; private set; }
        public int Hp { get; private set; }
        private GameObject gameObject;
        private SpriteRenderer render;


        public void Present() {
            Debug.Log("I'm a "+ type + " at "+ Position);
        }

        public void Move(Vector3 newPos) {
            if (CanMove)
            {
                Position = newPos;
                gameObject.transform.position = Position;
                CanMove = false;
            }
            else {
                Debug.Log("ERROR : MOVE HAVE BEEN CALLED BUT SHOULDN'T");
            }
        }

        public void NewTurn()
        {
            CanMove = true;
            CanAct = true;
            render.color = Color.white;
        }

        public void Attack(Unit on) {
            if (on != null) {
                if (CanAct) {

                    Debug.Log("---Attacker----");
                    Present();

                    Debug.Log("Attacker / defender = " + Instance.type_table[(int)type, (int)on.type]);

                    Debug.Log("Damage made : " + ((Hp / 2) + 1 + Instance.type_table[(int)type, (int)on.type]));

                    Hp -= on.BeenAttack((Hp / 2) + 1 + Instance.type_table[(int)type, (int)on.type], type);
                    CanAct = false;
                    render.color = Color.grey;

                    Debug.Log("Attacker life left : " + Hp);

                    CheckAlive();
                }
                else {
                    Debug.Log("ERROR : ATTACK HAVE BEEN CALLED BUT SHOULDN'T");
                }
            }
            else {
                CanAct = false;
                render.color = Color.grey;
                Debug.Log("USELESS ATTACK");
            }
        }

      

        public int BeenAttack(int damage, UnitType attacker) {
            int defense = 0;

            Hp -= damage;
            defense = Hp / 2;

            Debug.Log("---Defenders----");
            Present();

            Debug.Log("defender / attacker = " + Instance.type_table[(int)type, (int)attacker]);


            defense += Instance.type_table[(int)type, (int)attacker];
            defense = defense < 0 ? 0 : defense;

            Debug.Log("Damage made : " + defense);

            CheckAlive();

            Debug.Log("Defender life left : " + Hp);

            return defense;
        }

        public void CheckAlive() {
            if (Hp <= 0) {
                dead = true;
                gameObject.SetActive(false);
            }


        }

        public void Capture(BuildingManager.Building building) {
            if (CanAct) {
                building.Captured(team);
                CanAct = false;
                render.color = Color.grey;
            }
            else {
                Debug.Log("ERROR : CAPTURE HAVE BEEN CALLED BUT SHOULDN'T");
            }
        }

        public void ReUse(bool team, Vector3 position, UnitType type) {
            this.team = team;
            this.Position = position;
            this.type = type;

            Hp = 10;

            gameObject.SetActive(true);
            gameObject.transform.position = position;

            dead = false;
            CanAct = false;
            CanMove = false;



            if (team)
            {
                if (type == UnitType.Infantery)
                    render.sprite = Instance.gridAccess.GetSprite("RedInf");
                if (type == UnitType.Bazooka)
                    render.sprite = Instance.gridAccess.GetSprite("RedBazooka");
                else
                    render.sprite = Instance.gridAccess.GetSprite("RedTank");
            }
            else
            {
                if (type == UnitType.Infantery)
                    render.sprite = Instance.gridAccess.GetSprite("BlueInf");
                if (type == UnitType.Bazooka)
                    render.sprite = Instance.gridAccess.GetSprite("BlueBazooka");
                else
                    render.sprite = Instance.gridAccess.GetSprite("BlueTank");
            }
            render.color = Color.grey;

        }

        public Unit(bool team, Vector3 position, UnitType type) {
            this.team = team;
            this.Position = position;
            this.type = type;

            dead = false;
            CanAct = false;
            CanMove = false;

            Hp = 10;

            gameObject = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/UnitModel") as GameObject);
            gameObject.transform.position = Position;

            render = gameObject.GetComponent<SpriteRenderer>();


            if (team)
            {
                if (type == UnitType.Infantery)
                    render.sprite = Instance.gridAccess.GetSprite("RedInf");
                if (type == UnitType.Bazooka)
                    render.sprite = Instance.gridAccess.GetSprite("RedBazooka");
                if (type == UnitType.Tank)
                    render.sprite = Instance.gridAccess.GetSprite("RedTank");
            }
            else {
                if (type == UnitType.Infantery)
                    render.sprite = Instance.gridAccess.GetSprite("BlueInf");
                if (type == UnitType.Bazooka)
                    render.sprite = Instance.gridAccess.GetSprite("BlueBazooka");
                if (type == UnitType.Tank)
                    render.sprite = Instance.gridAccess.GetSprite("BlueTank");
            }

            render.color = Color.grey;

        }

        public void Killed() {
            this.dead = true;
        }
    }


    public enum UnitType {
        Infantery = 0,
        Bazooka = 1,
        Tank = 2
    }

}
