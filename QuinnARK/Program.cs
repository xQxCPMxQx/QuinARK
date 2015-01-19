﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using Color = System.Drawing.Color;
using SharpDX;

namespace Tristana
{
    class Program
    {
        static void Main(string[] args)
        {
            Game.PrintChat("<font color=\"#00BFFF\">Quinn ARK - <font color=\"#FFFFFF\">Successfully Loaded.</font>");
            CustomEvents.Game.OnGameLoad += OnLoad;
        }

        //Feel free to Edit this Assembly *ScienceARK*

        public const string CHAMP_NAME = "Quinn";
        public static int SpellRangeTick;
        public static Menu Config;
        public static Orbwalking.Orbwalker Orbwalker;
        public static Spell Q;
        public static Spell W;
        public static Spell E;
        public static Spell R;
        private static Obj_AI_Hero player = ObjectManager.Player;
        public static List<Spell> SpellList = new List<Spell>();
        public static SpellSlot IgniteSlot;

        private static void OnLoad(EventArgs args)
        {

                if (player.ChampionName != CHAMP_NAME)
                return;

            //Spells

            Q = new Spell(SpellSlot.Q,1000);
            Q.SetSkillshot(0.25f, 80, 1550, true, SkillshotType.SkillshotLine);

            E = new Spell(SpellSlot.E, 800);
            
            R = new Spell(SpellSlot.R, 550);




            //Main Menu Name


            Config = new Menu("Quinn By ScienceARK", "QuinnARK", true);



            //Orbwalker Menu


            Orbwalker = new Orbwalking.Orbwalker(Config.AddSubMenu(new Menu("Orbwalking", "Orbwalking")));
            TargetSelector.AddToMenu(Config.AddSubMenu(new Menu("Target Selector", "TS")));



            var combo = Config.AddSubMenu(new Menu("Combo", "combo"));
            combo.AddItem(new MenuItem("Combo Mode", "Combo Mode"))
                .SetValue(new KeyBind("X".ToCharArray()[0], KeyBindType.Press));


            //Misc Menu


            Config.SubMenu("Misc").AddItem(new MenuItem("AG", "Anti Gapcloser").SetValue(true));
            Config.SubMenu("Misc").AddItem(new MenuItem("INT", "Interrupt Spells").SetValue(true));
            Config.SubMenu("Misc").AddItem(new MenuItem("ALEVEL", "Use Autoleveler").SetValue(true));


            //Combo Menu


            combo.AddItem(new MenuItem("UseQ", "Use Q - Rapid Fire").SetValue(true));
            combo.AddItem(new MenuItem("UseE", "Use E - Explosive Shot").SetValue(true));
            combo.AddItem(new MenuItem("UseR", "Use R Finisher").SetValue(true));

           
            
            //Main MENU!!


            Config.AddToMainMenu();
              
            Game.OnGameUpdate += Game_OnGameUpdate;


            //Draw??


            Drawing.OnDraw += Drawing_OnDraw;


            
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            
        }

            // Combo Configuration


            private static void Game_OnGameUpdate(EventArgs args)
            {
 	      
                if (Config.Item("Combo Mode").GetValue<KeyBind>().Active)
                {
                    var Target = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Physical);
                    if (Target == null)
                        return;
                    if (Q.IsReady() && Target.IsValidTarget(Q.Range));
                    {
                        Q.CastIfHitchanceEquals(Target, HitChance.High, true);
                    }
                    if (E.IsReady() && Target.IsValidTarget(E.Range));
                    {
                        E.CastIfHitchanceEquals(Target, HitChance.High, true);
                    }
                    if (R.IsReady() && Target.IsValidTarget(E.Range));
                    {
                        R.Cast();
                    }
                }

            }      
    }
}

// Drawing.OnDraw += Drawing_OnDraw;
//          var combo = Config.AddSubMenu(new Menu("Combo", "combo"));
//          combo.AddItem(new MenuItem("Combo Mode", "Combo Mode"))
//         .SetValue(new KeyBind("X".ToCharArray()[0], KeyBindType.Press));
// ItemData. >>item list shows 
//
//

