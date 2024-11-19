using System.Collections.Generic;
using System.Windows.Forms;
using System.Numerics;

using SharpDX.Windows;

namespace Project;

public class Input
{
    private List<Keys> keysPressed = [];
    private List<Keys> keysDown = [];
    private List<Keys> keysUp = [];
    private List<MouseButtons> mouseButtonsPressed = [];
    private List<MouseButtons> mouseButtonsDown = [];
    private List<MouseButtons> mouseButtonsUp = [];
    private List<Keys> keysDownLastFrame = [];
    private List<Keys> keysUpLastFrame = [];
    private List<MouseButtons> mouseButtonsDownLastFrame = [];
    private List<MouseButtons> mouseButtonsUpLastFrame = [];
    private Vector2 mousePosition = Vector2.Zero;

    public Input(RenderForm renderForm) => StartListening(renderForm);

    private void StartListening(RenderForm renderForm)
    {
        renderForm.KeyDown += (o, e) => keysDownLastFrame.Add(e.KeyCode);
        renderForm.KeyUp += (o, e) => keysUpLastFrame.Add(e.KeyCode);
        renderForm.MouseDown += (o, e) => mouseButtonsDownLastFrame.Add(e.Button);
        renderForm.MouseUp += (o, e) => mouseButtonsUpLastFrame.Add(e.Button);
        renderForm.MouseMove += (o, e) => mousePosition = new(e.X, e.Y);
    }

    public void Update()
    {
        HandleKeyboardInput();
        HandleMouseInput();
    }

    private void HandleKeyboardInput()
    {
        keysDown.Clear();
        keysUp.Clear();

        foreach (Keys key in keysDownLastFrame)
        {
            if (keysPressed.Contains(key) == false)
            {
                keysDown.Add(key);
                keysPressed.Add(key);
            }
        }

        foreach (Keys key in keysUpLastFrame)
        {
            if (keysPressed.Contains(key) == true)
            {
                keysUp.Add(key);
                keysPressed.Remove(key);
            }
        }

        keysDownLastFrame.Clear();
        keysUpLastFrame.Clear();
    }

    private void HandleMouseInput()
    {
        mouseButtonsDown.Clear();
        mouseButtonsUp.Clear();

        foreach (MouseButtons button in mouseButtonsDownLastFrame)
        {
            if (mouseButtonsPressed.Contains(button) == false)
            {
                mouseButtonsDown.Add(button);
                mouseButtonsPressed.Add(button);
            }
        }

        foreach (MouseButtons button in mouseButtonsUpLastFrame)
        {
            if (mouseButtonsPressed.Contains(button) == true)
            {
                mouseButtonsUp.Add(button);
                mouseButtonsPressed.Remove(button);
            }
        }

        mouseButtonsDownLastFrame.Clear();
        mouseButtonsUpLastFrame.Clear();
    }

    public bool GetKey(Key key) => keysPressed.Contains((Keys)key);
    public bool GetKeyDown(Key key) => keysDown.Contains((Keys)key);
    public bool GetKeyUp(Key key) => keysUp.Contains((Keys)key);
    public bool GetMouseButton(int buttonID) => mouseButtonsPressed.Contains(TranslateMouseButton(buttonID));
    public bool GetMouseButtonDown(int buttonID) => mouseButtonsDown.Contains(TranslateMouseButton(buttonID));
    public bool GetMouseButtonUp(int buttonID) => mouseButtonsUp.Contains(TranslateMouseButton(buttonID));
    public Vector2 GetMousePosition() => mousePosition;

    private MouseButtons TranslateMouseButton(int buttonID)
    {
        return buttonID switch
        {
            0 => MouseButtons.Left,
            1 => MouseButtons.Right,
            2 => MouseButtons.Middle,
            _ => MouseButtons.None,
        };
    }
}