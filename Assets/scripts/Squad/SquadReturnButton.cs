using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SquadReturnButton : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => SceneManager.LoadScene("GlobalMap"));
    }
}
