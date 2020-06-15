using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class DataCache
{
    
    public static long LastReset = 0;
    public static long Now { get => stopWatch.Elapsed.Ticks; }
    private static Stopwatch stopWatch = Stopwatch.StartNew();

    public static void Reset()
    {
        LastReset = Now;
    }
}

public class DataCache<S,T> : DataCache
{
    public delegate T UpdateDelegate(S parameter);

    private UpdateDelegate _delegate;

    T cachedValue;
    private long updateTime = -1;

    public DataCache(UpdateDelegate updateDelegate)
    {
        _delegate = updateDelegate;
    }

    public T getValue(S parameter)
    {
        if(updateTime <= LastReset)
        {
            updateTime = DataCache.Now;
            cachedValue = _delegate(parameter);
        }
        return cachedValue;
    }
}

