using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPI311.GameEngine
{
    public class Player
    {
        public Vector2 _position = new(300, 300);
        public float _speed = 600f;
        public Texture2D _texture;
        public Dictionary<char, Animation> _animations = new();
        public bool up = false;
        public bool down = false;
        public bool left = false;
        public bool right = false;
        public bool moving = false;
        public bool movingUp = false;
        public bool movingDown = false;
        public bool movingRight = false;
        public bool movingLeft = false;
        public float buffer = 0;
        public char key = 'S';

        public Player(Texture2D texture)
        {
            InputManager.Initialize();
            _texture = texture;
            _animations.Add('W', new Animation(_texture, 8, 5, 0.1f, 1));
            _animations.Add('S', new Animation(_texture, 8, 5, 0.1f, 2));
            _animations.Add('A', new Animation(_texture, 8, 5, 0.1f, 3));
            _animations.Add('D', new Animation(_texture, 8, 5, 0.1f, 4));

            _animations['D'].Stop();
            _animations['D'].Reset();
        }

        public virtual void Update()
        {
            InputManager.Update();

            if (InputManager.IsKeyDown(Keys.W)) //up
            {
                movingUp = true;
                _position -= Vector2.UnitY * _speed * Globals.TotalSeconds;
            }
            else movingUp = false;
            if (InputManager.IsKeyDown(Keys.S)) //down
            {
                movingDown = true;
                _position += Vector2.UnitY * _speed * Globals.TotalSeconds;
            }
            else movingDown = false;
            if (InputManager.IsKeyDown(Keys.D)) //right
            {
                movingRight = true;
                _position += Vector2.UnitX * _speed * Globals.TotalSeconds;
            }
            else movingRight = false;
            if (InputManager.IsKeyDown(Keys.A)) //left
            {
                movingLeft = true;
                _position -= Vector2.UnitX * _speed * Globals.TotalSeconds;
            }
            else movingLeft = false;

            if (movingUp == true ||
                movingDown == true ||
                movingRight == true ||
                movingLeft == true)
            {
                moving = true;
            }
            else
            {
                moving = false;
            }

            if (!moving)
            {
                _animations['D'].Stop();
                _animations['D'].Reset();
            }
            else
            {
                _animations['D'].Start();
                _animations['D'].Update();
            }
        }

        public virtual void Draw(float scale)
        {
            _animations['D'].Draw(_position, scale);
        }
    }
}
