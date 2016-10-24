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
            var words = File.ReadAllLines(args[0]);
            var visualizer = new CloudVizualizer(Width, Height);
            var random = new Random();
            foreach (var word in words)
            {
                visualizer.AddWord(word, random.Next(14, 28), Color.Green);
            }
            visualizer.Save(args[1]);
        }
    }
}
