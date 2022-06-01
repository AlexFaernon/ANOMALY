using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = System.Random;

public class HeroUnit : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    [SerializeField] private CharacterClass characterClass;
    [SerializeField] private GameObject HPBar;
    [SerializeField] private GameObject MPBar;
    [SerializeField] private GameObject statusBar;
    [SerializeField] private GameObject scratchPrefab;
    private ICharacter character;
    private IAbility currentAbility;
    private Image image;
    private const float holdTime = 0.7f;
    private PointerEventData eventData;
    private Outline outline;
    private static Random random = new Random();

    private bool _isSelected;
    
    private bool IsSelected
    {
        get => _isSelected;
        set
        {
            if (value)
            {
                EventAggregator.DeselectCharacters.Publish();
                _isSelected = true;
                outline = gameObject.AddComponent<Outline>();
                outline.effectColor = Color.green;
                outline.effectDistance = new Vector2(1.5f, 1.5f);
            }
            else
            {
                _isSelected = false;
                Destroy(outline);
            }
        }
    }

    private void Awake()
    {
        character = Units.Characters[characterClass];

        if (character.IsDead)
        {
            transform.parent.gameObject.SetActive(false);
            return;
        }
        
        image = GetComponent<Image>();

        GetComponent<Button>().onClick.AddListener(SelectCharacter);

        EventAggregator.UpdateHP.Subscribe(CheckDeath);
        EventAggregator.UpdateMovability.Subscribe(UpdateMovability);
        EventAggregator.DeselectCharacters.Subscribe(Deselect);
        EventAggregator.CastAbilityType.Subscribe(StartAbility);
        EventAggregator.AbilityTypeInfo.Subscribe(ShowAbilityInfoByType);
        EventAggregator.NewTurn.Subscribe(NewTurn);
        EventAggregator.UnitDamagedUnit.Subscribe(OnDamage);
    }

    private void Start()
    {
        character.CanMove = true;
        character.MP = character.MaxMP;
        EventAggregator.BindHPBarToCharacter.Publish(HPBar, character);
        EventAggregator.BindMPBarToCharacter.Publish(MPBar, character);
        EventAggregator.BindStatusBarToUnit.Publish(statusBar, character);
    }

    private void UpdateMovability(IUnit unit)
    {
        if (unit != character) return;

        image.color = character.CanMove ? Color.white : new Color(1,1,1,0.5f);
    }
    
    private void OnDamage(int damage, IUnit source, IUnit target)
    {
        if (target != character) return;

        Instantiate(scratchPrefab, transform.parent).transform.Rotate(new Vector3(0, 0, random.Next(-40, 40)));
    }

    private void CheckDeath(IUnit unit)
    {
        if (unit != character) return;

        if (character.HP > 0) return;

        character.IsDead = true;
        EventAggregator.CharacterDied.Publish(character);
        StatusSystem.DispelOnUnit(character);
        transform.parent.gameObject.SetActive(false);
    }
    
    private void StartAbility(AbilityType abilityType)
    {
        if (!IsSelected) return;

        currentAbility = character.Abilities[abilityType];
        if (character.MP < currentAbility.Cost)
        {
            Debug.Log("No mana");
            return;
        }

        if (currentAbility.TargetCount == 0)
        {
            CastAbility(new List<IUnit> { character });
            return;
        }
        
        EventAggregator.GetTargets.Subscribe(CastAbility);
        EventAggregator.StartChooseTargets.Publish(currentAbility);
    }
    
    void OnLongPress()
    {
        EventAggregator.ShowEffectsInfo.Publish(character);
        eventData.eligibleForClick = false;
        Debug.Log("long");
    }

    private void ShowAbilityInfoByType(AbilityType abilityType)
    {
        if (!IsSelected) return;
        
        EventAggregator.ShowAbilityInfo.Publish(character.Abilities[abilityType]);
    }

    private void SelectCharacter()
    {
        if (!TargetPicker.isPicking && character.CanMove)
        {
            IsSelected = true;
            EventAggregator.ToggleAbilityList.Publish(true);
            EventAggregator.SwitchAbilities.Publish(character);
        }
        else
        {
            EventAggregator.PickTarget.Publish(character);
        }
    }

    private void Deselect()
    {
        if (IsSelected)
        {
            IsSelected = false;
        }
        
        EventAggregator.GetTargets.Unsubscribe(CastAbility);
    }

    private void CastAbility(List<IUnit> units)
    {
        IsSelected = false;
        character.CanMove = false;
        currentAbility.CastAbility(units, character);
        character.MP -= currentAbility.Cost;
        EventAggregator.GetTargets.Unsubscribe(CastAbility);
        EventAggregator.ToggleAbilityList.Publish(false);
        EventAggregator.AbilityCasted.Publish(currentAbility);
        EventAggregator.UpdateStatus.Publish();
    }

    private void NewTurn()
    {
        character.CanMove = true;
        if (character.MP < character.MaxMP)
        {
            character.MP += 1;
        }
    }

    private void OnDestroy()
    {
        EventAggregator.UpdateHP.Unsubscribe(CheckDeath);
        EventAggregator.DeselectCharacters.Unsubscribe(Deselect);
        EventAggregator.CastAbilityType.Unsubscribe(StartAbility);
        EventAggregator.AbilityTypeInfo.Unsubscribe(ShowAbilityInfoByType);
        EventAggregator.NewTurn.Unsubscribe(NewTurn);
        EventAggregator.UpdateMovability.Unsubscribe(UpdateMovability);
        EventAggregator.UnitDamagedUnit.Unsubscribe(OnDamage);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        this.eventData = eventData;
        Invoke(nameof(OnLongPress), holdTime);
    }
 
    public void OnPointerUp(PointerEventData eventData)
    {
        CancelInvoke(nameof(OnLongPress));
        EventAggregator.HideEffectsInfo.Publish();
    }
 
    public void OnPointerExit(PointerEventData eventData)
    {
        CancelInvoke(nameof(OnLongPress));
    }
}

public enum AbilityType
{
    Basic,
    First,
    Second,
    Ultimate
}

public enum CharacterClass
    {
        Damager,
        Medic,
        Tank
    }
