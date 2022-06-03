using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleButtonScript : MonoBehaviour
{
    [SerializeField] private GameObject currentNode;
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => currentNode.SendMessage(nameof(NodeScript.OnClick)));
    }
}
