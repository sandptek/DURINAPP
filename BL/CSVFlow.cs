using Entities.DB;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BL
{
	public class CSVFlow
	{
		public enum CSVType
		{
			Kappa = 0,
			Antigen = 1,
			Unknown = 2
		}

		public IFormFile csvFile;
		public CSVType csvType = CSVType.Unknown;
		public List<string> errors = new List<string>();
		public bool canSave = false;

		public CSVFlow(IFormFile csvFile)
		{
			this.csvFile = csvFile;
		}

		private bool IsTrueFormat()
		{
			bool isTrue = false;
			int counter = 0; 

			string line;
			string pattern = ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)";
			string[] rows;


			using (var stream = csvFile.OpenReadStream())
			using (StreamReader sr = new StreamReader(stream))
			{
				while (!sr.EndOfStream)
				{
					line = sr.ReadLine() ?? "";
					rows = Regex.Split(line, pattern);

					// Her değeri tırnaklardan arındır
					for (int i = 0; i < rows.Length; i++)
					{
						rows[i] = rows[i].Trim('\"');
					}

					if (rows.Length > 1)
					{
						if (rows[0] == "DataType:")
						{
							if (rows[1] == "Median")
							{
								counter++;
							}
							else if (rows[1] == "Avg Result")
							{
								counter++;
							}
							else if (rows[1] == "%CV Replicates")
							{
								counter++;
							}
						}
						else if (rows[0] == "ProtocolName")
						{
							counter++;
						}
					}
				}
			}

			if (counter >= 4)
			{
				isTrue = true;
			}

			if (!isTrue)
			{
				errors.Add("Cannot read CSV Format. [name: " + csvFile.Name + "]");
			}

			return isTrue;
		}

		private bool GetCSVType()
		{
			CSVType csvType = CSVType.Unknown;

			string line;
			string pattern = ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)";
			string[] rows;
			using (var stream = csvFile.OpenReadStream())
			using (StreamReader sr = new StreamReader(stream))
			{
				while (!sr.EndOfStream)
				{
					line = sr.ReadLine() ?? "";
					rows = Regex.Split(line, pattern);

					// Her değeri tırnaklardan arındır
					for (int i = 0; i < rows.Length; i++)
					{
						rows[i] = rows[i].Trim('\"');
					}

					if (rows.Length > 1)
					{
						if (rows[0] == "ProtocolName")
						{
							if (rows[1].Contains("Antigen"))
								csvType = CSVType.Antigen;
							else if (rows[1].Contains("Kappa"))
								csvType = CSVType.Kappa;
						}
					}
				}
			}

			if (csvType == CSVType.Unknown)
			{
				errors.Add("Cannot read CSV Type. [name: " + csvFile.Name + "]");
			}

			this.csvType = csvType;
			return csvType != CSVType.Unknown;
		}

		private bool CheckMedian()
		{
			bool isTrue = true;
			bool isMedian = false;
			double value = -999;

			int rowCounter = 0;
			int controlRow = 0;

			int lineCounter = 0;
			string line;
			string pattern = ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)";
			string[] rows;
			using (var stream = csvFile.OpenReadStream())
			using (StreamReader sr = new StreamReader(stream))
			{
				while (!sr.EndOfStream)
				{
					line = sr.ReadLine() ?? "";
					lineCounter++;
					rows = Regex.Split(line, pattern);

					// Her değeri tırnaklardan arındır
					for (int i = 0; i < rows.Length; i++)
					{
						rows[i] = rows[i].Trim('\"');
					}

					if (rows.Length > 1)
					{
						if (rows[0] == "DataType:")
						{
							if (rows[1] == "Median")
							{
								isMedian = true;
							}
							else
							{
								isMedian = false;
							}
						}

						if (rows.Count(x => !string.IsNullOrWhiteSpace(x)) == 0)
						{
							continue;
						}

						if (isMedian)
						{
							rowCounter++;
							if (rowCounter == 2)//Başlıklar
							{
								if (csvType == CSVType.Kappa)
								{
									controlRow = Array.IndexOf(rows, "Analyte 45");
								}
								else if (csvType == CSVType.Antigen)
								{
									controlRow = Array.IndexOf(rows, "IC45 Control");
								}
							}

							if (rowCounter > 2)
							{
								if (rows[1].Contains("Stand") || rows[0].Contains("Neg") || rows[0].Contains("Pos") || rows[0].Contains("No"))
								{
									continue;
								}

								double.TryParse(rows[controlRow], CultureInfo.InvariantCulture, out value);
								if (value == -999)
								{
									errors.Add("Cannot read Median value. [file: " + csvType.ToString("G") + " | line: " + lineCounter + " | location: " + rows[0] + " | sample: " + rows[1] + "]");
									isTrue = false;
								}
								else if(value < 8000)
								{
									errors.Add("Median value is lower than 8000. [file: " + csvType.ToString("G") + " | line: " + lineCounter + " | location: " + rows[0] + " | sample: " + rows[1] + " | value: " + value + "]");
									isTrue = false;
								}
								 
							}
						}
					}
				}
			}

			return isTrue;
		}

		private bool CheckAVGResult()
		{
			bool isTrue = true;
			bool isAVGResult = false;
			double value = -999;

			int rowCounter = 0;
			List<int> controlRows = new List<int>();
			List<string> headerRows = new List<string>();

			int lineCounter = 0;
			string line;
			string pattern = ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)";
			string[] rows;
			using (var stream = csvFile.OpenReadStream())
			using (StreamReader sr = new StreamReader(stream))
			{
				while (!sr.EndOfStream)
				{
					line = sr.ReadLine() ?? "";
					lineCounter++;
					rows = Regex.Split(line, pattern);

					// Her değeri tırnaklardan arındır
					for (int i = 0; i < rows.Length; i++)
					{
						rows[i] = rows[i].Trim('\"');
					}

					if (rows.Length > 1)
					{
						if (rows[0] == "DataType:")
						{
							if (rows[1] == "Avg Result")
							{
								isAVGResult = true;
							}
							else
							{
								isAVGResult = false;
							}
						}

						if (isAVGResult)
						{
							rowCounter++;
							if (rowCounter == 2)//Başlıklar
							{
								headerRows = rows.ToList();
								if (csvType == CSVType.Kappa)
								{
									controlRows.Add(Array.IndexOf(rows, "Anti-Kappa IgG"));
								}
								else if (csvType == CSVType.Antigen)
								{
									controlRows.Add(Array.IndexOf(rows, "MGC31944"));
									controlRows.Add(Array.IndexOf(rows, "CCL19"));
									controlRows.Add(Array.IndexOf(rows, "DNAJC8"));
									controlRows.Add(Array.IndexOf(rows, "LGALS1"));
									controlRows.Add(Array.IndexOf(rows, "MARK1"));
									controlRows.Add(Array.IndexOf(rows, "PUSL1"));
									controlRows.Add(Array.IndexOf(rows, "IL20"));
								}
							}

							if (rows.Count(x => !string.IsNullOrWhiteSpace(x)) == 0)
							{
								continue;
							}

							if (rowCounter > 2)
							{
								if (rows[0].Contains("Stand") || rows[0].Contains("Neg") || rows[0].Contains("Pos") || rows[0].Contains("No"))
								{
									continue;
								}

								foreach (var controlRow in controlRows)
								{
									double.TryParse(rows[controlRow], CultureInfo.InvariantCulture, out value);
									if (value == -999)
									{
										errors.Add("Cannot read Avg Result value. [file: " + csvType.ToString("G") + " | line: " + lineCounter + " | sample: " + rows[0] + " | column: " + headerRows[controlRow] + "]");
										isTrue = false;
									}
									else if (value < 0)
									{
										errors.Add("Avg Result value is lower than 0. [file: " + csvType.ToString("G") + " | line: " + lineCounter + " | sample: " + rows[0] + " | column: " + headerRows[controlRow] + " | value: " + value + "]");
										isTrue = false;
									}
									else if (value > 1.2)
									{
										errors.Add("Avg Result value is higher than 1.2. [file: " + csvType.ToString("G") + " | line: " + lineCounter + " | sample: " + rows[0] + " | column: " + headerRows[controlRow] + " | value: " + value + "]");
										isTrue = false;
									}
								}
							}
						}
					}
				}
			}
			return isTrue;
		}

		private bool CheckCVReplicates()
		{
			bool isTrue = true;
			bool isCVReplicates = false;
			double value = -999;

			int rowCounter = 0;
			List<int> controlRows = new List<int>();
			List<string> headerRows = new List<string>();

			int lineCounter = 0;
			string line;
			string pattern = ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)";
			string[] rows;
			using (var stream = csvFile.OpenReadStream())
			using (StreamReader sr = new StreamReader(stream))
			{
				while (!sr.EndOfStream)
				{
					line = sr.ReadLine() ?? "";
					lineCounter++;
					rows = Regex.Split(line, pattern);

					// Her değeri tırnaklardan arındır
					for (int i = 0; i < rows.Length; i++)
					{
						rows[i] = rows[i].Trim('\"');
					}

					if (rows.Length > 1)
					{
						if (rows[0] == "DataType:")
						{
							if (rows[1] == "%CV Replicates")
							{
								isCVReplicates = true;
							}
							else
							{
								isCVReplicates = false;
							}
						}

						if (rows.Count(x => !string.IsNullOrWhiteSpace(x)) == 0)
						{
							continue;
						}

						if (isCVReplicates)
						{
							rowCounter++;
							if (rowCounter == 2)//Başlıklar
							{
								headerRows = rows.ToList();
								if (csvType == CSVType.Kappa)
								{
									controlRows.Add(Array.IndexOf(rows, "Anti-Kappa IgG"));
								}
								else if (csvType == CSVType.Antigen)
								{
									controlRows.Add(Array.IndexOf(rows, "MGC31944"));
									controlRows.Add(Array.IndexOf(rows, "CCL19"));
									controlRows.Add(Array.IndexOf(rows, "DNAJC8"));
									controlRows.Add(Array.IndexOf(rows, "LGALS1"));
									controlRows.Add(Array.IndexOf(rows, "MARK1"));
									controlRows.Add(Array.IndexOf(rows, "PUSL1"));
									controlRows.Add(Array.IndexOf(rows, "IL20"));
								}
							}

							if (rowCounter > 2)
							{
								if (rows[0].Contains("Stand") || rows[0].Contains("Neg") || rows[0].Contains("Pos") || rows[0].Contains("No"))
								{
									continue;
								}

								foreach (var controlRow in controlRows)
								{
									double.TryParse(rows[controlRow], CultureInfo.InvariantCulture, out value);
									if (value == -999)
									{
										errors.Add("Cannot read %CV Replicates value. [file: " + csvType.ToString("G") + " | line: " + lineCounter + " | sample: " + rows[0] + " | column: " + headerRows[controlRow] + "]");
										isTrue = false;
									}
									else if (value > 20)
									{
										errors.Add("%CV Replicates value is higher than 20. [file: " + csvType.ToString("G") + " | line: " + lineCounter + " | sample: " + rows[0] + " | column: " + headerRows[controlRow] + " | value: " + value + "]");
										isTrue = false;
									}
								}
							}
						}
					}
				}
			}
			return isTrue;
		}

		private bool CheckSampleNames()
		{
			bool isTrue = true;
			bool isAVGResult = false;
			List<int> sampleNames = new List<int>();
			string sampleName = "";

			int rowCounter = 0;

			int lineCounter = 0;
			string line;
			string pattern = ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)";
			string[] rows;


			using (var context = new DAL.DurinDBContext())
			{
				using (var stream = csvFile.OpenReadStream())
				using (StreamReader sr = new StreamReader(stream))
				{
					while (!sr.EndOfStream)
					{
						line = sr.ReadLine() ?? "";
						lineCounter++;
						rows = Regex.Split(line, pattern);

						// Her değeri tırnaklardan arındır
						for (int i = 0; i < rows.Length; i++)
						{
							rows[i] = rows[i].Trim('\"');
						}

						if (rows.Length > 1)
						{
							if (rows[0] == "DataType:")
							{
								if (rows[1] == "Avg Result")
								{
									isAVGResult = true;
								}
								else
								{
									isAVGResult = false;
								}
							}

							if (isAVGResult)
							{
								rowCounter++;

								if (rows.Count(x => !string.IsNullOrWhiteSpace(x)) == 0)
								{
									continue;
								}

								if (rowCounter > 2)
								{
									if (rows[0].Contains("Stand") || rows[0].Contains("Neg") || rows[0].Contains("Pos") || rows[0].Contains("No"))
									{
										continue;
									}

									if (csvType == CSVType.Kappa)
									{
										sampleName = rows[0];
									}
									else if (csvType == CSVType.Antigen)
									{
										sampleName = rows[0];
									}
									sampleNames.Add(Helper.ToOriginalNumber(sampleName));

									if (!context.OrderItems.Any(x => !x.deleted && !x.order.deleted && x.id == Helper.ToOriginalNumber(sampleName)))
									{
										errors.Add("Sample isn't have in Durin Tests. [file: " + csvType.ToString("G") + " | line: " + lineCounter + " | sample: " + rows[0] + "]");
										isTrue = false;
									}
								}
							}
						}
					}
				}
			}

			var duplicates = sampleNames.GroupBy(x => x).Where(g => g.Count() > 1).Select(g => g.Key).ToList();
			if (duplicates.Any())
			{
				errors.Add("Repeating sample name present. [file: " + csvType.ToString("G") + " | line: " + lineCounter + " | samples: " + string.Join(", ", duplicates) + "]");
			}

			return isTrue;
		}

		public bool CheckFile(bool sampleName = true)
		{
			bool isTrue = true;
			bool isSample = true;

			isTrue = IsTrueFormat();
			if (!isTrue) return isTrue;

			isTrue = GetCSVType();
			if (!isTrue) return isTrue;

			isTrue = CheckMedian();
			isTrue = CheckAVGResult();
			isTrue = CheckCVReplicates();

			if (!sampleName)
				isTrue = true;

			if (sampleName)
				isSample = CheckSampleNames();
			isTrue = isSample;

			if (/*errors.Count > 2*/!isSample)
			{
				canSave = false;
			}
			else
			{
				canSave = true;
			}


			return isTrue;
		}

		public bool ReadAndSaveValues()
		{
			bool isTrue = true;
			isTrue = IsTrueFormat();
			if (!isTrue) return isTrue;
			isTrue = GetCSVType();
			if (!isTrue) return isTrue;


			bool isAVGResult = false;
			double value = -999;

			int rowCounter = 0;
			List<int> controlRows = new List<int>();
			List<string> headerRows = new List<string>();
			string sample = "";

			int lineCounter = 0;
			string line;
			string pattern = ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)";
			string[] rows;
			using (var context = new DAL.DurinDBContext())
			{
				Plate plate = new Plate();
				context.Plates.Add(plate);


				var uploadsFolder = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "plateFiles");
				if (!Directory.Exists(uploadsFolder))
				{
					Directory.CreateDirectory(uploadsFolder);
				}
				uploadsFolder = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "plateFiles", plate.id.ToString());
				if (!Directory.Exists(uploadsFolder))
				{
					Directory.CreateDirectory(uploadsFolder);
				}
				var fileExtension = System.IO.Path.GetExtension(csvFile.FileName);
				var randomFileName = $"{Guid.NewGuid()}{fileExtension}";
				var filePath = System.IO.Path.Combine(uploadsFolder, randomFileName);
				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					_ = csvFile.CopyToAsync(stream);
				}
				if (csvType == CSVType.Kappa)
				{
					plate.kappaFile = randomFileName;
				}
				else if (csvType == CSVType.Antigen)
				{
					plate.antigenFile = randomFileName;
				}

				using (var stream = csvFile.OpenReadStream())
				using (StreamReader sr = new StreamReader(stream))
				{
					while (!sr.EndOfStream)
					{
						line = sr.ReadLine() ?? "";
						lineCounter++;
						rows = Regex.Split(line, pattern);

						// Her değeri tırnaklardan arındır
						for (int i = 0; i < rows.Length; i++)
						{
							rows[i] = rows[i].Trim('\"');
						}

						if (rows.Length > 1)
						{
							if (rows[0] == "DataType:")
							{
								if (rows[1] == "Avg Result")
								{
									isAVGResult = true;
								}
								else
								{
									isAVGResult = false;
								}
							}

							if (isAVGResult)
							{
								rowCounter++;
								if (rowCounter == 2)//Başlıklar
								{
									headerRows = rows.ToList();
									if (csvType == CSVType.Kappa)
									{
										controlRows.Add(Array.IndexOf(rows, "Anti-Kappa IgG"));
									}
									else if (csvType == CSVType.Antigen)
									{
										controlRows.Add(Array.IndexOf(rows, "MGC31944"));
										controlRows.Add(Array.IndexOf(rows, "CCL19"));
										controlRows.Add(Array.IndexOf(rows, "DNAJC8"));
										controlRows.Add(Array.IndexOf(rows, "LGALS1"));
										controlRows.Add(Array.IndexOf(rows, "MARK1"));
										controlRows.Add(Array.IndexOf(rows, "PUSL1"));
										controlRows.Add(Array.IndexOf(rows, "IL20"));
									}
								}

								if (rows.Count(x => !string.IsNullOrWhiteSpace(x)) == 0)
								{
									continue;
								}

								if (rowCounter > 2)
								{
									if (rows[0].Contains("Stand") || rows[0].Contains("Neg") || rows[0].Contains("Pos") || rows[0].Contains("No"))
									{
										continue;
									}

									foreach (var controlRow in controlRows)
									{
										double.TryParse(rows[controlRow], CultureInfo.InvariantCulture, out value);
										if (csvType == CSVType.Kappa)
										{
											sample = rows[0];
										}
										else if (csvType == CSVType.Antigen)
										{
											sample = rows[0];
										}
										
										if (context.OrderItems.Any(x => !x.deleted && !x.order.deleted && x.id == Helper.ToOriginalNumber(sample)))
										{
											var orderItem = context.OrderItems.Include(x => x.order).FirstOrDefault(x => !x.deleted && !x.order.deleted && x.id == Helper.ToOriginalNumber(sample));
											orderItem.plate = plate;
											switch (headerRows[controlRow])
											{
												case "MGC31944":
													orderItem.MGC31944 = value;
													break;
												case "CCL19":
													orderItem.CCL19 = value;
													break;
												case "DNAJC8":
													orderItem.DNAJC8 = value;
													break;
												case "LGALS1":
													orderItem.LGALS1 = value;
													break;
												case "MARK1":
													orderItem.MARK1 = value;
													break;
												case "PUSL1":
													orderItem.PUSL1 = value;
													break;
												case "IL20":
													orderItem.IL20 = value;
													break;
												case "Anti-Kappa IgG":
													orderItem.KAPPA = value;
													break;
											}
											orderItem.updatedDate = DateTime.Now;
											context.SaveChanges();

											bool orderIsOk = true;
											if (orderItem.MGC31944 != null && orderItem.CCL19 != null && orderItem.DNAJC8 != null && orderItem.LGALS1 != null && orderItem.MARK1 != null && orderItem.PUSL1 != null && orderItem.IL20 != null && orderItem.KAPPA != null)
											{
												var orderItems = context.OrderItems.Where(x => !x.deleted && x.order.id == orderItem.order.id);
												foreach (var item in orderItems)
												{
													if (!(item.MGC31944 != null && item.CCL19 != null && item.DNAJC8 != null && item.LGALS1 != null && item.MARK1 != null && item.PUSL1 != null && item.IL20 != null && item.KAPPA != null))
													{
														orderIsOk = false;
													}
												}
												if (orderIsOk)
												{
													var order = context.Orders.FirstOrDefault(x => x.id == orderItem.order.id);
													if (order.status != Entities.DB.Order.Status.mdControl)
													{
														order.status = Entities.DB.Order.Status.mlaStart;
														order.updatedDate = DateTime.Now;
														context.SaveChanges();
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
			return isTrue;
		}
		
		public bool SetValues(List<Entities.ADNI> adnis)
		{
			bool isTrue = true;
			bool isAVGResult = false;
			double value = -999;

			int rowCounter = 0;
			List<int> controlRows = new List<int>();
			List<string> headerRows = new List<string>();
			string sample = "";

			int lineCounter = 0;
			string line;
			string pattern = ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)";
			string[] rows;

			using (var stream = csvFile.OpenReadStream())
			using (StreamReader sr = new StreamReader(stream))
			{
				while (!sr.EndOfStream)
				{
					line = sr.ReadLine() ?? "";
					lineCounter++;
					rows = Regex.Split(line, pattern);

					// Her değeri tırnaklardan arındır
					for (int i = 0; i < rows.Length; i++)
					{
						rows[i] = rows[i].Trim('\"');
					}

					if (rows.Length > 1)
					{
						if (rows[0] == "DataType:")
						{
							if (rows[1] == "Avg Result")
							{
								isAVGResult = true;
							}
							else
							{
								isAVGResult = false;
							}
						}

						if (isAVGResult)
						{
							rowCounter++;
							if (rowCounter == 2)//Başlıklar
							{
								headerRows = rows.ToList();
								if (csvType == CSVType.Kappa)
								{
									controlRows.Add(Array.IndexOf(rows, "Anti-Kappa IgG"));
								}
								else if (csvType == CSVType.Antigen)
								{
									controlRows.Add(Array.IndexOf(rows, "MGC31944"));
									controlRows.Add(Array.IndexOf(rows, "CCL19"));
									controlRows.Add(Array.IndexOf(rows, "DNAJC8"));
									controlRows.Add(Array.IndexOf(rows, "LGALS1"));
									controlRows.Add(Array.IndexOf(rows, "MARK1"));
									controlRows.Add(Array.IndexOf(rows, "PUSL1"));
									controlRows.Add(Array.IndexOf(rows, "IL20"));
								}
							}

							if (rows.Count(x => !string.IsNullOrWhiteSpace(x)) == 0)
							{
								continue;
							}

							if (rowCounter > 2)
							{
								if (rows[0].Contains("Stand") || rows[0].Contains("Neg") || rows[0].Contains("Pos") || rows[0].Contains("No"))
								{
									continue;
								}

								foreach (var controlRow in controlRows)
								{
									double.TryParse(rows[controlRow], CultureInfo.InvariantCulture, out value);
									if (csvType == CSVType.Kappa)
									{
										sample = rows[0];
									}
									else if (csvType == CSVType.Antigen)
									{
										sample = rows[0];
									}

									Entities.ADNI adni;
									if (adnis.Any(x => x.SAMPLE == sample))
									{
										adni = adnis.FirstOrDefault(x => x.SAMPLE == sample);
									}
									else
									{
										adni = new Entities.ADNI();
										adni.SAMPLE = sample;
										adnis.Add(adni);
									}

									switch (headerRows[controlRow])
									{
										case "MGC31944":
											adni.MGC31944 = value;
											break;
										case "CCL19":
											adni.CCL19 = value;
											break;
										case "DNAJC8":
											adni.DNAJC8 = value;
											break;
										case "LGALS1":
											adni.LGALS1 = value;
											break;
										case "MARK1":
											adni.MARK1 = value;
											break;
										case "PUSL1":
											adni.PUSL1 = value;
											break;
										case "IL20":
											adni.IL20 = value;
											break;
										case "Anti-Kappa IgG":
											adni.KAPPA = value;
											break;
									}
								}
							}
						}
					}
				}
			}
			return isTrue;
		}
	}
}
