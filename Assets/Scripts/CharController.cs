using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
	[SerializeField] private float m_JumpForce = 400f;							// Força do pulo;
	//[Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;			// Amount of maxSpeed applied to crouching movement. 1 = 100% 
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// Quantidade de suavização do movimento;
	[SerializeField] private bool m_AirControl = false;							// Controle de movimento enquanto estiver no ar;
	[SerializeField] private LayerMask m_WhatIsGround;							// Máscara para identificar chão;
	[SerializeField] private Transform m_GroundCheck;							// Marcador de posicionamento para saber se player toca o chão (Checador de chão);
	[SerializeField] private Transform m_CeilingCheck;							// Marcador de posicionamento para saber se player toca o teto;
	//[SerializeField] private Collider2D m_CrouchDisableCollider;				// A collider that will be disabled when crouching 

	const float k_GroundedRadius = .2f; // Raio do circulo pra checar se player toca o chão;
	private bool m_Grounded;            // Se player ta pisando no chão ou não;
	//const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up;
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;  // Determina para que lado o player está virado;
	private Vector3 m_Velocity = Vector3.zero;

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	//public BoolEvent OnCrouchEvent;
	//private bool m_wasCrouching = false;

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();

		/*if (OnCrouchEvent == null)
			OnCrouchEvent = new BoolEvent();*/
	}

	private void FixedUpdate()
	{
		bool wasGrounded = m_Grounded;
		m_Grounded = false;

		// Player estará "Grounded" quando o "Checador de Chão" colidir com qualquer coisa na layer "Ground";
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++){
			if (colliders[i].gameObject != gameObject){
				m_Grounded = true;
				if (!wasGrounded)
					OnLandEvent.Invoke();
			}
		}
	}

	public void Move(float move, /*bool crouch,*/bool jump)
	{
		// If crouching, check to see if the character can stand up
		/*if (!crouch)
		{
			// If the character has a ceiling preventing them from standing up, keep them crouching
			if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
			{
				crouch = true;
			}
		}

		//only control the player if grounded or airControl is turned on
		if (m_Grounded || m_AirControl)
		{

			// If crouching
			if (crouch)
			{
				if (!m_wasCrouching)
				{
					m_wasCrouching = true;
					OnCrouchEvent.Invoke(true);
				}

				// Reduce the speed by the crouchSpeed multiplier
				move *= m_CrouchSpeed;

				// Disable one of the colliders when crouching
				if (m_CrouchDisableCollider != null)
					m_CrouchDisableCollider.enabled = false;
			} else
			{
				// Enable the collider when not crouching
				if (m_CrouchDisableCollider != null)
					m_CrouchDisableCollider.enabled = true;

				if (m_wasCrouching)
				{
					m_wasCrouching = false;
					OnCrouchEvent.Invoke(false);
				}
			}*/

			//  Move o personagem com target velocity
			Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.linearVelocity.y);
			// Suaviza o movimento e aplica no player
			m_Rigidbody2D.linearVelocity = Vector3.SmoothDamp(m_Rigidbody2D.linearVelocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

   			// Se o input move o player pra direita e o player está virado para a esquerda...
			if (move > 0 && !m_FacingRight){
				// ... vire o player.
				Flip();
			}
   			// Se o input move o player pra direita e o player está virado para a esquerda...
			else (move < 0 && m_FacingRight){
				// ... vire o player.
				Flip();
			}
		}
		// Se o player pular...
		if (m_Grounded && jump){
			// ...adicione força vertical para o player.
			m_Grounded = false;
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
		}
	}

	// Vira o player para o lado oposto;
	private void Flip()
	{
		// Troque o lado que o player é rotulado dependendo do lado que ele está virado;
		m_FacingRight = !m_FacingRight;

  		// Multiplica a local scale x do player por -1;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
