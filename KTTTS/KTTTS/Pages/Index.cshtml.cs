using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using OpenPop.Mime;
using OpenPop.Pop3;
using OtpNet;

namespace KTTTS.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}