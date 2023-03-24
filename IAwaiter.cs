using System.Runtime.CompilerServices;

namespace Tasks
{
    public interface IAwaiter : INotifyCompletion
    {
        bool IsCompleted { get; }
        void Complete();
        void GetResult();
    }

    public interface IAwaiter<T> : INotifyCompletion
    {
        bool IsCompleted { get; }
        void Complete(T t);
        T GetResult();
    }
}