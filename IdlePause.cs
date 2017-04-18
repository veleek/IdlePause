using System;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;

namespace Ben.StardewValley
{
    public class IdlePause : Mod
    {
        private int idleTime;
        private int prevTimerSinceLastMovement;
        private int prevToolIndex;

        public IdlePauseConfig Config { get; set; }

        public override void Entry(IModHelper helper)
        {
            this.Config = helper.ReadConfig<IdlePauseConfig>();

            GameEvents.EighthUpdateTick += this.UpdateIdleTime;
        }

        /// <summary>
        ///     Check if the player has moved, and if they are idle do not update gametime.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateIdleTime(object sender, EventArgs e)
        {
            Farmer player = Game1.player;
            if (player == null)
            {
                return;
            }

            if (!this.CheckIdle())
            {
                // Reset the idle time if time if they're doing anything, or they've moved at all.
                this.idleTime = 0;
            }
            else
            {
                this.idleTime += player.timerSinceLastMovement - this.prevTimerSinceLastMovement;

                if (this.idleTime > this.Config.IdleDuration)
                {
                    // If we're currently idle, open the inventory
                    Game1.activeClickableMenu = new GameMenu();
                    this.idleTime = 0;
                }
            }

            this.prevTimerSinceLastMovement = player.timerSinceLastMovement;
            this.prevToolIndex = player.CurrentToolIndex;
        }

        /// <summary>
        /// Determines if the player is currently idle.
        /// </summary>
        /// <returns>True if the player is idle, false otherwise.</returns>
        private bool CheckIdle()
        {
            Farmer player = Game1.player;

            // If the game is paused or there's a menu open, we don't count as idle.
            if (Game1.paused || Game1.activeClickableMenu != null) return false;

            // Check for any tool use.
            if (player.UsingTool) return false;

            // Check if they've changed tools.
            if (player.CurrentToolIndex != this.prevToolIndex) return false;

            // Check for movement (or attempted movement)
            if (player.CanMove && player.isMoving()) return false;
            // This check may not be necessary. 
            if (player.timerSinceLastMovement <= this.prevTimerSinceLastMovement) return false;

            // Check for mouse movement (e.g. looking at stuff in inventory).
            if (Game1.getOldMouseY() != Game1.getMouseY() || Game1.getOldMouseX() != Game1.getMouseX()) return false;

            return true;
        }
    }
}