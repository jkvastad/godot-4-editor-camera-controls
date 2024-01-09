using Godot;

public partial class EditorCamera : Camera3D
{
    bool _looking = false;
    float _totalPitch = 0;
    float _mouseSensitivity = 0.25f;

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButton)
        {
            if (mouseButton.Pressed && mouseButton.ButtonIndex == MouseButton.Right)
            {
                Input.MouseMode = Input.MouseModeEnum.Captured;
            }
            if (!mouseButton.Pressed && mouseButton.ButtonIndex == MouseButton.Right)
            {
                Input.MouseMode = Input.MouseModeEnum.Visible;
            }
        }
        if (@event is InputEventMouseMotion mouseMotion)
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