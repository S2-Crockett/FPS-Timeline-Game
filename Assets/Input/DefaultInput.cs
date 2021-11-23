// GENERATED AUTOMATICALLY FROM 'Assets/Input/DefaultInput.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @DefaultInput : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @DefaultInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""DefaultInput"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""81f425af-6565-4d59-b303-1ee0420b45c3"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""PassThrough"",
                    ""id"": ""29a77b69-367d-4c92-8910-ac2959210275"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""View"",
                    ""type"": ""PassThrough"",
                    ""id"": ""759d20c0-d1ac-4eb3-a50b-65c04f3b82fb"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""10c3dba7-306e-4300-ab96-b6d9bc0c1f65"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Crouch"",
                    ""type"": ""Button"",
                    ""id"": ""39cb562f-1b0b-4c1c-8e94-f4c6427e91c2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Sprint"",
                    ""type"": ""Button"",
                    ""id"": ""b2f87d0c-a4ad-40fc-aa47-785ea5792ab3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""cf61dcb2-0ecc-4706-8f97-d3ea47031ce8"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""View"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""170697d1-6377-4428-8503-ffcc110e030b"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""ab11c47d-3e5e-4640-a08c-b314c968558e"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""378d64bb-8c07-4fda-bbb9-cad9582e3097"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""a679bcd0-96eb-4ed2-8e05-1de8d3e8c470"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""0421e6ea-b053-4348-9260-71224f0bfbe6"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""98a21602-f186-4907-9f13-1e38ba63a04f"",
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
                    ""id"": ""71fa0ee3-de14-4519-a345-955d37509fd1"",
                    ""path"": ""<Keyboard>/ctrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Crouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7a4e7b2c-1fe7-41df-84bd-957cc040c98e"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Crouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""29f680b2-e7d3-45be-a6bf-d97584bbd774"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": ""Hold"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Weapon"",
            ""id"": ""f8d45c80-470d-48b2-8b58-d21087d8ec92"",
            ""actions"": [
                {
                    ""name"": ""Shoot"",
                    ""type"": ""Button"",
                    ""id"": ""8e1f925c-b8db-4b22-b4fd-0229a29ca0ca"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Aim"",
                    ""type"": ""Button"",
                    ""id"": ""5db98e67-fea3-4137-a2ea-7a952c40431c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""WeaponSlot1"",
                    ""type"": ""Button"",
                    ""id"": ""0baa4d9b-4206-47e2-a9dc-065370ceb9af"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""WeaponSlot2"",
                    ""type"": ""Button"",
                    ""id"": ""03b80e2d-bf7e-42c9-b717-ce42b72972d5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""WeaponSlot3"",
                    ""type"": ""Button"",
                    ""id"": ""277ab48e-b96e-4a3c-975e-974acc3dae1a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""WeaponSlot4"",
                    ""type"": ""Button"",
                    ""id"": ""07736fe9-3068-40e0-bd23-cfe47330fda2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""d36d6c8d-4792-46b7-98bf-032762bb3a85"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": ""Hold"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""24e8b1e2-d3c9-4a38-886f-66bf8301fb4b"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f2ef0d07-5a04-40cc-b105-47bb90372bb1"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""WeaponSlot1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d5358d04-683f-4612-8947-5ed63cc77086"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""WeaponSlot2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""68f40313-e2e9-47e2-9ad5-4522b347503e"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""WeaponSlot3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c3624c67-3d73-426f-be28-d68dea8cb471"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""WeaponSlot4"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Movement = m_Player.FindAction("Movement", throwIfNotFound: true);
        m_Player_View = m_Player.FindAction("View", throwIfNotFound: true);
        m_Player_Jump = m_Player.FindAction("Jump", throwIfNotFound: true);
        m_Player_Crouch = m_Player.FindAction("Crouch", throwIfNotFound: true);
        m_Player_Sprint = m_Player.FindAction("Sprint", throwIfNotFound: true);
        // Weapon
        m_Weapon = asset.FindActionMap("Weapon", throwIfNotFound: true);
        m_Weapon_Shoot = m_Weapon.FindAction("Shoot", throwIfNotFound: true);
        m_Weapon_Aim = m_Weapon.FindAction("Aim", throwIfNotFound: true);
        m_Weapon_WeaponSlot1 = m_Weapon.FindAction("WeaponSlot1", throwIfNotFound: true);
        m_Weapon_WeaponSlot2 = m_Weapon.FindAction("WeaponSlot2", throwIfNotFound: true);
        m_Weapon_WeaponSlot3 = m_Weapon.FindAction("WeaponSlot3", throwIfNotFound: true);
        m_Weapon_WeaponSlot4 = m_Weapon.FindAction("WeaponSlot4", throwIfNotFound: true);
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

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_Movement;
    private readonly InputAction m_Player_View;
    private readonly InputAction m_Player_Jump;
    private readonly InputAction m_Player_Crouch;
    private readonly InputAction m_Player_Sprint;
    public struct PlayerActions
    {
        private @DefaultInput m_Wrapper;
        public PlayerActions(@DefaultInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_Player_Movement;
        public InputAction @View => m_Wrapper.m_Player_View;
        public InputAction @Jump => m_Wrapper.m_Player_Jump;
        public InputAction @Crouch => m_Wrapper.m_Player_Crouch;
        public InputAction @Sprint => m_Wrapper.m_Player_Sprint;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
                @View.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnView;
                @View.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnView;
                @View.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnView;
                @Jump.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                @Crouch.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCrouch;
                @Crouch.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCrouch;
                @Crouch.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCrouch;
                @Sprint.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSprint;
                @Sprint.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSprint;
                @Sprint.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSprint;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @View.started += instance.OnView;
                @View.performed += instance.OnView;
                @View.canceled += instance.OnView;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Crouch.started += instance.OnCrouch;
                @Crouch.performed += instance.OnCrouch;
                @Crouch.canceled += instance.OnCrouch;
                @Sprint.started += instance.OnSprint;
                @Sprint.performed += instance.OnSprint;
                @Sprint.canceled += instance.OnSprint;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);

    // Weapon
    private readonly InputActionMap m_Weapon;
    private IWeaponActions m_WeaponActionsCallbackInterface;
    private readonly InputAction m_Weapon_Shoot;
    private readonly InputAction m_Weapon_Aim;
    private readonly InputAction m_Weapon_WeaponSlot1;
    private readonly InputAction m_Weapon_WeaponSlot2;
    private readonly InputAction m_Weapon_WeaponSlot3;
    private readonly InputAction m_Weapon_WeaponSlot4;
    public struct WeaponActions
    {
        private @DefaultInput m_Wrapper;
        public WeaponActions(@DefaultInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Shoot => m_Wrapper.m_Weapon_Shoot;
        public InputAction @Aim => m_Wrapper.m_Weapon_Aim;
        public InputAction @WeaponSlot1 => m_Wrapper.m_Weapon_WeaponSlot1;
        public InputAction @WeaponSlot2 => m_Wrapper.m_Weapon_WeaponSlot2;
        public InputAction @WeaponSlot3 => m_Wrapper.m_Weapon_WeaponSlot3;
        public InputAction @WeaponSlot4 => m_Wrapper.m_Weapon_WeaponSlot4;
        public InputActionMap Get() { return m_Wrapper.m_Weapon; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(WeaponActions set) { return set.Get(); }
        public void SetCallbacks(IWeaponActions instance)
        {
            if (m_Wrapper.m_WeaponActionsCallbackInterface != null)
            {
                @Shoot.started -= m_Wrapper.m_WeaponActionsCallbackInterface.OnShoot;
                @Shoot.performed -= m_Wrapper.m_WeaponActionsCallbackInterface.OnShoot;
                @Shoot.canceled -= m_Wrapper.m_WeaponActionsCallbackInterface.OnShoot;
                @Aim.started -= m_Wrapper.m_WeaponActionsCallbackInterface.OnAim;
                @Aim.performed -= m_Wrapper.m_WeaponActionsCallbackInterface.OnAim;
                @Aim.canceled -= m_Wrapper.m_WeaponActionsCallbackInterface.OnAim;
                @WeaponSlot1.started -= m_Wrapper.m_WeaponActionsCallbackInterface.OnWeaponSlot1;
                @WeaponSlot1.performed -= m_Wrapper.m_WeaponActionsCallbackInterface.OnWeaponSlot1;
                @WeaponSlot1.canceled -= m_Wrapper.m_WeaponActionsCallbackInterface.OnWeaponSlot1;
                @WeaponSlot2.started -= m_Wrapper.m_WeaponActionsCallbackInterface.OnWeaponSlot2;
                @WeaponSlot2.performed -= m_Wrapper.m_WeaponActionsCallbackInterface.OnWeaponSlot2;
                @WeaponSlot2.canceled -= m_Wrapper.m_WeaponActionsCallbackInterface.OnWeaponSlot2;
                @WeaponSlot3.started -= m_Wrapper.m_WeaponActionsCallbackInterface.OnWeaponSlot3;
                @WeaponSlot3.performed -= m_Wrapper.m_WeaponActionsCallbackInterface.OnWeaponSlot3;
                @WeaponSlot3.canceled -= m_Wrapper.m_WeaponActionsCallbackInterface.OnWeaponSlot3;
                @WeaponSlot4.started -= m_Wrapper.m_WeaponActionsCallbackInterface.OnWeaponSlot4;
                @WeaponSlot4.performed -= m_Wrapper.m_WeaponActionsCallbackInterface.OnWeaponSlot4;
                @WeaponSlot4.canceled -= m_Wrapper.m_WeaponActionsCallbackInterface.OnWeaponSlot4;
            }
            m_Wrapper.m_WeaponActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Shoot.started += instance.OnShoot;
                @Shoot.performed += instance.OnShoot;
                @Shoot.canceled += instance.OnShoot;
                @Aim.started += instance.OnAim;
                @Aim.performed += instance.OnAim;
                @Aim.canceled += instance.OnAim;
                @WeaponSlot1.started += instance.OnWeaponSlot1;
                @WeaponSlot1.performed += instance.OnWeaponSlot1;
                @WeaponSlot1.canceled += instance.OnWeaponSlot1;
                @WeaponSlot2.started += instance.OnWeaponSlot2;
                @WeaponSlot2.performed += instance.OnWeaponSlot2;
                @WeaponSlot2.canceled += instance.OnWeaponSlot2;
                @WeaponSlot3.started += instance.OnWeaponSlot3;
                @WeaponSlot3.performed += instance.OnWeaponSlot3;
                @WeaponSlot3.canceled += instance.OnWeaponSlot3;
                @WeaponSlot4.started += instance.OnWeaponSlot4;
                @WeaponSlot4.performed += instance.OnWeaponSlot4;
                @WeaponSlot4.canceled += instance.OnWeaponSlot4;
            }
        }
    }
    public WeaponActions @Weapon => new WeaponActions(this);
    public interface IPlayerActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnView(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnCrouch(InputAction.CallbackContext context);
        void OnSprint(InputAction.CallbackContext context);
    }
    public interface IWeaponActions
    {
        void OnShoot(InputAction.CallbackContext context);
        void OnAim(InputAction.CallbackContext context);
        void OnWeaponSlot1(InputAction.CallbackContext context);
        void OnWeaponSlot2(InputAction.CallbackContext context);
        void OnWeaponSlot3(InputAction.CallbackContext context);
        void OnWeaponSlot4(InputAction.CallbackContext context);
    }
}
