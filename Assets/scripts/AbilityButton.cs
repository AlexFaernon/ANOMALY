using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
   [SerializeField] private TMP_Text cooldownText;
   private Button button;
   private Image image;
   private const float holdTime = 1f;
   private PointerEventData eventData;
   private AbilityType abilityType;
   private Dictionary<IAbility, int> abilitiesCooldown = new Dictionary<IAbility, int>();
   private void Awake()
   {
      EventAggregator.BindAbilityButton.Subscribe(BindAbilityType);
      EventAggregator.SwitchAbilities.Subscribe(SwitchAbilities);
      EventAggregator.NewTurn.Subscribe(ReduceCooldownOnTurn);
      button = GetComponent<Button>();
      image = GetComponent<Image>();
      button.onClick.AddListener(OnShortPress);
   }

   private void OnShortPress()
   {
      EventAggregator.CastAbilityType.Publish(abilityType);
      transform.parent.gameObject.SetActive(false);
   }

   void OnLongPress()
   {
      EventAggregator.AbilityTypeInfo.Publish(abilityType);
      eventData.eligibleForClick = false;
      Debug.Log("long");
   }

   void BindAbilityType(GameObject obj, AbilityType ability)
   {
      if (obj != gameObject) return;
      
      abilityType = ability;
   }

   private void SwitchAbilities(ICharacter character)
   {
      var ability = character.Abilities[abilityType];
      
      if (abilitiesCooldown.TryGetValue(ability, out var cooldown))
      {
         if (cooldown == 0)
         {
            button.interactable = true;
            cooldownText.text = "";
         }
         else
         {
            button.interactable = false;
            cooldownText.text = cooldown.ToString();
         }
      }
      else
      {
         abilitiesCooldown[ability] = 0;
         button.interactable = true;
         cooldownText.text = "";
      }
   }

   private void ReduceCooldownOnTurn()
   {
      foreach (var ability in abilitiesCooldown.Keys.Where(ability => abilitiesCooldown[ability] > 0))
      {
         abilitiesCooldown[ability]--;
      }
   }

   private void UpdateCooldown()
   {
      
   }
   
   public void OnPointerDown(PointerEventData eventData)
   {
      this.eventData = eventData;
      Invoke(nameof(OnLongPress), holdTime);
   }
 
   public void OnPointerUp(PointerEventData eventData)
   {
      CancelInvoke(nameof(OnLongPress));
      EventAggregator.HideAbilityInfo.Publish();
   }
 
   public void OnPointerExit(PointerEventData eventData)
   {
      CancelInvoke(nameof(OnLongPress));
   }

   private void OnDestroy()
   {
      EventAggregator.BindAbilityButton.Unsubscribe(BindAbilityType);
      EventAggregator.SwitchAbilities.Unsubscribe(SwitchAbilities);
      EventAggregator.NewTurn.Unsubscribe(ReduceCooldownOnTurn);
   }
}
