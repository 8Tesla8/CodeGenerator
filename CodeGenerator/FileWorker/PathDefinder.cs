using System;
namespace CodeGenerator.FileWorker
{
    public class PathDefinder
    {
        public readonly string CurrentPath;

        public PathDefinder()
        {
            CurrentPath = System.IO.Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory);
        }
    }
}
