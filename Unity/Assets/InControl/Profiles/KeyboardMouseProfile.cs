using UnityEngine;
using InControl;
using System.Collections;

public class KeyboardMouseProfile : UnityInputDeviceProfile
{

    public KeyboardMouseProfile()
    {
        Name = "Keyboard/Mouse";
        Meta = "Keyboard and mouse input handling";

        SupportedPlatforms = new[] 
        {
            "Windows",
            "Mac",
            "Linux"
        };

        Sensitivity = 1.0f;
        LowerDeadZone = 0.0f;
        UpperDeadZone = 1.0f;

        AnalogMappings = new[]
        {
            new InputControlMapping
            {
                Handle = "Move Y WASD",
                Target = InputControlType.LeftStickY,
                Source = KeyCodeAxis( KeyCode.S, KeyCode.W )
            },
            new InputControlMapping {
              Handle = "Move X WASD",
              Target = InputControlType.LeftStickX,
              Source = KeyCodeAxis( KeyCode.A, KeyCode.D )
            },
            new InputControlMapping
            {
                Handle = "Mouse Move X",
                Target = InputControlType.RightStickX,
                Source = MouseXAxis,
                Raw = true,
                Scale = 0.5f
            },
            new InputControlMapping
            {
                Handle = "Mouse Move Y",
                Target = InputControlType.RightStickY,
                Source = MouseYAxis,
                Raw = true,
                Scale = 0.2f
            }
        };

        ButtonMappings = new[]
        {
            new InputControlMapping
            {
                Handle = "Shoot",
                Target = InputControlType.RightTrigger,
                Source = MouseButton0
            },
            new InputControlMapping
            {
                Handle = "GameInfo",
                Target = InputControlType.RightStickButton,
                Source = KeyCodeButton(KeyCode.Tab)
            }
        };
    }

}
