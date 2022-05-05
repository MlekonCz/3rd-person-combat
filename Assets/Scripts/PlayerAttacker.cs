using System;
using DefaultNamespace;
using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    private AnimatorHandler _animatorHandler;
    private InputHandler _inputHandler;
    public string lastAttack;
    

    private void Awake()
    {
        _animatorHandler = GetComponentInChildren<AnimatorHandler>();
        _inputHandler = GetComponent<InputHandler>();
    }

    public void HandleWeaponCombo(WeaponItemDefinition weapon)
    {
        if (_inputHandler.comboFlag)
        {
            _animatorHandler.animator.SetBool("canDoCombo", false);
            
            Debug.Log("1");
            if (lastAttack == weapon.OH_Light_Attack_1)
            {
                Debug.Log("2");
                _animatorHandler.PlayTargetAnimation(weapon.OH_Light_Attack_2,true);
            }  
        }
       
    }
    
    public void HandleLightAttack(WeaponItemDefinition weapon)
    {
        _animatorHandler.PlayTargetAnimation(weapon.OH_Light_Attack_1, true);
        lastAttack = weapon.OH_Light_Attack_1;
    }
    public void HandleHeavyAttack(WeaponItemDefinition weapon)
    {
        _animatorHandler.PlayTargetAnimation(weapon.OH_Heavy_Attack_1, true);
        lastAttack = weapon.OH_Heavy_Attack_1;
    }
    
}