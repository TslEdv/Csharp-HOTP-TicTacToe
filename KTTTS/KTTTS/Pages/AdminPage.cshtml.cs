using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text.Encodings.Web;
using System.Web;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenPop.Mime;
using OpenPop.Pop3;

namespace KTTTS.Pages
{
    public class AdminPage : PageModel
    {
        public void OnGet()
        {
            var client = new Pop3Client();
            client.Connect("pop.gmail.com", 995, true);
            //add username and password of the managing email account
            client.Authenticate("", "");
            var count = client.GetMessageCount();
            var listOfMatches = new List<Message>();
            for (var i = 1; i < count+1; i++)
            {
                Message message = client.GetMessage(i);
                if (message.Headers.Subject == "Match")
                {
                    listOfMatches.Add(message);
                }
            }
            //add username and password of the managing email account
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("", ""),
                EnableSsl = true,
            };
            //add sender email same as username
            foreach (var email in listOfMatches)
            {
                var player2 = email.FindFirstPlainTextVersion().GetBodyAsText().Split("\n");
                var body = "You have been invited to a match of Tic-Tac-Toe!" 
                           + Environment.NewLine + "Click the link to accept the match!" 
                           + Environment.NewLine + "https://localhost:5001/GameAccepted?player1=" 
                           + HttpUtility.UrlEncode(email.Headers.From.Address) + "&player2=" + HttpUtility.UrlEncode(player2[0]);
                smtpClient.Send("", player2[0], "Invite to match!", body);
            }
            client.DeleteAllMessages();
            client.Disconnect();
        }
    }
}