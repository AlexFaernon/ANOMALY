using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUnit : MonoBehaviour
{
    [SerializeField] private TMP_Text HP;
    private readonly IEnemy enemy = new Enemy();
    private bool IsPicked;
    private Image image;
    
    private void Awake()
    {
        UpdateHP(enemy);
        image = GetComponent<Image>();
        GetComponent<Button>().onClick.AddListener(PickTarget);
        
        EventAggregator.UpdateHP.Subscribe(UpdateHP);
        EventAggregator.GetTargets.Subscribe(UnPick);
        EventAggregator.EnemyTurn.Subscribe(MakeMove);
        
        EventAggregator.NewEnemy.Publish(enemy);
    }
    
    private void UpdateHP(IUnit unit)
    {
        if (unit == enemy)
        {
            HP.text = enemy.HP.ToString();
        }
    }

    private void MakeMove(IEnemy other)
    {
        if (enemy != other) return;
        
        Debug.Log("Turn");
    }

    private void PickTarget()
    {
        if (!TargetPicker.isPicking) return;

        IsPicked = !IsPicked;
        image.color = IsPicked ? Color.red : Color.white;
        EventAggregator.PickTarget.Publish(enemy);
    }

    private void UnPick(List<IUnit> units)
    {
        IsPicked = false;
        image.color = Color.white;
    }

    private void OnDestroy()
    {
        EventAggregator.UpdateHP.Unsubscribe(UpdateHP);
        EventAggregator.GetTargets.Unsubscribe(UnPick);
        EventAggregator.EnemyTurn.Unsubscribe(MakeMove);
    }
}
