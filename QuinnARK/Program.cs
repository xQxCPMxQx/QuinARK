using System;
using System.Collections.Generic;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using Color = System.Drawing.Color;

namespace Quinn
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Game.PrintChat("<font color=\"#00BFFF\">Quinn ARK - <font color=\"#FFFFFF\">Successfully Loaded.</font>");
            CustomEvents.Game.OnGameLoad += OnLoad;
        }

        public const string ChampName = "Quinn";
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
            if (player.ChampionName != ChampName)
                return;

            //Spells
            Q = new Spell(SpellSlot.Q, 1010);
            Q.SetSkillshot(0.25f, 160f, 1150, true, SkillshotType.SkillshotLine);

            E = new Spell(SpellSlot.E, 880);
            E.SetTargetted(0.25f, 2000f);

            R = new Spell(SpellSlot.R, 550);

            //Main Menu Name
            Config = new Menu("Quinn By ScienceARK", "QuinnARK", true);

            //Orbwalker Menu
            TargetSelector.AddToMenu(Config.AddSubMenu(new Menu("Target Selector", "TS")));
            Orbwalker = new Orbwalking.Orbwalker(Config.AddSubMenu(new Menu("Orbwalking", "Orbwalking")));

            //Keykind Menu
            var combo = Config.AddSubMenu(new Menu("Combo", "combo"));
            combo.AddItem(new MenuItem("Combo Mode", "Combo Mode"))
                .SetValue(new KeyBind("space".ToCharArray()[0], KeyBindType.Press));

            //Combo Menu
            combo.AddItem(new MenuItem("UseQ", "Use Q").SetValue(true));
            combo.AddItem(new MenuItem("UseE", "Use E").SetValue(true));
            combo.AddItem(new MenuItem("UseR", "Use R").SetValue(true));

            //Laneclear Menu
            var Laneclear = new Menu("Laneclear", "Laneclear");
            Config.AddSubMenu(new Menu("Laneclear", "Laneclear"));

            //Misc Menu
            Config.SubMenu("Misc").AddItem(new MenuItem("AG", "Anti Gapcloser").SetValue(true));
            Config.SubMenu("Misc").AddItem(new MenuItem("INT", "Interrupt Spells").SetValue(true));
            Config.SubMenu("Misc").AddItem(new MenuItem("ALEVEL", "Use Autoleveler").SetValue(true));

            //Items Menu
            Config.SubMenu("Items").AddItem(new MenuItem("Botrk", "Botrk").SetValue(true));

            //Draw Menu
            var drawMenu = Config.AddSubMenu(new Menu("Drawing", "Drawing"));
            drawMenu.AddItem(new MenuItem("Draw_Disabled", "Disable all Drawing").SetValue(true));
            drawMenu.AddItem(new MenuItem("Draw_Q", "Draw Q").SetValue(true));
            drawMenu.AddItem(new MenuItem("Draw_W", "Draw W").SetValue(true));
            drawMenu.AddItem(new MenuItem("Draw_E", "Draw E").SetValue(true));
            drawMenu.AddItem(new MenuItem("Draw_R", "Draw R").SetValue(true)); // check with global R drawing 

            //Author Menu
            Config.AddSubMenu(new Menu("ScienceARK Series!", "ScienceARK Series!"));

            Config.AddToMainMenu();

            Game.OnGameUpdate += Game_OnGameUpdate;
            Drawing.OnDraw += Drawing_OnDraw;
            Interrupter.OnPossibleToInterrupt += Interrupter_OnPossibleToInterrupt;
            AntiGapcloser.OnEnemyGapcloser += AntiGapCloser_OnEnemyGapcloser;
        }

        //Configuration > Interrupter & gapcloser 
        private static void Interrupter_OnPossibleToInterrupt(Obj_AI_Hero unit, InterruptableSpell spell)
        {
            if (E.IsReady() && unit.IsValidTarget(E.Range))
                E.CastOnUnit(unit);
        }

        private static void AntiGapCloser_OnEnemyGapcloser(ActiveGapcloser gapcloser)
        {
            if (E.IsReady() && gapcloser.Sender.IsValidTarget(E.Range))
                E.CastOnUnit(gapcloser.Sender);
        }

        //Quinn's E will do noting when pantheon has his passive
        public static bool IsHePantheon(Obj_AI_Hero target)
        {
            return target.Buffs.All(buff => buff.Name == "pantheonpassivebuff");
        }

        //Configuration > Combo (simple)
        private static void Game_OnGameUpdate(EventArgs args)
        {

            if (Config.Item("Combo Mode").GetValue<KeyBind>().Active)
            {
                var target = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Physical);
                var Botrk = ItemData.Blade_of_the_Ruined_King.GetItem();
                if (!target.IsValidTarget())
                    return;

                if (Q.IsReady() && target.IsValidTarget(Q.Range))
                {
                    Q.CastIfHitchanceEquals(target, HitChance.High, true);
                }

                if (E.IsReady() && target.IsValidTarget(E.Range))
                {
                    E.CastOnUnit(target);
                }

                if (R.IsReady() && target.IsValidTarget(E.Range))
                {
                    R.Cast();
                }
            }
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            //Draw Skill Cooldown on Champ   // Add Rrdy tonight
            var pos = Drawing.WorldToScreen(ObjectManager.Player.Position);
            if (R.IsReady() && Config.Item("Rrdy").GetValue<bool>())
            {
                Drawing.DrawText(pos.X, pos.Y, Color.Gold, "R is Ready!");
            }

            if (!Config.Item("Draw_Disabled").GetValue<bool>())
            {
                if (Config.Item("Qdraw").GetValue<bool>() & Q.Level > 0)
                    Render.Circle.DrawCircle(
                        ObjectManager.Player.Position, Q.Range, Q.IsReady() ? Color.DarkGreen : Color.DarkOrange);
                if (Config.Item("Edraw").GetValue<bool>() && E.Level > 0)
                    Render.Circle.DrawCircle(
                        ObjectManager.Player.Position, Q.Range, Q.IsReady() ? Color.DarkGreen : Color.DarkOrange);
            }

        }
    }
}
