using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public static class Helper
    {
        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash1
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                return Convert.ToHexString(hashBytes);
            }
        }
        public static List<string> StringParcala(string parcalanacakMetin, int parcaBoyutu)
        {
            List<string> ret = new List<string>();

            int parcaSayisi = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(parcalanacakMetin.Length) / Convert.ToDecimal(parcaBoyutu)));
            int min = 0;
            int max = 0;

            for (int i = 0; i < parcaSayisi; i++)
            {
                min = i * parcaBoyutu;
                max = ((i + 1) * parcaBoyutu) < (parcalanacakMetin.Length - 1) ? parcaBoyutu : (parcalanacakMetin.Length - (i * parcaBoyutu));

                ret.Add(parcalanacakMetin.Substring(min, max));
            }

            return ret;
        }

		private const string Salt = "314159265358";
		public static string ToMixedNumber(int inputNumber)
		{
			string inputStr = inputNumber.ToString().PadLeft(12, '0'); // Sayıyı 12 karaktere tamamla
			char[] encrypted = new char[12];

			for (int i = 0; i < inputStr.Length; i++)
			{
				// Her bir karakteri SECRET_KEY'e göre değiştir
				int encryptedChar = (int.Parse(inputStr[i].ToString()) + int.Parse(Salt[i].ToString())) % 10;
				encrypted[i] = encryptedChar.ToString()[0];
			}

			return new string(encrypted);
		}

		public static int ToOriginalNumber(string mixedNumber)
		{
			char[] original = new char[12];

			for (int i = 0; i < mixedNumber.Length; i++)
			{
				// SECRET_KEY'e göre orijinal sayıyı geri al
				int originalChar = (int.Parse(mixedNumber[i].ToString()) - int.Parse(Salt[i].ToString()) + 10) % 10;
				original[i] = originalChar.ToString()[0];
			}

			return int.Parse(new string(original)); // Orijinal numarayı geri döndür
		}

		public static string RastgeleParolaOlustur(int uzunluk)
		{
			const string harflerKucuk = "abcdefghijklmnopqrstuvwxyz";
			const string harflerBuyuk = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
			const string rakamlar = "0123456789";
			const string ozelKarakterler = "!@#$%^&*()_-+=<>?";

			string tumKarakterler = harflerKucuk + harflerBuyuk + rakamlar + ozelKarakterler;
			Random rnd = new Random();

			// Parola, her karakter grubundan en az bir tane içermelidir
			string parola = new string(Enumerable.Repeat(harflerKucuk, 1)
							  .Select(s => s[rnd.Next(s.Length)]).ToArray()) +
							new string(Enumerable.Repeat(harflerBuyuk, 1)
							  .Select(s => s[rnd.Next(s.Length)]).ToArray()) +
							new string(Enumerable.Repeat(rakamlar, 1)
							  .Select(s => s[rnd.Next(s.Length)]).ToArray()) +
							new string(Enumerable.Repeat(ozelKarakterler, 1)
							  .Select(s => s[rnd.Next(s.Length)]).ToArray());

			// Kalan karakterleri rastgele doldur
			parola += new string(Enumerable.Repeat(tumKarakterler, uzunluk - 4)
						  .Select(s => s[rnd.Next(s.Length)]).ToArray());

			// Parolayı karıştırarak daha güvenli hale getir
			parola = new string(parola.ToCharArray().OrderBy(x => rnd.Next()).ToArray());

			return parola;
		}

		public static int YasHesapla(DateTime dogumTarihi)
		{
			DateTime bugun = DateTime.Today;
			int yas = bugun.Year - dogumTarihi.Year;

			// Doğum günü bu yıl henüz gelmediyse yaş bir eksilt
			if (dogumTarihi.Date > bugun.AddYears(-yas))
			{
				yas--;
			}

			return yas;
		}
	}
}
