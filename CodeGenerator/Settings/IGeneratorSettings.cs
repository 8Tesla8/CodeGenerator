using System;
using System.Collections.Generic;

namespace CodeGenerator.Settings
{
    public interface IGeneratorSettings
    {
        List<SettingsModel> Models { get; }

        void ReadSettings(string fileName);
    }
}
