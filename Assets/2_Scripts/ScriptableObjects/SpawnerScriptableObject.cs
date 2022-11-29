using UnityEngine;
[CreateAssetMenu(fileName = "SpawnerScriptableObject", menuName = "ScriptableObjects/EnemySpawner")]

public class SpawnerScriptableObject : ScriptableObject
{
    public int spawnerType = 0;
    public float health = 5.0f;
    public float spawnRate = 10.0f;

}
