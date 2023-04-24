using System;

namespace Tasks
{
    public struct ActionTask : ITask
    {
        private readonly IAwaiter _awaiter;

        public ActionTask(ref Action action)
        {
            _awaiter = new Awaiter();
            action += _awaiter.Complete;
        }

        public IAwaiter GetAwaiter()
        {
            return _awaiter;
        }
    }
}