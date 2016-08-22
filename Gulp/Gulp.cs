using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Text.RegularExpressions;
using NTask = System.Threading.Tasks.Task;

namespace NGulp
{
    public sealed class Gulp
    {
        private readonly Dictionary<string, GulpTask> tasks;

        public Gulp()
        {
            tasks = new Dictionary<string, GulpTask>(StringComparer.OrdinalIgnoreCase);
        }

        public void Task(string taskName, string[] taskDeps, Func<IExecutable> taskFactory = null)
        {
            tasks.Add(taskName, new GulpTask(taskName, taskDeps, taskFactory));
        }

        public async NTask StartAsync(params string[] args)
        {
            var taskNames = args.ToList();
            if (taskNames.Count == 0) taskNames.Add(@"default");
            foreach (var taskName in taskNames)
            {
                await RunAsync(taskName).ConfigureAwait(false);
            }
        }

        public async NTask RunAsync(string taskName)
        {
            if (taskName == null) throw new ArgumentNullException(nameof(taskName));

            await NTask.Delay(0).ConfigureAwait(false);

            GulpTask task;
            if (tasks.TryGetValue(taskName, out task))
            {
                await NTask.WhenAll(task.Deps.Select(RunAsync)).ConfigureAwait(false);
                var executable = task.Factory?.Invoke();
                if (executable != null)
                {
                    await executable.ExecuteAsync().ConfigureAwait(false);
                }
            }
        }

        public Sink<GulpFile> Src(string dirPath, string pattern = @".*", bool read = true,
            SearchOption option = SearchOption.AllDirectories)
        {
            var search = new Regex(pattern,
                RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase |
                RegexOptions.CultureInvariant);
            return Pipeline.Src<GulpFile>(source =>
            {
                if (Directory.Exists(dirPath))
                {
                    var dirUri = new Uri(Path.GetFullPath(dirPath) + Path.DirectorySeparatorChar);
                    foreach (var filePath in Directory.EnumerateFiles(dirPath, @"*.*", option))
                    {
                        var fullPath = Path.GetFullPath(filePath);
                        if (search.IsMatch(fullPath))
                        {
                            var path = dirUri.MakeRelativeUri(new Uri(fullPath))
                                .ToString()
                                .Replace('/', Path.DirectorySeparatorChar);
                            source.OnNext(new GulpFile(path, fullPath, read ? File.OpenRead(fullPath) : Stream.Null));
                        }
                    }
                }
                source.OnCompleted();
            });
        }

        public Func<IObservable<GulpFile>, IObservable<GulpFile>> Dest(string dirPath)
        {
            return source =>
            {
                Directory.CreateDirectory(dirPath);
                return source.Do(file =>
                {
                    using (var dest = File.OpenWrite(Path.Combine(dirPath, file.Path)))
                    {
                        file.Stream.Position = 0;
                        file.Stream.CopyTo(dest);
                    }
                });
            };
        }
    }
}