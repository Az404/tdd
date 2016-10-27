using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Fclp;

namespace TagsCloudVisualization
{
    class Program
    {
        private const int MinFontSize = 14;
        private const int MaxFontSize = 28;
        private const int Width = 2000;
        private const int Height = 1000;
        private static readonly Point Center = new Point(Width / 2, Height / 2);

        private static readonly Random Random = new Random(0);

        private class VisualiserOptions
        {
            public string TagsFileName { get; set; }
            public string ImageFileName { get; set; }
        }

        static void Main(string[] args)
        {
            var commandLineParser = new FluentCommandLineParser<VisualiserOptions>();

            commandLineParser
                .Setup(options => options.TagsFileName)
                .As('t')
                .Required()
                .WithDescription("Path to tags file");

            commandLineParser
                .Setup(options => options.ImageFileName)
                .As('i')
                .Required()
                .WithDescription("Path to output image");

            var usage = $"Usage: {AppDomain.CurrentDomain.FriendlyName} [ -h | -help ] -t tags-file -i image-file";

            commandLineParser
                .SetupHelp("h", "help")
                .WithHeader(usage)
                .Callback(text => Console.WriteLine(text));

            var result = commandLineParser.Parse(args);
            if (result.HelpCalled)
                return;
            if (result.HasErrors)
            {
                Console.WriteLine(usage);
                return;
            }
            
            if (!File.Exists(commandLineParser.Object.TagsFileName))
            {
                Console.WriteLine("File not found");
                return;
            }

            Visualise(commandLineParser.Object);
        }

        private static void Visualise(VisualiserOptions options)
        {
            var words = File.ReadAllLines(options.TagsFileName);
            var tags = PutWords(words);
            var rectangles = tags.Select(tag => tag.Rectangle).ToArray();
            using (var visualizer = new CloudVizualizer(Width, Height))
            {
                visualizer.DrawRectangles(rectangles, Color.GreenYellow);
                visualizer.DrawTags(tags, Color.Green);
                visualizer.Save(options.ImageFileName);
            }
        }

        private static Tag[] PutWords(string[] words)
        {
            var layouter = new CircularCloudLayouter(Center);
            return words.Select(word =>
            {
                var font = new Font(FontFamily.GenericSansSerif, Random.Next(MinFontSize, MaxFontSize));
                var size = TextRenderer.MeasureText(word, font);
                var rect = layouter.PutNextRectangle(size);
                return new Tag(rect, word, font);
            }).ToArray();
        }
    }
}
