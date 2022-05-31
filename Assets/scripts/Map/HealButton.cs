using UnityEngine;
using UnityEngine.UI;

public class HealButton : MonoBehaviour
{
    [SerializeField] private GameObject campWindow;

    private static ICharacter _character;
    public static ICharacter Character
    {
        get => _character;
        set
        {
            button.interactable = value != null;
            _character = value;
        }
    }

    private static Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.interactable = false;
        
        button.onClick.AddListener(() =>
        {
            Character.Heal(Character.HPSegmentLength, true);
            Character = null;
            EventAggregator.CampCharacterSelected.Publish();
            campWindow.SetActive(false);
        });
    }
}
