using Godot;
using System;
using System.Collections.Generic;

public class Piece : Node2DExtended
{
    protected int[,] _map;
    public int _x;
    public int _y;
    public int _width {get{return _map.GetLength(0);}}
    public int _height {get{return _map.GetLength(1);}}

    private Piece(){}
    // Map is a piece, x and y are the positions.
    private Piece(int[,] map, int x, int y)
    {
        _map = map;
        _x = x;
        _y = y;
    }
    // Creates a random piece.
    public static Piece GetRandomPiece(int x=0, int y=0)
    {
        var random = new Random();
        PiecesTypes type = (PiecesTypes)random.Next(0, PiecesMaps._pieces.Count);
        return CreatePiece(type, x, y);
    }
    // Create a piece of a specific type.
    public static Piece CreatePiece(PiecesTypes type, int x, int y)
    {
        return new Piece(PiecesMaps._pieces[type], x, y);
    }
    public int[,] GetMap()
    {
        return _map;
    }
    // Move the position of the piece(after a flip the new positions looks odd, this method makes it looks more natural).
    private void AdjustPosition(int[,] newMap)
    {
        if (newMap.GetLength(0) > _width)
            _x--;
        else if(newMap.GetLength(0) < _width)
            _x++;

        if (newMap.GetLength(1) > _height)
            _y--;
        else if (newMap.GetLength(1) < _height)
            _y++;
    }
    public Piece DummyFlipRight()
    {
        var newPiece = new Piece(_map, _x, _y);
        newPiece.FlipRight();
        return newPiece;
    }
    public void FlipRight()
    {
        int[,] newMap = new int[_height, _width];
        for (var j=0; j<_height; j++)
            for (var i=0; i<_width; i++)
                newMap[_height-1-j, i] = _map[i, j];

        AdjustPosition(newMap);
        _map = newMap;
    }
    public void FlipLeft()
    {
        int[,] newMap = new int[_height, _width];

        for (var j=0; j<_height; j++)
            for (var i=0; i<_width; i++)
                newMap[j, _width-1-i] = _map[i, j];

        AdjustPosition(newMap);
        _map = newMap;
    }

    public override void _Draw()
    {
        var size = 30;
        var borderSize = 2;
        for (var j=0; j<_height; j++)
        {
            for (var i=0; i<_width; i++)
            {
                if (_map[i, j] == 0)
                    continue;
                var color = ColorsCode.GetColor(_map[i,j]);
                DrawRectWithBorder(
                    new Vector2(
                        i * size + (_x * size),
                        j * size + (_y * size)
                    ),
                    new Vector2(size, size),
                    color,
                    Colors.Black,
                    borderSize
                );
            }
        }
    }
    public void MoveLeft(){_x--;}
    public void MoveRight(){_x++;}
    public void MoveUp(){_y--;}
    public void MoveDown(){_y++;}
}
