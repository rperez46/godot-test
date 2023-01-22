using System.Collections.Generic;
public class PiecesMaps
{
    public static Dictionary<PiecesTypes, int[,]> _pieces = new Dictionary<PiecesTypes, int[,]>{
        {PiecesTypes.Long, new int[1,4]{{2,2,2,2}}},
        {PiecesTypes.Snake, new int[3,2]{{0,4},{4,4},{4,0}}},
        {PiecesTypes.InvertedSnake, new int[3,2]{{3,0},{3,3},{0,3}}},
        {PiecesTypes.T, new int[3,2]{{0,5},{5,5},{0,5}}},
        {PiecesTypes.L, new int[2,3]{{6,6,6},{0,0,6}}},
        {PiecesTypes.InvertedL, new int[2,3]{{0,0,7},{7,7,7}}},
        {PiecesTypes.Square, new int[2,2]{{1,1}, {1,1}}}
    };
}