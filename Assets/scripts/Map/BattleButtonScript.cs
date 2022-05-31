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
        if (NodeScript.currentNodeNumber % 5 == 0 && NodeScript.currentNodeNumber != 0)
        {
            campWindow.SetActive(true);
            EventAggregator.CampOpened.Publish();
        }
        else
        {
            SceneManager.LoadScene("Battle");
        }
        NodeScript.currentNodeNumber++;
    }
}
