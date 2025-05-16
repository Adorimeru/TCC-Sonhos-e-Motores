using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InteractionSystem : MonoBehaviour
{
    [Header("Detection Fields")]
    public Transform detectionPoint;                        // Ponto de detecção de interação;
    private const float detectionRadius = 0.2f;             // Raio da detecção de interação;
    public LayerMask detectionLayer;                        // Camada de detecção de interação;
    public GameObject detectedObject;                       // GameObject do objeto detectado;
    
    [Header("Examine Fields")]
    public GameObject workbenchWindow;                      // Workbench window object
    public GameObject examineWindow;                        // Examine window object
    public GameObject grabbedObject;
    public float grabbedObjectYValue;
    public Transform grabPoint;
    public Image examineImage;
    public Text examineText;
    public bool isSitting;
    public bool isExamining;
    public bool isGrabbing;


    void Start()
    {
        workbenchWindow.SetActive(false);
    }


    void Update()
    {
        if(DetectObject())
        {
            if(InteractInput())
            {
                //If we are grabbing something don't interact with other items, drop the grabbed item first
                if(isGrabbing)
                {
                    GrabDrop();
                    return;
                } 

                detectedObject.GetComponent<Item>().Interact();
            }
        }

        // Botão ESC também sai da tela da bancada;
        if(Input.GetKeyDown(KeyCode.Escape))
            {
                workbenchWindow.SetActive(false);
            
            }
    }

    // Pinta o circulo de detecção de verde;
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(detectionPoint.position, detectionRadius);
    }

    // Botão de interagir;
    bool InteractInput()
    {
        return Input.GetKeyDown(KeyCode.E);
    }

    bool DetectObject()
    {        
                   
        Collider2D obj = Physics2D.OverlapCircle(detectionPoint.position,detectionRadius,detectionLayer); 
        
        if(obj==null)
        {
            detectedObject = null;
            return false;
        }
        else
        {
            detectedObject = obj.gameObject;
            return true;
        }
    }

    // Interação com a bancada;
    public void Workbench(Item item)
    {
        // Se o personagem...
        if(isSitting)
        {
            // ...não estiver interagindo com a bancada...
            isSitting = false;
            // ...mantenha a janela da bancada oculta!
            workbenchWindow.SetActive(false);    
        }
        else
        {
            //... estiver interagindo com a bancada...
            isSitting = true;
            // ...abra a janela da bancada!
            workbenchWindow.SetActive(true);
        }
    }

    // Interação de examinar itens;
    public void ExamineItem(Item item)
    {
        // Se o personagem...
        if(isExamining)
        {
            //...não estiver examinando um item...
            isExamining = false;
            //...oculte a janela de examinar.
            examineWindow.SetActive(false);
            
        }
        else
        {
            // ...estiver examinando um item...
            isExamining = true;
            // ...sette a imagem do item...
            examineImage.sprite = item.GetComponent<SpriteRenderer>().sprite;
            // ...sette a descrição do item...
            examineText.text = item.descriptionText;
            // ...e abra a janela de examinar!
            examineWindow.SetActive(true);
            
        }        
    }

    public void GrabDrop()
    {        
        //Check if we do have a grabbed object => drop it
        if(isGrabbing)
        {
            //make isGrabbing false
            isGrabbing=false;
            //unparent the grabbed object
            grabbedObject.transform.parent=null;            
            //set the y position to its origin
            grabbedObject.transform.position = 
                new Vector3(grabbedObject.transform.position.x,grabbedObjectYValue,grabbedObject.transform.position.z);
            //null the grabbed object reference
            grabbedObject=null;
        }
        //Check if we have nothing grabbed grab the detected item
        else
        {
            //Enable the isGrabbing bool
            isGrabbing=true;
            //assign the grabbed object to the object itself
            grabbedObject=detectedObject;
            //Parent the grabbed object to the player
            grabbedObject.transform.parent=transform;
            //Cache the y value of the object
            grabbedObjectYValue=grabbedObject.transform.position.y;
            //Adjust the position of the grabbed object to be closer to hands                        
            grabbedObject.transform.localPosition=grabPoint.localPosition;
        }
    }
}
