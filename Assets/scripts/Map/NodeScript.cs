using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NodeScript : MonoBehaviour
{
    [SerializeField] private int offsetFromCurrent;
    [SerializeField] private GameObject campWindow;
    [SerializeField] private GameObject locker;
    [SerializeField] private GameObject previousLink;
    [SerializeField] private Sprite battleSprite;
    [SerializeField] private Sprite completedSprite;
    [SerializeField] private Sprite chosenSprite;
    [SerializeField] private Sprite campSprite;
    [SerializeField] private Sprite completedCampSprite;
    [SerializeField] private Sprite chosenCampSprite;
    public static int _currentNodeNumber;
    public static int CurrentNodeNumber
    {
        get => _currentNodeNumber;
        set
        {
            _currentNodeNumber = value;
            EventAggregator.NodeNumberChanged.Publish();
            SaveScript.SaveLevels();
        }
    }

    private bool IsCamp =>
        (CurrentNodeNumber + offsetFromCurrent) % 4 == 0 && CurrentNodeNumber + offsetFromCurrent != 0;
    private Button button;
    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
        if (offsetFromCurrent == 0)
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(OnClick);
        }

        EventAggregator.NodeNumberChanged.Subscribe(ChangeStatus);
        ChangeStatus();
    }

    public void OnClick()
    {
        if (IsCamp)
        {
            campWindow.SetActive(true);
            CurrentNodeNumber++;
        }
        else
        {
            SceneManager.LoadScene("Battle");
        }
    }

    private void ChangeStatus()
    {
        previousLink.SetActive(CurrentNodeNumber + offsetFromCurrent > 0);
        gameObject.SetActive(CurrentNodeNumber + offsetFromCurrent >= 0);

        if (offsetFromCurrent == 0)
        {
            image.sprite = IsCamp ? chosenCampSprite : chosenSprite;
        }
        else if (offsetFromCurrent < 0)
        {
            image.sprite = IsCamp ? completedCampSprite : completedSprite;
        }
        else
        {
            locker.SetActive(true);
            image.sprite = IsCamp ? campSprite : battleSprite;
        }
    }

    private void OnDestroy()
    {
        EventAggregator.NodeNumberChanged.Unsubscribe(ChangeStatus);
    }
}
