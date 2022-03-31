using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LongPress : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    [SerializeField]
    [Tooltip("How long must pointer be down on this object to trigger a long press")]
    private float holdTime = 1f;

    private PointerEventData eventData;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => Debug.Log("shortNorm"));
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        this.eventData = eventData;
        Invoke("OnLongPress", holdTime);
    }
 
    public void OnPointerUp(PointerEventData eventData)
    {
        CancelInvoke("OnLongPress");
        
    }
 
    public void OnPointerExit(PointerEventData eventData)
    {
        CancelInvoke("OnLongPress");
    }
 
    void OnLongPress()
    {
        eventData.eligibleForClick = false;
        Debug.Log("long");
    }
}
