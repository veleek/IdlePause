# Stardew Valley IdlePause Mod

One of the big parts of Stardrew Valley is time management.  The days are reasonably short and figuring out exactly what you want to, or more accurately, will be able to finish in a day is part of the fun.  However, the game time continues to progress even when you're not doing anything unless you've opened up a game menu or are browsing through a chest.  

If you leave your computer or are distracted by something and you don't pause the game, you may come back to find that your entire day has been lost.  You can restart the day but at the cost of redoing all of the things you already completed.

In the worst case, you may have missed an entire day and passed out potentially losing items or a large chunk of cash.  The auto-save means that the only way to get your day back is to manually revert to the backed up save from the previous day that the game makes automatically.

Now you can save yourself the trouble by automatically pausing the game (by opening your inventory) if you have been idle for a certain period of time.

## Installation

1. Make sure you have [installed SMAIP](http://canimod.com/for-players/install-smapi)
1. Download the [latest release on GitHub](./releases/latest) or get it [from NexusMods](http://www.nexusmods.com/stardewvalley/mods/1092).
1. Unzip the mod into the SMAPI Mods folder

## Configuration

The default length of time you must be idle before the menu opens is 5 seconds (5000ms).  You can modify the value by changing `IdleDuration` in the `config.json`.  This represents the duration in milliseconds before you will be considred idle.

```json
{
  "IdleDuration": 5000
}
```

