using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private WeaponSlotManager _weaponSlotManager;
 
 
    public WeaponItemDefinition rightWeapon;
    public WeaponItemDefinition leftWeapon;

    private void Awake() 
    { 
        _weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
        
    }

    private void Start()
    {
        _weaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);
        _weaponSlotManager.LoadWeaponOnSlot(leftWeapon,true);
    }
}