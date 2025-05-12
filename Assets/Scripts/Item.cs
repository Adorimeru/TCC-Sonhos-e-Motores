using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class Item : MonoBehaviour
{    
    public enum InteractionType { NONE, PickUp, Examine, GrabDrop, Workbench }
    public enum ItemType { Staic, Consumables}
    [Header("Attributes")]
    public InteractionType interactType;
    public ItemType type;
    [Header("Examine")]
     public string descriptionText;
     public Sprite slotImage;
     public Sprite descriptionImage;
    [Header("Custom Events")]
    public UnityEvent customEvent;
    public UnityEvent consumeEvent;

    private void Reset()
    {
        GetComponent<Collider2D>().isTrigger = true;
        gameObject.layer = 10;
    }

    public void Interact()
    {
        switch(interactType)
        {
            case InteractionType.Workbench:
                Object.FindFirstObjectByType<InteractionSystem>().Workbench(this);
                break;
    
            case InteractionType.PickUp:
                
                if (Object.FindFirstObjectByType<InventorySystem>().InventoryFull())
                {
                Debug.Log("INVENTÁRIO CHEIO!");
                return;
                }
                else
                {
                //Add the object to the PickedUpItems list
                Object.FindFirstObjectByType<InventorySystem>().PickUp(gameObject);
                //Disable
                gameObject.SetActive(false);
                }
                break;

            case InteractionType.Examine:
                //Call the Examine item in the interaction system
                Object.FindFirstObjectByType<InteractionSystem>().ExamineItem(this);                
                break;

            case InteractionType.GrabDrop:
                //Grab interaction
                Object.FindFirstObjectByType<InteractionSystem>().GrabDrop();
                break;

            default:
                Debug.Log("NULL ITEM");
                break;
        }

        //Invoke (call) the custom event(s)
        customEvent.Invoke();
    }
}