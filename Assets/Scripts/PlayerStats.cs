using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{ 
        [Title("health")]
        public int healthLevel = 10;
        public int maxHealth;
        public int currentHealth;
        public HealthBar healthBar;

        [Title("Stamina")] 
        public int staminaLevel;
        public int maxStamina;
        public int currentStamina;
        public StaminaBar staminaBar;
        
        
        private AnimatorHandler _animatorHandler;
        private void Start()
        {
                maxHealth = SetMaxHealthFromHealthLevel();
                currentHealth = maxHealth;
                healthBar.SetMaxHealth(maxHealth);
                
                maxStamina = SetMaxStaminaFromStaminaLevel();
                currentStamina = maxStamina;
                staminaBar.SetMaxStamina(maxStamina);
                
                _animatorHandler = GetComponentInChildren<AnimatorHandler>();
        }

        private int SetMaxHealthFromHealthLevel()
        {
                maxHealth = healthLevel * 10;
                return maxHealth;
        }
        private int SetMaxStaminaFromStaminaLevel()
        {
                maxStamina = staminaLevel * 10;
                return maxStamina;
        }

        public void TakeDamage(int damage)
        {
                currentHealth -= damage;
                healthBar.SetCurrentHealth(currentHealth);
                _animatorHandler.PlayTargetAnimation("Damaged",true);

                if (currentHealth <= 0)
                {
                        currentHealth = 0;
                        _animatorHandler.PlayTargetAnimation("Dead", true);
                        //HANDLE PLAYER DEATH
                }
        }

        public void TameStaminaDamage(int damage)
        {
                currentStamina -= damage;
                staminaBar.SetCurrentStamina(currentStamina);
        }
        
}
