using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Utils;

namespace XBoxControlTesting
{
    public static class Config
    {
        public static IEnumerable<ListBoxStuff> LoadConfig()
        {
            try
            {
                var configFile = XDocument.Load("ProgramList.xml");

                var gameList = configFile.Descendants("Programs");

                var data = from game in gameList.Descendants("Program")
                           select new ListBoxStuff
                           {
                               Name = game.Element("Name").Value,
                               Path = game.Element("Path").Value,
                               Arguments = game.Element("Argument")?.Value
                           };

                return data;
            }
            catch (Exception ex)
            {
                Debugging.TraceInformation(string.Format("Could not load config file: {0}", ex.Message));

                return null;
            }
        }
    }
}
