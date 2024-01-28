using INTTest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using System.Threading.Tasks;
using System.Diagnostics;

namespace INTTest.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly IConfiguration _configuration;

        public InvoiceController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<IActionResult> Index()
        {
            var UOM = await Uommasters();
            var itemmaster = await Itemmasters();            
            var partymasters = await Partymasters(); 
            List<ListViewModel> model = new List<ListViewModel>
                {
                    new ListViewModel { uommasters = UOM, itemmasters = itemmaster, partymasters = partymasters }
                };
            return View(model);
        }

        [HttpGet]
        public async Task<List<uommaster>> Uommasters()
        {
            List<uommaster> data;
            string connction = _configuration.GetConnectionString("TestConnectionString");
            using (var con = new SqlConnection(connction))
            {
                con.Open();
                    data =(await con.QueryAsync<uommaster>("select * from UOM WHERE UOM_NAME IS NOT NULL")).ToList();
            }return data;
        }

        [HttpGet]
        public async Task<List<partymaster>> Partymasters()
        {
            List<partymaster> data;
            string connction = _configuration.GetConnectionString("TestConnectionString");
            using (var con = new SqlConnection(connction))
            {
                con.Open();
                data = (await con.QueryAsync<partymaster>("select * from PARTY_MASTER WHERE PARTY_NAME IS NOT NULL")).ToList();
            }
            return data;
        }

        [HttpGet]
        public async Task<List<itemmaster>> Itemmasters()
        {
            List<itemmaster> data;
            string connction = _configuration.GetConnectionString("TestConnectionString");
            using (var con = new SqlConnection(connction))
            {
                con.Open();
                data = (await con.QueryAsync<itemmaster>("select * from ITEM_MASTER")).ToList();
            }
            return data;
        }
    }
}
