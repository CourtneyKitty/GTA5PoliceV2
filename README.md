# GTA5PoliceV2
GTA5Police Version 2. This is a Discord bot used by the gaming community GTA5Police.com. Check out the original bot that I wrote a while back, the old version is outdated and will not work on the current Discord version. https://github.com/byBlurr/GTA5POLICE-Bot

## Unique Features
- Server status detection. This will detect whether the game servers specified are online or not.
- Status update timer. This will create an announcement if a game server switches state.
- Message timer. This will send a set message every <MessageTimerInterval> amount of minutes.
- Profanity Filter. This will delete any messages with the blacklisted words and will send admins the information for the message.
- Auto ban. This system will check a user against the autoban list when they join the discord. This will help catch those who should be banned but leave the Discord before an admin is able to ban.
- Bug Reports & Suggestion Components. This filters and deletes any messages in the #bug-reports and #suggestions channels in the discord. This helps keep all messages in these channels clean and following the required layout.
- Devs bypass. Developers bypass the Bug Reports & Suggestions component.

## Commands
- The bot has many commands that can be used by members in the discord.
- The bot also has administration commands to help moderate and admin the server easily.
- The bot even has commands to edit configs from Discord itself.
- The bot has commands that higher ups who aren't admins can use to assign certain roles.

## Setting the bot up
- Make sure you have the .NET Core installed on your pc (should be if youre using the code).
- Run it once, let the configs generate, follow whats on the console.
- Close the console and then head to the directory.
- Edit all the configs to how you want them.
- Rememeber when adding commanders and developers the int of how many there are needs to match how many there are.

## Setting the workspace up
- Create a workspace using the correct framework.
- Install the Discord API to the project.
- Install the dependencies to the project.
- Download a copy of the source code and place it within the workspace.
- Run the project, should have no errors.

## Configs
- Main Config. All the main settings such as timer intervals, discord token and more.
- URL Config. This config is used by the links command and is where all the links are held.
- Dev Config. This config is used by the bug reports and suggestions components.
- Auto Bans. Holds the array of all user ids of users that need a ban.
- Connections Config. This config is used by the server status detection. This holds ports and ips.
- Ranks Config. This config is used by the rank commands.

## Pull Requests
I am about done with my plans for this bot. Here is a list of things I thought about adding.
- Feel free to make pull requests of features you would like added.
- A way to add and remove people from auto bans.
- More commands for rank stuff so the config can be edited.
- A way to add and remove servers from the status checks.
- Fix anything within the issues tab also add any you find...
- Make sure there are no bugs within your changes, obviously a few might slip through but DO NOT pr anything that will break the bot.

## Further Information
- This bot is using the .net Discord API (version 1.0.2)
- The workspace is set up using .NET Core (.NET Framework 4.6.1)
- All Deps
  - Discord.Net v1.0.2
  - Discord.Net.Core v1.0.2
  - Discord.Net.Commands v1.0.2
  - Discord.Net.Providers.WS4Net v1.0.2
  - Discord.Net.Rest v1.0.2
  - Discord.Net.Rpc v1.0.2
  - Discord.Net.Webhoook v1.0.2
  - Discord.Net.WenSocket v1.0.2
  - Microsoft.NETCore.App v2.0.0
- Bruze MPG Community Discord (Use this for any further information or help) - http://www.discord.me/bruze

## License
This work is licensed under the Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License. To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/4.0/ or send a letter to Creative Commons, PO Box 1866, Mountain View, CA 94042, USA.
