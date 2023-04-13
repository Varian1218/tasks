using System;
using System.Runtime.CompilerServices;

namespace Tasks
{
    public class AsyncMethodBuilder
    {
        private readonly Awaiter _awaiter = new();
        public static AsyncMethodBuilder Create() => new();
        public ITask Task => new AwaiterTask(_awaiter);

        public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : INotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            throw new NotImplementedException(nameof(AwaitOnCompleted));
        }

        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : ICriticalNotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            throw new NotImplementedException(nameof(AwaitUnsafeOnCompleted));
        }

        public void SetException(Exception exception)
        {
            throw exception;
        }

        public void SetResult()
        {
            _awaiter.Complete();
        }

        public void SetStateMachine(IAsyncStateMachine stateMachine)
        {
            throw new NotImplementedException(nameof(SetStateMachine));
        }

        public void Start<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : IAsyncStateMachine
        {
            throw new NotImplementedException(nameof(Start));
        }
    }
}