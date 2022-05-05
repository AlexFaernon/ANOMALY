using System;
using UnityEngine;
using UnityEngine.UI;

public class CampCharacterIconScript : MonoBehaviour
{
    [SerializeField] private CharacterClass characterClass;
    [SerializeField] private GameObject hpBar;
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(ChooseCharacter);
        EventAggregator.CampCharacterSelected.Subscribe(DestroyOutline);
    }

    private void Start()
    {
        EventAggregator.BindHPBarToCharacter.Publish(hpBar, Units.Characters[characterClass]);
    }

    private void ChooseCharacter()
    {
        HealButton.Character = Units.Characters[characterClass];
        EventAggregator.CampCharacterSelected.Publish();
        if (!TryGetComponent(typeof(Outline), out _))
        {
            gameObject.AddComponent<Outline>().effectColor = Color.red;
        }
    }

    private void DestroyOutline()
    {
        Destroy(GetComponent<Outline>());
    }

    private void OnDestroy()
    {
        EventAggregator.CampCharacterSelected.Unsubscribe(DestroyOutline);
    }
}
