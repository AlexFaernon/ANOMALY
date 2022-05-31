using TMPro;
using UnityEngine;

public class CharacterName : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<TMP_Text>().text = CharacterInfoButton.SelectedCharacter.Name;
    }
}
