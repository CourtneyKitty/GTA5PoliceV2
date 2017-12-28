# GTA5PoliceV2
New version to my old GTA5Police bot

## Unique Features
- Server status detection. This will detect whether the servers specified are online or not.
- Status update timer. This will create an announcement if a server switches state.
- Message timer. This will send a set message every <MessageTimerInterval> amount of minutes.

## Commands
- !rules - Displays the rules
- !links - Displays useful links
- !apply - Displays links to the whitelist servers and jobs applications
- !status - Displays the status of the servers

## Bot Commander Commands
- !g5p settings - Displays the current filtered words, commanders, serverId, logsId, serverIp and ServerPorts
- filter add <word> - Adds the word to the filtered words

## Config
  - "Prefix": "!", - Bot prefix
  - "Token": null, - Bot token
  - "ServerId": 0, - Id of the discord server
  - "LogsId": 0, - Id of the channel you would like the logs in
  - "TimerChannelId": 0, - Id of the channel you would like the timer messages in
  - "StatusTimerInterval": 1, -  How often in minutes the bot will check the status of the servers specified
  - "MessageTimerInterval": 30, - How often the message timer will send the message
  - "Commanders": 0, - Amount of commanders
  - "BotCommanders": [], - Array of all commander ids
  - "Filters": 0, - Amount of filtered words
  - "FilteredWords": [] - Array of filtered words
