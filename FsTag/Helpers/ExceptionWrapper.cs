namespace FsTag;

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
    
    public static bool TryExecute<TResult>(Func<TResult> method, out TResult? result)
    {
        result = default;
        
        try
        {
            result = method.Invoke();

            return true;
        }
        catch (Exception e)
        {
            WriteFormatter.Error(e.Message);

            return false;
        }
    }
}