using System;

namespace NGulp.Reactive
{
    public sealed class Observer<T> : IObserver<T>
    {
        private readonly Action onCompleted;
        private readonly Action<Exception> onError;
        private readonly Action<T> onNext;

        public Observer(Action<T> onNext = null, Action<Exception> onError = null, Action onCompleted = null)
        {
            this.onNext = onNext;
            this.onError = onError;
            this.onCompleted = onCompleted;
        }

        public void OnNext(T value)
        {
            onNext?.Invoke(value);
        }

        public void OnError(Exception error)
        {
            onError?.Invoke(error);
        }

        public void OnCompleted()
        {
            onCompleted?.Invoke();
        }
    }
}