using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    [SerializeField] GameObject shading;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => shading.SetActive(true));
    }
}
