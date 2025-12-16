using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
public class ObstaclesPool : DataPoolBehaviour<ObstacleParent, ObstacleData>
{
    public IReadOnlyList<ObstacleData> EntriesData
    {
        get
        {
            return entries.Select(entry => entry.prefab.Data).ToList();
        }
    }
}
