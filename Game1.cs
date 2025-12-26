using System.IO; 
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace StardewMini
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        
        private Player _player;
        private GridMap _gridMap;
        private TileSelector _tileSelector;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content"; 
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 600;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            int tileSize = 64;

            // --- CARREGANDO AS IMAGENS ---
            
            // Carrega o chão (Certifique-se que o arquivo grass.png existe na pasta Content)
            Texture2D grassTexture = LoadTexture("Content/grass.png");

            // Carrega as 4 imagens do boneco
            Texture2D imgFrente = LoadTexture("Content/frente.png");
            Texture2D imgCosta  = LoadTexture("Content/costa.png");
            Texture2D imgEsq    = LoadTexture("Content/ladoesquerdo.png");
            Texture2D imgDir    = LoadTexture("Content/ladodireito.png");

            // -----------------------------

            // IMPORTANTE: Se der erro aqui, é porque seu GridMap.cs está antigo!
            // Atualize o GridMap.cs para aceitar (Texture2D, int).
            _gridMap = new GridMap(grassTexture, tileSize);
            
            _tileSelector = new TileSelector(GraphicsDevice, tileSize);

            Vector2 screenSize = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            
            // Passamos as 4 imagens para o Player
            _player = new Player(imgFrente, imgCosta, imgEsq, imgDir, new Vector2(100, 100), screenSize);
        }

        // Método auxiliar para carregar PNGs sem o MGCB
        private Texture2D LoadTexture(string filepath)
        {
            try
            {
                using (FileStream fileStream = new FileStream(filepath, FileMode.Open))
                {
                    return Texture2D.FromStream(GraphicsDevice, fileStream);
                }
            }
            catch
            {
                // Se o arquivo não existir, avisa no console e retorna um quadrado rosa
                System.Diagnostics.Debug.WriteLine($"ERRO CRÍTICO: Arquivo não encontrado: {filepath}");
                
                Texture2D error = new Texture2D(GraphicsDevice, 32, 32);
                Color[] data = new Color[32*32];
                for(int i=0; i<data.Length; i++) data[i] = Color.Magenta;
                error.SetData(data);
                return error;
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _player.Update(gameTime);
            _tileSelector.Update(_player.Position);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue); 

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            _gridMap.Draw(_spriteBatch, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            _tileSelector.Draw(_spriteBatch);
            _player.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}