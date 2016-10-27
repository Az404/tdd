using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    public class Tag
    {
        public Rectangle Rectangle;
        public string Word;
        public Font Font;

        public Tag(Rectangle rectangle, string word, Font font)
        {
            Rectangle = rectangle;
            Word = word;
            Font = font;
        }
    }
}
