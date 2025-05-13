using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class Item : MonoBehaviour
{    
    public enum InteractionType { NONE, PickUp, Examine, GrabDrop, Workbench }         // Drop down dos tipos de interação;
    public enum ItemType { Staic, Consumables}                                         // Drop down do tipo de item;
    [Header("Attributes")]
    public InteractionType interactType;
    public ItemType type;
    [Header("Examine")]
    public string descriptionText;                                                     // Descrição do item no inventário;
    public Sprite slotImage;                                                           // Imagem do item no slot do inventário;
    public Sprite descriptionImage;                                                    // Imagem da descrição do item;
    [Header("Custom Events")]
    public UnityEvent customEvent;
    public UnityEvent consumeEvent;

    private void Reset()
    {
        GetComponent<Collider2D>().isTrigger = true;
        // Layer "Interactable"
        gameObject.layer = 10;
    }
    
    // Interage;
    public void Interact()
    {
        // Caso interaja com item de...
        switch(interactType)
        {
            // Bancada...
            case InteractionType.Workbench:
                // 
                Object.FindFirstObjectByType<InteractionSystem>().Workbench(this);
                break;
                
            // Pegar...
            case InteractionType.PickUp:
                // Se o inventário estiver cheio...
                if (Object.FindFirstObjectByType<InventorySystem>().InventoryFull()){
                // Escreva uma mensagem no console!
                Debug.Log("INVENTÁRIO CHEIO!");
                return;
                }
                // Se não...
                else{
                // ...Adicione o objeto para a lista de itens coletados!...
                Object.FindFirstObjectByType<InventorySystem>().PickUp(gameObject);
                // ...E desabilite a imagem do item!
                gameObject.SetActive(false);
                }
                break;
                
            // Examinar...
            case InteractionType.Examine:
                // ...Execute a interação de Examinar do script <InteractiveSystem>!
                Object.FindFirstObjectByType<InteractionSystem>().ExamineItem(this);                
                break;
            
            // Pegar e Soltar...
            case InteractionType.GrabDrop:
                //...Execute a interação de pegar e soltar do script <InteractiveSystem>!
                Object.FindFirstObjectByType<InteractionSystem>().GrabDrop();
                break;
        
            default:
                Debug.Log("NULL ITEM");
                break;
        }

        // Invocar (chamar) o evento custom event;
        customEvent.Invoke();
    }
}
