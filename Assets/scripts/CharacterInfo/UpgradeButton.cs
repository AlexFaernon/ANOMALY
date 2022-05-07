using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UpgradeButton : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => SceneManager.LoadScene("AbilityImprovement"));
    }
}
