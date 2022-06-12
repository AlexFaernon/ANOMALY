using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStart : MonoBehaviour
{
    [SerializeField] private GameObject newGameConfirm;
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        if (GameState.IsGame)
        {
            newGameConfirm.SetActive(true);
            return;
        }
        
        SceneManager.LoadScene("GlobalMap");
    }
}
