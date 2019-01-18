using System;
using CodeGenerator.FileWorker;
using CodeGenerator.Generators;

namespace CodeGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            // improvments 
            // write own path
            // create deserializer class for setting  like in JsonReader
            // remove path from Generators
            // ControllerGenerator - set "controller" file name

            // create autofac =+++
            Console.WriteLine("Current path: " + new PathDefinder().CurrentPath);


            var model = new ModelGenerator();
            model.Generate();

            var repository = new ControllerGenerator();
            repository.Generate();

            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }
    }
}
