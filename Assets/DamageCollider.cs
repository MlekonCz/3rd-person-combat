using UnityEngine;

public class DamageCollider : MonoBehaviour
{
   private Collider _damageCollider;
   public int currentWeaponDamage = 25;

   private void Awake()
   {
      _damageCollider = GetComponent<Collider>();
      _damageCollider.gameObject.SetActive(true);
      _damageCollider.isTrigger = true;
      _damageCollider.enabled = false;
   }

   public void EnableDamageCollider()
   {
      _damageCollider.enabled = true;
   }
   public void DisableDamageCollider()
   {
      _damageCollider.enabled = false;
   }

   private void OnTriggerEnter(Collider collider)
   {
      if (collider.CompareTag(TagManager.Player))
      {
         PlayerStats playerStats = collider.GetComponent<PlayerStats>();
         
         if (playerStats != null)
         {
            playerStats.TakeDamage(currentWeaponDamage);
         }
      }

      if (collider.CompareTag(TagManager.Enemy))
      {
         EnemyStats enemyStats = collider.GetComponent<EnemyStats>();
         if (enemyStats != null)
         {
            enemyStats.TakeDamage(currentWeaponDamage);
         }
      }
   }
}
