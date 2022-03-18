using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using KTTTS.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OtpNet;
using TicTacToeBrain;

namespace KTTTS.Pages
{
    public class GameWindowTest : PageModel
    {
        private readonly DAL.AppDbContext _ctx;

        public GameWindowTest(DAL.AppDbContext context)
        {
            _ctx = context;
        }
        public BoardSquareState[,]? Board { get; private set; }
        public bool MoveAllow;
        public int Id;
        public string Email = default!;
        public string Winner = "In progress";
        [BindProperty] public string Password { get; set; } = default!;
        public async Task<IActionResult> OnGetAsync(int id, string player, int x, int y, int move)
        {
            Game currentGame = await _ctx.Games.FindAsync(id);
            Id = id;
            player = HttpUtility.HtmlDecode(player);
            Email = HttpUtility.UrlEncode(player);
            TicTacToeBrain.TicTacToeBrain brain = new();
            brain.RestoreBrainFromJson(currentGame.GameState);
            var turn = brain.GetCurrentPlayer();
            Board = brain.Board;
            if (brain.GameFinish() != "No")
            {
                Winner = brain.GameFinish();
                MoveAllow = false;
                return Page();
            }
            switch (turn)
            {
                case 1 when player == currentGame.P1Email:
                {
                    MoveAllow = true;
                    if (move != 0)
                    {
                        if (Board[x,y].IsX == false && Board[x,y].IsO == false)
                        {
                            Board![x, y].IsX = true;
                        }
                    }

                    break;
                }
                case 1:
                    MoveAllow = false;
                    break;
                case 2 when player == currentGame.P2Email:
                {
                    MoveAllow = true;
                    if (move != 0)
                    {
                        if (Board[x,y].IsX == false && Board[x,y].IsO == false)
                        {
                            Board![x, y].IsO = true;
                        }
                    }

                    break;
                }
                case 2:
                    MoveAllow = false;
                    break;
            }

            return Page();
        }
        public async Task<IActionResult> OnPostAsync(int id, string player, int x, int y)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            player = HttpUtility.HtmlDecode(player);
            Game currentGame = await _ctx.Games.FindAsync(id);
            TicTacToeBrain.TicTacToeBrain brain = new();
            brain.RestoreBrainFromJson(currentGame.GameState);
            var turn = brain.GetCurrentPlayer();
            if (brain.GameFinish() != "No")
            {
                Winner = brain.GameFinish();
                Board = brain.Board;
                return RedirectToPage("/GameWindowTest", new {id = id, player = player});
            }
            switch (turn)
            {
                case 1:
                {
                    if (player == currentGame.P1Email)
                    {
                        var base32Bytes = Base32Encoding.ToBytes(currentGame.P1Key);
                        var hotp = new Hotp(base32Bytes, OtpHashMode.Sha512);
                        if (hotp.VerifyHotp(Password, currentGame.P1Counter))
                        {
                            brain.Board[x, y].IsX = true;
                            brain._currentPlayerNo = 2;
                            currentGame.GameState = brain.GetBrainJson();
                            currentGame.P1Counter++;
                            await _ctx.SaveChangesAsync();
                            var body1 = "https://localhost:5001/GameWindowTest?id=" + currentGame.Id + "&player=" + HttpUtility.UrlEncode(player) 
                                        + System.Environment.NewLine + "Your One Time Password:" + hotp.ComputeHOTP(currentGame.P1Counter);
                            //add username and password of managing email
                            var smtpClient = new SmtpClient("smtp.gmail.com")
                            {
                                Port = 587,
                                Credentials = new NetworkCredential("", ""),
                                EnableSsl = true,
                            };
                            //add sender same as username
                            smtpClient.Send("", player, "Next move password!", body1);
                        }
                    }

                    break;
                }
                case 2:
                {
                    if (player == currentGame.P2Email)
                    {
                        var base32Bytes = Base32Encoding.ToBytes(currentGame.P2Key);
                        var hotp = new Hotp(base32Bytes, OtpHashMode.Sha512);
                        if (hotp.VerifyHotp(Password, currentGame.P2Counter))
                        {
                            brain.Board[x, y].IsO = true;
                            brain._currentPlayerNo = 1;
                            currentGame.GameState = brain.GetBrainJson();
                            currentGame.P2Counter++;
                            await _ctx.SaveChangesAsync();
                            var body1 = "https://localhost:5001/GameWindowTest?id=" + currentGame.Id + "&player=" + HttpUtility.UrlEncode(player)
                                        + System.Environment.NewLine + "Your One Time Password:" + hotp.ComputeHOTP(currentGame.P2Counter);
                            //add username and password of managing email
                            var smtpClient = new SmtpClient("smtp.gmail.com")
                            {
                                Port = 587,
                                Credentials = new NetworkCredential("", ""),
                                EnableSsl = true,
                            };
                            //add sender same as username
                            smtpClient.Send("", player, "Next move password!", body1);
                        }
                    }

                    break;
                }
            }

            Board = brain.Board;
            return RedirectToPage("/GameWindowTest", new {id = id, player = player});
        }
    }
}