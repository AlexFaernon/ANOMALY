using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleButtonScript : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        MapSingleton.Nodes[Convert.ToInt32(MapSingleton.ChosenNode.name)].IsCompleted = true;
        EventAggregator.NodeCompleted.Publish(MapSingleton.ChosenNode.name);
        SceneManager.LoadScene("Battle");
    }
}
