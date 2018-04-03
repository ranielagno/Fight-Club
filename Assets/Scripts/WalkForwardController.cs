using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WalkForwardController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        FighterController.mvFwd = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        FighterController.mvFwd = false;
    }
}
