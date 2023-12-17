using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPI311.GameEngine
{
    public class GameManager
    {
        private Item item;

        public void Init(Texture2D texture)
        {
            item = new(new(300, 300), texture);
        }

        public void Update()
        {
            item.Update();
        }

        public void Draw()
        {
            //item.Draw();
        }
    }
}
