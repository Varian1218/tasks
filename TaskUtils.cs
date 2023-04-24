using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tasks
{
    public static class TaskUtils
    {
        private static ITask _completedTask;
        public static ITask CompletedTask => _completedTask ??= new AwaiterTask(new Awaiter(true));

        public static async void Continue(this ITask task, Action action)
        {
            await task;
            action();
        }

        public static ITask<T> FromResult<T>(T t)
        {
            return new AwaiterTask<T>(new Awaiter<T>(true, t));
        }

        public static ITask<T> Timeout<T>(this ITask<T> task, TimeoutData<T> data)
        {
            return WaitAny(
                task,
                Wait(Wait(data.Time), data.Result)
            );
        }

        public static ITask<T> Timeout<T>(this ITask<T> task, TimeoutData<T> data, Action<Func<TimeSpan, bool>> step)
        {
            return WaitAny(
                task,
                Wait(Wait(data.Time, step), data.Result)
            );
        }

        public static ITask Until(Func<bool> predicate, Action<Func<bool>> step)
        {
            var awaiter = new Awaiter();
            step(() =>
            {
                if (!predicate()) return false;
                awaiter.Complete();
                return true;
            });
            return new AwaiterTask(awaiter);
        }

        public static ITask Wait(ref Action action)
        {
            var awaiter = new Awaiter();
            action += awaiter.Complete;
            return new AwaiterTask(awaiter);
        }

        public static ITask Wait(Func<float, bool> func, Action<Func<float, bool>> step)
        {
            var awaiter = new Awaiter();
            step(dt =>
            {
                if (!func(dt)) return false;
                awaiter.Complete();
                return true;
            });
            return new AwaiterTask(awaiter);
        }

        public static ITask Wait(IEnumerator enumerator, Action<IEnumerator> step)
        {
            var awaiter = new Awaiter();
            step(Wrap(awaiter.Complete, enumerator));
            return new AwaiterTask(awaiter);
        }

        public static ITask<T> Wait<T>(IEnumerator<T> enumerator, Action<IEnumerator> step)
        {
            var awaiter = new Awaiter<T>();
            step(Wrap(awaiter.Complete, enumerator));
            return new AwaiterTask<T>(awaiter);
        }

        public static ITask<T> Wait<T>(ITask task, T v)
        {
            var awaiter = new Awaiter<T>();
            task.Continue(() => awaiter.Complete(v));
            return new AwaiterTask<T>(awaiter);
        }

        public static ITask<bool> Wait<T>(ITask<T> task, T v)
        {
            var awaiter = new Awaiter<bool>();
            Wait(awaiter.Complete, task, v);
            return new AwaiterTask<bool>(awaiter);
        }

        public static ITask Wait(TimeSpan ts)
        {
            return WrapTask(Task.Delay(ts));
        }

        public static ITask Wait(TimeSpan ts, Action<Func<TimeSpan, bool>> step)
        {
            var awaiter = new Awaiter();
            step(dt =>
            {
                ts -= dt;
                if (ts > TimeSpan.Zero) return false;
                awaiter.Complete();
                return true;
            });
            return new AwaiterTask(awaiter);
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

        public static ITask While(Func<bool> predicate, Action<Func<bool>> step)
        {
            var awaiter = new Awaiter();
            step(() =>
            {
                if (predicate()) return false;
                awaiter.Complete();
                return true;
            });
            return new AwaiterTask(awaiter);
        }

        public static ITask WrapTask(Task task)
        {
            var awaiter = new Awaiter();
            WrapTask(awaiter.Complete, task);
            return new AwaiterTask(awaiter);
        }

        public static ITask<T> WrapTask<T>(Task<T> task)
        {
            var awaiter = new Awaiter<T>();
            WrapTask(awaiter.Complete, task);
            return new AwaiterTask<T>(awaiter);
        }

        public static async void WrapTask(Action call, Task task)
        {
            await task;
            call();
        }

        public static async void WrapTask<T>(Action<T> call, Task<T> task)
        {
            call(await task);
        }

        public static IEnumerator Wrap(Action complete, IEnumerator enumerator)
        {
            yield return enumerator;
            complete();
        }

        public static IEnumerator Wrap<T>(Action<T> complete, IEnumerator<T> enumerator)
        {
            yield return enumerator;
            complete(enumerator.Current);
        }
    }
}