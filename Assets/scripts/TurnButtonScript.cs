using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnButtonScript : MonoBehaviour
{
    private Button button;
    private TMP_Text text;

    private void Awake()
    {
        button = GetComponent<Button>();
        text = GetComponentInChildren<TMP_Text>();
        button.onClick.AddListener(OnClick);
        EventAggregator.NewTurn.Subscribe(OnNewTurn);
    }

    private void OnNewTurn()
    {
        button.interactable = true;
        text.text = "ЗАКОНЧИТЬ ХОД";
    }

    private void OnClick()
    {
        EventAggregator.EnemyTurn.Publish();
        button.interactable = false;
        text.text = "ХОД ВРАГА";
    }
}
