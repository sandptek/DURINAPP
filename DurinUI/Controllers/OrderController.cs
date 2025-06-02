using BL;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Entities.DB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Data;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Net;
using System.Reflection.PortableExecutable;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;

namespace DurinUI.Controllers
{
    [Authorize(AuthenticationSchemes = "Admin")]
    public class OrderController : Controller
	{
		private readonly IWebHostEnvironment _webHostEnvironment;

		public OrderController(IWebHostEnvironment webHostEnvironment)
		{
			_webHostEnvironment = webHostEnvironment;
		}

		public IActionResult Index()
        {
            return View();
        }

		public IActionResult Plate()
		{
			return View();
		}

        public IActionResult UploadTest(IFormFile[] csvFile)
        {
            List<string> errors = new List<string>();
            bool canSave = false;

			if (csvFile.Length != 2)
            {
                errors.Add("Please upload 2 files.");
			}
            else
			{
				CSVFlow csvFlow1 = new CSVFlow(csvFile[0]);
				CSVFlow csvFlow2 = new CSVFlow(csvFile[1]);
				bool csv1 = csvFlow1.CheckFile();
				bool csv2 = csvFlow2.CheckFile(); 
                canSave = csvFlow1.canSave && csvFlow2.canSave;

				if (csvFlow1.csvType == csvFlow2.csvType)
				{
					errors.Add("File type is same, please upload different files.");
				}
                else
				{
					errors.AddRange(csvFlow1.errors);
					errors.AddRange(csvFlow2.errors);
				}

			}
			return Ok(new { errors, canSave });

		}
		
		public IActionResult SaveTest(IFormFile[] csvFile)
		{
			CSVFlow csvFlow1 = new CSVFlow(csvFile[0]);
			CSVFlow csvFlow2 = new CSVFlow(csvFile[1]);
			bool csv1 = csvFlow1.ReadAndSaveValues();
			bool csv2 = csvFlow2.ReadAndSaveValues();
			return Ok();

		}

		public async Task<IActionResult> UploadTestRequestFile()
        {
            if (HttpContext.Request.Form.Files.Count <= 0)
                return BadRequest();

            if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\media")))
            {
                Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\media"));
            }

            IFormFile file = HttpContext.Request.Form.Files[0];
            string fileExt = Path.GetExtension(file.FileName);
            var fileName = Guid.NewGuid() + fileExt;
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\media", fileName);

            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
                stream.Close();
            }

            return Ok(fileName);
        }

        public IActionResult SetOrderPayment(int orderID, string transaction)
        {
            int userID = Convert.ToInt32(User.Claims.Where(x => x.Type == System.Security.Claims.ClaimTypes.NameIdentifier).First().Value);

            using (var context = new DAL.DurinDBContext())
            {
                var order = context.Orders.First(x => x.id == orderID);
                order.paypalTransaction = transaction;
                order.paypalPaidDate = DateTime.Now;
                order.status = Entities.DB.Order.Status.paid;
                order.updatedDate = DateTime.Now;
                order.updatedUserID = userID;
                context.Orders.Update(order);
                context.SaveChanges();
            }

            return Ok();
        }

        public IActionResult GetOrderPrice(int orderID)
        {
            using (var context = new DAL.DurinDBContext())
            {
                var orderItems = context.OrderItems.Where(x => x.order.id == orderID).ToList();

				if (orderItems.Any(x => string.IsNullOrWhiteSpace(x.mnf) || string.IsNullOrWhiteSpace(x.lisic) || string.IsNullOrWhiteSpace(x.trf)))
				{
					return BadRequest("Plese upload all files");
				}

				return Ok(orderItems.Sum(x => x.price));
            }

        }

