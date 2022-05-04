using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = System.Random;

public class NodeScript : MonoBehaviour
{
    [SerializeField] private List<GameObject> linkedNodes;
    [SerializeField] private GameObject locker;
    [SerializeField] private bool isUnlocked;
    private bool isComplited;
    private Button button;
    private bool isCamp;
    private static Random random = new Random();
    
    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
        
        EventAggregator.NodeComplited.Subscribe(CheckUnlocking);
        
        isCamp = random.Next(2) == 1;
        
        ChangeStatus();
    }

    private void OnClick()
    {
        EventAggregator.NodeComplited.Publish(gameObject);
        isComplited = true;
        if (!isCamp)
        {
            SceneManager.LoadScene("Battle");
        }
        ChangeStatus();
    }

    private void CheckUnlocking(GameObject node)
    {
        if (isUnlocked || !linkedNodes.Contains(node)) return;
        
        isUnlocked = true;
        ChangeStatus();
    }

    private void ChangeStatus()
    {
        if (isUnlocked)
        {
            locker.SetActive(false);
            button.interactable = true;
            if (isCamp) GetComponent<Image>().color = Color.green;

            if (!isComplited) return;
            GetComponent<Image>().color = Color.yellow;
            button.interactable = false;
        }
        else
        {
            button.interactable = false;
        }
    }

    private void OnDestroy()
    {
        EventAggregator.NodeComplited.Unsubscribe(CheckUnlocking);
    }
}
