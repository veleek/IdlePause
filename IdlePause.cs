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

            // If the game is not paused and there's no menu open
            if (!Game1.paused && Game1.activeClickableMenu == null)
            {
                
                if (player.UsingTool || player.isMoving() || player.timerSinceLastMovement <= this.prevTimerSinceLastMovement)
                {
                    // Reset the idle time if time if they're doing anything, or they've moved at all.
                    this.idleTime = 0;
                }
                else
                {
                    this.idleTime += player.timerSinceLastMovement - this.prevTimerSinceLastMovement;

                    this.Monitor.Log($"IdleTime: {this.idleTime}");

                    if (this.idleTime > this.Config.IdleDuration)
                    {
                        // If we're currently idle, open the inventory
                        Game1.activeClickableMenu = new GameMenu();
                        this.idleTime = 0;
                    }
                }
            }

            this.prevTimerSinceLastMovement = player.timerSinceLastMovement;
        }
    }
}