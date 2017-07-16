using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using Utils;

namespace XBoxControlTesting
{
    public static class Config
    {
        public static IEnumerable<ApplicationItem> LoadConfig()
        {
            try
            {
                var configFile = XDocument.Load("ProgramList.xml");
                var gameList = configFile.Descendants("Programs");
                var data = new List<ApplicationItem>();

                foreach (var game in gameList.Descendants("Program"))
                {
                    if (File.Exists(game.Element("Path").Value))
                    {
                        data.Add(new ApplicationItem
                        {
                            Name = game.Element("Name").Value,
                            Path = game.Element("Path").Value,
                            Arguments = game.Element("Argument")?.Value,
                            Icon = GetFileIcon(game.Element("Path").Value)
                        });
                    }
                }

                return data.OrderBy(x => x.Name);
            }
            catch (Exception ex)
            {
                Debugging.TraceInformation(string.Format($"Could not load config file: {ex.Message}"));

                return null;
            }
        }

        private static ImageSource GetFileIcon(string path)
        {
            var bitmap = Icon.ExtractAssociatedIcon(path).ToBitmap();
            var stream = new MemoryStream();
            bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);

            return BitmapFrame.Create(stream);
        }
    }

}
