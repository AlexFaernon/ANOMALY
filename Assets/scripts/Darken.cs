using UnityEngine;
using UnityEngine.UI;

public class Darken : MonoBehaviour
{
    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void ToggleDarkenOn()
    {
        image.color =  new Color(0.31f, 0.31f, 0.31f, 0.9f);
    }
    
    private void ToggleDarkenOff()
    {
        image.color = Color.clear;
    }

    private void OnDestroy()
    {
    }
}
