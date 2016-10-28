using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CloudVizualizer
    {
        private const int BorderSize = 10;

        public static Size CalcCloudSize(IEnumerable<Rectangle> rectangles)
        {
            var enumerable = rectangles as Rectangle[] ?? rectangles.ToArray();
            if (enumerable.Length == 0)
                return new Size(0, 0);
            var width = enumerable.Max(rect => rect.Right) - enumerable.Min(rect => rect.X);
            var height = enumerable.Max(rect => rect.Bottom) - enumerable.Min(rect => rect.Y);
            return new Size(width, height);
        }
        

        public static Bitmap DrawRectangles(Size size, IEnumerable<Rectangle> rectangles, Color color)
        {
            var bitmap = new Bitmap(size.Width, size.Height);
            using (var graphics = Graphics.FromImage(bitmap))
            {
                foreach (var rectangle in rectangles)
                    graphics.DrawRectangle(new Pen(color), rectangle);
            }
            return bitmap;
        }

        public static Bitmap DrawTags(Size size, IEnumerable<Tag> tags, Color color)
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
