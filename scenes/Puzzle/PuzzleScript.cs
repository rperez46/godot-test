using Godot;
using System;

public class PuzzleScript : EventsHelper
{
    private Board _board;
    private Piece _activePiece;

    // Config.
    private int _updateAfter = 20;
    private int _elapsedFrames = 0;

    public override void _Ready()
    {
        // Get board instance
        _board = GetNode<Board>("Board");
        AddNewActivePiece();
    }
    private void AddNewActivePiece()
    {
        _activePiece = Piece.GetRandomPiece();
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
    public void CheckInputs()
    {
        if (!ShoudlCheckInputs())return;
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
                        SetPieceInPlace();
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
    public override void _Process(float delta)
    {
        CheckInputs();
        _elapsedFrames++;
        if (_elapsedFrames >= _updateAfter)
        {
            if (_board.WillCollideIfMoveDown(_activePiece))
            {
                SetPieceInPlace();
            }
            else
            {
                _activePiece.MoveDown();
                _activePiece.Update();
                _board.Update();
                _elapsedFrames = 0;
            }
            _board.CheckForClearLines();
        }
    }
}
