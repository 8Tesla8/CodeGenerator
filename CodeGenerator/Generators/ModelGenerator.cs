﻿using System;
using System.Collections.Generic;
using System.IO;
using CodeGenerator.Generators;
using CodeGenerator.Settings;

namespace CodeGenerator.Generators
{
    public class ModelGenerator : Generator
    {
        public ModelGenerator() : base()
        { }

        //todo generate ts model
        public override void Generate()
        {
            _generatorSettings.ReadSettings("model");

            foreach (var model in _generatorSettings.Models)
            {
                // .net models
                CreateClassFile(
                    model.Settings[SettingsKeys.Name], "cs", 
                    GenarateModel(model, true));


                model.Settings[SettingsKeys.Name] += "DTO";

                CreateClassFile(
                    model.Settings[SettingsKeys.Name], "cs",
                    GenarateModel(model, false));


                // TypeScript model
                var className = model.Settings[SettingsKeys.Name];

                model.Settings[SettingsKeys.Name] = char.ToLower(className[0]) + className.Substring(1);

                CreateClassFile(
                    model.Settings[SettingsKeys.Name], "ts",
                    GenarateModel(model));
            }
        }

        #region .net model

        private string GenarateModel(SettingsModel model, bool createJsonAttribute)
        {
            string data = string.Empty;

            if (createJsonAttribute)
                data += "using Newtonsoft.Json;\n";
            data += GenerateUsing(model.Settings) + "\n";
            

            data += GenerateNamespace(model.Settings) + "{\n";

            data += "\t" + "public class " + GenerateName(model.Settings) 
                + GenerateInherit(model.Settings) +  "\n\t{ \n";

            var ctor = GenerateCtor(model.Settings);
            if (!string.IsNullOrEmpty(ctor)) 
            {
                data += "\t\t" + ctor + " { }\n\n";
            }


            var names = CreatePropertyNames(GenereteProperty(model.Settings));

            for (int i = 0; i < names.properties.Count; i++)
            {
                if (createJsonAttribute) 
                {
                    data += "\t\t" + "[JsonProperty(\"" 
                        + names.attributes[i] + "\")] \n";
                }

                data += "\t\t" + "public string " 
                    + names.properties[i] + " { get; set; } \n";

                if (createJsonAttribute)
                    data += "\n";
            }

            data += "\t} \n}";

            return data;
        }


        private (List<string> properties, List<string> attributes) CreatePropertyNames(List<string> names)
        {
            var jsonAttributes = new List<string>();
            var properties = new List<string>();

            foreach (var property in names)
            {
                var prop = property.ToLower();

                jsonAttributes.Add(prop);


                string propName = string.Empty;
                foreach (var str in prop.Split('_'))
                    propName += char.ToUpper(str[0]) + str.Substring(1);

                properties.Add(propName);
            }

            return (properties, jsonAttributes);
        }

        #endregion


        #region TypeScript model

        private string GenarateModel(SettingsModel model) 
        {
            string data = string.Empty;

            data += "export class " + model.Settings[SettingsKeys.Name] + " {\n";


            var properties = new List<string>();
            foreach (var property in GenereteProperty(model.Settings))
            {
                var prop = property.ToLower().Split('_');

                properties.Add(prop[0] + 
                    char.ToUpper(prop[1][0]) // second [] take first letter
                    + prop[1].Substring(1));
            }

            foreach(var property in properties) 
            {
                data += "\tpublic " + property + "?: string = null;\n";
            }

            data += "}";

            return data;
        }

        #endregion
    }

}
