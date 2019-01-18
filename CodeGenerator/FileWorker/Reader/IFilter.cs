using System;
using System.Collections.Generic;

namespace CodeGenerator.FileWorker.Reader
{
    public interface IFilter
    {
        List<string> Filt(string[] data);
    }


    public class WordsFilter : IFilter
    {

        public List<string> Filt(string[] data)
        {
            var list = new List<string>();

            foreach (var item in data)
            {
                if (!item.Contains("//"))
                { 
                    list.Add(item);
                }
            }

            return list;
        }
    }
}
