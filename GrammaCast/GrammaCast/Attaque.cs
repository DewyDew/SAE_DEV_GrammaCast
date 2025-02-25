﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Content;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Sprites;
using System;


namespace GrammaCast
{
    /*
    Attaque (lettre) demandée pour tuer un ennemi
    */
    public class Attaque
    {
        // Chemins des sprites des animations possibles à fiare
        public static string[] spriteChemin = new string[] { "IceCastSprite.sf",
            "FireCastSprite.sf", "HolyExplosionSprite.sf", "IceShatterSprite.sf", "PoisonCastSprite.sf"};
        
        // Lettres d'attaque possibles
        private string[] alphabet = new string[] { "A", "B", "C", "D", "E", "F", "G",
            "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
        private SpriteSheet[] attaqueSprite = new SpriteSheet[spriteChemin.Length];

        public Hero perso;
        public Ennemi ennemi;

        private string fontPath;
        private string pointFontPath;
        private SpriteFont pointFont;
        private SpriteFont attaqueFont;
        private AnimatedSprite asAttack;
        private string attaqueLettre;

        public Timer timerAnimation;
        public Timer timerAttaque;

        Random rand = new Random();

        public float point = 350; //point de base que donne chaque attaque
        public float sommePoint = 0;   //somme totale des points     
        private int vitesse = 100; //vitesse pour le sprite des points

        public Attaque()
        {
            FontPath = "font";
            PointFontPath = "pointfont";
            Actif = false;
            Final = false;
            Animation = false;
            AttaqueLettre = this.alphabet[rand.Next(alphabet.Length)];
            
        }

        // Charge le Sprite (visuel) de l'attaque depuis le chemin donné par le constructeur
        public void LoadContent(Microsoft.Xna.Framework.Content.ContentManager Content)
        {
            this.AttaqueFont = Content.Load<SpriteFont>(this.FontPath);
            this.PointFont = Content.Load<SpriteFont>(this.PointFontPath);
            for (int i = 0; i < attaqueSprite.Length; i++)
            {
                attaqueSprite[i] = Content.Load<SpriteSheet>(spriteChemin[i], new JsonContentLoader());
            }
            this.AsAttack = new AnimatedSprite(attaqueSprite[rand.Next(attaqueSprite.Length)]);
        }
        public void Update(GameTime gameTime, float windowWidth, float windowHeight)
        {
            
            float deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            float apparitionSpeed = deltaSeconds * this.Vitesse;

            //position de la lettre en fonction de la position du joueur
            if (perso.PositionHero.Y >= windowHeight / 2)
                this.PositionAttaque = new Vector2(perso.PositionHero.X, perso.PositionHero.Y - 100);
            else
                this.PositionAttaque = new Vector2(perso.PositionHero.X, perso.PositionHero.Y + 25);

            if (this.Actif)
            {
                
                if (timerAttaque == null)
                {
                    timerAttaque = new Timer(1000);
                }
                else
                    timerAttaque.AddTick(deltaSeconds);
            }
            if (this.Final)
            {
                this.PositionPoint = new Vector2(this.PositionPoint.X, this.PositionPoint.Y - apparitionSpeed);
                this.AsAttack.Play("attack");
                
                if (timerAnimation.AddTick(deltaSeconds) == false)
                {
                    sommePoint += point / timerAttaque.Tick;
                    timerAttaque = null;
                    this.Final = false;
                    this.Animation = false;
                    this.Actif = false;
                    this.AttaqueLettre = this.alphabet[rand.Next(alphabet.Length)];
                    this.AsAttack = new AnimatedSprite(attaqueSprite[rand.Next(attaqueSprite.Length)]);
                    
                }
            }
            else
            {
                if (this.PositionPoint != perso.PositionHero)
                    PositionPoint = new Vector2(perso.PositionHero.X, perso.PositionHero.Y-30);
                this.GetLetter();
            }
            this.AsAttack.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch _spriteBatch)
        {
            if (this.Animation)
            {
                _spriteBatch.Draw(this.AsAttack, new Vector2(perso.PositionHero.X, perso.PositionHero.Y));
                _spriteBatch.DrawString(this.PointFont, $"{Math.Round(this.sommePoint,0)}", this.PositionPoint, Color.Black);
            }
            else
                _spriteBatch.DrawString(this.AttaqueFont, $"{this.AttaqueLettre}", this.PositionAttaque, Color.White);
            
        }
        public string FontPath
        {
            get => fontPath;
            private set => fontPath = value;
        }
        public string PointFontPath
        {
            get => pointFontPath;
            private set => pointFontPath = value;
        }
        public SpriteFont PointFont
        {
            get => pointFont;
            private set => pointFont = value;
        }
        public SpriteFont AttaqueFont
        {
            get => attaqueFont;
            private set => attaqueFont = value;
        }
        public AnimatedSprite AsAttack
        {
            get => asAttack;
            private set => asAttack = value;
        }
        public bool Actif; //s'il est actif un combat est lancé
        public bool Final; //le final désigne quand l'attaque est fini pour afficher les animations de l'attaque et les points
        public bool Animation; //permet de faire l'animation de l'attaque quand le final est true

        public Vector2 PositionAttaque;
        public Vector2 PositionPoint;
        public string AttaqueLettre
        {
            get => attaqueLettre;
            private set => attaqueLettre = value;
        }
        public int Vitesse
        {
            get => vitesse;
            private set => vitesse = value;
        }
        public void GetLetter()
        {
            //permet de vérifier si la touche du clavier appuyée correspond à la bonne lettre de l'attaque
            var keyboardState = Keyboard.GetState();
            var keys = keyboardState.GetPressedKeys();
            foreach (var key in keys)
            {
                if (key.ToString() == this.AttaqueLettre)
                {
                    timerAnimation = new Timer(0.4f); //lance un timer pour la durée de l'animation,
                                                      // sinon ça n'affichait que la 1ere frame
                    this.Final = true;
                    this.Animation = true;
                }
            }
        }

        public bool NbrPoint()
        {
            //permet de vérifier si le personnage possède assez de point pour aller combattre le boss
            if (sommePoint >= 3000)
                return true;
            else
                return false;
        }   
    }
}
