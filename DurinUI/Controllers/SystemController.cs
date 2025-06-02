using BL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Data;

namespace DurinUI.Controllers
{
	public class SystemController : Controller
	{
		[HttpPost]
		public IActionResult TableOperations(string jsonData)
		{
			string data = jsonData;
			if (!string.IsNullOrEmpty(data))
			{
				JsonSerializerSettings settings = new JsonSerializerSettings();
				settings.Formatting = Formatting.Indented;
				settings.NullValueHandling = NullValueHandling.Include;
				settings.DefaultValueHandling = DefaultValueHandling.Ignore;

				try
				{
					JObject jobject = JObject.Parse(data, new JsonLoadSettings()
					{
						CommentHandling = CommentHandling.Ignore,
						LineInfoHandling = LineInfoHandling.Ignore
					});

					if (jobject != null)
					{
						string type = jobject["type"].ToString();//I = Insert, U = Update, D = Delete, S = Select
						string tableName = jobject["tableName"].ToString();

						List<Property> keys = new List<Property>();
						List<Property> values = new List<Property>();
						List<Property> orders = new List<Property>();
						List<Property> ordersDesc = new List<Property>();

						if (jobject["keys"] != null)
						{
							foreach (JProperty jproperty in (IEnumerable<JToken>)jobject["keys"])
							{
								string key = jproperty.Name;
								var value = JTokenToConventionalDotNetObject(jproperty.Value);

								keys.Add(new Property { key = key, value = value });
							}
						}

						if (jobject["values"] != null)
						{
							foreach (JProperty jproperty in (IEnumerable<JToken>)jobject["values"])
							{
								string key = jproperty.Name;
								var value = JTokenToConventionalDotNetObject(jproperty.Value);

								if (key == "password")
								{
									value = Helper.CreateMD5(jproperty.Value.ToString());
								}

								values.Add(new Property { key = key, value = value });
							}
						}

						string queryString = "";

						SQLConn sqlConn = new SQLConn();

						if (type == "I")
						{
							if (values.Count <= 0)
							{
								return BadRequest("Values is required for [I]nsert.");
							}
							else
							{
								queryString = string.Format("INSERT INTO [{0}] ({1}) VALUES ({2})",
									tableName,
									string.Join(", ", values.Select(x => "[" + x.key + "]")),
									string.Join(", ", values.Select(x => "@" + x.key)));

								List<SqlParameter> parameters = new List<SqlParameter>();
								foreach (var item in values)
								{
									parameters.Add(new SqlParameter(item.key, item.value));
								}

								bool sonuc = sqlConn.Set(queryString, System.Data.CommandType.Text, parameters.ToArray());
								if (sonuc)
								{
									return Ok();
								}
								else
								{
									return BadRequest(sqlConn.lastError);
								}
							}
						}
						else if (type == "U")
						{
							if (values.Count <= 0)
							{
								return BadRequest("Values is required for [U]pdate.");
							}
							else if (keys.Count <= 0)
							{
								return BadRequest("Keys is required for [U]pdate.");
							}
							else
							{
								queryString = string.Format("UPDATE [{0}] SET {1} WHERE 1 = 1 {2}",
									tableName,
									string.Join(", ", values.Select(x => "[" + x.key + "] = @V_" + x.key)),
									" AND " + string.Join(" AND ", keys.Select(x => "[" + x.key + "] " + (x.value is List<object> ? (" IN (" + string.Join(",", x.value) + ")") : (" = @K_" + x.key)))));

								List<SqlParameter> parameters = new List<SqlParameter>();
								foreach (var item in values)
								{
									parameters.Add(new SqlParameter("V_" + item.key, item.value));
								}
								foreach (var item in keys)
								{
									if (item.value is not List<object>)
									{
										parameters.Add(new SqlParameter("K_" + item.key, item.value));
									}
								}

								bool sonuc = sqlConn.Set(queryString, System.Data.CommandType.Text, parameters.ToArray());
								if (sonuc)
								{
									return Ok();
								}
								else
								{
									return BadRequest(sqlConn.lastError);
								}
							}
						}
						else if (type == "D")
						{
							if (keys.Count <= 0)
							{
								return BadRequest("Keys is required for [D]elete.");
							}
							else
							{
								queryString = string.Format("DELETE FROM [{0}] WHERE 1 = 1 {1}",
									tableName,
									" AND " + string.Join(" AND ", keys.Select(x => "[" + x.key + "] " + (x.value is List<object> ? (" IN (" + string.Join(",", x.value) + ")") : (" = @K_" + x.key)))));

								List<SqlParameter> parameters = new List<SqlParameter>();
								foreach (var item in keys)
								{
									if (item.value is not List<object>)
									{
										parameters.Add(new SqlParameter("K_" + item.key, item.value));
									}
								}

								bool sonuc = sqlConn.Set(queryString, System.Data.CommandType.Text, parameters.ToArray());
								if (sonuc)
								{
									return Ok();
								}
								else
								{
									return BadRequest(sqlConn.lastError);
								}
							}
						}
						else if (type == "S")
						{
							if (tableName == "Orders")
							{
								queryString = string.Format("SELECT *, totalPrice = (SELECT SUM(oi.price) FROM OrderItems oi WHERE oi.deleted = 0 AND oi.orderid = Orders.id) FROM [{0}] WHERE 1 = 1 {1}",
										tableName,
										" AND " + string.Join(" AND ", keys.Select(x => "[" + x.key + "] " + (x.value is List<object> ? (" IN (" + string.Join(",", x.value) + ")") : (" = @K_" + x.key))))
										);
							}
							else
							{
								queryString = string.Format("SELECT * FROM [{0}] WHERE 1 = 1 {1}",
										tableName,
										" AND " + string.Join(" AND ", keys.Select(x => "[" + x.key + "] " + (x.value is List<object> ? (" IN (" + string.Join(",", x.value) + ")") : (" = @K_" + x.key))))
										);
							}


							List<SqlParameter> parameters = new List<SqlParameter>();
							foreach (var item in keys)
							{
								if (item.value is not List<object>)
								{
									parameters.Add(new SqlParameter("K_" + item.key, item.value));
								}
							}

							var dt = sqlConn.Get(queryString, CommandType.Text, parameters.ToArray());
							if (string.IsNullOrEmpty(sqlConn.lastError))
							{
								return Ok(dt);
							}
							else
							{
								return BadRequest(sqlConn.lastError);
							}
						}
						else
						{
							return BadRequest("Type is invalid.");
						}
					}
				}
				catch (Exception e)
				{
					return BadRequest(e.Message);
				}
			}

			return Ok();
		}

