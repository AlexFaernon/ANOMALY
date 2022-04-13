using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
   private AbilityType abilityType;
   private Button button;
   private float holdTime = 1f;
   private PointerEventData eventData;

   private void Awake()
   {
      EventAggregator.BindAbilityButton.Subscribe(BindAbilityButton);
      button = GetComponent<Button>();
      button.onClick.AddListener(OnShortPress);
   }

   private void OnShortPress()
   {
      var parent = transform.parent.gameObject;
      EventAggregator.CastAbilityType.Publish(parent, abilityType);
      EventAggregator.ToggleDarkenOff.Publish();
      parent.SetActive(false);
   }

   void OnLongPress()
   {
      EventAggregator.AbilityTypeInfo.Publish(transform.parent.gameObject, abilityType);
      eventData.eligibleForClick = false;
      Debug.Log("long");
   }

   void BindAbilityButton(GameObject obj, AbilityType ability)
   {
      if (obj != gameObject) return;
      
      abilityType = ability;
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
