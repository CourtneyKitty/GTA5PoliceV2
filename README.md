# GTA5PoliceV2
New version to my old GTA5Police bot

## Commands
- !status - Displays the status of the servers

## Bot Commander Commands
- !g5p settings - Displays the current filtered words, commanders, serverId, logsId, serverIp and ServerPorts
- filter add <word> - Adds the word to the filtered words

## Config
{
  "Prefix": null, - Bot prefix
  "Token": null, - Bot token
  "ServerId": 0, - Id of the discord server
  "LogsId": 0, - Id of the channel you would like the logs in
  "StatusTimerInterval": 0, - Unused at this moment in time
  "MessageTimerInterval": 0, - Unused at this moment in time
  "Commanders": 0, - Amount of commanders
  "BotCommanders": [
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0
  ], - Array of all commander ids
  "Filters": 0, - Amount of filtered words
  "FilteredWords": [
    null,
    null,
    null,
    null,
    null,
    null,
    null,
    null,
    null,
    null,
    null,
    null,
    null,
    null,
    null,
    null,
    null,
    null,
    null,
    null
  ] - Array of filtered words
}