		public object JTokenToConventionalDotNetObject(JToken token)
		{
			switch (token.Type)
			{
				case JTokenType.Object:
					return ((JObject)token).Properties()
						.ToDictionary(prop => prop.Name, prop => JTokenToConventionalDotNetObject(prop.Value));
				case JTokenType.Array:
					return token.Values().Select(JTokenToConventionalDotNetObject).ToList();
				default:
					return token.ToObject<object>();
			}
		}

		public IActionResult PrintBarcode(string barcode)
		{
			ViewBag.barcode = barcode;
			return View();
		}

		public IActionResult UserCheck()
		{
			using (var context = new DAL.DurinDBContext())
			{
				var user = context.Users.FirstOrDefault(x => x.deleted == false && x.email == "admin@durinls.com");
				if (user == null)
				{
					context.Users.Add(new Entities.DB.User
					{
						email = "admin@durinls.com",
						password = Helper.CreateMD5("1234"),
						firstname = "Durin",
						lastname = "Admin",
						phone = "1234567890",
						createdDate = DateTime.Now,
						createdUserID = 1,
						deleted = false,
						type = Entities.DB.User.Type.admin
					});
					context.SaveChanges();
				}
			}
			return Ok();
		}
	}
	public class Property
	{
		public string key { get; set; }
		public dynamic value { get; set; }
	}
}
