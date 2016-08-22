#r "NGulp.dll"
#r "NGulp.V8.dll"
#r "ClearScript.dll"
#r "System.Reactive.Core.dll"
#r "System.Reactive.Interfaces.dll"
#r "System.Reactive.Linq.dll"

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reactive.Linq;
using NGulp;
using NGulp.V8;

public sealed class Program
{
    public static void Start(string[] args)
    {
        var gulp = new Gulp();

        gulp.Task(@"clean",
            () => gulp.Src(@".\dest", read: false)
                .Pipe(Using())
                .Pipe(Rimraf()));

        gulp.Task(@"javascript",
            () => gulp.Src(@".\src", @"\.coffee")
                .Pipe(Using())
                .Pipe(Coffee())
                .Pipe(Concat(@"all.js"))
                .Pipe(Uglify())
                .Pipe(gulp.Dest(@".\dest")));

        gulp.Task(@"default", new[] { @"javascript" });

        gulp.Start(args);
    }

    public static Func<IObservable<GulpFile>, IObservable<GulpFile>> Using()
    {
        return source => source.Do(file => Console.WriteLine($"Using file: {file.FullPath}"));
    }

    public static Func<IObservable<GulpFile>, IObservable<GulpFile>> Rimraf()
    {
        return source => source.Do(file => File.Delete(file.FullPath));
    }

    public static Func<IObservable<GulpFile>, IObservable<GulpFile>> Concat(string fileName)
    {
        Func<IEnumerable<Stream>, Stream> concat = inputs =>
        {
            var output = new MemoryStream();
            foreach (var input in inputs)
            {
                input.CopyTo(output);
            }
            return output;
        };

        return
            source => source.ToList()
                .Select(files => new GulpFile(fileName, fileName, concat(files.Select(x => x.Stream))));
    }

    public static Func<IObservable<GulpFile>, IObservable<GulpFile>> Coffee()
    {
        return source => from file in source
                         let path = Path.ChangeExtension(file.Path, @".js")
                         let stream = CoffeeScript.Compile(file.Stream)
                         select new GulpFile(path, path, stream);
    }

    public static Func<IObservable<GulpFile>, IObservable<GulpFile>> Uglify()
    {
        return source => from file in source
                         let stream = UglifyScript.Compile(file.Stream)
                         select new GulpFile(file.Path, file.FullPath, stream);
    }
}

Program.Start(Env.ScriptArgs.ToArray());