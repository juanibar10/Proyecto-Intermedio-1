using UnityEngine;

public class PlayerStats : MonoBehaviour, ICollector
{
    public void Collect()
    {
        StatisticsSystem.Instance.AddCollectible();
    }
}
