using System;
using System.Collections.Generic;

namespace NGulp.Reactive
{
    public sealed class Subject<T> : IObserver<T>, IObservable<T>
    {
        private readonly List<IObserver<T>> observers;

        public Subject()
        {
            observers = new List<IObserver<T>>();
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            observers.Add(observer);
            return new ActionDisposable(() => observers.Remove(observer));
        }

        public void OnNext(T value)
        {
            foreach (var observer in observers.ToArray())
            {
                observer.OnNext(value);
            }
        }

        public void OnError(Exception error)
        {
            foreach (var observer in observers.ToArray())
            {
                observer.OnError(error);
            }
        }

        public void OnCompleted()
        {
            foreach (var observer in observers.ToArray())
            {
                observer.OnCompleted();
            }
        }
    }
}