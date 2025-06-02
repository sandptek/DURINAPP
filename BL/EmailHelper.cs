using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
	public class EmailHelper
	{
		private string host = "smtp.gmail.com";
		private int port = 587;

		public bool Send(string from, List<string> to, string subject, string body, bool html = false)
		{
			string username = "no-reply@durinlifesciences.com";//"info@theequ.com";// StaticData.settings.siteEmail;
			string password = "qbfa qage tdgw yvpj";//"clwr yztu lamt hzvg";// StaticData.settings.siteEmailPass;


			MailMessage ePosta = new MailMessage();
			ePosta.From = new MailAddress(from);
			foreach (var item in to)
			{
				ePosta.To.Add(item);
			}
			//ePosta.Attachments.Add(new Attachment(@"C:\deneme-upload.jpg"));

			ePosta.IsBodyHtml = html;
			ePosta.Subject = subject;
			ePosta.Body = body;

			SmtpClient smtp = new SmtpClient();

			smtp.Credentials = new System.Net.NetworkCredential(username, password);
			smtp.UseDefaultCredentials = false;
			smtp.Port = port;
			smtp.Host = host;
			smtp.EnableSsl = true;
			bool kontrol = true;

			try
			{
				smtp.Send(ePosta);
			}
			catch (SmtpException ex)
			{
				kontrol = false;
			}

			return kontrol;
		}
	}
}