        public IActionResult CreateShipment(int orderID)
        {
            using (var context = new DAL.DurinDBContext())
            {
                var order = context.Orders.Include(x => x.doctor).Include(x => x.patient).FirstOrDefault(x => x.id == orderID);
                var orderItems = context.OrderItems.Include(x => x.product).Where(x => !x.deleted && x.order.id == orderID);
                BL.FedEx.Conn fedEx = new BL.FedEx.Conn();
                if (fedEx.Authorization())
                {
                    BL.FedEx.Models.Response.CreateShipment? createShipment = fedEx.CreateShipment(new BL.FedEx.Models.Request.CreateShipment
                    {
                        requestedShipment = new BL.FedEx.Models.Request.CreateShipmentRequestRequestedShipment
                        {
                            //shipDatestamp = "2024-12-10",
                            shipper = new BL.FedEx.Models.Request.CreateShipmentRequestRequestedShipmentShipper
                            {
                                address = new BL.FedEx.Models.Request.CreateShipmentRequestRequestedShipmentShipperAddress
                                {
                                    streetLines = Helper.StringParcala(order.doctor.address, 30),
                                    city = order.doctor.city,
                                    stateOrProvinceCode = order.doctor.stateOrProvinceCode,
                                    countryCode = order.doctor.countryCode,
                                    postalCode = order.doctor.postalCode
                                },
                                contact = new BL.FedEx.Models.Request.CreateShipmentRequestRequestedShipmentShipperContact
                                {
                                    personName = order.doctor.firstname + " " + order.doctor.lastname,
                                    phoneNumber = order.doctor.phone
                                }
                            },
                            recipients = new List<BL.FedEx.Models.Request.CreateShipmentRequestRequestedShipmentRecipient>
                            {
                                new BL.FedEx.Models.Request.CreateShipmentRequestRequestedShipmentRecipient
                                {
                                    address = new BL.FedEx.Models.Request.CreateShipmentRequestRequestedShipmentRecipientAddress
                                    {
                                        streetLines = Helper.StringParcala(DAL.AppConfig.settings.fedEx.streetLines, 30),
                                        city = DAL.AppConfig.settings.fedEx.city,
                                        stateOrProvinceCode = DAL.AppConfig.settings.fedEx.stateOrProvinceCode,
                                        countryCode = DAL.AppConfig.settings.fedEx.countryCode,
                                        postalCode = DAL.AppConfig.settings.fedEx.postalCode,
                                        residential = DAL.AppConfig.settings.fedEx.residential,
                                    },
                                    contact = new BL.FedEx.Models.Request.CreateShipmentRequestRequestedShipmentRecipientContact
                                    {
                                        personName = DAL.AppConfig.settings.fedEx.personName,
                                        phoneNumber = DAL.AppConfig.settings.fedEx.personNumber
                                    }
                                }
                            },
                            pickupType = "USE_SCHEDULED_PICKUP",
                            serviceType = "PRIORITY_OVERNIGHT",
                            packagingType = "YOUR_PACKAGING",
                            /*shipmentSpecialServices =  new BL.FedEx.Models.Request.CreateShipmentRequestRequestedShipmentShipmentSpecialServices { 
                                specialServiceTypes = new List<string> { "DRY_ICE" },
                                shipmentDryIceDetail = new BL.FedEx.Models.Request.CreateShipmentRequestRequestedShipmentShipmentSpecialServicesshipmentDryIceDetail
                                {
                                    totalWeight = new BL.FedEx.Models.Request.CreateShipmentRequestRequestedShipmentShipmentSpecialServicesshipmentDryIceDetailTotalWeight
                                    {
                                        units = "LB",
                                        value = 0.5
                                    },
                                    packageCount = 1
                                }
                            },*/
                            shippingChargesPayment = new BL.FedEx.Models.Request.CreateShipmentRequestRequestedShipmentShippingChargesPayment
                            {
                                paymentType = "SENDER",
                                payor = new BL.FedEx.Models.Request.CreateShipmentRequestRequestedShipmentShippingChargesPaymentPayor
                                {
                                    responsibleParty = new BL.FedEx.Models.Request.CreateShipmentRequestRequestedShipmentShippingChargesPaymentPayorResponsibleParty
                                    {
                                        accountNumber = new BL.FedEx.Models.Request.CreateShipmentRequestRequestedShipmentShippingChargesPaymentPayorResponsiblePartyAccountNumber
                                        {
                                            value = DAL.AppConfig.settings.fedEx.accountNumber
                                        }
                                    }
                                }
                            },
                            labelSpecification = new BL.FedEx.Models.Request.CreateShipmentRequestRequestedShipmentLabelSpecification
                            {
                                labelStockType = "PAPER_4X6",
                                imageType = "PNG"
                            },
                            requestedPackageLineItems = new List<BL.FedEx.Models.Request.CreateShipmentRequestRequestedShipmentRequestedPackageLineItems>
                            {
                                new BL.FedEx.Models.Request.CreateShipmentRequestRequestedShipmentRequestedPackageLineItems
                                {
                                    weight = new BL.FedEx.Models.Request.CreateShipmentRequestRequestedShipmentRequestedPackageLineItemsWeight
                                    {
                                        units = "LB",
                                        value = 1
                                    },
                                    declaredValue = new BL.FedEx.Models.Request.CreateShipmentRequestRequestedShipmentRequestedPackageLineItemsDeclaredValue
                                    {
                                        amount = Convert.ToDouble(orderItems.Sum(x => x.product.declaredValue)),
                                        currency = "USD"
                                    },
                                    customerReferences = new List<BL.FedEx.Models.Request.CreateShipmentRequestRequestedShipmentRequestedPackageLineItemsCustomerReferences>
                                    {
                                        new BL.FedEx.Models.Request.CreateShipmentRequestRequestedShipmentRequestedPackageLineItemsCustomerReferences
                                        {
                                            customerReferenceType = "CUSTOMER_REFERENCE",
                                            value = "DURIN-" + orderID
                                        }
                                    },
                                    packageSpecialServices = new BL.FedEx.Models.Request.CreateShipmentRequestRequestedShipmentRequestedPackageLineItemsPackageSpecialServices {
                                        specialServiceTypes = new List<string>{ "DRY_ICE" },
                                        dryIceWeight = new BL.FedEx.Models.Request.CreateShipmentRequestRequestedShipmentRequestedPackageLineItemsPackageSpecialServicesDryIceWeight
                                        {
                                            units = "LB",
                                            value = 0.5
                                        }
                                    }
                                }
                            },
                            totalDeclaredValue = new BL.FedEx.Models.Request.CreateShipmentRequestRequestedShipmentTotalDeclaredValue
                            {
                                amount = Convert.ToDouble(orderItems.Sum(x => x.product.declaredValue)),
                                currency = "USD"
                            }
                        },
                        labelResponseOptions = "URL_ONLY",
                        accountNumber = new BL.FedEx.Models.Request.CreateShipmentRequestAccountNumber
                        {
                            value = DAL.AppConfig.settings.fedEx.accountNumber
                        }
                    });

                    if (createShipment != null)
                    {
                        string fedExRoot = "wwwroot\\FedEx\\";
                        string orderPath = Path.Combine(Directory.GetCurrentDirectory(), fedExRoot);
                        if (!Directory.Exists(orderPath))
                        {
                            Directory.CreateDirectory(orderPath);
                        }
                        string itemPath = orderPath + orderID + "\\";
                        if (!Directory.Exists(itemPath))
                        {
                            Directory.CreateDirectory(itemPath);
                        }
                        string json = itemPath + createShipment.output.transactionShipments.First().masterTrackingNumber + ".json";
                        System.IO.File.WriteAllText(json, JsonConvert.SerializeObject(createShipment));

                        List<string> trackNumbers = new List<string>();
                        foreach (var item in createShipment.output.transactionShipments.First().pieceResponses)
                        {
                            using (WebClient client = new WebClient())
                            {
                                client.DownloadFile(new Uri(item.packageDocuments.First().url), orderPath + orderID + "\\" + item.trackingNumber + ".PNG");
                            }
                            trackNumbers.Add(item.trackingNumber);
                        }

                        order.shippingNumber = String.Join(",", trackNumbers);
                        order.carrier = "FedEx";
                        order.status = Entities.DB.Order.Status.shipped;
                        context.Orders.Update(order);
                        context.SaveChanges();
                    }
                    else
                    {
                        return BadRequest(string.Join(" || ", fedEx.error.errors.Select(x => "[[" + x.code + "]]" + x.message)));
                    }
                }
                else
                {
                    return BadRequest(string.Join(" || ", fedEx.error.errors.Select(x => "[[" + x.code + "]]" + x.message)));
                }
            }
            return Ok();
        }

