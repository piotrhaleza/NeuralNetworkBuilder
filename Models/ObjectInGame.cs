using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;

namespace SnakeGame.Models
{
    internal class ObjectInGame
    {
        public string Type;
        public string Direction;
        public int x;
        public int y;
        public Rectangle rectangle;
        public Ellipse ellipse;

        public ObjectInGame(string type, string direction, int x, int y)
        {
            Type = type;
            this.x = x;
            this.y = y;
            Direction = direction;

            rectangle = new Rectangle();
            rectangle.Width = 10;
            rectangle.Height = 10;
            rectangle.Fill = Brushes.Red;

            ellipse = new Ellipse();
            ellipse.Width = 10;
            ellipse.Height = 10;
            ellipse.Fill = Brushes.White;
        }
    }
}
