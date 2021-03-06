using UnityEngine;
using UnityEngine.UI;

public class OpenEnemies : MonoBehaviour
{
   [SerializeField] private GameObject enemies;
   private void Awake()
   {
      GetComponent<Button>().onClick.AddListener(() => enemies.SetActive(true));
   }
}
