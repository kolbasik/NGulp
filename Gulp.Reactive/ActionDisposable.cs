using System;

namespace NGulp.Reactive
{
    public sealed class ActionDisposable : IDisposable
    {
        private readonly Action dispose;

        public ActionDisposable(Action dispose)
        {
            this.dispose = dispose;
        }

        public void Dispose()
        {
            dispose?.Invoke();
        }
    }
}