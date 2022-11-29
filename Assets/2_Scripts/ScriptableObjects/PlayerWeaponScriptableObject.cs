using UnityEngine;

[CreateAssetMenu(fileName = "PlayerWeaponScriptableObject", menuName = "ScriptableObjects/PlayerWeapon")]

public class PlayerWeaponScriptableObject : ScriptableObject
{
    public float bulletSpeed = 1500f;
    public float gunFireRate = 0.1f;
    public float gunCooldownTime = 0.0f;

    public float damageAmount = 1.0f;
    public float lifespanTime = 4.0f;

    public int bulletType = 0;
}
