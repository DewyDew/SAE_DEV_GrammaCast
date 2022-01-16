﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;

namespace GrammaCast
{
    public class MapBoss
    {
        private TiledMap tileMap;
        private TiledMapRenderer tileMapRenderer;
        private TiledMapTileLayer tileMapLayerTransition;
        private TiledMapTileLayer tileMapLayerObstacles;
        private TiledMapTileLayer tileMapLayerObstacles2;
        private string path;

        public MapBoss(string path)
        {
            Path = path;
            Actif = false;
        }

        public void LoadContent(Microsoft.Xna.Framework.Content.ContentManager Content, GraphicsDevice gd)
        {
            this.TileMap = Content.Load<TiledMap>(this.Path);
            this.TileMapRenderer = new TiledMapRenderer(gd, this.TileMap);
            this.TileMapLayerTransition = this.TileMap.GetLayer<TiledMapTileLayer>("transition");
            this.TileMapLayerObstacles = this.TileMap.GetLayer<TiledMapTileLayer>("obstacles");
            this.TileMapLayerObstacles2 = this.TileMap.GetLayer<TiledMapTileLayer>("obstacles2");

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
            set => path = value;
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
        public TiledMapTileLayer TileMapLayerObstacles2
        {
            get => tileMapLayerObstacles2;
            private set => tileMapLayerObstacles2 = value;
        }
        public bool Actif;
        public bool IsCollisionHero(ushort x, ushort y)
        {
            TiledMapTile? tile;
            if (this.TileMapLayerObstacles.TryGetTile(x, y, out tile) == false)
                return true;
            if (!tile.Value.IsBlank)
                return true;
            if (this.TileMapLayerObstacles2.TryGetTile(x, y, out tile) == false)
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
