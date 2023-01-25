using Godot;
using System;
using System.Collections.Generic;
public class EventsHelper : Node2D
{
    private int _elapsedFrames = 0;
    private int _checkAfter = 4;

    private List<string> _pressedKeys = new List<string>();
    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey eventKey)
        {
            if (eventKey.Pressed)
            {
                if (!_pressedKeys.Contains(eventKey.Scancode.ToString()))
                    _pressedKeys.Add(eventKey.Scancode.ToString());
            }
            else
            {
                if (_pressedKeys.Contains(eventKey.Scancode.ToString()))
                    _pressedKeys.Remove(eventKey.Scancode.ToString());
            }
        }
    }
    public bool ShoudlCheckInputs()
    {
        _elapsedFrames++;
        if (_elapsedFrames >= _checkAfter)
        {
            _elapsedFrames = 0;
            return true;
        }
        return false;
    }
    public void CheckInputs()
    {
        if (ShoudlCheckInputs())
            CheckInputs(_pressedKeys);
    }
    public virtual void CheckInputs(List<string> _pressedKeys){}
}