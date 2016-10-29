using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;

namespace TagsCloudVisualization
{
    public static class CloudVizualizer
    {
        private const int BorderSize = 10;

        public static Bitmap DrawRectangles(IEnumerable<Rectangle> rectangles, Size size, Color color)
        {
            var bitmap = new Bitmap(size.Width, size.Height);
            using (var graphics = Graphics.FromImage(bitmap))
            {
                foreach (var rectangle in rectangles)
                    graphics.DrawRectangle(new Pen(color), rectangle);
            }
            return bitmap;
        }

        public static Bitmap DrawTags(IEnumerable<Tag> tags, Size size, Color color)
        {
            var bitmap = new Bitmap(size.Width, size.Height);
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
                foreach (var tag in tags)
                {
                    graphics.DrawRectangle(new Pen(color), tag.Rectangle);
                    var boundingRect = new Rectangle(tag.Rectangle.Location,
                        tag.Rectangle.Size + new Size(BorderSize, BorderSize));
                    graphics.DrawString(tag.Word, tag.Font, new SolidBrush(color), boundingRect);
                }
            }
            return bitmap;
        }
    }
}