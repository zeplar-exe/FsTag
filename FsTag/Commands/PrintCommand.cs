using CommandDotNet;

using FsTag.Data;
using FsTag.Data.Models;
using FsTag.Helpers;
using FsTag.Resources;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FsTag;

public partial class Program
{
    [Command("print", Description = nameof(Descriptions.PrintCommand))]
    [Subcommand]
    public class PrintCommand
    {
        [DefaultCommand]
        public int Execute(
            [Operand("keys", Description = nameof(Descriptions.PrintKeysOperand))] 
            string[]? keys = null)
        {
            if (keys == null)
            {
                WriteFormatter.NewLine();
                
                WriteFormatter.Plain(CommonOutput.ValidArgumentList);
                
                WriteFormatter.NewLine();
                
                foreach (var key in App.PrintDataProvider.EnumerateData())
                {
                    WriteFormatter.Plain($"{key.Key} - {key.Description}");
                }
                
                WriteFormatter.NewLine();
                
                return 0;
            }
            
            foreach (var key in keys)
            {
                if (App.PrintDataProvider.Get(key) is {} data) // not null
                {
                    data.Action.Invoke();

                    return 0;
                }
                
                WriteFormatter.Error(string.Format(CommandOutput.PrintKeyNotFound, key));

                return 1;
            }

            return 0;
        }
    }
}