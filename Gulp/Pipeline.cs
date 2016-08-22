using System;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace NGulp
{
    public static class Pipeline
    {
        public static Sink<TInput> Src<TInput>(Action<IObserver<TInput>> fulfill)
        {
            var subject = new Subject<TInput>();
            return new Sink<TInput>(() => fulfill(subject), subject);
        }
    }

    public interface IExecutable
    {
        Task ExecuteAsync();
    }

    public class Sink<TInput> : IExecutable
    {
        private readonly IObservable<TInput> source;
        private readonly Action start;

        public Sink(Action start, IObservable<TInput> source)
        {
            if (start == null) throw new ArgumentNullException(nameof(start));
            if (source == null) throw new ArgumentNullException(nameof(source));
            this.start = start;
            this.source = source;
        }

        public Task ExecuteAsync()
        {
            var tcs = new TaskCompletionSource<bool>();
            source.Subscribe(x => { }, ex => tcs.TrySetException(ex), () => tcs.TrySetResult(true));
            start();
            return tcs.Task;
        }

        public Sink<TOutput> Pipe<TOutput>(Func<IObservable<TInput>, IObservable<TOutput>> pipe)
        {
            return new Sink<TOutput>(start, pipe(source));
        }
    }
}