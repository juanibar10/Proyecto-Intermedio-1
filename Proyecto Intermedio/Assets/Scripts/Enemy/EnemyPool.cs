using UnityEngine;

[DisallowMultipleComponent]
public class EnemyPool : BasePoolBehaviour<BaseEnemy>, IEnemyPool
{
    /// <summary>
    /// Retrieves the unique ID from the prefab to identify pool entries
    /// </summary>
    protected override int GetIdFromPrefab(BaseEnemy prefab) => prefab.data.id;
    
}
