//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.11.1
//     from Assets/PlayerController/PlayerControls.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerControls: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""Default"",
            ""id"": ""bfa504c1-896c-49a4-8474-510633a76edc"",
            ""actions"": [
                {
                    ""name"": ""MoveHoriz"",
                    ""type"": ""Value"",
                    ""id"": ""75658d93-a5d4-4d48-9d47-6ac007db1915"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""f991cb4c-ebfc-4508-8d66-10da34f482c5"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""8253b0ae-c313-42c1-9e71-c8fb2ca45804"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveHoriz"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""e144c636-0170-4f86-b434-f7ae9c6448d1"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveHoriz"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""631f594d-1f1b-4707-b041-7c2c0aca4585"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveHoriz"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""f0c64b12-137a-4e38-9d17-2c5e98dfd99d"",
                    ""path"": ""<Gamepad>/leftStick/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveHoriz"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dab14477-cf03-41c7-b44d-01cb2648617a"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6129a348-186c-4d1b-b972-1544d81ca80d"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Default
        m_Default = asset.FindActionMap("Default", throwIfNotFound: true);
        m_Default_MoveHoriz = m_Default.FindAction("MoveHoriz", throwIfNotFound: true);
        m_Default_Jump = m_Default.FindAction("Jump", throwIfNotFound: true);
    }

    ~@PlayerControls()
    {
        UnityEngine.Debug.Assert(!m_Default.enabled, "This will cause a leak and performance issues, PlayerControls.Default.Disable() has not been called.");
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Default
    private readonly InputActionMap m_Default;
    private List<IDefaultActions> m_DefaultActionsCallbackInterfaces = new List<IDefaultActions>();
    private readonly InputAction m_Default_MoveHoriz;
    private readonly InputAction m_Default_Jump;
    public struct DefaultActions
    {
        private @PlayerControls m_Wrapper;
        public DefaultActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @MoveHoriz => m_Wrapper.m_Default_MoveHoriz;
        public InputAction @Jump => m_Wrapper.m_Default_Jump;
        public InputActionMap Get() { return m_Wrapper.m_Default; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(DefaultActions set) { return set.Get(); }
        public void AddCallbacks(IDefaultActions instance)
        {
            if (instance == null || m_Wrapper.m_DefaultActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_DefaultActionsCallbackInterfaces.Add(instance);
            @MoveHoriz.started += instance.OnMoveHoriz;
            @MoveHoriz.performed += instance.OnMoveHoriz;
            @MoveHoriz.canceled += instance.OnMoveHoriz;
            @Jump.started += instance.OnJump;
            @Jump.performed += instance.OnJump;
            @Jump.canceled += instance.OnJump;
        }

        private void UnregisterCallbacks(IDefaultActions instance)
        {
            @MoveHoriz.started -= instance.OnMoveHoriz;
            @MoveHoriz.performed -= instance.OnMoveHoriz;
            @MoveHoriz.canceled -= instance.OnMoveHoriz;
            @Jump.started -= instance.OnJump;
            @Jump.performed -= instance.OnJump;
            @Jump.canceled -= instance.OnJump;
        }

        public void RemoveCallbacks(IDefaultActions instance)
        {
            if (m_Wrapper.m_DefaultActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IDefaultActions instance)
        {
            foreach (var item in m_Wrapper.m_DefaultActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_DefaultActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public DefaultActions @Default => new DefaultActions(this);
    public interface IDefaultActions
    {
        void OnMoveHoriz(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
    }
}
