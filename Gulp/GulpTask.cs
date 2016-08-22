using System;

namespace NGulp
{
    public sealed class GulpTask
    {
        public GulpTask(string name, string[] deps, Func<IExecutable> factory)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (deps == null) throw new ArgumentNullException(nameof(deps));

            Name = name;
            Deps = deps;
            Factory = factory;
        }

        public string Name { get; }
        public string[] Deps { get; }
        public Func<IExecutable> Factory { get; }
    }
}