        public IActionResult GetOrderItems()
        {
            using (var context = new DAL.DurinDBContext())
            {
                var orderItems = context.OrderItems
                    .Include(x => x.product)
                    .Include(x => x.order)
                    .Where(x => 
                        !x.deleted 
                        && !x.order.deleted 
                        && (x.order.status == Entities.DB.Order.Status.delivered || x.order.status == Entities.DB.Order.Status.inProcess || x.order.status == Entities.DB.Order.Status.qualityControl)
                    ).ToList();
                return Ok(orderItems);
            }
        }

		[HttpPost]
		public IActionResult GetCSV1(int[] selectedItems)
		{
			// Örnek veri
			var data = new List<string[]>
			{
				new string[] { "Pos Cntrl" },
				new string[] { "Neg Cntrl" },
				new string[] { "No Ab" },
			};
            foreach (var item in selectedItems)
			{
				data.Add(new string[] { Helper.ToMixedNumber(item) });
			}

			// CSV içeriğini oluştur

			var sb = new StringBuilder();
			foreach (var row in data)
			{
				sb.AppendLine(string.Join(",", row));
			}
			var csvContent = sb.ToString();

			// CSV dosyasını indirilebilir olarak gönder
			var fileName = "data.csv";
			var fileBytes = Encoding.UTF8.GetBytes(csvContent);

			// Dosya içeriği geri döndürülür
			return File(fileBytes, "text/csv", fileName);
		}
		[HttpPost]
		public IActionResult GetCSV2(int[] selectedItems)
		{
			// Örnek veri
			var data = new List<string[]>
			{
				new string[] { "Control Positive 1_AntiKappa_2plex" },
				new string[] { "Control Positive 2_AntiKappa_2plex" },
				new string[] { "Control Negative 1_AntiKappa_2plex" },
				new string[] { "Control Negative 2_AntiKappa_2plex" }
			};
			foreach (var item in selectedItems)
			{
				data.Add(new string[] { Helper.ToMixedNumber(item) + "_AntiKappa_2plex" });
			}

			// CSV içeriğini oluştur

			var sb = new StringBuilder();
			foreach (var row in data)
			{
				sb.AppendLine(string.Join(",", row));
			}
			var csvContent = sb.ToString();

			// CSV dosyasını indirilebilir olarak gönder
			var fileName = "data.csv";
			var fileBytes = Encoding.UTF8.GetBytes(csvContent);

			// Dosya içeriği geri döndürülür
			return File(fileBytes, "text/csv", fileName);
		}
        [HttpPost]
        public IActionResult GetXLSX(int[] selectedItems)
        {
			// Dosya yolu
			string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "formTemplates", "PlateMapTemplateV2.xlsx");
			var newFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "plateMaps", "PlateMap_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xlsx"); // Burada mevcut dosyanın yolunu belirleyin
			System.IO.File.Copy(filePath, newFilePath, true);  // Ana dosyayı kopyala

			var replacements = new Dictionary<string, string>
			{
				{ "C5", selectedItems.Length >= 1 ? Helper.ToMixedNumber(selectedItems[0]) : "" },
				{ "C6", selectedItems.Length >= 2 ? Helper.ToMixedNumber(selectedItems[1]) : "" },
				{ "C7", selectedItems.Length >= 3 ? Helper.ToMixedNumber(selectedItems[2]) : "" },
				{ "C8", selectedItems.Length >= 4 ? Helper.ToMixedNumber(selectedItems[3]) : "" },
				{ "C9", selectedItems.Length >= 5 ? Helper.ToMixedNumber(selectedItems[4]) : "" },
				{ "C10", selectedItems.Length >= 6 ? Helper.ToMixedNumber(selectedItems[5]) : "" },
				{ "C11", selectedItems.Length >= 7 ? Helper.ToMixedNumber(selectedItems[6]) : "" },
				{ "C12", selectedItems.Length >= 8 ? Helper.ToMixedNumber(selectedItems[7]) : "" },
				{ "C13", selectedItems.Length >= 9 ? Helper.ToMixedNumber(selectedItems[8]) : "" },
				{ "C14", selectedItems.Length >= 10 ? Helper.ToMixedNumber(selectedItems[9]) : "" },
				{ "C15", selectedItems.Length >= 11 ? Helper.ToMixedNumber(selectedItems[10]) : "" },
				{ "C16", selectedItems.Length >= 12 ? Helper.ToMixedNumber(selectedItems[11]) : "" },
				{ "C17", selectedItems.Length >= 13 ? Helper.ToMixedNumber(selectedItems[12]) : "" },
				{ "C18", selectedItems.Length >= 14 ? Helper.ToMixedNumber(selectedItems[13]) : "" },
				{ "C19", selectedItems.Length >= 15 ? Helper.ToMixedNumber(selectedItems[14]) : "" },
				{ "C20", selectedItems.Length >= 16 ? Helper.ToMixedNumber(selectedItems[15]) : "" },
				{ "C21", selectedItems.Length >= 17 ? Helper.ToMixedNumber(selectedItems[16]) : "" },
				{ "C22", selectedItems.Length >= 18 ? Helper.ToMixedNumber(selectedItems[17]) : "" },
				{ "C23", selectedItems.Length >= 19 ? Helper.ToMixedNumber(selectedItems[18]) : "" },
				{ "C24", selectedItems.Length >= 20 ? Helper.ToMixedNumber(selectedItems[19]) : "" },
				{ "C25", selectedItems.Length >= 21 ? Helper.ToMixedNumber(selectedItems[20]) : "" },
				{ "C26", selectedItems.Length >= 22 ? Helper.ToMixedNumber(selectedItems[21]) : "" },
				{ "C27", selectedItems.Length >= 23 ? Helper.ToMixedNumber(selectedItems[22]) : "" },
				{ "C28", selectedItems.Length >= 24 ? Helper.ToMixedNumber(selectedItems[23]) : "" },
				{ "C29", selectedItems.Length >= 25 ? Helper.ToMixedNumber(selectedItems[24]) : "" },
				{ "C30", selectedItems.Length >= 26 ? Helper.ToMixedNumber(selectedItems[25]) : "" },
				{ "C31", selectedItems.Length >= 27 ? Helper.ToMixedNumber(selectedItems[26]) : "" },
				{ "C32", selectedItems.Length >= 28 ? Helper.ToMixedNumber(selectedItems[27]) : "" },
				{ "C33", selectedItems.Length >= 29 ? Helper.ToMixedNumber(selectedItems[28]) : "" },
				{ "C34", selectedItems.Length >= 30 ? Helper.ToMixedNumber(selectedItems[29]) : "" },
				{ "C35", selectedItems.Length >= 31 ? Helper.ToMixedNumber(selectedItems[30]) : "" },
				{ "C36", selectedItems.Length >= 32 ? Helper.ToMixedNumber(selectedItems[31]) : "" },
				{ "C37", selectedItems.Length >= 33 ? Helper.ToMixedNumber(selectedItems[32]) : "" },
				{ "C38", selectedItems.Length >= 34 ? Helper.ToMixedNumber(selectedItems[33]) : "" },
				{ "C39", selectedItems.Length >= 35 ? Helper.ToMixedNumber(selectedItems[34]) : "" },
				{ "C40", selectedItems.Length >= 36 ? Helper.ToMixedNumber(selectedItems[35]) : "" },
				{ "C41", selectedItems.Length >= 37 ? Helper.ToMixedNumber(selectedItems[36]) : "" }
			};

            // Open XML SDK ile Excel dosyasını açma
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(newFilePath, true))
            {
                // Çalışma kitabı ve sayfası
                WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
				Sheet sheet = workbookPart.Workbook.Sheets.Elements<Sheet>().FirstOrDefault(s => s.Name == "Sample List");

				if (sheet != null)
				{
					// SheetId üzerinden WorksheetPart'a eriş
					WorksheetPart worksheetPart = (WorksheetPart)(workbookPart.GetPartById(sheet.Id));

					// Çalışma sayfasındaki hücreyi bulma
					SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

					foreach (Row row in sheetData.Elements<Row>())
					{
						foreach (Cell cell in row.Elements<Cell>())
						{
							// Hücre referansı Dictionary'de varsa, değeri değiştir
							if (replacements.ContainsKey(cell.CellReference.Value))
							{
								// Hücreyi yeni değeriyle güncelleme
								cell.CellValue = new CellValue(replacements[cell.CellReference.Value]);
								cell.DataType = new EnumValue<CellValues>(CellValues.String);
							}
							else if (cell.CellFormula != null)
							{
								// Formül hücresinin güncellenmesine gerek yok, sadece koruyalım
								// (Formülü değiştirmiyoruz)
							}
						}
					}

					// Değişiklikleri kaydetme
					spreadsheetDocument.WorkbookPart.Workbook.CalculationProperties.FullCalculationOnLoad = true;
					//document.WorkbookPart.Workbook.CalculationProperties.ForceFullCalculation = true;
					workbookPart.Workbook.Save();
				}
			}
			byte[] fileBytes = System.IO.File.ReadAllBytes(newFilePath);
			// Güncellenmiş dosyayı kullanıcıya indirme
			return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PlateMap_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xlsx");
		}

		[Authorize(AuthenticationSchemes = "User")]
		public IActionResult GetMLReport(int id)
        {
			using (var context = new DAL.DurinDBContext())
            {
                var orderItem = context.OrderItems.Include(x => x.product).Include(x => x.order).Include(x => x.order.patient).Include(x => x.order.doctor).FirstOrDefault(x => x.id == id);
				
				// Mevcut Word dosyasının yolu
				var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "formTemplates", "MLReportSchema.docx"); // Burada mevcut dosyanın yolunu belirleyin
				var newFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "mlReports", orderItem.id + "_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".docx"); // Burada mevcut dosyanın yolunu belirleyin


                string RESULT = "";
                string RESULT_TEXT = "";
				if (orderItem.mlScore1 >= 0 && orderItem.mlScore1 < 40)
				{
					RESULT = "NEGATIVE";
				}
				else if (orderItem.mlScore1 >= 40 && orderItem.mlScore1 < 68.9)
				{
					RESULT = "INTERMEDIATE";
				}
				else if (orderItem.mlScore1 >= 68.9 && orderItem.mlScore1 <= 100)
				{
					RESULT = "POSITIVE";
				}
				else
				{
					RESULT = "ERROR";
				}

				if (orderItem.mlScore1 >= 0 && orderItem.mlScore1 < 40)
				{
					RESULT_TEXT = "Typical risk of AD pathology detected";
				}
				else if (orderItem.mlScore1 >= 40 && orderItem.mlScore1 < 68.9)
				{
					RESULT_TEXT = "Indeterminate risk of AD pathology detected";
				}
				else if (orderItem.mlScore1 >= 68.9 && orderItem.mlScore1 <= 100)
				{
					RESULT_TEXT = "Increased risk of AD pathology detected";
				}
				else
				{
					RESULT_TEXT = "SCORE ERROR";
				}

				var replacements = new Dictionary<string, string>
				{
					{ "$[PATIENT]", orderItem.order.patient.firstname + " " + orderItem.order.patient.lastname },
					{ "$[DOB]", (orderItem.order.patient.dateOfBirth ?? DateTime.Now).ToString("MM/dd/yyyy") },
					{ "$[NUID]", Helper.ToMixedNumber(orderItem.id) },
					{ "$[PHONE]", orderItem.order.patient.phone },
					{ "$[ORDER_DATE]", orderItem.order.createdDate.ToString("MM/dd/yyyy") },
					{ "$[ORDER_ID]", Helper.ToMixedNumber(orderItem.order.id) },
					{ "$[RESULT]", RESULT },
					{ "$[RESULT_TEXT]", RESULT_TEXT },
					{ "$[RS]", (orderItem.mlScore1 ?? 0 * 10).ToString()},
					{ "$[SAMPLE_ID]", Helper.ToMixedNumber(orderItem.id) },
					{ "$[COLLECTED_DATE]", orderItem.order.createdDate.ToString("MM/dd/yyyy") },
					{ "$[APPROVED_DATE]", (orderItem.order.updatedDate ?? DateTime.Now).ToString("MM/dd/yyyy") },
					{ "$[APPROVED_BY]", orderItem.order.doctor.firstname + " " + orderItem.order.doctor.lastname }
				};

				// Ana dosyayı açma ve yeni dosya olarak kaydetme
				System.IO.File.Copy(filePath, newFilePath, true);  // Ana dosyayı kopyala

				// Word dosyasını açıyoruz
				using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(newFilePath, true))
				{
					var body = wordDoc.MainDocumentPart.Document.Body;
					foreach (var text in body.Descendants<DocumentFormat.OpenXml.Wordprocessing.Text>())
					{
						foreach (var key in replacements.Keys)
						{
							if (text.Text.Contains(key))
							{
								text.Text = text.Text.Replace(key, replacements[key]); // Değeri değiştir
							}
						}
					}
					wordDoc.MainDocumentPart.Document.Save();
				}
				byte[] fileBytes = System.IO.File.ReadAllBytes(newFilePath);

				// Dosyayı kullanıcıya indirmesi için geri gönderiyoruz
				return File(fileBytes, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "MLReport_" + orderItem.id + "_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".docx");
			}
		}

		[Authorize(AuthenticationSchemes = "User")]
		public IActionResult GetMNFReport(int id)
		{
			using (var context = new DAL.DurinDBContext())
			{
				var orderItem = context.OrderItems.Include(x => x.product).Include(x => x.order).Include(x => x.order.patient).Include(x => x.order.doctor).FirstOrDefault(x => x.id == id);

				// Mevcut Word dosyasının yolu
				var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "formTemplates", "mnfTemplate.docx"); // Burada mevcut dosyanın yolunu belirleyin
				var newFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "mnfForms", orderItem.id + "_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".docx"); // Burada mevcut dosyanın yolunu belirleyin

				var replacements = new Dictionary<string, string>
				{
					{ "DOCTOR_NAME", orderItem.order.doctor.firstname + " " + orderItem.order.doctor.lastname },
					{ "DOCTOR_COMPANY", orderItem.order.doctor.company },
					{ "DOCTOR_ADDRESS", orderItem.order.doctor.address },
					{ "DOCTOR_CITY", orderItem.order.doctor.city },
					{ "DOCTOR_STATE", orderItem.order.doctor.stateOrProvinceCode },
					{ "DOCTOR_ZIP_CODE", orderItem.order.doctor.postalCode },

					{ "DATE_NOW", DateTime.Now.ToString("MM/dd/yyyy") },
					{ "TEST_NAME", orderItem.product.type == Product.Type.AD ? "AD" : "PD" },

					{ "PATIENT_NAME", orderItem.order.patient.firstname + " " + orderItem.order.patient.lastname },
					{ "PATIENT_DOB", (orderItem.order.patient.dateOfBirth ?? DateTime.MinValue).ToString("MM/dd/yyyy")},
					{ "PATIENT_AGE", Helper.YasHesapla(orderItem.order.patient.dateOfBirth ?? DateTime.Now).ToString() },
					{ "PATIENT_GENDER", orderItem.order.patient.gender },
					{ "PATIENT_PHONE", orderItem.order.patient.phone }

				};

				// Ana dosyayı açma ve yeni dosya olarak kaydetme
				System.IO.File.Copy(filePath, newFilePath, true);  // Ana dosyayı kopyala

				// Word dosyasını açıyoruz
				using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(newFilePath, true))
				{
					var body = wordDoc.MainDocumentPart.Document.Body;
					foreach (var text in body.Descendants<DocumentFormat.OpenXml.Wordprocessing.Text>())
					{
						foreach (var key in replacements.Keys)
						{
							if (text.Text.Contains(key))
							{
								text.Text = text.Text.Replace(key, replacements[key]); // Değeri değiştir
							}
						}
					}
					wordDoc.MainDocumentPart.Document.Save();
				}
				byte[] fileBytes = System.IO.File.ReadAllBytes(newFilePath);

				// Dosyayı kullanıcıya indirmesi için geri gönderiyoruz
				return File(fileBytes, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "MNFTemplate_" + orderItem.id + "_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".docx");
			}
		}

		[Authorize(AuthenticationSchemes = "User")]
		public async Task<IActionResult> UploadMNF(IFormFile file, int id)
		{
			using (var context = new DAL.DurinDBContext())
			{
				if (file == null || file.Length == 0)
				{
					return new JsonResult(new { message = "No file." });
				}

				var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "mnfForms");

				if (!Directory.Exists(uploadsFolder))
				{
					Directory.CreateDirectory(uploadsFolder);
				}
				var fileExtension = Path.GetExtension(file.FileName);
				var randomFileName = $"UPL_{id}_{DateTime.Now.ToString("ddMMyyyyHHmmss")}{fileExtension}";
				var filePath = Path.Combine(uploadsFolder, randomFileName);

				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					await file.CopyToAsync(stream);
				}

				context.OrderItems.First(x => x.id == id).mnf = randomFileName;
				context.SaveChanges();

				return new JsonResult(new { message = "Medical Necessity Form Uploaded." });
			}
		}

		[Authorize(AuthenticationSchemes = "User")]
		public async Task<IActionResult> UploadTRF(IFormFile file, int id)
		{
			using (var context = new DAL.DurinDBContext())
			{
				if (file == null || file.Length == 0)
				{
					return new JsonResult(new { message = "No file." });
				}

				var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "trfForms");

				if (!Directory.Exists(uploadsFolder))
				{
					Directory.CreateDirectory(uploadsFolder);
				}
				var fileExtension = Path.GetExtension(file.FileName);
				var randomFileName = $"UPL_{id}_{DateTime.Now.ToString("ddMMyyyyHHmmss")}{fileExtension}";
				var filePath = Path.Combine(uploadsFolder, randomFileName);

				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					await file.CopyToAsync(stream);
				}

				context.OrderItems.First(x => x.id == id).trf = randomFileName;
				context.SaveChanges();

				return new JsonResult(new { message = "Test Requisition Form Uploaded." });
			}
		}

		[Authorize(AuthenticationSchemes = "User")]
		public async Task<IActionResult> UploadLISIC(IFormFile file, int id)
		{
			using (var context = new DAL.DurinDBContext())
			{
				if (file == null || file.Length == 0)
				{
					return new JsonResult(new { message = "No file." });
				}

				var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "lisicForms");

				if (!Directory.Exists(uploadsFolder))
				{
					Directory.CreateDirectory(uploadsFolder);
				}
				var fileExtension = Path.GetExtension(file.FileName);
				var randomFileName = $"UPL_{id}_{DateTime.Now.ToString("ddMMyyyyHHmmss")}{fileExtension}";
				var filePath = Path.Combine(uploadsFolder, randomFileName);

				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					await file.CopyToAsync(stream);
				}

				context.OrderItems.First(x => x.id == id).lisic = randomFileName;
				context.SaveChanges();

				return new JsonResult(new { message = "Patient Consent Form Uploaded." });
			}
		}
	}
}
