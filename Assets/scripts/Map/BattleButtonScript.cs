using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleButtonScript : MonoBehaviour
{
    [SerializeField] private GameObject campWindow;
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        if (NodeScript.CurrentNodeNumber % 5 == 0 && NodeScript.CurrentNodeNumber != 0)
        {
            campWindow.SetActive(true);
        }
        else
        {
            SceneManager.LoadScene("Battle");
        }
        NodeScript.CurrentNodeNumber++;
    }
}
