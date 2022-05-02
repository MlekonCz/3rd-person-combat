using System;
using DefaultNamespace;
using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    private AnimatorHandler _animatorHandler;

    private void Awake()
    {
        _animatorHandler = GetComponentInChildren<AnimatorHandler>();
    }

    public void HandleLightAttack(WeaponItemDefinition weapon)
    {
        _animatorHandler.PlayTargetAnimation(weapon.OH_Light_Attack_1, true);
    }
    public void HandleHeavyAttack(WeaponItemDefinition weapon)
    {
        _animatorHandler.PlayTargetAnimation(weapon.OH_Heavy_Attack_1, true);

    }
    
}