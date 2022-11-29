using UnityEngine;

[CreateAssetMenu(fileName = "BossScriptableObject", menuName = "ScriptableObjects/BossEnemy")]

public class BossScriptableObject : ScriptableObject
{
    public float health;
    public float speed;
    public float attackSpeed;
    public float bodySpeed;
    
}
