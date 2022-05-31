using System.Linq;
using UnityEngine;

public class StatusBarScript : MonoBehaviour
{
    [SerializeField] private GameObject statusPrefab;
    private IUnit unit;
    private void Awake()
    {
        EventAggregator.BindStatusBarToUnit.Subscribe(BindStatusBarToUnit);
        EventAggregator.UpdateStatus.Subscribe(UpdateStatus);
    }

    private void BindStatusBarToUnit(GameObject other, IUnit unit1)
    {
        if (gameObject != other) return;

        unit = unit1;
    }

    private void UpdateStatus()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        foreach (var sprite in StatusSystem.StatusList.Where(status => status.Target == unit))
        {
            Instantiate(statusPrefab, transform).SendMessage(nameof(StatusIconScript.ChangeSprite), sprite);
        }
    }

    private void OnDestroy()
    {
        EventAggregator.BindStatusBarToUnit.Unsubscribe(BindStatusBarToUnit);
        EventAggregator.UpdateStatus.Unsubscribe(UpdateStatus);
    }
}
