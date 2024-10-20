using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent (typeof(CharacterMovement))]
public class PlayerController : MonoBehaviour
{
	private PlayerControls m_ActionMap;
	private CharacterMovement m_Movement;
	private HealthComponent m_HealthComponent;
	private bool m_InMoveActive;
	private Coroutine m_MoveCoroutine;

	private void Awake()
	{
		m_ActionMap = new PlayerControls();
		m_Movement = GetComponent<CharacterMovement>();

		m_HealthComponent = GetComponent<HealthComponent>();
		if(m_HealthComponent == null) { Debug.Log("No Health Component"); }
	}

	private void OnEnable()
	{
		m_ActionMap.Enable();

		m_ActionMap.Default.MoveHoriz.performed += Handle_MovePerformed;
		m_ActionMap.Default.MoveHoriz.canceled += Handle_MoveCancelled;
		m_ActionMap.Default.Jump.performed += Handle_JumpPerformed;
		m_ActionMap.Default.Jump.canceled += Handle_JumpCancelled;
		m_ActionMap.Default.Crouch.performed += Handle_CrouchPerformed;
		m_ActionMap.Default.Crouch.canceled += Handle_CrouchCancelled;
		m_ActionMap.Default.MoveDown.performed += Handle_DownPerformed;

		m_HealthComponent.onDamaged += Handle_HealthDamaged;
		m_HealthComponent.onDead += Handle_OnDead;
	}

	private void OnDisable()
	{
		m_ActionMap.Disable();

		m_ActionMap.Default.MoveHoriz.performed -= Handle_MovePerformed;
		m_ActionMap.Default.MoveHoriz.canceled -= Handle_MoveCancelled;
		m_ActionMap.Default.Jump.performed -= Handle_JumpPerformed;
		m_ActionMap.Default.Jump.canceled -= Handle_JumpCancelled;
        m_ActionMap.Default.Crouch.canceled -= Handle_CrouchCancelled;
        m_ActionMap.Default.MoveDown.performed -= Handle_DownPerformed;

        m_HealthComponent.onDamaged -= Handle_HealthDamaged;
        m_HealthComponent.onDead -= Handle_OnDead;
    }

	private void Handle_MovePerformed(InputAction.CallbackContext context)
	{
		//m_Movement.SetInMove(context.ReadValue<float>());
		float inMove = context.ReadValue<float>();

		m_InMoveActive = true;

		if(m_MoveCoroutine == null)
		{
			m_MoveCoroutine = StartCoroutine(C_MoveUpdate(inMove));
		}
	}
	private void Handle_MoveCancelled(InputAction.CallbackContext context)
	{
		m_InMoveActive = false;
		if(m_MoveCoroutine != null)
		{
			StopCoroutine(m_MoveCoroutine);
			m_MoveCoroutine = null;
			m_Movement.SetInMove(0f);
		}
	}

	IEnumerator C_MoveUpdate(float value)
	{
		while (m_InMoveActive)
		{
			m_Movement.SetInMove(value);
			yield return new WaitForFixedUpdate();
			
		}
	}

	public void Handle_DownPerformed(InputAction.CallbackContext context)
	{
		m_Movement.FallDown();
	}

	private void Handle_JumpPerformed(InputAction.CallbackContext context)
	{
		m_Movement.StartJump();
	}
	private void Handle_JumpCancelled(InputAction.CallbackContext context)
	{
		m_Movement.StopJump();
	}

	private void Handle_CrouchPerformed(InputAction.CallbackContext context)
	{
		m_Movement.StartCrouch();
	}

	private void Handle_CrouchCancelled(InputAction.CallbackContext context)
	{
        m_Movement.StopCrouch();
    }

	

	void Handle_HealthDamaged(float p_CurrentHealth, float p_MaxHealth, float p_Change)
	{
		Debug.Log($"Current health is: {p_CurrentHealth} out of: {p_MaxHealth} and the damaged taken was: {p_Change}");
	}

	void Handle_OnDead(MonoBehaviour p_Attacker)
	{
		Debug.Log($"Died and the attacker was: {p_Attacker.gameObject.name}");
	}
}
