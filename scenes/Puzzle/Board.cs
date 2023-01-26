using Godot;
using System;
using System.Collections.Generic;

public class Board : Node2DExtended
{
    private int[,] _board;
    private int _squareSize = 30;
    private int _width {get{return _board.GetLength(0);}}
    private int _height {get{return _board.GetLength(1);}}

    public Vector2 GetSize()
    {
        return new Vector2(_width*_squareSize, _height*_squareSize);
    }
    public void SetSquareSize(int size)
    {
        _squareSize = size;
    }

    public Board() {_board = new int[10,20];}
    public Board(int x, int y){_board = new int[x, y];}

    public void Reset()
    {
        _board = new int[_width, _height];
    }
    public void AddPiece(Piece piece)
    {
        try
        {
            var pieceMap = piece.GetMap();
            for (var i=0; i<piece._width; i++)
                for (var j=0; j<piece._height; j++)
                {
                    if (pieceMap[i,j] != 0)
                        _board[piece._x+i, piece._y+j] = pieceMap[i,j];
                }
        }
        catch(Exception)
        {
            GD.Print("Board cell doesnt exist.");
        }
    }
    public List<int> CheckForClearLines()
    {
        var clearedLines = new List<int>();
        int originalPosition =_height;
        for (var j=_height-1; j>0; j--)
        {
            originalPosition--;
            var lineIsFull = true;
            var emptyCells = 0;
            for (var i=0; i<_width; i++)
            {
                if (_board[i, j] == 0)
                {
                    lineIsFull = false;
                    emptyCells++;
                }
            }
            if (lineIsFull)
            {
                clearedLines.Add(originalPosition);
                CleanLine(j);
                j++;
                // If all the cells are empty, then dont need to check the rest of the lines(because it cant be lines on top of empty lines).
                if (emptyCells == _width)
                    break;
            }
        }
        return clearedLines;
    }
    protected bool CleanLine(int y)
    {
        if (y > _height)
            return false;
        // Move all the lines from "y" to 0 one line down.
        for (var j=y; j>0; j--)
        {
            for (var i=0; i<_width; i++)
            {
                _board[i,j] = _board[i,(j-1)];
            }
        }
        // Delete the first line.
        for (var i=0; i<_width; i++)
        {
            _board[i, 0] = 0;
        }
        return true;
    }
    
    //draw the board
    public override void _Draw()
    {
        drawBoard();
        drawPieces();
    }
    private void drawBoard()
    {
        var size = _squareSize;
        var borderSize = 2;
        for (var j=0; j<_height; j++)
        {
            for (var i=0; i<_width; i++)
            {
                DrawRectWithBorder(
                    new Vector2(i * size, j * size),
                    new Vector2(size, size),
                    Colors.White,
                    Colors.Black,
                    borderSize
                );
            }
        }
    }
    private void drawPieces()
    {
        var size = _squareSize;
        var borderSize = 2;
        for (var j=0; j<_height; j++)
        {
            for (var i=0; i<_width; i++)
            {
                if (_board[i, j] == 0)
                    continue;
                var color = ColorsCode.GetColor(_board[i,j]);
                DrawRectWithBorder(
                    new Vector2(i*size, j*size),
                    new Vector2(size, size),
                    color,
                    Colors.Black,
                    borderSize
                );
            }
        }
    }
    public void KeepInBounds(Piece _piece)
    {
        if (_piece._x < 0)
            _piece._x = 0;
        if (_piece._x + _piece._width > _width)
            _piece._x = _width-_piece._width;
    }
    public bool Collides(Piece _piece)
    {
        //check if piece is out of height bounds
        if (_piece._y+_piece._height > _height)
        {
            return true;
        }
        return false;
    }
    public bool CollidesWithOtherPieces(Piece _piece, int offset = 0)
    {
        var pieceMap = _piece.GetMap();
        //check if piece is colliding with other pieces
        for (var i=0; i<_piece._width; i++)
        {
            for (var j=0; j<_piece._height; j++)
            {
                if (pieceMap[i,j] != 0)
                {
                    if (_piece._x+i < 0 || _piece._x+i > _width-1 || _piece._y+j < 0 || _piece._y+j+offset > _height-1)
                        continue;
                    if (_board[_piece._x+i, _piece._y+j+offset] != 0)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
    public bool WillCollideIfMoveDown(Piece _piece)
    {
        try
        {
            //check if piece is out of height bounds
            if (_piece._y+_piece._height > _height-1)
                return true;
            return CollidesWithOtherPieces(_piece,1);
        }
        catch(Exception ex)
        {
            GD.Print(ex.Message);
            GD.Print(ex.InnerException);
            return true;
        }
    }
    public bool CanMakeMove(Piece _activePiece, int xMove)
    {
        var pieceMap = _activePiece.GetMap();
        if (_activePiece._x+xMove < 0)
        {
            return false;
        }
        if (_activePiece._x+xMove+_activePiece._width > _width)
        {
            return false;
        }
        //check if piece is colliding with other pieces
        for (var i=0; i<_activePiece._width; i++)
        {
            for (var j=0; j<_activePiece._height; j++)
            {
                if (pieceMap[i,j] != 0)
                {
                    if (_board[_activePiece._x+i+xMove, _activePiece._y+j] != 0)
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }
}
