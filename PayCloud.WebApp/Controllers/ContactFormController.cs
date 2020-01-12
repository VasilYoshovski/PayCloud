using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PayCloud.WebApp.Models;

namespace PayCloud.WebApp.Controllers
{
    public class ContactFormController : Controller
    {
        // GET: ContactForm/SendUserMessage
        public ActionResult GetSendUserMessage()
        {
            return this.PartialView("_SendUserMessagePartial");
        }

        // POST: ContactForm/SendUserMessage
        [HttpPost]
        public ActionResult SendUserMessage(ContactFormDto contactFormDto)
        {
            try
            {
                // TODO: Send and email
                return this.Json(true);
            }
            catch
            {
                return this.Json(false);
            }
        }
    }
}
