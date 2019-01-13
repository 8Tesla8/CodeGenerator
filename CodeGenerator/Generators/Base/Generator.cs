using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CodeGenerator.FileWorker;
using CodeGenerator.FileWorker.Reader;
using CodeGenerator.Settings;

namespace CodeGenerator.Generators
{
    public abstract class Generator : IGenerator
    {
        protected char _separatorStr = ' ';
        protected char _separatorMethod = '/';

        protected readonly IReader _reader;
        protected readonly string _currentPath;

        protected IGeneratorSettings _generatorSettings;

        protected Generator()
        {
            _currentPath = new PathDefinder().CurrentPath;
            _reader = new FileReader();
            _generatorSettings = new GeneratorSettings();
        }

        //create interface todo
        protected void CreateClassFile(string fileName, string data)
        {
            string directoryPath = _currentPath + "/Files/";

            string path = directoryPath + fileName + ".cs";

            if (!Directory.Exists(directoryPath))
            {
                DirectoryInfo di = Directory.CreateDirectory(directoryPath);
            }

            if (File.Exists(path))
                File.Delete(path);

            _reader.Write(path, data);
        }

        #region Generation

        public abstract void Generate();


        protected string GenerateName(Dictionary<string, string> settings)
        {

            if (settings.ContainsKey(SettingsKeys.Name))
                return settings[SettingsKeys.Name];

            return "MyClass";
        }

        protected string GenerateNamespace(Dictionary<string, string> settings)
        {

            if (!settings.ContainsKey(SettingsKeys.Namespace))
                return "namespace Default \n";

            return "namespace " + settings[SettingsKeys.Namespace].Trim() + '\n';
        }

        protected string GenerateUsing(Dictionary<string, string> settings)
        {

            if (!settings.ContainsKey(SettingsKeys.Using))
                return string.Empty;

            var usings = settings[SettingsKeys.Using].Split(_separatorStr);

            var result = string.Empty;
            foreach (var str in usings)
            {
                if (!string.IsNullOrWhiteSpace(str))
                    result += "using " + str + ";\n";
            }

            return result;
        }

        protected string GenerateInherit(Dictionary<string, string> settings)
        {
            if (!settings.ContainsKey(SettingsKeys.Inherit))
                return string.Empty;

            return " : " + settings[SettingsKeys.Inherit];
        }


        protected string GenerateCtor(Dictionary<string, string> settings)
        {
            var ctor = string.Empty;

            // ctor
            if (!settings.ContainsKey(SettingsKeys.Ctor))
                return ctor;

            // "ctor": ""
            if (string.IsNullOrEmpty(settings[SettingsKeys.Ctor]))
                ctor = "public " + settings[SettingsKeys.Name] + "()";


            // "ctor": "public(int i, double d)"
            if (settings[SettingsKeys.Ctor].Contains('('))
            {
                var strs = settings[SettingsKeys.Ctor].Split('(');
                ctor = strs[0] + " " + settings[SettingsKeys.Name] + "(" + strs[1];
            }

            // "ctor": "int i, double d"
            else
            {
                ctor = "public " + settings[SettingsKeys.Name] + "("
                    + settings[SettingsKeys.Ctor] + ")";
            }


            //base-ctor
            var baseCtor = string.Empty;

            if (!settings.ContainsKey(SettingsKeys.BaseCtor))
                return ctor;

            // "base-ctor": ""
            if (string.IsNullOrEmpty(settings[SettingsKeys.BaseCtor]))
            {
                baseCtor = " : base()";
            }

            // "base-ctor": "int i, double d"
            else
            {
                baseCtor = " : base(" + settings[SettingsKeys.BaseCtor] + ")";
            }

            return ctor + baseCtor;
        }


        protected List<string> GenereteProperty(Dictionary<string, string> settings)
        {
            return SplitSettingValue(SettingsKeys.Property, settings, _separatorStr);
        }

        protected List<string> GenerateMethod(Dictionary<string, string> settings)
        {
            var result = new List<string>();
            foreach (var method in SplitSettingValue(SettingsKeys.Method, settings, _separatorMethod))
            {
                result.Add(method.TrimStart().TrimEnd());
            }

            return result;
        }

        #endregion

        private List<string> SplitSettingValue(string key, Dictionary<string, string> settings, char separator)
        {

            if (!settings.ContainsKey(key))
                return new List<string>();

            var result = new List<string>();
            foreach (var str in settings[key].Split(separator))
            {
                if (!string.IsNullOrWhiteSpace(str))
                    result.Add(str);
            }
            return result;
        }
    }
}
