using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Flip : MonoBehaviour
{
    [SerializeField] private List<Sprite> sprites = new List<Sprite>();
    [SerializeField] private Image panel;
    [SerializeField] private Button Forward;
    [SerializeField] private Button Back;
    private int index;

    private void Awake()
    {
        Forward.onClick.AddListener(OnClickForward);
        Back.onClick.AddListener(OnClickBack);
    }
    private void OnClickForward()
    {
        if (sprites.Count == index + 1)
            return;

        index++;
        panel.sprite = sprites[index];
    }

    private void OnClickBack()
    {
        if (index == 0)
            return;

        index --;
        panel.sprite = sprites[index];
    }
}
