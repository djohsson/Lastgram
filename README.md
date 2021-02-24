# Lastgram
A Telegram bot that will query last.fm for the currently playing song of the specified user, and send a link to the song on Spotify (if it exists)

## Usage

Running the bot with Docker requires the following environment variables to be set:
- LASTGRAM_DOCKER_CONNECTIONSTRING
- LASTGRAM_LASTFM_APIKEY
- LASTGRAM_LASTFM_APISECRET
- LASTGRAM_SPOTIFY_CLIENTID
- LASTGRAM_SPOTIFY_CLIENTSECRET
- LASTGRAM_TELEGRAM_KEY
- LASTGRAM_DB_PASSWORD (if using the docker-compose.yml in this repo)

Running the bot outside of docker:
- Swap the connection string environment variable for LASTGRAM_CONNECTIONSTRING


## Dependencies
### Telegram.Bot
https://github.com/TelegramBots/Telegram.Bot

### SpotifyAPI.Web
https://github.com/JohnnyCrazy/SpotifyAPI-NET

### Inflatable.Lastfm
https://github.com/inflatablefriends/lastfm

### efcore.pg
https://github.com/npgsql/efcore.pg
