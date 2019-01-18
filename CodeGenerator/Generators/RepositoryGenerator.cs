using System;
using System.Collections.Generic;
using CodeGenerator.Settings;

namespace CodeGenerator.Generators
{
    //change that
    public class RepositoryGenerator : Generator
    {
        public RepositoryGenerator() : base()
        { }

        public override void Generate()
        {
            _generatorSettings.ReadSettings("repository");

            foreach (var model in _generatorSettings.Models)
            {
                CreateClassFile(
                    model.Settings[SettingsKeys.Name], "cs",
                    GenereteRepository(model));
            }
        }


        private string GenereteRepository(SettingsModel model)
        {
            var methods = CreateMethods(GenerateMethod(model.Settings));

            string className = GenerateName(model.Settings);

            string data = string.Empty;

            data += GenerateUsing(model.Settings) + "\n";
            data += GenerateNamespace(model.Settings) + "{\n";


            data += "\t" + "public interface I" + className + "\n\t{\n";

            foreach (var signature in methods.signature)
            {
                data += "\t\t" + "Task<ResponseWrapper" + signature + ";\n";
            }
            data += "\t}\n\n";


            data += "\t" + "public class " + className + GenerateInherit(model.Settings) 
                + ", I" + className + "\n\t{ \n";

            var ctor = GenerateCtor(model.Settings);
            if (!string.IsNullOrEmpty(ctor))
            {
                data += "\t\t" + ctor + " { }\n\n";
            }


            for (int i = 0; i < methods.signature.Count; i++)
            {
                data += "\t\t" + "public Task<ResponseWrapper" 
                    + methods.signature[i] + "\n\t\t{\n";

                data += "\t\t\t" + "return client.Execute"
                    + methods.body[i] + ";\n\t\t}\n\n";
            }

            data += "\t}\n}\n";

            return data;
        }


        private (List<string> signature, List<string> body) CreateMethods(List<string> data) {
            var signature = new List<string>();
            var body = new List<string>();

            foreach (var str in data)
            {
                var splited = str.Split(';');
                signature.Add(splited[0].TrimStart().TrimEnd());
                body.Add(splited[1].TrimStart().TrimEnd());
            }

            return (signature, body);
        }
    }
}
