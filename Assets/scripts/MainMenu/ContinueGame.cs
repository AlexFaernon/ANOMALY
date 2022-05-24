using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ContinueGame : MonoBehaviour
{
    private void Awake()
    {
        var button = GetComponent<Button>();
        button.interactable = GameState.isGame;
        button.onClick.AddListener(() => SceneManager.LoadScene("Map"));
    }
}
