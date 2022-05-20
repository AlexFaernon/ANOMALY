using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Random = System.Random;

public class EnemyUnit : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    [SerializeField] private GameObject hpBar;
    [SerializeField] private GameObject statusBar;
    private IEnemy enemy;
    private bool IsPicked;
    private const float holdTime = 1f;
    private PointerEventData eventData;
    private static readonly Random random = new Random();

    private void Awake()
    {
        enemy = new IEnemy[] {new Weakling(), new FatBoy(), new Killer()}[random.Next(3)];
        Units.Enemies.Add(enemy);
        enemy.CanMove = true;
        
        GetComponent<Button>().onClick.AddListener(PickTarget);

        EventAggregator.UpdateHP.Subscribe(CheckDeath);
        EventAggregator.EnemyTurn.Subscribe(MakeMove);
        EventAggregator.NewTurn.Subscribe(NewTurn);
    }

    private void Start()
    {
        EventAggregator.BindHPBarToEnemy.Publish(hpBar, enemy);
        EventAggregator.BindStatusBarToUnit.Publish(statusBar, enemy);
    }

    private void CheckDeath(IUnit unit)
    {
        if (unit != enemy) return;

        if (enemy.HP > 0) return;
        EventAggregator.EnemyDied.Publish(enemy);
        gameObject.SetActive(false);
    }

    private void MakeMove(IEnemy other)
    {
        if (enemy != other || !enemy.CanMove) return;

        var character = Units.Characters.Values.OrderByDescending(character => character.HP).First();
        character.TakeDamage(enemy.Attack, enemy);
        Debug.Log(character);
    }

    private void NewTurn()
    {
        enemy.CanMove = true;
    }

    private void PickTarget()
    {
        if (!TargetPicker.isPicking) return;
        
        EventAggregator.PickTarget.Publish(enemy);
    }

    private void ShowEffectsInfo()
    {
        EventAggregator.ShowEffectsInfo.Publish(enemy);
        eventData.eligibleForClick = false;
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        this.eventData = eventData;
        Invoke(nameof(ShowEffectsInfo), holdTime);
    }
 
    public void OnPointerUp(PointerEventData eventData)
    {
        CancelInvoke(nameof(ShowEffectsInfo));
        EventAggregator.HideEffectsInfo.Publish();
    }
 
    public void OnPointerExit(PointerEventData eventData)
    {
        CancelInvoke(nameof(ShowEffectsInfo));
    }

    private void OnDestroy()
    {
        EventAggregator.UpdateHP.Unsubscribe(CheckDeath);
        EventAggregator.EnemyTurn.Unsubscribe(MakeMove);
        EventAggregator.NewTurn.Unsubscribe(NewTurn);
    }
}
