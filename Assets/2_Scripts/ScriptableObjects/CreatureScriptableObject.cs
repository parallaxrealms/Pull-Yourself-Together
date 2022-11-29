using UnityEngine;

[CreateAssetMenu(fileName = "CreatureScriptableObject", menuName = "ScriptableObjects/Creature")]

public class CreatureScriptableObject : ScriptableObject
{
    public int health;
    
    public float speed;
    public float attackSpeed;
    public float moveAwaySpeed;

    public float aggroResetTimer;
    public float detectionRadius;
}
