using System;
using UnityEngine;


public class EnemyStats : MonoBehaviour
{
    public int healthLevel = 10;
    public int maxHealth;
    public int currentHealth;

    private Animator _animator;
    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        maxHealth = SetMaxHealthFromHealthLevel();
        currentHealth = maxHealth;
    }

    private int SetMaxHealthFromHealthLevel()
    {
        maxHealth = healthLevel * 10;
        return maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        _animator.Play("Damaged");
    
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            _animator.Play("Dead");
            //HANDLE Enemy DEATH
        }
    }
    
}
