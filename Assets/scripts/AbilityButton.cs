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
   private readonly Dictionary<IAbility, int> abilitiesCooldown = new Dictionary<IAbility, int>();
   private IAbility currentAbility;

   private void Awake()
   {
      EventAggregator.BindAbilityButton.Subscribe(BindAbilityType);
      EventAggregator.SwitchAbilities.Subscribe(SwitchAbilities);
      EventAggregator.NewTurn.Subscribe(ReduceCooldownOnTurn);
      EventAggregator.AbilityCasted.Subscribe(SetCooldownOnCast);
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
      currentAbility = character.Abilities[abilityType];
      image.sprite = currentAbility.Icon;

      if (abilitiesCooldown.TryGetValue(currentAbility, out var cooldown))
      {
         SetButtonInteractable(cooldown == 0);
      }
      else
      {
         abilitiesCooldown[currentAbility] = 0;
         SetButtonInteractable(true);
      }
   }

   private void ReduceCooldownOnTurn()
   {
      foreach (var ability in abilitiesCooldown.Keys.Where(ability => abilitiesCooldown[ability] > 0).ToList())
      {
         abilitiesCooldown[ability]--;
      }

      if (currentAbility != null)
         SetButtonInteractable(abilitiesCooldown[currentAbility] <= 0);
   }

   private void SetCooldownOnCast(IAbility ability)
   {
      if (abilitiesCooldown.ContainsKey(ability))
      {
         abilitiesCooldown[ability] = ability.Cooldown;
      }
   }

   private void SetButtonInteractable(bool isInteractable)
   {
      if (isInteractable)
      {
         cooldownText.text = "";
         image.color = Color.white;
         button.interactable = true;
      }
      else
      {
         cooldownText.text = abilitiesCooldown[currentAbility].ToString();
         image.color = Color.gray;
         button.interactable = false;
      }
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
      EventAggregator.AbilityCasted.Unsubscribe(SetCooldownOnCast);
   }
}
