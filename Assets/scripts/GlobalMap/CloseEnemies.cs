using UnityEngine;
using UnityEngine.EventSystems;

public class CloseEnemies : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        transform.parent.gameObject.SetActive(false);
    }
}
