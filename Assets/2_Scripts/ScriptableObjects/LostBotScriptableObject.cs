using UnityEngine;

[CreateAssetMenu(fileName = "LostBotScriptableObject", menuName = "ScriptableObjects/LostBotEnemy")]

public class LostBotScriptableObject : ScriptableObject
{
    public int health;
    
    public float idleSpeed;
    public float attackSpeed;
    public float moveAwaySpeed;

    public float body_idleSpeed;
    public float body_attackSpeed;
    public float body_moveAwaySpeed;

    public float legs_idleSpeed;
    public float legs_attackSpeed;
    public float legs_moveAwaySpeed;

    public float aggroResetTimer;
    public float detectionRadius;

    public float gunCooldownTimer;
    public float drillCooldownTimer;
}
