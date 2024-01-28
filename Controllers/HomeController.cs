using INTTest.Models;
using Microsoft.AspNetCore.Mvc;

using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;

namespace INTTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult Index()
        {

            List<itemmaster> itemList = GetItemDataFromDatabase();
            List<partymaster> partyList = GetPartyDataFromDatabase();
            List<uommaster> oumList = GetUOMDataFromDatabase();
            List<Dochdr> Dochdr = GetDOCHDRDataFromDatabase();

            List<Docdet> Docdet = GetDOCDETDataFromDatabase();

            return View(new Tuple<List<itemmaster>, List<partymaster>, List<uommaster>, List<Dochdr>, List<Docdet>>(itemList, partyList, oumList, Dochdr,Docdet));
        }
        public JsonResult GetuomData(string itemName)
        {
            var uomData = new List<object>();

            string connectionString = _configuration.GetConnectionString("TestConnectionString");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT UOM.UOM_ID, UOM.UOM_NAME FROM UOM LEFT JOIN ITEM_MASTER ON ITEM_MASTER.UID = UOM.UOM_ID WHERE ITEM_MASTER.ITEM_ID = @itemname";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Use SqlParameter to avoid SQL injection
                    command.Parameters.AddWithValue("@itemname", itemName);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var uomObject = new
                            {
                                UOM_ID = Convert.ToInt32(reader["UOM_ID"]),
                                UOM_NAME = reader["UOM_NAME"].ToString()
                            };

                            uomData.Add(uomObject);
                        }
                    }
                }

                connection.Close();
            }

            return Json(uomData);
        }

        private List<itemmaster> GetItemDataFromDatabase()
        {
            List<itemmaster> itemList = new List<itemmaster>();

            string connectionString = _configuration.GetConnectionString("TestConnectionString");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM ITEM_MASTER";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            itemmaster item = new itemmaster
                            {
                                ITEM_ID = Convert.ToInt32(reader["ITEM_ID"]),
                                ITEM_NAME = reader["ITEM_NAME"].ToString(),
                                UID = Convert.ToInt32(reader["UID"])
                            };

                            itemList.Add(item);
                        }
                    }
                }

                connection.Close();
            }

            return itemList;
        }
        private List<partymaster> GetPartyDataFromDatabase()
        {
            List<partymaster> partyList = new List<partymaster>();

            string connectionString = _configuration.GetConnectionString("TestConnectionString");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM PARTY_MASTER WHERE PARTY_NAME IS NOT NULL";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            partymaster party = new partymaster
                            {
                                PARTY_ID = Convert.ToInt32(reader["PARTY_ID"]),
                                PARTY_NAME = reader["PARTY_NAME"].ToString(),
                                ADDRESS = reader["ADDRESS"].ToString(),
                                CITY = reader["CITY"].ToString(),
                                CONTACT_NO = reader["CONTACT_NO"].ToString(),
                                PINCODE = reader["PINCODE"].ToString()
                            };

                            partyList.Add(party);
                        }
                    }
                }

                connection.Close();
            }

            return partyList;
        }
        private List<uommaster> GetUOMDataFromDatabase()
        {
            List<uommaster> partyList = new List<uommaster>();

            string connectionString = _configuration.GetConnectionString("TestConnectionString");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM UOM WHERE UOM_NAME IS NOT NULL";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            uommaster party = new uommaster
                            {
                                UOM_ID = Convert.ToInt32(reader["UOM_ID"]),
                                UOM_NAME = reader["UOM_NAME"].ToString()
                            };

                            partyList.Add(party);
                        }
                    }
                }

                connection.Close();
            }

            return partyList;
        }

        [HttpPost]
        public IActionResult SaveInvoice([FromBody] InvoiceFormModel formData)
        {
            string connectionString = _configuration.GetConnectionString("TestConnectionString");
            if (!string.IsNullOrEmpty(formData.PartyName) && !string.IsNullOrEmpty(formData.InvoiceNo))
            {

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Insert into DOCHDR table
                    var insertDochdrQuery = @"
                    INSERT INTO DOCHDR (Party_ID, Invoice_No, InvoiceDate, totalamount, [User], DefaultDateTime)
                    VALUES (@Party_ID, @Invoice_No, @InvoiceDate, @totalamount, @User, @DefaultDateTime);
                    SELECT SCOPE_IDENTITY();";

                    using (var command = new SqlCommand(insertDochdrQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Party_ID", formData.PartyName);
                        command.Parameters.AddWithValue("@Invoice_No", formData.InvoiceNo);
                        command.Parameters.AddWithValue("@InvoiceDate", formData.InvoiceDate);
                        command.Parameters.AddWithValue("@totalamount", formData.TotalAmount);
                        command.Parameters.AddWithValue("@User", "Sukurulla Shaikh");
                        command.Parameters.AddWithValue("@DefaultDateTime", DateTime.Now);

                        // Execute the insert query
                        var dochdrId = Convert.ToInt32(command.ExecuteScalar());

                        // Insert into DOCDET table
                        var insertDocdetQuery = @"
                            INSERT INTO DOCDET (HDRAuto_ID, SrNo, Item_ID, UOM_ID, Qty, Rate, Value)
                            VALUES (@HDRAuto_ID, @SrNo, @Item_ID, @UOM_ID, @Qty, @Rate, @Value);";
                        int cont = 0;
                        foreach (var rowData in formData.TableData)
                        {
                            using (var docdetCommand = new SqlCommand(insertDocdetQuery, connection))
                            {


                                if (!string.IsNullOrEmpty(rowData.ItemName))
                                {
                                    docdetCommand.Parameters.AddWithValue("@HDRAuto_ID", dochdrId);
                                    docdetCommand.Parameters.AddWithValue("@SrNo", rowData.Srno);
                                    docdetCommand.Parameters.AddWithValue("@Item_ID", rowData.ItemName);
                                    docdetCommand.Parameters.AddWithValue("@UOM_ID", rowData.UnitName);
                                    docdetCommand.Parameters.AddWithValue("@Qty", rowData.Quantity);
                                    docdetCommand.Parameters.AddWithValue("@Rate", rowData.Rate);
                                    docdetCommand.Parameters.AddWithValue("@Value", rowData.Value);

                                    docdetCommand.ExecuteNonQuery();
                                    cont++;
                                }
                                else
                                {
                                    if (cont >0)
                                    {
                                        return Json(new { success = true, message = "SUCCESS" });
                                    }
                                    else
                                    {
                                        return Json(new { success = false, message = "Please Add tallest one Item" });
                                    }
                                    
                                }
                            }
                        }
                    }
                }

                return Json(new { success = true, message = "SUCCESS" });
            }
            catch (Exception ex)
            {
                // Handle exceptions
                return Json(new { success = false, message = "Please Add at list one product in your invoice" });
                
            }

            }
            else
            {
                return Json(new { success = false, message = "Please select Party Name" });
            }
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private List<Dochdr> GetDOCHDRDataFromDatabase()
        {
            List<Dochdr> itemList = new List<Dochdr>();

            string connectionString = _configuration.GetConnectionString("TestConnectionString");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT top 8 * FROM DOCHDR order by DefaultDateTime desc";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Dochdr item = new Dochdr
                            {
                                HDRAutoID = reader["HDRAutoID"] as int?, // Use 'as' for nullable types
                                Party_ID = reader["Party_ID"] as int?,
                                Invoice_No = reader["Invoice_No"] as string,
                                InvoiceDate = reader["InvoiceDate"] as string,
                                totalamount = reader["totalamount"] as decimal?,
                                User = reader["User"] as string,
                                DefaultDateTime = reader["DefaultDateTime"].ToString() as string
                            };

                            itemList.Add(item);
                        }
                    }
                }

                connection.Close();
            }

            return itemList;
        }
        private List<Docdet> GetDOCDETDataFromDatabase()
        {
            List<Docdet> itemList = new List<Docdet>();

            string connectionString = _configuration.GetConnectionString("TestConnectionString");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT top 8 * FROM DOCDET order by HDRAuto_ID desc";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Docdet item = new Docdet
                            {
                                DocDetail_ID = reader["DocDetail_ID"] == DBNull.Value ? null : (int?)Convert.ToInt32(reader["DocDetail_ID"]),
                                HDRAuto_ID = reader["HDRAuto_ID"] == DBNull.Value ? null : (int?)Convert.ToInt32(reader["HDRAuto_ID"]),
                                SrNo = reader["SrNo"] == DBNull.Value ? null : (int?)Convert.ToInt32(reader["SrNo"]),
                                Item_ID = reader["Item_ID"] == DBNull.Value ? null : (int?)Convert.ToInt32(reader["Item_ID"]),
                                UOM_ID = reader["UOM_ID"] == DBNull.Value ? null : (int?)Convert.ToInt32(reader["UOM_ID"]),
                                Qty = reader["Qty"] == DBNull.Value ? null : (decimal?)Convert.ToDecimal(reader["Qty"]),
                                Rate = reader["Rate"] == DBNull.Value ? null : (decimal?)Convert.ToDecimal(reader["Rate"]),
                                Value = reader["Value"] == DBNull.Value ? null : (decimal?)Convert.ToDecimal(reader["Value"])

                            };

                            itemList.Add(item);
                        }
                    }
                }

                connection.Close();
            }

            return itemList;
        }
    }
}
