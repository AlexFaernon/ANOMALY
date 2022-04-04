using UnityEngine;
using UnityEngine.UI;

public class TurnButtonScript : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(TurnsScript.PassTurnToEnemy);
    }
}
