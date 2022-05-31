using System;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSprite : MonoBehaviour
{
    [SerializeField] private Sprite tankSprite;
    [SerializeField] private Sprite medicSprite;
    [SerializeField] private Sprite damagerSprite;
    private void Awake()
    {
        GetComponent<Image>().sprite = CharacterInfoButton.SelectedCharacter switch
        {
            Damager _ => damagerSprite,
            Medic _ => medicSprite,
            Tank _ => tankSprite,
            Character _ => throw new NotImplementedException(),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
