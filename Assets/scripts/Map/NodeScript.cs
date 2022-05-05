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
    [SerializeField] private GameObject campWindow;
    private Node node;
    private Button button;
    private static readonly Random random = new Random();
    
    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);

        if (MapSingleton.Nodes[Convert.ToInt32(name)] == null)
        {
            node = new Node();
            if (name == "0")
            {
                node.IsUnlocked = true;
            }
            node.IsCamp = random.Next(2) == 1;

            MapSingleton.Nodes[Convert.ToInt32(name)] = node;
        }
        else
        {
            node = MapSingleton.Nodes[Convert.ToInt32(name)];
        }
        
        EventAggregator.NodeCompleted.Subscribe(CheckUnlocking);

        ChangeStatus();
    }

    private void OnClick()
    {
        EventAggregator.NodeCompleted.Publish(gameObject);
        node.IsCompleted = true;
        if (node.IsCamp)
        {
            campWindow.SetActive(true);
        }
        else
        {
            SceneManager.LoadScene("Battle");
        }
        ChangeStatus();
    }

    private void CheckUnlocking(GameObject otherNode)
    {
        if (node.IsUnlocked || !linkedNodes.Contains(otherNode)) return;
        
        node.IsUnlocked = true;
        ChangeStatus();
    }

    private void ChangeStatus()
    {
        if (node.IsUnlocked)
        {
            locker.SetActive(false);
            button.interactable = true;
            if (node.IsCamp) GetComponent<Image>().color = Color.green;

            if (!node.IsCompleted) return;
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
        EventAggregator.NodeCompleted.Unsubscribe(CheckUnlocking);
    }
}

public class Node
{
    public bool IsCamp;
    public bool IsUnlocked;
    public bool IsCompleted;
}
