using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ContinueGame : MonoBehaviour
{
    private void Awake()
    {
        LoadScript.Load();
        var button = GetComponent<Button>();
        button.interactable = GameState.IsGame;
        button.onClick.AddListener(() => SceneManager.LoadScene("Map"));
    }
}
