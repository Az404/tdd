using System;
using System.Drawing;
using System.IO;

namespace TagsCloudVisualization
{
    class Program
    {
        private const int Width = 2000;
        private const int Height = 1000;

        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                PrintHelp();
                return;
            }
            var words = File.ReadAllLines(args[0]);
            var visualizer = new CloudVizualizer(Width, Height);
            var random = new Random();
            foreach (var word in words)
            {
                visualizer.AddWord(word, random.Next(14, 28), Color.Green);
            }
            visualizer.Save(args[1]);
        }

        static void PrintHelp()
        {
            Console.WriteLine("Usage: TagsCloudVisualisation.exe tags_file image_file");
            Console.WriteLine("tags_file: text file, every tag on new line");
            Console.WriteLine("image_file: output image file (BMP)");
        }
    }
}
