using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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

                List<ListBoxStuff> data = new List<ListBoxStuff>();

                foreach (var game in gameList.Descendants("Program"))
                {
                    if (File.Exists(game.Element("Path").Value))
                    {
                        data.Add(new ListBoxStuff
                        {
                            Name = game.Element("Name").Value,
                            Path = game.Element("Path").Value,
                            Arguments = game.Element("Argument")?.Value,
                            Icon = IconToImage(Icon.ExtractAssociatedIcon(game.Element("Path").Value))
                        });
                    }
                }

                return data;
            }
            catch (Exception ex)
            {
                Debugging.TraceInformation(string.Format("Could not load config file: {0}", ex.Message));

                return null;
            }
        }

        private static ImageSource IconToImage(Icon icon)
        {
            Bitmap bmp = icon.ToBitmap();
            var stream = new MemoryStream();
            bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
            return BitmapFrame.Create(stream);
        }
    }

}
