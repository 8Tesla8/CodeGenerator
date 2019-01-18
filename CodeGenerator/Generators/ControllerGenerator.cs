using CodeGenerator.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenerator.Generators
{
    public class ControllerGenerator : Generator
    {
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
            var methods = GenerateMethod(model.Settings);

            var className = GenerateName(model.Settings);

            string data = string.Empty;

            data += GenerateUsing(model.Settings) + "\n";
            data += GenerateNamespace(model.Settings) + "{\n";


            data += "\t" + "public class " + className
                      + GenerateInherit(model.Settings) + "\n\t{ \n";


            var repositoryType = model.Settings[SettingsKeys.RepositoryTyle];
            data += "\t\t" + "GenericRepository<" + repositoryType
                + "> repository; \n\n";

            //ctor
            data += "\t\t" + "public " + className + "(GenericRepository<" 
                + repositoryType + "> repository)\n";
            data += "\t\t{\n" + "\t\t\t" + "this.repository = repository;\n\t\t} \n\n";


            //methods
            foreach (var method in methods)
            {
                data += "\t\t[Http" + method + "] \n";
                data += "\t\t" + "public async Task<IActionResult> " + method + "\n\t\t{ \n";
                data += "\t\t\t" + "try\n" + "\t\t\t{ \n";
                data += "\t\t\t\t" + "var res = await repository." + method + ";\t\t\t{ \n";
                 data += "\t\t\t\t" + "return GetResponse<" + model.Settings[SettingsKeys.ModelsDTO]
                    + ">(res); \n\t\t\t} \n";

                data += "\t\t\t" + "catch(Exception ex) {\n";
                data += "\t\t\t\t" + "return BadRequest(ex.Message); \n";
                data += "\t\t\t} \n\t\t}\n";
            }
            data += "\t} \n} \n";

            return data;
        }
    }
}
