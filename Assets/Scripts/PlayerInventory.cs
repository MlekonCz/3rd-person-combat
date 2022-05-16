using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private WeaponSlotManager _weaponSlotManager;
 
 
    public WeaponItemDefinition rightWeapon;
    public WeaponItemDefinition leftWeapon;

    public WeaponItemDefinition unarmedWeapon;

    public WeaponItemDefinition[] weaponsInRightHandSlots = new WeaponItemDefinition[1];
    public WeaponItemDefinition[] weaponsInLeftHandSlots = new WeaponItemDefinition[1];

    public int currentRightWeaponIndex = -1;
    public int currentLeftWeaponIndex = -1;
    
    private void Awake() 
    { 
        _weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
      
    }

    private void Start()
    {
        rightWeapon = unarmedWeapon;
        leftWeapon = unarmedWeapon;
    }

    #region ChangeWeapon
  public void ChangeRightWeapon()
    {
        currentRightWeaponIndex += 1;
        if (currentRightWeaponIndex == 0 && weaponsInRightHandSlots[0] != null)
        {
            rightWeapon = weaponsInRightHandSlots[currentRightWeaponIndex];
            _weaponSlotManager.LoadWeaponOnSlot(weaponsInRightHandSlots[currentRightWeaponIndex],false);
        }
        else if (currentRightWeaponIndex == 0 && weaponsInRightHandSlots[0] == null)
        {
            currentRightWeaponIndex += 1;
        }
        else if (currentRightWeaponIndex == 1 && weaponsInRightHandSlots[1] != null)
        {
            rightWeapon = weaponsInRightHandSlots[currentRightWeaponIndex];
            _weaponSlotManager.LoadWeaponOnSlot(weaponsInRightHandSlots[currentRightWeaponIndex],false);
        }
        else
        {
            currentRightWeaponIndex += 1;
        }
        if (currentRightWeaponIndex > weaponsInRightHandSlots.Length -1)
        {
            currentRightWeaponIndex = -1;
            rightWeapon = unarmedWeapon;
            _weaponSlotManager.LoadWeaponOnSlot(unarmedWeapon,false);
        }
    }
    public void ChangeLeftWeapon()
    {
        currentLeftWeaponIndex += 1;
        if (currentLeftWeaponIndex == 0 && weaponsInLeftHandSlots[0] != null)
        {
            leftWeapon = weaponsInLeftHandSlots[currentLeftWeaponIndex];
            _weaponSlotManager.LoadWeaponOnSlot(weaponsInLeftHandSlots[currentLeftWeaponIndex],true);
        }
        else if (currentLeftWeaponIndex == 0 && weaponsInLeftHandSlots[0] == null)
        {
            currentLeftWeaponIndex += 1;
        }
        else if (currentLeftWeaponIndex == 1 && weaponsInLeftHandSlots[1] != null)
        {
            leftWeapon = weaponsInLeftHandSlots[currentLeftWeaponIndex];
            _weaponSlotManager.LoadWeaponOnSlot(weaponsInLeftHandSlots[currentLeftWeaponIndex],true);
        }
        else
        {
            currentLeftWeaponIndex += 1;
        }
        if (currentLeftWeaponIndex > weaponsInLeftHandSlots.Length -1)
        {
            currentLeftWeaponIndex = -1;
            leftWeapon = unarmedWeapon;
            _weaponSlotManager.LoadWeaponOnSlot(unarmedWeapon,true);
        }
    }
    

    #endregion
  
    
    
    
    
}
