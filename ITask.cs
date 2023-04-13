using System.Runtime.CompilerServices;

namespace Tasks
{
    [AsyncMethodBuilder(typeof(AsyncMethodBuilder))]
    public interface ITask 
    {
        IAwaiter GetAwaiter();
    }

    [AsyncMethodBuilder(typeof(AsyncMethodBuilder<>))]
    public interface ITask<T>
    {
        IAwaiter<T> GetAwaiter();
    }
}