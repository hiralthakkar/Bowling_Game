using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bowling.Models
{
    public class Roll
    {
        public Roll(int pinsKnocked)
        {
            PinsKnocked = pinsKnocked;
        }
        public int PinsKnocked { get; set; }
    }
}