using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SnakeGame.UI.Entities
{
    class SnakeElement : GameEntity
    {
        public SnakeElement(int size, bool isHead = false)
        {
            UIElement = new Rectangle
            {
                Width = size - 4,
                Height = size - 4,
                Fill = isHead? Brushes.Yellow: Brushes.Green
            };
            Size = size;
        }
        public int Size { get; set; }
        public bool IsHead {
            
            get => isHead; 
            set
            {
                isHead = value;
                (UIElement as Rectangle).Fill = isHead ? Brushes.Yellow : Brushes.Green;
            } 
        
        
        }
        private bool isHead;

        public static int green = 255;

    }
}
