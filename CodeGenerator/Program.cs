using System;
using CodeGenerator.FileWorker;
using CodeGenerator.Generators;

namespace CodeGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            // create autofac =+++
            Console.WriteLine("Current path: " + new PathDefinder().CurrentPath);


            var model = new ModelGenerator();
            model.Generate();

            var repository = new RepositoryGenerator();
            repository.Generate();

            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }
    }
}
