namespace Tasks
{
    public interface ITask
    {
        IAwaiter GetAwaiter();
    }

    public interface ITask<T>
    {
        IAwaiter<T> GetAwaiter();
    }
}