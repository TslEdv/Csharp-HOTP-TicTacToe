using System.Collections.Generic;

namespace TicTacToeBrain
{
    public class SaveGameDto
    {
        public int CurrentPlayerNo { get; set; }
        public List<List<BoardSquareState>>? Board { get; set; }
    }
}