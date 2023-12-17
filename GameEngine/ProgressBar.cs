using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPI311.GameEngine
{
    public class ProgressBar
    {
        Texture2D texture;
        public Rectangle rectangleBackground;
        public Rectangle rectangleBar;
        Vector2 position;
        public int barAmount;
        public int height;
        public int x;
        public int y;
        bool fill;
        Color color;

        public ProgressBar(Texture2D newTexture, int barLength, int barHeight, int xPos, int yPos, bool fillSetting, Color newColor)
        {
            texture = newTexture;
            barAmount = barLength;
            height = barHeight;
            x = xPos;   
            y = yPos;
            fill = fillSetting;
            color = newColor;
        }

        public void Initialize()
        {
            rectangleBackground = new Rectangle(x, y, barAmount, height);
            if(fill)
            {
                rectangleBar = new Rectangle(x, y, 0, height);
            }
            else
            {
                rectangleBar = new Rectangle(x, y, barAmount, height);
            }
        }

        public void Update(int barState)
        {
            if(barState <= 0) barState = 0;
            if(barState >= barAmount) barState = barAmount;

            rectangleBackground = new Rectangle(x, y, barAmount, height);
            rectangleBar = new Rectangle(x, y, barState, height);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rectangleBackground, Color.White);
            spriteBatch.Draw(texture, rectangleBar, color);
        }
    }
}
