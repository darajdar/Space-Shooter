using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{

    //Grab score data to save
    //public GameObject gameController;
    //public GameController score;

    public int Score;
    public int Lives;

    public float LaserWeaponCharge = 300;
    public float LaserWeaponChargeMax = 300;

    public float MissileWeaponCharge = 20;
    public float MissileWeaponChargeMax = 20;

    public float ShieldWeaponCharge = 200;
    public float ShieldWeaponChargeMax = 250;

    public float MeleeWeaponCharge = 300;
    public float MeleeWeaponChargeMax = 300;

    public bool Boss1Defeated;
    public bool Boss2Defeated;
    public bool Boss3Defeated;
    public bool Boss4Defeated;

    void Start()
    {
        //score = gameController.GetComponent<GameController>();
    }

    //Replace Gamedata's score with the ModGlobalControl's score
    public GameData(ModGlobalControl data)
    {
         Score = data.Score;
        Lives = data.Lives;
        LaserWeaponCharge = data.LaserWeaponCharge;
        MissileWeaponCharge = data.MissileWeaponCharge;
        ShieldWeaponCharge = data.ShieldWeaponCharge;
        MeleeWeaponCharge = data.MeleeWeaponCharge;
    }

  
}
