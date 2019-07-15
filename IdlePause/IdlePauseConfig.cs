﻿namespace Ben.StardewValley
{
    public class IdlePauseConfig
    {
        /// <summary>The duration in milliseconds you must be idle before the menu will open.</summary>
        public int IdleDuration { get; set; } = 5000;

        /// <summary>Gets or sets a value that indicates whether to open the inventory menu when the user goes idle.</summary>
        public bool OpenMenuOnPause { get; set; } = false;

        /// <summary>Gets or sets a value that determines whether to show a tooltip when the user is idle.</summary>
        public bool ShowIdleTooltip { get; set; } = true;

        /// <summary>Gets or sets the text that will be displayed in the Idle Tooltip.</summary>
        public string IdleText { get; set; } = "Zzzzz";
    }
}
