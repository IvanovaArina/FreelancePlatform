using FreelancePlatform.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancePlatform.Application.DesignPatterns.Command
{
    public class CommandInvoker
    {
        private readonly List<string> _commandLog = new List<string>();

        public void ExecuteCommand(ICommand command)
        {
            command.Execute();
            _commandLog.Add($"Command executed at {DateTime.UtcNow}: {command.GetType().Name}");
        }

        public IReadOnlyList<string> GetCommandLog() => _commandLog.AsReadOnly();
    }
}
