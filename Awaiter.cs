using System;

namespace Tasks
{
    public class Awaiter : IAwaiter
    {
        private Action _continuation;
        public bool IsCompleted { get; private set; }

        public Awaiter()
        {
        }

        public Awaiter(bool completed)
        {
            IsCompleted = completed;
        }

        public void Clear()
        {
            _continuation = null;
            IsCompleted = false;
        }

        public void Complete()
        {
            IsCompleted = true;
            _continuation?.Invoke();
        }

        public void GetResult()
        {
        }

        public void OnCompleted(Action continuation)
        {
            _continuation = continuation;
        }
    }

    public class Awaiter<T> : IAwaiter<T>
    {
        private Action _continuation;
        private T _result;

        public Awaiter()
        {
        }

        public Awaiter(bool completed, T result)
        {
            IsCompleted = completed;
            _result = result;
        }

        public bool IsCompleted { get; private set; }

        public void Clear()
        {
            _continuation = null;
            IsCompleted = false;
        }

        public void Complete(T t)
        {
            _result = t;
            IsCompleted = true;
            _continuation?.Invoke();
        }

        public T GetResult()
        {
            return _result;
        }

        public void OnCompleted(Action continuation)
        {
            _continuation = continuation;
        }
    }
}