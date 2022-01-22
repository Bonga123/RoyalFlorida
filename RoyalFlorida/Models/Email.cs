using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace RoyalFlorida.Models
{
    public class Email
    {
        public string To { get; set; }
        public string from { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
       public Byte[] Attachment { get; set; }

        public void Sendmail()
        {
            MailMessage mc = new MailMessage("21821217@dut4life.ac.za", To);
            mc.Subject = Subject;
            mc.Body = Body;
            if (Attachment != null)
            {
                MemoryStream ms = new MemoryStream(Attachment);
                mc.Attachments.Add(new Attachment(ms, "Reciet.pdf"));
            }
            mc.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient("smtp.office365.com", 587);
            smtp.Timeout = 1000000;
            smtp.UseDefaultCredentials = false;
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            NetworkCredential nc = new NetworkCredential("21821217@dut4life.ac.za","Dut971118");
            smtp.Credentials = nc;
            smtp.Send(mc);
        }
    }
}