// GENERATED AUTOMATICALLY FROM 'Assets/Developers/Sam C/Input/Select.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @Select : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @Select()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Select"",
    ""maps"": [
        {
            ""name"": ""Move"",
            ""id"": ""1d7794e5-df56-4e5f-9bc8-0ee53d604b6d"",
            ""actions"": [
                {
                    ""name"": ""MoveLeft"",
                    ""type"": ""Button"",
                    ""id"": ""cd4eb077-54db-4a1d-8bac-a4474c4c3e9a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MoveRight"",
                    ""type"": ""Button"",
                    ""id"": ""f008bf7c-33cd-41b4-a6be-cb872192ad4e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Select"",
                    ""type"": ""Button"",
                    ""id"": ""3f7fdbc9-20b9-4e21-aff9-af3de9ec4ad6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Leave"",
                    ""type"": ""Button"",
                    ""id"": ""cb75a3b1-980d-486d-b151-429ae8967d85"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MoveDown"",
                    ""type"": ""Button"",
                    ""id"": ""5432643c-ef32-4868-bc7d-40291a54239a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MoveUp"",
                    ""type"": ""Button"",
                    ""id"": ""e012164e-6cf4-491e-853e-63aa2c423532"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""325dfae2-3fe7-4baa-9d4a-43227ffb35d4"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": ""Tap"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6634289e-9e21-4b91-b5fb-8462cf76e4de"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": ""Tap"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""966e0e18-e9ce-4fe4-bb72-2a3c16958480"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": ""Tap"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""798bd468-3914-440d-bf12-a6d27ed1c845"",
                    ""path"": ""<Keyboard>/p"",
                    ""interactions"": ""Tap"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Leave"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cb18f857-0018-4f38-8e2f-9b9202553827"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": ""Tap"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveDown"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f01e5ca0-4113-47d6-8017-a52ea9249853"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": ""Tap"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveUp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Move
        m_Move = asset.FindActionMap("Move", throwIfNotFound: true);
        m_Move_MoveLeft = m_Move.FindAction("MoveLeft", throwIfNotFound: true);
        m_Move_MoveRight = m_Move.FindAction("MoveRight", throwIfNotFound: true);
        m_Move_Select = m_Move.FindAction("Select", throwIfNotFound: true);
        m_Move_Leave = m_Move.FindAction("Leave", throwIfNotFound: true);
        m_Move_MoveDown = m_Move.FindAction("MoveDown", throwIfNotFound: true);
        m_Move_MoveUp = m_Move.FindAction("MoveUp", throwIfNotFound: true);
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

    // Move
    private readonly InputActionMap m_Move;
    private IMoveActions m_MoveActionsCallbackInterface;
    private readonly InputAction m_Move_MoveLeft;
    private readonly InputAction m_Move_MoveRight;
    private readonly InputAction m_Move_Select;
    private readonly InputAction m_Move_Leave;
    private readonly InputAction m_Move_MoveDown;
    private readonly InputAction m_Move_MoveUp;
    public struct MoveActions
    {
        private @Select m_Wrapper;
        public MoveActions(@Select wrapper) { m_Wrapper = wrapper; }
        public InputAction @MoveLeft => m_Wrapper.m_Move_MoveLeft;
        public InputAction @MoveRight => m_Wrapper.m_Move_MoveRight;
        public InputAction @Select => m_Wrapper.m_Move_Select;
        public InputAction @Leave => m_Wrapper.m_Move_Leave;
        public InputAction @MoveDown => m_Wrapper.m_Move_MoveDown;
        public InputAction @MoveUp => m_Wrapper.m_Move_MoveUp;
        public InputActionMap Get() { return m_Wrapper.m_Move; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MoveActions set) { return set.Get(); }
        public void SetCallbacks(IMoveActions instance)
        {
            if (m_Wrapper.m_MoveActionsCallbackInterface != null)
            {
                @MoveLeft.started -= m_Wrapper.m_MoveActionsCallbackInterface.OnMoveLeft;
                @MoveLeft.performed -= m_Wrapper.m_MoveActionsCallbackInterface.OnMoveLeft;
                @MoveLeft.canceled -= m_Wrapper.m_MoveActionsCallbackInterface.OnMoveLeft;
                @MoveRight.started -= m_Wrapper.m_MoveActionsCallbackInterface.OnMoveRight;
                @MoveRight.performed -= m_Wrapper.m_MoveActionsCallbackInterface.OnMoveRight;
                @MoveRight.canceled -= m_Wrapper.m_MoveActionsCallbackInterface.OnMoveRight;
                @Select.started -= m_Wrapper.m_MoveActionsCallbackInterface.OnSelect;
                @Select.performed -= m_Wrapper.m_MoveActionsCallbackInterface.OnSelect;
                @Select.canceled -= m_Wrapper.m_MoveActionsCallbackInterface.OnSelect;
                @Leave.started -= m_Wrapper.m_MoveActionsCallbackInterface.OnLeave;
                @Leave.performed -= m_Wrapper.m_MoveActionsCallbackInterface.OnLeave;
                @Leave.canceled -= m_Wrapper.m_MoveActionsCallbackInterface.OnLeave;
                @MoveDown.started -= m_Wrapper.m_MoveActionsCallbackInterface.OnMoveDown;
                @MoveDown.performed -= m_Wrapper.m_MoveActionsCallbackInterface.OnMoveDown;
                @MoveDown.canceled -= m_Wrapper.m_MoveActionsCallbackInterface.OnMoveDown;
                @MoveUp.started -= m_Wrapper.m_MoveActionsCallbackInterface.OnMoveUp;
                @MoveUp.performed -= m_Wrapper.m_MoveActionsCallbackInterface.OnMoveUp;
                @MoveUp.canceled -= m_Wrapper.m_MoveActionsCallbackInterface.OnMoveUp;
            }
            m_Wrapper.m_MoveActionsCallbackInterface = instance;
            if (instance != null)
            {
                @MoveLeft.started += instance.OnMoveLeft;
                @MoveLeft.performed += instance.OnMoveLeft;
                @MoveLeft.canceled += instance.OnMoveLeft;
                @MoveRight.started += instance.OnMoveRight;
                @MoveRight.performed += instance.OnMoveRight;
                @MoveRight.canceled += instance.OnMoveRight;
                @Select.started += instance.OnSelect;
                @Select.performed += instance.OnSelect;
                @Select.canceled += instance.OnSelect;
                @Leave.started += instance.OnLeave;
                @Leave.performed += instance.OnLeave;
                @Leave.canceled += instance.OnLeave;
                @MoveDown.started += instance.OnMoveDown;
                @MoveDown.performed += instance.OnMoveDown;
                @MoveDown.canceled += instance.OnMoveDown;
                @MoveUp.started += instance.OnMoveUp;
                @MoveUp.performed += instance.OnMoveUp;
                @MoveUp.canceled += instance.OnMoveUp;
            }
        }
    }
    public MoveActions @Move => new MoveActions(this);
    public interface IMoveActions
    {
        void OnMoveLeft(InputAction.CallbackContext context);
        void OnMoveRight(InputAction.CallbackContext context);
        void OnSelect(InputAction.CallbackContext context);
        void OnLeave(InputAction.CallbackContext context);
        void OnMoveDown(InputAction.CallbackContext context);
        void OnMoveUp(InputAction.CallbackContext context);
    }
}
