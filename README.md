# SoloTournamentCreator

Application realised in C# to help creating League of Legends Tournament with solo player inscription.  
The application try to create the most balanced teams config possible depending on each summoner skill level (solo Queue rank),
retrieved via their pseudo.

It will then be possible to access a Tournament Tree, that will be updated depending on the result of each match.  

The Final objective is to automatically generate Riot Tournament Code for each match, and update the Tournament Tree depending on the result from those Tournament Code Match, without any input required by the admin.  

This application function in client mode (readonly access to the database, can only see the tournaments and their brackets/result) and admin mode (can create new tournament/players, modify the results, start and archive tournaments ...)  

It is by default parametered in Client mode.

To use in Admin mode :
A League of Legends Developer API Key is required to use the program (https://developer.riotgames.com/docs/getting-started)  
A Database (MySQL Server v >= 5.1) is required.  
Create at least 2 account on your database, one with write/read access, the other with only read access.  
In the parameters, if you connect using a write/read access account, you'll unlock the Admin commands.  
If you don't, you'll only have the client options  
