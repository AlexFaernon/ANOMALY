using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Flip : MonoBehaviour
{
    [SerializeField] private List<Sprite> sprites = new List<Sprite>();
    [SerializeField] private Image panel;
    [SerializeField] private Button Forward;
    [SerializeField] private Button Back;
    [SerializeField] private Button startButton;
    [SerializeField] private GameObject LocationClosed;
    [SerializeField] private GameObject LocationDot;
    [SerializeField] private Transform Dots;
    [SerializeField] private Button Enemies;
    [SerializeField] private List<string> LocationNames = new List<string>();
    [SerializeField] private TMP_Text LocationTitle;
    private int _index;

    private int Index
    {
        get => _index;
        set
        {
            _index = value;
            startButton.interactable = _index == 0;
            LocationClosed.SetActive(_index != 0);
            Enemies.interactable = _index == 0;
            EventAggregator.LocationSwitched.Publish(value);
        }
    }

    private void Awake()
    {
        Forward.onClick.AddListener(OnClickForward);
        Back.onClick.AddListener(OnClickBack);
        for (var i = 0; i < sprites.Count; i++)
        {
            Instantiate(LocationDot, Dots).name = i.ToString();
        }
    }

    private void Start()
    {
        EventAggregator.LocationSwitched.Publish(Index);
    }

    private void OnClickForward()
    {
        if (sprites.Count == Index + 1)
            return;

        Index++;
        panel.sprite = sprites[Index];
        LocationTitle.text = LocationNames[Index];
    }

    private void OnClickBack()
    {
        if (Index == 0)
            return;

        Index--;
        panel.sprite = sprites[Index];
        LocationTitle.text = LocationNames[Index];
    }
}
