using System; // Necessário para usar a matemática (Math.Sin)
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace StardewMini
{
    public class Player
    {
        private Texture2D _texFrente;
        private Texture2D _texCosta;
        private Texture2D _texEsq;
        private Texture2D _texDir;
        private Texture2D _currentTexture; 

        private Vector2 _position;
        private float _speed;
        private Vector2 _screenBounds;

        // --- VARIÁVEIS DE ANIMAÇÃO (O PULO) ---
        private float _animTimer; // Cronômetro para controlar o ritmo do passo
        private float _bounceY;   // O quanto ele sobe ou desce (em pixels)

        public Vector2 Position 
        { 
            get { return _position; } 
        }

        public Player(Texture2D frente, Texture2D costa, Texture2D esq, Texture2D dir, Vector2 startingPosition, Vector2 screenBounds)
        {
            _texFrente = frente;
            _texCosta = costa;
            _texEsq = esq;
            _texDir = dir;
            _currentTexture = _texFrente;

            _position = startingPosition;
            _screenBounds = screenBounds;
            _speed = 200f;
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();
            Vector2 direction = Vector2.Zero;
            bool isMoving = false; // Flag para saber se estamos andando

            // --- LÓGICA DE MOVIMENTO ---
            if (state.IsKeyDown(Keys.W)) 
            { 
                direction.Y -= 1; 
                _currentTexture = _texCosta;
                isMoving = true;
            }
            else if (state.IsKeyDown(Keys.S)) 
            { 
                direction.Y += 1; 
                _currentTexture = _texFrente;
                isMoving = true;
            }
            else if (state.IsKeyDown(Keys.A)) 
            { 
                direction.X -= 1; 
                _currentTexture = _texEsq;
                isMoving = true;
            }
            else if (state.IsKeyDown(Keys.D)) 
            { 
                direction.X += 1; 
                _currentTexture = _texDir;
                isMoving = true;
            }

            if (direction != Vector2.Zero) direction.Normalize();

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _position += direction * _speed * deltaTime;

            // Limites da tela
            _position.X = MathHelper.Clamp(_position.X, 0, _screenBounds.X - _currentTexture.Width);
            _position.Y = MathHelper.Clamp(_position.Y, 0, _screenBounds.Y - _currentTexture.Height);

            // --- A MÁGICA DO PULO (BOBBING) ---
            if (isMoving)
            {
                // Aumentamos o cronômetro baseado na velocidade (multipliquei por 15 para ficar rápido, ritmo de passos)
                _animTimer += deltaTime * 15;

                // Math.Sin cria uma onda que vai de -1 a 1.
                // Math.Abs deixa tudo positivo (0 a 1), fazendo um movimento de "quicar" (como uma bola).
                // Multiplicamos por -5 para ele subir 5 pixels (Y negativo é pra cima).
                _bounceY = (float)Math.Abs(Math.Sin(_animTimer)) * -5;
            }
            else
            {
                // Se parou de andar, zera o pulo e o timer
                _bounceY = 0;
                _animTimer = 0;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Criamos uma posição temporária só para o desenho
            // Pegamos a posição real (_position) e somamos o pulinho (_bounceY) no eixo Y
            Vector2 drawPosition = new Vector2(_position.X, _position.Y + _bounceY);

            spriteBatch.Draw(_currentTexture, drawPosition, Color.White);
        }
    }
}