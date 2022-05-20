using UnityEngine;
using UnityEngine.UI;

public class AbilityUpgradeScript : MonoBehaviour
{
    [SerializeField] private AbilityType abilityType;
    [SerializeField] private UpgradeLevel upgradeLevel;
    [SerializeField] private StatsUpgradeType statsUpgradeType;
    [SerializeField] private int cost;
    [SerializeField] private GameObject lockedPrefab;
    private IAbility ability;
    private Image image;
    private GameObject lockedIcon;
    private Outline outline;
    private Button button;
    private Color color;
    private ICharacter character;
    private bool hpUpgraded => statsUpgradeType == StatsUpgradeType.HP &&
                               (character.HP1Upgrade && upgradeLevel == UpgradeLevel.Second ||
                                character.HP2Upgrade && upgradeLevel == UpgradeLevel.Fourth);
    private bool mpUpgraded => statsUpgradeType == StatsUpgradeType.MP &&
                               (character.MP1Upgrade && upgradeLevel == UpgradeLevel.Second ||
                                character.MP2Upgrade && upgradeLevel == UpgradeLevel.Fourth);

    private bool isUpgraded => (int)upgradeLevel <= ability.OverallUpgradeLevel &&
                               (statsUpgradeType == StatsUpgradeType.None || abilityType != AbilityType.Basic ||
                                hpUpgraded || mpUpgraded);

    private bool isOpened => (int)upgradeLevel == ability.OverallUpgradeLevel + 1 && statsUpgradeType != StatsUpgradeType.HPMP ||
                             (int)upgradeLevel <= ability.OverallUpgradeLevel && !hpUpgraded && !mpUpgraded ||
                             abilityType == AbilityType.Ultimate && (int)upgradeLevel == ability.OverallUpgradeLevel + 2 ||
                             statsUpgradeType == StatsUpgradeType.HPMP &&
                             character.Abilities[AbilityType.First].OverallUpgradeLevel + 1 == (int)upgradeLevel &&
                             character.Abilities[AbilityType.Second].OverallUpgradeLevel + 1 == (int)upgradeLevel;
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
        EventAggregator.UpgradeAbilitySelected.Publish(abilityType, upgradeLevel, statsUpgradeType, cost);
        image.color = Color.cyan;
    }

    private void Deselect(AbilityType type, UpgradeLevel level, StatsUpgradeType arg3, int arg4)
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
        if ((int)upgradeLevel % 2 == 0)
        {
            image.sprite = ability.Icon;
        }

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

    private void OnDestroy()
    {
        EventAggregator.UpgradeCharacterSelected.Unsubscribe(RedrawButton);
        EventAggregator.UpgradeAbilitySelected.Unsubscribe(Deselect);
        EventAggregator.AbilityUpgraded.Unsubscribe(OnAbilityUpgrade);
    }
}

public enum UpgradeLevel
{
    First,
    Second,
    Third,
    Fourth,
    Fifth
}
