using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
   private IAbility ability;
   private Button button;
   private float holdTime = 1f;
   private PointerEventData eventData;

   private void Awake()
   {
      EventAggregator.BindAbilityButton.Subscribe(BindAbilityButton);
      button = GetComponent<Button>();
      button.onClick.AddListener(ToggleOffAbilities);
   }

   private void ToggleOffAbilities()
   {
      EventAggregator.ToggleDarken.Publish();
      transform.parent.gameObject.SetActive(false);
   }
   
   void OnLongPress()
   {
      eventData.eligibleForClick = false;
      EventAggregator.ShowAbilityInfo.Publish(ability);
      Debug.Log("long");
   }

   void BindAbilityButton(GameObject obj, IAbility ability)
   {
      if (obj != gameObject) return;
      
      this.ability = ability;
   }
   
   public void OnPointerDown(PointerEventData eventData)
   {
      this.eventData = eventData;
      Invoke(nameof(OnLongPress), holdTime);
   }
 
   public void OnPointerUp(PointerEventData eventData)
   {
      CancelInvoke(nameof(OnLongPress));
        
   }
 
   public void OnPointerExit(PointerEventData eventData)
   {
      CancelInvoke(nameof(OnLongPress));
   }

   private void OnDestroy()
   {
      EventAggregator.BindAbilityButton.Unsubscribe(BindAbilityButton);
   }
}
