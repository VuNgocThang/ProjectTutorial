using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveRightButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public PlayerController PlayerController;
    bool isPressed = false;
    private void Update()
    {
        if (isPressed)
        {
            PlayerController.MoveRight();
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("isPressed");

        isPressed = true;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("NotPressed");
        isPressed = false;
    }
    
}
