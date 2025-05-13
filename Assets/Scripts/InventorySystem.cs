using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    [Header("General Fields")]
    public List<GameObject> items= new List<GameObject>();      // Lista dos itens coletados;
    public bool isOpen;                                         // Indicador se o inventário está aberto;
    
    [Header("UI Items Section")]
    public GameObject inventoryWindow;                          // Janela do inventário;
    public Image[] items_images;                                // Lista das imagens dos itens no inventário;

    [Header("UI Item Description")]
    public GameObject ui_Description_Window;                    // Janela de detalhes do item;
    public Image description_Image;                             // Imagem do item; 
    public TextMeshProUGUI description_Title;                   // Título(Nome) do item;
    public TextMeshProUGUI description_Text;                    // Texto da descrição

    void Start()
    {
        inventoryWindow.SetActive(false);
    }

    private void Update()
    {
        // Se apertar a tecla I...    
        if(Input.GetKeyDown(KeyCode.I))
        {
            //...Abra o inventário!
            ToggleInventory();
        }
    }

    // Define se o inventário está aberto ou fechado;
    void ToggleInventory()
    {
        // Aberto e Não aberto;
        isOpen = !isOpen;
        // Abra a janela do inventário;
        inventoryWindow.SetActive(isOpen);
        // Atualize itens do inventário; 
        UpdateInventory_UI();
    }

    // Esconde todos os as imagens UI dos itens;
    void HideAll() 
    { 
        foreach (var i in items_images) {i.gameObject.SetActive(false);}

        HideDescription();
    }
    
    // Adicione item à lista de itens coletados;
    public void PickUp(GameObject item)
    {
        items.Add(item);
        UpdateInventory_UI();                          
    }

    // Checando se o inventário está cheio;
    public bool InventoryFull()
    {
        // Se o número de itens for igual a 6 (máx de slots)...
        if(items.Count == 6)
        //...O inventário está lotado!
        {return true;}
        else
        //...O inventário tem espaço!...por enquanto...
        {return false;}
    }
    
    // Recarrega os elementos da UI na janela do inventário;
    void UpdateInventory_UI()
    {
        HideAll();
        
        // Coloca os items em ordem;
        for(int i=0;i<items.Count;i++)
        {
            // Para cada item na lista de itens, mostre o item em seu respectivo slot;
            items_images[i].sprite = items[i].GetComponent<Item>().slotImage;
            // Revela imagem do item;
            items_images[i].gameObject.SetActive(true);
        }
    }
    // Mostra elementos da descrição do item;
    public void ShowDescription(int id)
    {
        description_Image.sprite = items[id].GetComponent<Item>().descriptionImage;        // Setta imagem de descrição do item;'
        description_Title.text = items[id].name;                                           // Setta nome do item;
        description_Text.text = items[id].GetComponent<Item>().descriptionText;            // Setta descrição do item;
        
        // Revela tudo;
        description_Image.gameObject.SetActive(true);
        description_Title.gameObject.SetActive(true);
        description_Text.gameObject.SetActive(true);
    }

    // Esconde a descrição pra quando nenhum item for analizado;
    public void HideDescription()
    {
        description_Image.gameObject.SetActive(false);
        description_Title.gameObject.SetActive(false);
        description_Text.gameObject.SetActive(false);
    }
    // DEPOIS FAÇO SAPORRA QUE PREGUIÇA!!!
    public void Consume(int id)
    {
        if(items[id].GetComponent<Item>().type== Item.ItemType.Consumables)
        {
            Debug.Log($"CONSUMED {items[id].name}");
            //Invoke the cunsume custome event
            items[id].GetComponent<Item>().consumeEvent.Invoke();
            //Destroy the item in very tiny time
            Destroy(items[id], 0.1f);
            //Clear the item from the list
            items.RemoveAt(id);
            //Update UI
            UpdateInventory_UI();
        }
    }
}
