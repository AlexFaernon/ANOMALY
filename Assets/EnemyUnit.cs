using UnityEngine;
using UnityEngine.UI;

public class EnemyUnit : MonoBehaviour
{
    private readonly IUnit enemy = new Enemy();
    
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => EventAggregator.PickTarget.Publish(enemy));
    }
}
