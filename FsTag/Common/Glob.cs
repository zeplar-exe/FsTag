namespace FsTag.Common;

public static class Glob
{
    public static bool IsMatch(string glob, string text)
    {
        var pos = 0;

        while (glob.Length != pos)
        {
            switch (glob[pos])
            {
                case '?':
                {
                    break;
                }
                case '*':
                {
                    for (var i = text.Length; i >= pos; i--)
                    {
                        if (IsMatch(text.Substring(i), glob.Substring(pos + 1)))
                        {
                            return true;
                        }
                    }
                    return false;
                }
                default:
                {
                    if (text.Length == pos || char.ToUpper(glob[pos]) != char.ToUpper(text[pos]))
                    {
                        return false;
                    }
                    
                    break;
                }
            }

            pos++;
        }

        return text.Length == pos;
    } // https://stackoverflow.com/a/8094334/16324801
}