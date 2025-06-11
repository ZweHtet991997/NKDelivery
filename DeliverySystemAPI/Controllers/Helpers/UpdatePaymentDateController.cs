using DeliverySystemAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DeliverySystemAPI.Controllers.Helpers
{
    public class UpdatePaymentDateController : ControllerBase
    {
        private IConfiguration Configuration;
        public UpdatePaymentDateController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        [HttpPost]
        [Route("api/helper/updateclientorderpaymentdate")]
        public IActionResult Index()
        {
            DefaultModel dm = new DefaultModel();
            string connection = Configuration.GetConnectionString(dm.DB_Name);
            SqlConnection con = new SqlConnection(connection);
            con.Open();
            SqlCommand update_cmd = null;
            for (int i = 1;i<= 317;i++)
            {
                update_cmd = new SqlCommand("Update ClientOrder Set Payment_Date=REPLACE((Select Payment_Date+'2' From ClientOrder Where CO_ID=@co_id And Payment_Date IS NOT NULL),'202          2', '2022') Where CO_ID=@co_id And Payment_Date IS NOT NULL ", con);
                update_cmd.Parameters.AddWithValue("@co_id", i);
                update_cmd.ExecuteNonQuery();
            }
            con.Close();
            return StatusCode(StatusCodes.Status200OK,"Updated");
        }
    }
}
