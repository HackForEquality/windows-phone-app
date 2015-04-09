using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YesEquality.Models.Messages
{
    public class CommandMessage
    {
        public CommandMessage(Commands command)
        {
            Command = command;
        }

        public CommandMessage()
        {
        }

        public Commands Command { get; set; }
    }

    public enum Commands
    {
        ClearCheckedItems,
    }
}
