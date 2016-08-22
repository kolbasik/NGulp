using System;
using System.Threading.Tasks;

namespace NGulp
{
    public static class GulpExtensions
    {
        public static void Task(this Gulp gulp, string taskName, Func<IExecutable> taskFactory = null)
        {
            gulp.Task(taskName, new string[0], taskFactory);
        }

        public static void Start(this Gulp gulp, params string[] args)
        {
            gulp.StartAsync(args).GetAwaiter().GetResult();
        }

        public static void Run(this Gulp gulp, string taskName)
        {
            gulp.RunAsync(taskName).GetAwaiter().GetResult();
        }

        public static void Execute(this IExecutable executable)
        {
            executable.ExecuteAsync().GetAwaiter().GetResult();
        }
    }
}