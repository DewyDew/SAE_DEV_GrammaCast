﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;

namespace GrammaCast
{
    public class MapForet
    {
        private TiledMap tileMap;
        private TiledMapRenderer tileMapRenderer;
        private TiledMapTileLayer tileMapLayerZone;
        private TiledMapTileLayer tileMapLayerTransition;
        private TiledMapTileLayer tileMapLayerObstacles;
        
        private string path;

        public MapForet(string path)
        {
            Path = path;
            Actif = false;
        }

        public void LoadContent(Microsoft.Xna.Framework.Content.ContentManager Content, GraphicsDevice gd)
        {
            this.TileMap = Content.Load<TiledMap>(this.Path);
            this.TileMapRenderer = new TiledMapRenderer(gd, this.TileMap);
            this.TileMapLayerZone = this.TileMap.GetLayer<TiledMapTileLayer>("zone");
            this.TileMapLayerTransition = this.TileMap.GetLayer<TiledMapTileLayer>("transition");
            this.TileMapLayerObstacles = this.TileMap.GetLayer<TiledMapTileLayer>("obstacles");

        }
        public void Update(GameTime gameTime)
        {
            this.TileMapRenderer.Update(gameTime);
        }
        public void Draw()
        {
            this.TileMapRenderer.Draw();
        }

        public string Path
        {
            get => path;
            private set => path = value;
        }
        public TiledMap TileMap
        {
            get => tileMap;
            private set => tileMap = value;
        }
        public TiledMapRenderer TileMapRenderer
        {
            get => tileMapRenderer;
            private set => tileMapRenderer = value;
        }
        public TiledMapTileLayer TileMapLayerZone
        {
            get => tileMapLayerZone;
            private set => tileMapLayerZone = value;
        }
        public TiledMapTileLayer TileMapLayerTransition
        {
            get => tileMapLayerTransition;
            private set => tileMapLayerTransition = value;
        }
        public TiledMapTileLayer TileMapLayerObstacles
        {
            get => tileMapLayerObstacles;
            private set => tileMapLayerObstacles = value;
        }
        public bool Actif;
        public bool IsCollisionZone(Hero perso)
        {
            TiledMapTile? tile;
            if (this.TileMapLayerZone.TryGetTile((ushort)perso.PositionHero.X, (ushort)perso.PositionHero.Y, out tile) == false)
                return true;
            if (!tile.Value.IsBlank)
                return true;
            return false;
        }
        public bool IsCollisionEnnemi(ushort x, ushort y)
        {
            TiledMapTile? tile;
            if (this.TileMapLayerZone.TryGetTile(x, y, out tile) == false)
                return true;
            if (tile.Value.IsBlank)
                return true;
            return false;
        }
        public bool IsCollisionHero(ushort x, ushort y)
        {
            TiledMapTile? tile;
            if (this.TileMapLayerObstacles.TryGetTile(x, y, out tile) == false)
                return true;
            if (!tile.Value.IsBlank)
                return true;
            return false;
        }
        public bool IsTransition(ushort x, ushort y)
        {
            TiledMapTile? tile;
            if (this.TileMapLayerTransition.TryGetTile(x, y, out tile) == false)
                return true;
            if (!tile.Value.IsBlank)
                return true;
            return false;
        }
    }
}
