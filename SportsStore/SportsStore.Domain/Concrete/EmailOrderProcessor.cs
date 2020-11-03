using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;

namespace SportsStore.Domain.Concrete
{
    public class EmailSettings
    {
        public string MailToAddress = "admin@yahoo.com";
        public string MailFromAddress = "sportsstore@yahoo.com";
        public bool UseSsl = true;
        public string Username = "Username";
        public string Password = "Password";
        public string ServerName = "ORTENCA-PC";
        public int ServerPort = 587;
        public bool WriteAsFile = false;
        public string FileLocation = @"C:\Users\user\Desktop\emails";
    }
    public class EmailOrderProcessor : IOrderProcessor
    {
        EmailSettings emailSettings;
        public EmailOrderProcessor(EmailSettings emailSettings)
        {
            this.emailSettings = emailSettings;
        }

        public void ProcessOrder(Cart cart,ShippingDetails shippingDetails)
        {
            using(var smtpClient=new SmtpClient())
            {
                smtpClient.EnableSsl = emailSettings.UseSsl;
                smtpClient.Port = emailSettings.ServerPort;
                smtpClient.Host = emailSettings.ServerName;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(emailSettings.Username,emailSettings.Password);

                if(emailSettings.WriteAsFile)
                {
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtpClient.PickupDirectoryLocation = emailSettings.FileLocation;
                    smtpClient.EnableSsl = false;
                }

                StringBuilder body = new StringBuilder();
                body.AppendLine("Porosia eshte bere submit").AppendLine("-----------");
                foreach(var line in cart.Lines)
                {
                    var subtotal = line.Product.Price * line.Quantity;
                    body.AppendFormat("{0}*{1} = {2:c}", line.Quantity, line.Product.Name, subtotal);
                }
                body.AppendFormat("Totali:{0}", cart.ComputeTotalValue()).AppendLine("---------");
                body.AppendLine("Ship to:{0}").AppendLine(shippingDetails.Name).AppendLine(shippingDetails.Line1).AppendLine(shippingDetails.Line2 ?? "").AppendLine(shippingDetails.Line3 ?? "").AppendLine(shippingDetails.City).AppendFormat("Gift wrap:{0}", shippingDetails.GiftWrap ? "Yes" : "No");
                MailMessage mail = new MailMessage(emailSettings.MailFromAddress, emailSettings.MailToAddress, "Porosia", body.ToString());
                if (emailSettings.WriteAsFile)
                    mail.BodyEncoding = Encoding.ASCII;
                smtpClient.Send(mail);
            }
        }
    }
}
