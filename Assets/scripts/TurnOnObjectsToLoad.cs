using System.Collections.Generic;
using UnityEngine;

public class TurnOnObjectsToLoad : MonoBehaviour
{
    [SerializeField] private List<GameObject> TurnOn;

    private void Awake()
    {
        foreach (var obj in TurnOn)
        {
            obj.SetActive(true);
        }
    }
}