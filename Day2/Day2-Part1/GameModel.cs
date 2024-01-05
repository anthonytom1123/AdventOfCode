using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day2
{
    public class GameModel
    {
        public int Id { get; set; }
        public int[,] Rounds { get; set; }
        public bool IsPossible { get; set; }
        public int ErrorCode {  get; set; }
        public string ErrorMessage {  get; set; } 
    }
}
