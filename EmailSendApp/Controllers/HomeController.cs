using Confluent.Kafka;
using EmailSendApp.Models;
using EmailSendApp.Service.Abstract;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace EmailSendApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMailService _mailService;
        private readonly ProducerConfig _config;


        public HomeController(ILogger<HomeController> logger, IMailService mailService, ProducerConfig config)
        {
            _logger = logger;
            _mailService = mailService;
            _config = config;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult SendEmail()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> SendEmail(EmailSend emailSend)
        {
            string serializedEmailSend = JsonConvert.SerializeObject(emailSend);
            using (var producer = new ProducerBuilder<Null, string>(_config).Build())
            {
                await producer.ProduceAsync(topic: "test", new Message<Null, string> { Value = serializedEmailSend });
                producer.Flush(TimeSpan.FromSeconds(10));
                _mailService.SendMessageAsync(emailSend.ReceicerMail, emailSend.Title, emailSend.Contents);
                return RedirectToAction("Index");
            }
        }
    }
}