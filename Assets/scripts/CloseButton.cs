using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseButton : MonoBehaviour
{
    [SerializeField] GameObject shading;

    public void Close()
    {
        shading.SetActive(false);
    }
}
