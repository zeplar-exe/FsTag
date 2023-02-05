namespace FsTag.Helpers;

public static class ExceptionWrapper
{
    public static int TryExecute(Action method)
    {
        try
        {
            method.Invoke();

            return 0;
        }
        catch (Exception e)
        {
            WriteFormatter.Error(e.Message);

            return 1;
        }
    }
    
    public static int TryExecute(Func<int> method)
    {
        try
        {
            return method.Invoke();
        }
        catch (Exception e)
        {
            WriteFormatter.Error(e.Message);

            return 1;
        }
    }
}