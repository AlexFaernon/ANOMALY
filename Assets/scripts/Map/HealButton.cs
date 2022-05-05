using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealButton : MonoBehaviour
{
    [SerializeField] private GameObject campWindow;
    public static ICharacter Character;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            Character.Heal(Character.HPSegmentLength, true);
            campWindow.SetActive(false);
        });
    }
}
