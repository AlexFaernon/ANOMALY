using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OnClick : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => SceneManager.LoadScene("MainMenu"));
    }
}
