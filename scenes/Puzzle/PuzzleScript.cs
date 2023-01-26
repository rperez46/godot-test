using Godot;
using System;
using System.Collections.Generic;

public class PuzzleScript : EventsHelper
{
    private Board _board;
    private Piece _activePiece;
    private int _squareSize = 30;//Indivial square size(each piece is made of multiple squares).

    // Config.
    private int _updateAfter = 20;
    private int _elapsedFrames = 0;

    public override void _Ready()
    {
        // Get board instance
        _board = GetNode<Board>("Board");
        _board.SetSquareSize(_squareSize);
        AddNewActivePiece();
        //center board on the screen.
        _board.Position = new Vector2((GetViewportRect().Size.x - _board.GetSize().x) / 2, 0);
    }
    private void AddNewActivePiece()
    {
        _activePiece = Piece.GetRandomPiece(_squareSize);
        _board.AddChild(_activePiece);
    }
    // Add the active piece to the board and create a new one, it also checks if the game is over.
    private void SetPieceInPlace()
    {
        AddActivePieceToBoard();
        AddNewActivePiece();
        if (IsGameOver())
            _board.Reset();
    }
    private void AddActivePieceToBoard()
    {
        _board.AddPiece(_activePiece);
        _board.RemoveChild(_activePiece);
    }
    // This method should be called just inmediately after the piece was created, otherwise it could return a false positive.
    private bool IsGameOver()
    {
        return _board.Collides(_activePiece) || _board.CollidesWithOtherPieces(_activePiece);
    }
    // Boolean to check if the piece was already flipped(used to prevent the piece from flipping twice in a single keypress).
    private bool _isFliped = false;
    public override void CheckInputs(List<string> _pressedKeys)
    {
        var isFliped = false;
        foreach (var key in _pressedKeys)
        {
            switch (key)
            {
                // Left
                case "65":
                    if(_board.CanMakeMove(_activePiece, -1))
                        _activePiece.MoveLeft();
                    break;
                // Right
                case "68":
                    if(_board.CanMakeMove(_activePiece, 1))
                        _activePiece.MoveRight();
                    break;
                // Flip Right
                case "87":
                    isFliped = true;
                    if (!this._isFliped)
                    {
                        var newPiece =_activePiece.DummyFlipRight();
                        if (!_board.Collides(newPiece) && !_board.CollidesWithOtherPieces(newPiece))
                            _activePiece.FlipRight();
                    }
                    break;
                // Down
                case "83":
                    if (_board.WillCollideIfMoveDown(_activePiece))
                    {
                        SetPieceInPlace();
                        CheckForClearLines();
                    }
                    else
                        _activePiece.MoveDown();
                    break;
            }
        }
        this._isFliped = isFliped;
        _board.KeepInBounds(_activePiece);
        _activePiece.Update();
        _board.Update();
    }
    public void AddClearLinesEffect(List<int> clearedLines)
    {
        foreach (var line in clearedLines)
        {
            GD.Print("Clearing line: " + line);
            var effect = new ClearLineEffect(
                new Vector2(0, line * _squareSize),
                _squareSize,
                (int)_board.GetSize().x
            );
            _board.AddChild(effect);
        }
    }
    public void CheckForClearLines()
    {
        var clearedLines = _board.CheckForClearLines();
        if (clearedLines.Count > 0)
        {
            // Add the efect of the lines being cleared.
            AddClearLinesEffect(clearedLines);
        }
    }
    public override void _Process(float delta)
    {
        CheckInputs();
        _elapsedFrames++;
        if (_elapsedFrames >= _updateAfter)
        {
            if (_board.WillCollideIfMoveDown(_activePiece))
            {
                SetPieceInPlace();
                CheckForClearLines();
            }
            else
            {
                _activePiece.MoveDown();
                _activePiece.Update();
                _board.Update();
                _elapsedFrames = 0;
            }
        }
    }
}
