using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = System.Random;

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
    public static int currentNodeNumber;

    private bool IsCamp =>
        (currentNodeNumber + offsetFromCurrent) % 5 == 0 && currentNodeNumber + offsetFromCurrent != 0;
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

        EventAggregator.CampOpened.Subscribe(ChangeStatus);
        ChangeStatus();
    }

    private void OnClick()
    {
        if (IsCamp)
        {
            campWindow.SetActive(true);
            ChangeStatus();
        }
        else
        {
            SceneManager.LoadScene("Battle");
        }
        currentNodeNumber++;
    }

    private void ChangeStatus()
    {
        if (currentNodeNumber + offsetFromCurrent <= 0)
        {
            previousLink.SetActive(false);
        }
        
        if (currentNodeNumber + offsetFromCurrent < 0)
        {
            gameObject.SetActive(false);
            return;
        }
        
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
}
