using System;
using System.Collections.Generic;
using System.IO;
using CodeGenerator.FileWorker;
using CodeGenerator.FileWorker.Reader;
using Newtonsoft.Json;

namespace CodeGenerator.Settings
{

    public class SettingsModel {
        public Dictionary<string, string> Settings { get; set; }
    }


    public class GeneratorSettings : IGeneratorSettings
    {
        protected readonly string _currentPath;
        protected readonly IReader _reader;
        protected readonly IFilter _filter;

        public List<SettingsModel> Models { get; private set; }

        public GeneratorSettings()
        {
            _currentPath = new PathDefinder().CurrentPath;
            _reader = new FileReader();
            _filter = new WordsFilter();

            Models = new List<SettingsModel>();
        }


        public virtual void ReadSettings(string fileName)
        {
            var pathToFile = _currentPath + '/' + fileName + ".txt";

            if (!File.Exists(pathToFile))
                return;

            var data = _filter.Filt(
                _reader.Read(pathToFile));


            for (int i = 0, j = -1; i < data.Count; i++)
            {
                if (string.IsNullOrEmpty(data[i]))
                    continue;
                    
                if (data[i].Contains('{'))
                {
                    var settingsRow = string.Empty;

                    settingsRow += data[i];

                    i++;

                    for (; i < data.Count; i++)
                    {
                        settingsRow += data[i] + ' '; 

                        if (data[i].Contains('}'))
                        {
                            break;
                        }
                    }

                    var setting = JsonConvert.DeserializeObject<Dictionary<string, string>>(settingsRow);

                    Models.Add(new SettingsModel
                    {
                        Settings = setting
                    });

                    j++;
                }
            }
        }
    }
}
