using UnityEngine;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour
{
   private Button button;
   
   private void Awake()
   {
      button = GetComponent<Button>();
      button.onClick.AddListener(ToggleOffAbilities);
   }

   private void ToggleOffAbilities()
   {
      EventAggregator.ToggleDarken.Publish();
      transform.parent.gameObject.SetActive(false);
   }
   
}
