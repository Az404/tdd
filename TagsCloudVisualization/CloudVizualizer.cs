using System.Drawing;

namespace TagsCloudVisualization
{
    public class CloudVizualizer
    {
        private readonly Bitmap bitmap;
        private readonly CircularCloudLayouter layouter;

        private const int BorderSize = 2;

        public CloudVizualizer(int width, int heigth)
        {
            bitmap = new Bitmap(width, heigth);
            layouter = new CircularCloudLayouter(new Point(width / 2, heigth / 2));
        }

        public void AddWord(string word, int fontHeight, Color color)
        {
            using (var graphics = Graphics.FromImage(bitmap))
            {
                var font = new Font(FontFamily.GenericMonospace, fontHeight);
                var size = graphics.MeasureString(word, font) + new SizeF(BorderSize, BorderSize);
                var rect = layouter.PutNextRectangle(size.ToSize());
                graphics.DrawRectangle(new Pen(color), rect);
                graphics.DrawString(word, font, new SolidBrush(color), rect);
            }
        }

        public void Save(string fileName)
        {
            bitmap.Save(fileName);
        }
    }
}
