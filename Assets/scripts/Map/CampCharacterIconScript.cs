using UnityEngine;
using UnityEngine.UI;

public class CampCharacterIconScript : MonoBehaviour
{
    [SerializeField] private CharacterClass characterClass;
    [SerializeField] private GameObject hpBar;

    private void Start()
    {
        EventAggregator.BindHPBarToCharacter.Publish(hpBar, Units.Characters[characterClass]);
    }
}
