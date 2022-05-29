using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Random = System.Random;

public class EnemyUnit : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    [SerializeField] private GameObject hpBar;
    [SerializeField] private GameObject statusBar;
    [SerializeField] private Sprite blackDude;
    [SerializeField] private Sprite worm;
    [SerializeField] private Sprite stump;
    private IEnemy enemy;
    private bool IsPicked;
    private const float holdTime = 0.7f;
    private PointerEventData eventData;
    private static readonly Random random = new Random();

    private void Awake()
    {
        enemy = new IEnemy[] {new Weakling(), new FatBoy(), new Killer()}[random.Next(3)];
        GetComponent<Image>().sprite = enemy switch
        {
            FatBoy _ => stump,
            Killer _ => worm,
            Weakling _ => blackDude,
            Enemy _ => throw new ArgumentOutOfRangeException(nameof(enemy)),
            _ => throw new ArgumentOutOfRangeException(nameof(enemy))
        };
        Units.Enemies.Add(enemy);
        enemy.CanMove = true;
        
        GetComponent<Button>().onClick.AddListener(PickTarget);

        EventAggregator.UpdateHP.Subscribe(CheckDeath);
        EventAggregator.EnemyMove.Subscribe(MakeMove);
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
        StatusSystem.DispelOnUnit(enemy);
        transform.parent.gameObject.SetActive(false);
    }

    private void MakeMove(IEnemy other)
    {
        if (enemy != other || !enemy.CanMove) return;

        StartCoroutine(DealDamage());
    }

    private IEnumerator DealDamage()
    {
        var outline = gameObject.AddComponent<Outline>();
        outline.effectColor = Color.red;
        yield return new WaitForSeconds(1);
        var character = Units.Characters.Values.OrderByDescending(character => character.HP).First();
        character.TakeDamage(enemy.Attack, enemy);
        Debug.Log(character);
        yield return new WaitForSeconds(1);
        Destroy(outline);
        TurnsScript.enemyMoved = true;
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
        EventAggregator.EnemyMove.Unsubscribe(MakeMove);
        EventAggregator.NewTurn.Unsubscribe(NewTurn);
    }
}
