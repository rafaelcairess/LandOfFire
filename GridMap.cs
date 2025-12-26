using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewMini
{
    public class GridMap
    {
        private Texture2D _texture;
        private int _cellSize;

        public GridMap(Texture2D grassTexture, int cellSize)
        {
            _texture = grassTexture;
            _cellSize = cellSize;
        }

        public void Draw(SpriteBatch spriteBatch, int screenWidth, int screenHeight)
        {
            // CORREÇÃO: "Uma imagem só"
            // Em vez de fazer loops (for) repetindo a imagem, desenhamos ela UMA vez.
            
            // Rectangle(X, Y, Largura, Altura)
            // Aqui mandamos ela começar no 0,0 e ocupar a largura e altura totais da tela.
            spriteBatch.Draw(_texture, new Rectangle(0, 0, screenWidth, screenHeight), Color.White);
        }
    }
}