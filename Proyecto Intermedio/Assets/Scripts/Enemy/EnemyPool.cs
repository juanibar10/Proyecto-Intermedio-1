using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
public class EnemyPool : DataPoolBehaviour<BaseEnemy, EnemyData>
{
    public IReadOnlyList<EnemyData> EntriesData
    {
        get
        {
            return entries.Select(entry => entry.prefab.Data).ToList();
        }
    }
}
