using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using TagsCloudVisualization.Geometry;

namespace TagsCloudVisualization
{
    public class CloudVizualizer: IDisposable
    {
        private const int BorderSize = 10;

        private Bitmap bitmap;
        private readonly Size offset = new Size(0, 0);

        private void Initialize(int width, int height)
        {
            bitmap = new Bitmap(width, height);
        }

        public CloudVizualizer(int width, int height)
        {
            Initialize(width, height);
        }

        public CloudVizualizer(Rectangle[] rectangles)
        {
            var width = rectangles.Max(rect => rect.Right) - rectangles.Min(rect => rect.X) + BorderSize;
            var height = rectangles.Max(rect => rect.Bottom) - rectangles.Min(rect => rect.Y) + BorderSize;
            offset = new Size(width / 2, height / 2);
            Initialize(width, height);
        }

        public void DrawRectangles(Rectangle[] rectangles, Color color)
        {
            using (var graphics = Graphics.FromImage(bitmap))
            {
                foreach (var rectangle in rectangles)
                    graphics.DrawRectangle(new Pen(color), rectangle.Shift(offset));
            }
        }

        public void DrawTags(Tag[] tags, Color color)
        {
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.TextRenderingHint = TextRenderingHint.AntiAlias;;
                foreach (var tag in tags)
                {
                    var rect = tag.Rectangle.Shift(offset);
                    rect.Size += new Size(BorderSize, BorderSize);
                    graphics.DrawString(tag.Word, tag.Font, new SolidBrush(color), rect);
                }
            }
        }

        public void Save(string fileName)
        {
            bitmap.Save(fileName);
        }

        public void Dispose()
        {
            bitmap.Dispose();
        }
    }
}
