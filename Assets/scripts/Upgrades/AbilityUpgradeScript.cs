using UnityEngine;
using UnityEngine.UI;

public class AbilityUpgradeScript : MonoBehaviour
{
    [SerializeField] private AbilityType abilityType;
    [SerializeField] private UpgradeLevel upgradeLevel;
    [SerializeField] private int cost;
    [SerializeField] private GameObject lockedPrefab;
    private IAbility ability;
    private Image image;
    private GameObject lockedIcon;
    private Outline outline;
    private Button button;
    private Color color;
    private ICharacter character;
    private bool isUpgraded => (int)upgradeLevel <= ability.UpgradeLevel;
    private bool isOpened => (int)upgradeLevel == ability.UpgradeLevel + 1;
    private bool isAbleToBy => isOpened && AbilityResources.Resources[abilityType] >= cost;

    private void Awake()
    {
        lockedIcon = Instantiate(lockedPrefab, transform);
        lockedIcon.SetActive(false);
        image = GetComponent<Image>();
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
        button.interactable = false;
        EventAggregator.UpgradeCharacterSelected.Subscribe(RedrawButton);
        EventAggregator.UpgradeAbilitySelected.Subscribe(Deselect);
        EventAggregator.AbilityUpgraded.Subscribe(OnAbilityUpgrade);

    }

    private void OnClick()
    {
        EventAggregator.UpgradeAbilitySelected.Publish(abilityType, upgradeLevel, cost);
        image.color = Color.cyan;
    }

    private void Deselect(AbilityType type, UpgradeLevel level, int cost1)
    {
        image.color = color;
    }

    private void RedrawButton(ICharacter character)
    {
        this.character = character;
        image.color = Color.white;
        color = image.color;
        button.interactable = true;
        ability = character.Abilities[abilityType];
        image.sprite = ability.Icon;
        lockedIcon.SetActive(false);
        Destroy(outline);
        
        if (isUpgraded) return;
        
        image.color = Color.gray;
        color = image.color;
        if (isOpened)
        {
            if (!isAbleToBy) return;
            
            outline = gameObject.AddComponent<Outline>();
            outline.effectColor = Color.red;
            color = image.color;
        }
        else
        {
            lockedIcon.SetActive(true);
        }
    }

    private void OnAbilityUpgrade()
    {
        RedrawButton(character);
    }

    public enum UpgradeLevel
    {
        First,
        Second,
        Third
    }

    private void OnDestroy()
    {
        EventAggregator.UpgradeCharacterSelected.Unsubscribe(RedrawButton);
        EventAggregator.UpgradeAbilitySelected.Unsubscribe(Deselect);
        EventAggregator.AbilityUpgraded.Unsubscribe(OnAbilityUpgrade);
    }
}
