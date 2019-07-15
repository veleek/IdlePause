using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;

namespace Ben.StardewValley
{
    /// <summary>The mod entry class.</summary>
    public class ModEntry : Mod
    {
        /// <summary>The length of time the user has been idle.</summary>
        private double _idleTime;

        /// <summary>The last time of day that the user was not idle during.</summary>
        private int _lastNonIdleTimeOfDay;

        /// <summary>The index of the last tool the user was using.</summary>
        private int _lastToolIndex;

        private IdlePauseConfig Config;

        private bool IsIdle => _idleTime > Config.IdleDuration;

        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            Config = helper.ReadConfig<IdlePauseConfig>();
            helper.Events.GameLoop.UpdateTicked += OnUpdateTicked;

            if (Config.ShowIdleTooltip)
                helper.Events.Display.RenderedHud += OnRenderedHud;
        }

        /// <summary>Raised after drawing the HUD (item toolbar, clock, etc) to the sprite batch, but before it's rendered to the screen. The vanilla HUD may be hidden at this point (e.g. because a menu is open).</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnRenderedHud(object sender, EventArgs e)
        {
            if (!IsIdle) return;

            SpriteBatch b = Game1.spriteBatch;
            SpriteFont font = Game1.smallFont;

            string idleText = Config.IdleText;
            int margin = Game1.tileSize * 3 / 8;
            int width = (int)font.MeasureString(idleText).X + 2 * margin;
            int height = Math.Max(60, (int)font.MeasureString(idleText).Y + 2 * margin); //60 is "cornerSize" * 3 on SDV source
            const int x = 10, y = 10;

            IClickableMenu.drawTextureBox(b, x, y, width, height, Color.White);

            Vector2 tPos = new Vector2(x + margin, y + margin + 4);
            b.DrawString(font, idleText, tPos + new Vector2(2, 2), Game1.textShadowColor);
            b.DrawString(font, idleText, tPos, Game1.textColor);

        }

        /// <summary>Raised after the game state is updated (≈60 times per second).</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnUpdateTicked(object sender, EventArgs e)
        {
            // Check if the player has moved, and if they are idle do not update game time.
            if (Game1.currentLocation == null) return;

            Farmer player = Game1.player;
            if (player == null) return;

            if (!CheckIdle())
            {
                // Reset the idle time if time if they're doing anything, or they've moved at all.
                _idleTime = 0;
            }
            else
            {
                _idleTime += Game1.currentGameTime.ElapsedGameTime.TotalMilliseconds;

                if (_idleTime > Config.IdleDuration)
                {
                    // If we're currently idle, pause the game

                    if (Config.OpenMenuOnPause)
                    {
                        Game1.activeClickableMenu = new GameMenu();
                        _idleTime = 0;
                    }
                    else
                    {
                        Game1.timeOfDay = _lastNonIdleTimeOfDay;
                    }
                }
                else
                {
                    // If we're not idle, store the time of day.
                    _lastNonIdleTimeOfDay = Game1.timeOfDay;
                }
            }

            _lastToolIndex = player.CurrentToolIndex;
        }

        /// <summary>Determines if the player is currently idle.</summary>
        /// <returns>True if the player is idle, false otherwise.</returns>
        private bool CheckIdle()
        {
            Farmer player = Game1.player;

            // If the game is paused or there's a menu open, we don't count as idle.
            if (Game1.paused || Game1.activeClickableMenu != null) return false;

            // Check for any tool use.
            if (player.UsingTool) return false;

            // Check if they've changed tools.
            if (player.CurrentToolIndex != _lastToolIndex) return false;

            // Check for movement (or attempted movement)
            if (player.CanMove && player.isMoving()) return false;

            // Check for mouse movement (e.g. looking at stuff in inventory).
            if (Game1.getOldMouseY() != Game1.getMouseY() || Game1.getOldMouseX() != Game1.getMouseX()) return false;

            return true;
        }
    }
}