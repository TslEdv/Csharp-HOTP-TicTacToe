using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using KTTTS.DAL;
using KTTTS.Domain;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OtpNet;

namespace KTTTS.Pages
{
    public class GameAccepted : PageModel
    {
        private readonly AppDbContext _ctx;
            
        public GameAccepted(AppDbContext ctx)
        {
            _ctx = ctx;
        }
        
        public async Task<PageResult> OnGet(string player1, string player2)
        {
            //create a new empty game
            TicTacToeBrain.TicTacToeBrain brain = new();
            player1 = HttpUtility.HtmlDecode(player1);
            player2 = HttpUtility.HtmlDecode(player2);
            
            //get brain as json string
            var jsonStr = brain.GetBrainJson();
            var saveGameDb = new Game
            {
                GameState = jsonStr,
                P1Email = player1
            };
            
            //generate keys and save them
            var key = KeyGeneration.GenerateRandomKey(20);
            saveGameDb.P1Key = Base32Encoding.ToString(key);
            saveGameDb.P1Counter = 0;
            var key2 = KeyGeneration.GenerateRandomKey(20);
            saveGameDb.P2Email = player2;
            saveGameDb.P2Key = Base32Encoding.ToString(key2);
            saveGameDb.P2Counter = 0;
            //update the database
            _ctx.Games.Add(saveGameDb);
            await _ctx.SaveChangesAsync();
            var gameId = saveGameDb.Id;
            
            //add username and password of the managing email account
            //create new SmtpClient connection
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("", ""),
                EnableSsl = true,
            };
            //generate one-time-passwords for players and send them an email
            var base32Bytes1 = Base32Encoding.ToBytes(saveGameDb.P1Key);
            var hotp1 = new Hotp(base32Bytes1, OtpHashMode.Sha512);
            var base32Bytes2 = Base32Encoding.ToBytes(saveGameDb.P2Key);
            var hotp2 = new Hotp(base32Bytes2, OtpHashMode.Sha512);
            string currentplayer = brain.GetCurrentPlayer() == 1 ? player1 : player2;
            var body1 = "First goes " + currentplayer + System.Environment.NewLine + "https://localhost:5001/GameWindowTest?id=" + gameId + "&player=" + HttpUtility.UrlEncode(player1)
                        + System.Environment.NewLine + "Your One Time Password:" + hotp1.ComputeHOTP(saveGameDb.P1Counter);
            var body2 = "First goes " + currentplayer + System.Environment.NewLine + "https://localhost:5001/GameWindowTest?id=" + gameId + "&player=" + HttpUtility.UrlEncode(player2)
                        + System.Environment.NewLine + "Your One Time Password:" + hotp2.ComputeHOTP(saveGameDb.P2Counter);
            
            //add sender same as username
            smtpClient.Send("", player1, "Match accepted!", body1);
            smtpClient.Send("", player2, "Match accepted!", body2);
            return Page();
        }
    }
}