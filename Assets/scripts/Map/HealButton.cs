using Agava.YandexGames;
using UnityEngine;
using UnityEngine.UI;

public class HealButton : MonoBehaviour
{
    [SerializeField] private GameObject campWindow;
    private static Button button;

    private void Awake()
    {
        button = GetComponent<Button>();

        button.onClick.AddListener(() =>
        {
            foreach (var character in Units.Characters.Values)
            {
                character.Heal(character.HPSegmentLength, true);
                campWindow.SetActive(false);
            }
            //VideoAd.Show();
        });
    }
}
