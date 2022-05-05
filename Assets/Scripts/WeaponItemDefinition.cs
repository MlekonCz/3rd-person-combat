using UnityEngine;


[CreateAssetMenu(menuName = "Item/Weapon", order = 0)]
public class WeaponItemDefinition : ItemDefinition
{ 
    public GameObject modelPrefab;
    public bool isUnarmed;

    [Header("One Handed Attack Animations")]
    public string OH_Light_Attack_1;
    public string OH_Light_Attack_2;
    public string OH_Light_Attack_3;
    public string OH_Heavy_Attack_1;

}