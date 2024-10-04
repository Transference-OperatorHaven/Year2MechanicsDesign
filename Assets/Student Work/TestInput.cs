using UnityEngine;
using UnityEngine.InputSystem;

public class TestInput : MonoBehaviour
{
    TestInputAction m_ActionMap;

    private void OnEnable()
    {
        m_ActionMap.Enable();


        m_ActionMap.Test_Control_Map.Move.performed += Handle_MovePerformed;
        m_ActionMap.Test_Control_Map.Move.canceled += Handle_MoveCanceled;
        m_ActionMap.Test_Control_Map.Jump.performed += Handle_JumpPerformed;
        m_ActionMap.Test_Control_Map.Interact.performed += Handle_InteractPerformed;
        m_ActionMap.Test_Control_Map.Slam.performed += Handle_DTSlamPerformed;
    }
    private void OnDisable()
    {
        m_ActionMap.Disable();


        m_ActionMap.Test_Control_Map.Move.performed -= Handle_MovePerformed;
        m_ActionMap.Test_Control_Map.Move.canceled -= Handle_MoveCanceled;
        m_ActionMap.Test_Control_Map.Jump.performed -= Handle_JumpPerformed;
        m_ActionMap.Test_Control_Map.Interact.performed -= Handle_InteractPerformed;
        m_ActionMap.Test_Control_Map.Slam.performed -= Handle_DTSlamPerformed;
    }

    private void Awake()
    {
        m_ActionMap = new TestInputAction();

    }

    void Handle_MovePerformed(InputAction.CallbackContext context)
    {
        Debug.Log("Movement performed with value: " + context.ReadValue<float>());
    }
    void Handle_MoveCanceled(InputAction.CallbackContext context)
    {
        Debug.Log("Movement canceled with value: " + context.ReadValue<float>());
    }
    void Handle_JumpPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("Jumped");
    }
    void Handle_InteractPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("Interacted");
    }
    void Handle_DTSlamPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("Slammed");
    }
}
