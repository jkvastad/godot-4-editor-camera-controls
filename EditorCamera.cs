using Godot;
using System.Collections.Generic;

public partial class EditorCamera : Camera3D
{
    bool _isLooking = false;
    float _totalPitch = 0;
    float _mouseSensitivity = 0.25f;
    float speed = 5;

    Dictionary<Key, float> _isKeyPressed = new()
    {
        { Key.Q, 0},
        { Key.W, 0},
        { Key.E, 0},
        { Key.A, 0},
        { Key.S, 0},
        { Key.D, 0}
    };

    public override void _Process(double delta)
    {
        Vector3 direction = new Vector3(
            _isKeyPressed[Key.D] - _isKeyPressed[Key.A],
            _isKeyPressed[Key.E] - _isKeyPressed[Key.Q],
            _isKeyPressed[Key.S] - _isKeyPressed[Key.W]
            );
        direction = direction.Normalized();
        Translate(direction * (float)delta * speed);
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventKey keyEvent)
        {
            if (keyEvent.Pressed && _isKeyPressed.ContainsKey(keyEvent.Keycode))
            {
                _isKeyPressed[keyEvent.Keycode] = 1;
            }
            if (!keyEvent.Pressed && _isKeyPressed.ContainsKey(keyEvent.Keycode))
            {
                _isKeyPressed[keyEvent.Keycode] = 0;
            }
        }

        if (@event is InputEventMouseButton mouseButton)
        {
            if (_isLooking && mouseButton.ButtonIndex == MouseButton.WheelUp)
            {
                speed += 1;
            }
            if (_isLooking && mouseButton.ButtonIndex == MouseButton.WheelDown)
            {
                if (speed > 1) speed -= 1;
            }
            if (mouseButton.Pressed && mouseButton.ButtonIndex == MouseButton.Right)
            {
                Input.MouseMode = Input.MouseModeEnum.Captured;
                _isLooking = true;
            }
            if (!mouseButton.Pressed && mouseButton.ButtonIndex == MouseButton.Right)
            {
                Input.MouseMode = Input.MouseModeEnum.Visible;
                _isLooking = false;
            }
        }
        if (@event is InputEventMouseMotion mouseMotion && _isLooking)
        {
            Vector2 relativeMotion = mouseMotion.Relative * _mouseSensitivity;
            float yaw = relativeMotion.X;

            float pitch = relativeMotion.Y;
            pitch = Mathf.Clamp(pitch, -90 - _totalPitch, 90 - _totalPitch);
            _totalPitch += pitch;

            RotateY(Mathf.DegToRad(-yaw));
            RotateObjectLocal(Vector3.Right, Mathf.DegToRad(-pitch));
        }

    }
}