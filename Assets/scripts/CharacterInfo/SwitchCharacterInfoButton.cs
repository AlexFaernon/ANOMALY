using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SwitchCharacterInfoButton : MonoBehaviour
{
    [SerializeField] private bool isNext;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        var characters = Units.Characters.Values.ToList();
        var index = characters.IndexOf(CharacterInfoButton.SelectedCharacter);
        index += isNext ? 1 : -1;
        index = index switch
        {
            -1 => 2,
            3 => 0,
            _ => index
        };
        CharacterInfoButton.SelectedCharacter = characters[index];
        SceneManager.LoadScene("CharacterInfo");
    }
}
