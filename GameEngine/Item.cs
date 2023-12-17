using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPI311.GameEngine
{
    public class Item
    {
        public Texture2D _texture;
        public Vector2 _position;
        public Animation _anim;

        public Item(Vector2 pos, Texture2D texture)
        {
            _texture = texture;
            _anim = new(_texture, 6, 1, 0.1f);
            _position = pos;
        }

        public virtual void Update()
        {
            _anim.Update();
        }

        public virtual void Draw(float scale)
        {
            _anim.Draw(_position, scale);
        }
    }
}
