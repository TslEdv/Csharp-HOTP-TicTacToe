using System;
using System.Collections.Generic;
using System.Text.Json;

namespace TicTacToeBrain
{
    public class TicTacToeBrain
    {
        public int _currentPlayerNo;
        public BoardSquareState[,] Board { get; set; }

        public TicTacToeBrain()
        {
            Random random = new();
            _currentPlayerNo = random.Next(1, 3);
            Board = new BoardSquareState[3, 3];
            for (var x = 0; x < 3; x++)
            {
                for (var y = 0; y < 3; y++)
                {
                    Board[x,y] = new BoardSquareState
                    {
                        IsO = false,
                        IsX = false
                    };
                }
            }
        }

        public string GetBrainJson()
        {
            var jsonOptions = new JsonSerializerOptions()
            {
                WriteIndented = true
            };
            var dto = new SaveGameDto
            {
                CurrentPlayerNo = _currentPlayerNo,
                Board = new List<List<BoardSquareState>>()
            };
            for (var x = 0; x < 3; x++)
            {
                var xValues = new List<BoardSquareState>();
                for (var y = 0; y < 3; y++)
                {
                    xValues.Add(Board[x,y]);
                }
                dto.Board.Add(xValues);
            }

            var jsonStr = JsonSerializer.Serialize(dto, jsonOptions);
            return jsonStr;
        }

        public void RestoreBrainFromJson(string json)
        {
            var restore = JsonSerializer.Deserialize<SaveGameDto>(json);
            _currentPlayerNo = restore!.CurrentPlayerNo;
            Board = new BoardSquareState[3, 3];
            var x = 0;
            var y = 0;
            foreach (var variable in restore.Board!)
            {
                foreach (var squareState in variable)
                {
                    Board[x, y] = squareState;
                    y++;
                }

                y = 0;
                x++;
            }
        }

        public int GetCurrentPlayer()
        {
            return _currentPlayerNo;
        }

        public string GameFinish()
        {
            var xCounter = 0;
            var oCounter = 0;
            //check horizontal lines
            for (var i = 0; i < 3; i++)
            {
                for (var y = 0; y < 3; y++)
                {
                    if (Board[i, y].IsX)
                    {
                        xCounter++;
                    }
                    if (Board[i, y].IsO)
                    {
                        oCounter++;
                    }
                }

                if (xCounter == 3)
                {
                    return "Player 1 wins!";
                }

                if (oCounter == 3)
                {
                    return "Player 2 wins!";
                }
                xCounter = 0;
                oCounter = 0;
            }
            //check vertical lines
            for (var i = 0; i < 3; i++)
            {
                for (var y = 0; y < 3; y++)
                {
                    if (Board[y, i].IsX)
                    {
                        xCounter++;
                    }
                    if (Board[y, i].IsO)
                    {
                        oCounter++;
                    }
                }

                if (xCounter == 3)
                {
                    return "Player 1 wins!";
                }

                if (oCounter == 3)
                {
                    return "Player 2 wins!";
                }
                xCounter = 0;
                oCounter = 0;
            }
            
            //check diagonals
            if (Board[0, 0].IsX && Board[1, 1].IsX && Board[2, 2].IsX || Board[0,2].IsX && Board[1,1].IsX && Board[2,0].IsX)
            {
                return "Player 1 wins!";
            }
            if (Board[0, 0].IsO && Board[1, 1].IsO && Board[2, 2].IsO || Board[0,2].IsO && Board[1,1].IsO && Board[2,0].IsO)
            {
                return "Player 2 wins!";
            }

            var counter = 0;
            for (var i = 0; i < 3; i++)
            {
                for (var y = 0; y < 3; y++)
                {
                    if (Board[i, y].IsX || Board[i, y].IsO)
                    {
                        counter++;
                    }
                }
            }

            return counter == 9 ? "Draw!" : "No";
        }
        
        
    }
}