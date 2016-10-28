using System.Drawing;

namespace TagsCloudVisualization
{
    public class Tag
    {
        public Rectangle Rectangle { get; private set; }
        public string Word { get; private set; }
        public Font Font { get; private set; }

        public Tag(Rectangle rectangle, string word, Font font)
        {
            Rectangle = rectangle;
            Word = word;
            Font = font;
        }
    }
}
