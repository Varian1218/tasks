using System;
using System.Threading.Tasks;

namespace Tasks
{
    public class TaskUtils
    {
        private static ITask _completedTask;
        public static ITask CompletedTask => _completedTask ??= new AwaiterTask(new Awaiter(true));

        public static ITask<T> FromResult<T>(T t)
        {
            return new AwaiterTask<T>(new Awaiter<T>(true, t));
        }

        public static ITask<T> Timeout<T>(TimeoutData<T> data, ITask<T> task)
        {
            async Task<T> Delay()
            {
                await Task.Delay(data.Time);
                return data.Result;
            }

            return WaitAny(
                task,
                WrapTask(Delay())
            );
        }

        public static ITask<bool> WaitTask<T>(ITask<T> task, T v)
        {
            var awaiter = new Awaiter<bool>();
            Wait(awaiter.Complete, task, v);
            return new AwaiterTask<bool>(awaiter);
        }

        public static async void Wait<T>(Action<bool> call, ITask<T> task, T v)
        {
            call((await task).Equals(v));
        }

        public static ITask WaitAny(params ITask[] tasks)
        {
            var awaiter = new Awaiter();
            foreach (var task in tasks)
            {
                var a = task.GetAwaiter();
                if (a.IsCompleted)
                {
                    awaiter.Complete();
                    break;
                }

                a.OnCompleted(() =>
                {
                    if (awaiter.IsCompleted) return;
                    awaiter.Complete();
                });
            }

            return new AwaiterTask(awaiter);
        }

        public static ITask<T> WaitAny<T>(params ITask<T>[] tasks)
        {
            var awaiter = new Awaiter<T>();
            foreach (var task in tasks)
            {
                var a = task.GetAwaiter();
                if (a.IsCompleted)
                {
                    awaiter.Complete(a.GetResult());
                    break;
                }

                a.OnCompleted(() =>
                {
                    if (awaiter.IsCompleted) return;
                    awaiter.Complete(a.GetResult());
                });
            }

            return new AwaiterTask<T>(awaiter);
        }

        public static ITask<T> WrapTask<T>(Task<T> task)
        {
            var awaiter = new Awaiter<T>();
            WrapTask(awaiter.Complete, task);
            return new AwaiterTask<T>(awaiter);
        }

        public static async void WrapTask<T>(Action<T> call, Task<T> task)
        {
            call(await task);
        }
    }
}