using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Tasks
{
    [AsyncMethodBuilder(typeof(AsyncMethodBuilder))]
    public interface ITask 
    {
        IAwaiter GetAwaiter();
    }

    public interface ITask<T>
    {
        IAwaiter<T> GetAwaiter();
    }
}