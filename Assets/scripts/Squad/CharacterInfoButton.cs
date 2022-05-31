using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterInfoButton : MonoBehaviour
{
    [SerializeField] private CharacterClass characterClass;
    public static ICharacter SelectedCharacter;
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(SelectCharacter);
    }

    private void SelectCharacter()
    {
        SelectedCharacter = Units.Characters[characterClass];
        SceneManager.LoadScene("CharacterInfo");
    }
}
