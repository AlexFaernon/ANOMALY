using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = System.Random;

public class NodeScript : MonoBehaviour
{
    [SerializeField] private List<GameObject> linkedNodes;
    [SerializeField] private GameObject locker;
    [SerializeField] private GameObject campWindow;
    [SerializeField] private Sprite completedSprite;
    [SerializeField] private Button battleButton;
    private Node node;
    private Button button;
    private static readonly Random random = new Random();
    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);

        if (MapSingleton.Nodes[Convert.ToInt32(name)] == null)
        {
            node = new Node();
            if (name == "0")
            {
                node.IsUnlocked = true;
            }

            if (name == "0" || name == "6")
            {
                node.IsCamp = false;
            }
            else
            {
                node.IsCamp = random.Next(4) == 0;
            }

            MapSingleton.Nodes[Convert.ToInt32(name)] = node;
        }
        else
        {
            node = MapSingleton.Nodes[Convert.ToInt32(name)];
        }
        
        EventAggregator.NodeCompleted.Subscribe(CheckUnlocking);
        EventAggregator.NodeIsChosen.Subscribe(ChangeStatus);

        ChangeStatus();
    }

    private void OnClick()
    {
        if (node.IsCamp)
        {
            campWindow.SetActive(true);
            battleButton.interactable = false;
            node.IsCompleted = true;
            EventAggregator.NodeCompleted.Publish(name);
        }
        else
        {
            battleButton.interactable = true;
            EventAggregator.NodeIsChosen.Publish();
            image.color = Color.cyan;
            MapSingleton.ChosenNode = gameObject;
        }
    }

    private void CheckUnlocking(string nodeName)
    {
        if (linkedNodes.All(other => other.name != nodeName)) return;
        
        node.IsUnlocked = true;
        ChangeStatus();
    }

    private void ChangeStatus()
    {
        if (node.IsUnlocked)
        {
            locker.SetActive(false);
            button.interactable = true;
            image = GetComponent<Image>();
            if (node.IsCamp) image.color = Color.green;

            if (!node.IsCompleted) return;
            image.color = Color.white;
            image.sprite = completedSprite;
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
        EventAggregator.NodeIsChosen.Unsubscribe(ChangeStatus);
    }
}

public class Node
{
    public bool IsCamp;
    public bool IsUnlocked;
    public bool IsCompleted;
}
