using CommandDotNet.Prompts;

namespace FsTag;

public static class Confirmation
{
    public static bool Prompt(string text)
    {
        if (Program.Quiet || Program.NoPrompt)
            return true;
        
        var prompt = new Prompter(Program.IConsole);
        
        if (prompt.TryPromptForValue(text, out var input, out _))
        {
            return input?.ToLower() is "y" or "yes";
        }

        return false;
    }
}