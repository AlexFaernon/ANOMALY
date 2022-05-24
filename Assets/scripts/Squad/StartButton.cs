using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class StartButton : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        GameState.isGame = true;
        SceneManager.LoadScene("Map");
    }
}
