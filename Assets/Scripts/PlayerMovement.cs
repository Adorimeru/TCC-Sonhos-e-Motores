using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public CharacterController2D controller; 		// Para referenciar o controlador do player;
	public float runSpeed = 40f;				// Velocidade padrão do andar;
	float horizontalMove = 0f;				// Quantitativo de movimento;
	bool jump = false;					// Pulo settado para falso para que possa ser ativado com a função;
	
	void Update () {	
		
		// Conta que move o personagem;
		horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
		// Se o botão configurado para "Jump" for pressionado...
		if (Input.GetButtonDown("Jump")){
			//... pule!
			jump = true;
		}
	}

    void FixedUpdate()
    {
        	// Certifica que o movimento seja contínuo;
		controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
	    	// Reconhece que o player não está pulando assim que começa a fase;
		jump = false;
    }
}
