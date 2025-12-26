using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewMini
{
    public class TileSelector
    {
        private Texture2D _texture;
        private int _tileSize;
        private Vector2 _currentTile; // Guarda o índice da grade (ex: 3, 5)

        public TileSelector(GraphicsDevice graphicsDevice, int tileSize)
        {
            _tileSize = tileSize;

            // Criamos uma textura 1x1 branca para pintar depois
            _texture = new Texture2D(graphicsDevice, 1, 1);
            _texture.SetData(new[] { Color.White });
        }

        public void Update(Vector2 playerPosition)
        {
            // --- A MÁGICA DO 4x4 ---
            // Para saber em qual quadrado estamos, dividimos a posição (pixels) pelo tamanho do quadrado.
            // Exemplo: Se estou no pixel X=130 e o quadrado é 64...
            // 130 / 64 = 2.03125.
            // (int) joga fora a parte decimal, sobrando 2.
            // Significa que estamos na coluna 2 da grade!
            
            // Adicionamos +32 (metade do tamanho do boneco) para pegar o centro dele, e não o canto.
            float centerX = playerPosition.X + 32; 
            float centerY = playerPosition.Y + 32;

            int gridX = (int)(centerX / _tileSize);
            int gridY = (int)(centerY / _tileSize);

            _currentTile = new Vector2(gridX, gridY);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Calculamos onde desenhar o quadrado na tela
            // Multiplicamos o índice da grade pelo tamanho para voltar para pixels.
            Vector2 drawPosition = _currentTile * _tileSize;

            // Desenhamos um quadrado amarelo, meio transparente
            spriteBatch.Draw(
                _texture, 
                new Rectangle((int)drawPosition.X, (int)drawPosition.Y, _tileSize, _tileSize), 
                Color.Yellow * 0.5f // 50% de transparência
            );
        }
    }
}