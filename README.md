# C# HOTP TicTacToe

Following NuGet Packages have been installed:
- Microsoft.EntityFrameworkCore.Design
- Microsoft.EntityFrameworkCore.SQLServer
- OpenPop
- OpenPop.Core
- OpenPop.NET
- Otp.NET


Add the neccesary database connection string in appsettings.json.

Add managing email address and password in following files:

- AdminPage
- GameAccepted
- GameWindowTest

To initialize the needed tables:

- dotnet ef database update --project KTTTS --startup-project KTTTS

The Admin Page needs to be open in a separate tab for the application to refresh it's email every 15 seconds.

Game Process

- Send and email to the managing server with title "Match" and body text of only the other player's email
Example: 
To: serveremail@gmail.com
Subject: Match
Body: player2@gmail.com

- Player2 receives a link to game creation
- Both players get sent a email with a link to the game and a one time password to register their move
- Game goes on until a draw or a victory.
