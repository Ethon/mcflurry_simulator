using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace UFO.Commander.Util {
    public class SMTPMailer {

        private SmtpClient client;
        private String senderMailAdress;

        public SMTPMailer(String mailAdress,String password,String smtpAdress,uint smtpPort) {
            this.senderMailAdress = mailAdress;
            client = new SmtpClient(smtpAdress, 587) {
                Credentials = new NetworkCredential(mailAdress, password),
                EnableSsl = true
            };
        }
        public void sendMailTo(String recipient,String subject,String content) {
            client.Send(senderMailAdress, recipient, subject, content);
        }
    }
}
