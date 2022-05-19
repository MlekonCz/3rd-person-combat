using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlotManager : MonoBehaviour
{
    public WeaponItemDefinition attackingWeapon;
    
    private WeaponHolderSlot _leftHandSlot;
    private WeaponHolderSlot _rightHandSlot;

    private DamageCollider _leftHandDamageCollider;
    private DamageCollider _rightHandDamageCollider;

    private Animator _animator;
    private QuickSlotsUI _quickSlotsUI;
    private PlayerStats _playerStats;

    
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _quickSlotsUI = FindObjectOfType<QuickSlotsUI>();
        _playerStats = GetComponentInParent<PlayerStats>();
        SetWeaponSlots();
    }
    private void SetWeaponSlots()
    {
        WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
        foreach (WeaponHolderSlot weaponSlot in weaponHolderSlots)
        {
            if (weaponSlot.isLeftHandSlot)
            {
                _leftHandSlot = weaponSlot;
            }
            else if (weaponSlot.isRightHandSlot)
            {
                _rightHandSlot = weaponSlot;
            }
        }
    }

    public void LoadWeaponOnSlot(WeaponItemDefinition weaponItemDefinition, bool isLeft)
    {
        if (isLeft)
        {
            _leftHandSlot.LoadWeaponModel(weaponItemDefinition);
            LoadLeftWeaponDamageCollider();
            _quickSlotsUI.UpdateWeaponQuickSlotsUI(true,weaponItemDefinition);
            #region Handle Weapon Idle Animation
            if (weaponItemDefinition != null)
            {
                _animator.CrossFade(weaponItemDefinition.leftHandIdle,0.2f);
            }
            else
            {
                _animator.CrossFade("Left Arm Empty", 0.2f);
            }
            #endregion
        }
        else
        {
            _rightHandSlot.LoadWeaponModel(weaponItemDefinition);
            LoadRightWeaponDamageCollider();
            _quickSlotsUI.UpdateWeaponQuickSlotsUI(false,weaponItemDefinition);
            #region Handle Weapon Idle Animation
            if (weaponItemDefinition != null)
            {
                _animator.CrossFade(weaponItemDefinition.rightHandIdle,0.2f);
            }
            else
            {
                _animator.CrossFade("Right Arm Empty", 0.2f);
            }
            #endregion
        }
    }

    #region Handle Weapon's Damage Colliders
    private void LoadLeftWeaponDamageCollider()
    {
        _leftHandDamageCollider = _leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
    }
    private void LoadRightWeaponDamageCollider()
    {
        _rightHandDamageCollider = _rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
    }
    public void OpenRightDamageCollider()
    {
        _rightHandDamageCollider.EnableDamageCollider();
    }
    public void OpenLeftHandDamageCollider()
    {
        _leftHandDamageCollider.EnableDamageCollider();
    }
    public void CloseRightDamageCollider()
    {
        _rightHandDamageCollider.DisableDamageCollider();
    }
    public void CloseLeftHandDamageCollider()
    {
        _leftHandDamageCollider.DisableDamageCollider();
    }
    #endregion

    #region HandleStaminaWeaponDrainage
    public void DrainStaminaLightAttack()
    {
        _playerStats.TameStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStaminaDrain * attackingWeapon.lightAttackMultiplier));
    }
    public void DrainStaminaHeavyAttack()
    {
        _playerStats.TameStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStaminaDrain * attackingWeapon.heavyAttackMultiplier));
    }
    #endregion
    
    
    
}
