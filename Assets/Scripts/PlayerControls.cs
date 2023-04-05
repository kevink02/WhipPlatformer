// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/PlayerControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""Movement"",
            ""id"": ""08cd69e7-507a-4bed-86d9-70e77c0b7a6c"",
            ""actions"": [
                {
                    ""name"": ""HorizontalMove"",
                    ""type"": ""Button"",
                    ""id"": ""de0905f7-95e5-4510-b349-5fb30a5386ab"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""VerticalMove"",
                    ""type"": ""Button"",
                    ""id"": ""fad83282-9206-4370-adf5-8b386acf86e7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""bb001824-1819-4eb1-b174-017a6c600e06"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HorizontalMove"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""0818c5dc-f843-4db2-ab5e-2d9918779968"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HorizontalMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""b01bab7e-89ad-47db-a165-83f5c5d90e18"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HorizontalMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""ad9461b2-5a86-4af2-95b3-cf7e3ff2fe66"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""VerticalMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Abilities"",
            ""id"": ""dbb45081-e1a8-46cd-b61d-d3e8bed84cfe"",
            ""actions"": [
                {
                    ""name"": ""Attack"",
                    ""type"": ""Button"",
                    ""id"": ""2cd126a3-d07a-491c-a3ab-668e5a78d952"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""4b015748-ae13-48e7-bff1-03dbd91e85e8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""da606efd-129d-4c7d-aef9-5996698d411b"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4a32712e-64df-4f73-8d86-09509eb3016a"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Camera"",
            ""id"": ""a6476a69-f5f3-495d-a7b0-62123c286c57"",
            ""actions"": [
                {
                    ""name"": ""ZoomOut"",
                    ""type"": ""Button"",
                    ""id"": ""7faafad0-4fce-409c-b645-73324d50f376"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""cda76cdc-e4f0-4219-9897-18e3e7ffae6f"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ZoomOut"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Movement
        m_Movement = asset.FindActionMap("Movement", throwIfNotFound: true);
        m_Movement_HorizontalMove = m_Movement.FindAction("HorizontalMove", throwIfNotFound: true);
        m_Movement_VerticalMove = m_Movement.FindAction("VerticalMove", throwIfNotFound: true);
        // Abilities
        m_Abilities = asset.FindActionMap("Abilities", throwIfNotFound: true);
        m_Abilities_Attack = m_Abilities.FindAction("Attack", throwIfNotFound: true);
        m_Abilities_Interact = m_Abilities.FindAction("Interact", throwIfNotFound: true);
        // Camera
        m_Camera = asset.FindActionMap("Camera", throwIfNotFound: true);
        m_Camera_ZoomOut = m_Camera.FindAction("ZoomOut", throwIfNotFound: true);
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

    // Movement
    private readonly InputActionMap m_Movement;
    private IMovementActions m_MovementActionsCallbackInterface;
    private readonly InputAction m_Movement_HorizontalMove;
    private readonly InputAction m_Movement_VerticalMove;
    public struct MovementActions
    {
        private @PlayerControls m_Wrapper;
        public MovementActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @HorizontalMove => m_Wrapper.m_Movement_HorizontalMove;
        public InputAction @VerticalMove => m_Wrapper.m_Movement_VerticalMove;
        public InputActionMap Get() { return m_Wrapper.m_Movement; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MovementActions set) { return set.Get(); }
        public void SetCallbacks(IMovementActions instance)
        {
            if (m_Wrapper.m_MovementActionsCallbackInterface != null)
            {
                @HorizontalMove.started -= m_Wrapper.m_MovementActionsCallbackInterface.OnHorizontalMove;
                @HorizontalMove.performed -= m_Wrapper.m_MovementActionsCallbackInterface.OnHorizontalMove;
                @HorizontalMove.canceled -= m_Wrapper.m_MovementActionsCallbackInterface.OnHorizontalMove;
                @VerticalMove.started -= m_Wrapper.m_MovementActionsCallbackInterface.OnVerticalMove;
                @VerticalMove.performed -= m_Wrapper.m_MovementActionsCallbackInterface.OnVerticalMove;
                @VerticalMove.canceled -= m_Wrapper.m_MovementActionsCallbackInterface.OnVerticalMove;
            }
            m_Wrapper.m_MovementActionsCallbackInterface = instance;
            if (instance != null)
            {
                @HorizontalMove.started += instance.OnHorizontalMove;
                @HorizontalMove.performed += instance.OnHorizontalMove;
                @HorizontalMove.canceled += instance.OnHorizontalMove;
                @VerticalMove.started += instance.OnVerticalMove;
                @VerticalMove.performed += instance.OnVerticalMove;
                @VerticalMove.canceled += instance.OnVerticalMove;
            }
        }
    }
    public MovementActions @Movement => new MovementActions(this);

    // Abilities
    private readonly InputActionMap m_Abilities;
    private IAbilitiesActions m_AbilitiesActionsCallbackInterface;
    private readonly InputAction m_Abilities_Attack;
    private readonly InputAction m_Abilities_Interact;
    public struct AbilitiesActions
    {
        private @PlayerControls m_Wrapper;
        public AbilitiesActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Attack => m_Wrapper.m_Abilities_Attack;
        public InputAction @Interact => m_Wrapper.m_Abilities_Interact;
        public InputActionMap Get() { return m_Wrapper.m_Abilities; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(AbilitiesActions set) { return set.Get(); }
        public void SetCallbacks(IAbilitiesActions instance)
        {
            if (m_Wrapper.m_AbilitiesActionsCallbackInterface != null)
            {
                @Attack.started -= m_Wrapper.m_AbilitiesActionsCallbackInterface.OnAttack;
                @Attack.performed -= m_Wrapper.m_AbilitiesActionsCallbackInterface.OnAttack;
                @Attack.canceled -= m_Wrapper.m_AbilitiesActionsCallbackInterface.OnAttack;
                @Interact.started -= m_Wrapper.m_AbilitiesActionsCallbackInterface.OnInteract;
                @Interact.performed -= m_Wrapper.m_AbilitiesActionsCallbackInterface.OnInteract;
                @Interact.canceled -= m_Wrapper.m_AbilitiesActionsCallbackInterface.OnInteract;
            }
            m_Wrapper.m_AbilitiesActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Attack.started += instance.OnAttack;
                @Attack.performed += instance.OnAttack;
                @Attack.canceled += instance.OnAttack;
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
            }
        }
    }
    public AbilitiesActions @Abilities => new AbilitiesActions(this);

    // Camera
    private readonly InputActionMap m_Camera;
    private ICameraActions m_CameraActionsCallbackInterface;
    private readonly InputAction m_Camera_ZoomOut;
    public struct CameraActions
    {
        private @PlayerControls m_Wrapper;
        public CameraActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @ZoomOut => m_Wrapper.m_Camera_ZoomOut;
        public InputActionMap Get() { return m_Wrapper.m_Camera; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CameraActions set) { return set.Get(); }
        public void SetCallbacks(ICameraActions instance)
        {
            if (m_Wrapper.m_CameraActionsCallbackInterface != null)
            {
                @ZoomOut.started -= m_Wrapper.m_CameraActionsCallbackInterface.OnZoomOut;
                @ZoomOut.performed -= m_Wrapper.m_CameraActionsCallbackInterface.OnZoomOut;
                @ZoomOut.canceled -= m_Wrapper.m_CameraActionsCallbackInterface.OnZoomOut;
            }
            m_Wrapper.m_CameraActionsCallbackInterface = instance;
            if (instance != null)
            {
                @ZoomOut.started += instance.OnZoomOut;
                @ZoomOut.performed += instance.OnZoomOut;
                @ZoomOut.canceled += instance.OnZoomOut;
            }
        }
    }
    public CameraActions @Camera => new CameraActions(this);
    public interface IMovementActions
    {
        void OnHorizontalMove(InputAction.CallbackContext context);
        void OnVerticalMove(InputAction.CallbackContext context);
    }
    public interface IAbilitiesActions
    {
        void OnAttack(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
    }
    public interface ICameraActions
    {
        void OnZoomOut(InputAction.CallbackContext context);
    }
}
