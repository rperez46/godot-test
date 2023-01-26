using Godot;
using System;

public class ClearLineEffect : Node2D
{
    private Vector2 _position;
    private int _squareSize;
    private int _width;
    public ClearLineEffect(Vector2 position, int squareSize, int width)
    {
        _position = position;
        _squareSize = squareSize;
        _width = width;
    }
    public override void _Draw()
    {
        DrawRect(new Rect2(_position, new Vector2(_width, _squareSize)), Colors.Black);
        //destroy the node after 0.1 seconds
        GetTree().CreateTimer(0.1f).Connect("timeout", this, "queue_free");
    }
}
