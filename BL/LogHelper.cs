using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
	public class LogHelper
	{
		public void Log(string folder, string message)
		{
			var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "log");

			if (!Directory.Exists(uploadsFolder))
			{
				Directory.CreateDirectory(uploadsFolder);
			}

			uploadsFolder = Path.Combine(uploadsFolder, folder);

			if (!Directory.Exists(uploadsFolder))
			{
				Directory.CreateDirectory(uploadsFolder);
			}
			
            string filePath = uploadsFolder + "\\" + DateTime.Now.Date.ToString("yyyyMMdd") + ".txt";
			using (StreamWriter writer = new StreamWriter(filePath, true))
			{
				writer.WriteLine($"{DateTime.Now}: {message}");
				writer.Close();
			}
		}


	}
}
