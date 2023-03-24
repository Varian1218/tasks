namespace Tasks
{
    public readonly struct AwaiterTask : ITask
    {
        private readonly IAwaiter _awaiter;

        public AwaiterTask(IAwaiter awaiter)
        {
            _awaiter = awaiter;
        }

        public IAwaiter GetAwaiter()
        {
            return _awaiter;
        }
    }
    
    public readonly struct AwaiterTask<T> : ITask<T>
    {
        private readonly IAwaiter<T> _awaiter;

        public AwaiterTask(IAwaiter<T> awaiter)
        {
            _awaiter = awaiter;
        }

        public IAwaiter<T> GetAwaiter()
        {
            return _awaiter;
        }
    }
}