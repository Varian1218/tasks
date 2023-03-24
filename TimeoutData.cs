using System;

namespace Tasks
{
    public struct TimeoutData<T>
    {
        public T Result;
        public TimeSpan Time;
    }
}