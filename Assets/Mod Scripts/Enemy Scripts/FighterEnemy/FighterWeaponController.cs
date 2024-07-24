using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterWeaponController : WeaponController
{
    // Start is called before the first frame update
    public override void Start()
    {
        MaxAmmo = 3;
        Ammo = MaxAmmo;
        delay = .1f;
        FireRate = .4f;
        StartWait = 2;
        StartCoroutine(AttackPattern());
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Ammunition();
        base.ResetRotation();
    }
}
