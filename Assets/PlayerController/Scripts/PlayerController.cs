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
	private bool m_InMoveActive;
	private Coroutine m_MoveCoroutine;

	private void Awake()
	{
		m_ActionMap = new PlayerControls();
		m_Movement = GetComponent<CharacterMovement>();
	}

	private void OnEnable()
	{
		m_ActionMap.Enable();

		m_ActionMap.Default.MoveHoriz.performed += Handle_MovePerformed;
		m_ActionMap.Default.MoveHoriz.canceled += Handle_MoveCancelled;
		m_ActionMap.Default.Jump.performed += Handle_JumpPerformed;
		m_ActionMap.Default.Jump.canceled += Handle_JumpCancelled;
	}

	private void OnDisable()
	{
		m_ActionMap.Disable();

		m_ActionMap.Default.MoveHoriz.performed -= Handle_MovePerformed;
		m_ActionMap.Default.MoveHoriz.canceled -= Handle_MoveCancelled;
		m_ActionMap.Default.Jump.performed -= Handle_JumpPerformed;
		m_ActionMap.Default.Jump.canceled -= Handle_JumpCancelled;
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

	private void Handle_JumpPerformed(InputAction.CallbackContext context)
	{
		m_Movement.StartJump();
	}
	private void Handle_JumpCancelled(InputAction.CallbackContext context)
	{
		m_Movement.StopJump();
	}
}
