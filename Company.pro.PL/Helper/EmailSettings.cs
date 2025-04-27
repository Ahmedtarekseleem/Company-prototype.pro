using System.Net;
using System.Net.Mail;
using System.Runtime.Intrinsics.X86;

namespace Company.pro.PL.Helper
{
	public class EmailSettings
	{
		public static bool SendEmail(Email email)
		{
			try
			{
				var client = new SmtpClient("smtp.gmail.com", 587);
				client.EnableSsl = true;
				client.Credentials = new NetworkCredential("abdallahanwar899@gmail.com", "ljojqeapobchhqsv");
				client.Send("abdallahanwar899@gmail.com", email.To, email.Subject, email.Body);
				return true;
			}
			catch (Exception ex) 
			{
			
				return false;
			}
		}
	}
}
