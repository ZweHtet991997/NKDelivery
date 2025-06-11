using DeliverySystemAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DeliverySystemAPI.Controllers.Data
{
    public class ClientController : ControllerBase
    {
        private IConfiguration Configuration;
        public ClientController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        #region Get
        [HttpGet]
        [Route("api/data/client")]
        public IActionResult Get_Client(String Business_Name)
        {
            try
            {
                DefaultModel defaultModel = new DefaultModel();
                string connection = Configuration.GetConnectionString(defaultModel.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                SqlCommand get_client_cmd = new SqlCommand("SELECT ROW_NUMBER() OVER (ORDER BY Client_ID DESC)  AS SrNo,Client_ID,Client_Name,Phone_Number,Address,Bank_Account,Bank_Account_Owner,Contact_Person,Create_Date,Update_Date From Client Where Business_Name=@business_name And IsDeleted=@is_deleted Order By Client_ID DESC", con);
                get_client_cmd.Parameters.AddWithValue("@business_name", Business_Name);
                get_client_cmd.Parameters.AddWithValue("@is_deleted", '0');
                SqlDataAdapter da = new SqlDataAdapter(get_client_cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                con.Close();
                GC.Collect();
                return StatusCode(StatusCodes.Status200OK, JsonConvert.SerializeObject(dt));
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }
        #endregion Get
        #region Create
        [HttpPost]
        [Route("api/data/client")]
        public IActionResult Create_Client([FromBody]ClientModel clientModel)
        {
            try
            {
                DefaultModel defaultModel = new DefaultModel();
                string connection = Configuration.GetConnectionString(defaultModel.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                //checking tha data want to create already exist or not.
                SqlCommand get_client_cmd = new SqlCommand("Select Client_ID From Client Where Client_Name=@client_name And Business_Name=@business_name And IsDeleted=@is_deleted", con);
                get_client_cmd.Parameters.AddWithValue("@client_name", clientModel.Client_Name);
                get_client_cmd.Parameters.AddWithValue("@business_name", clientModel.Business_Name);
                get_client_cmd.Parameters.AddWithValue("@is_deleted", '0');
                SqlDataReader reader = get_client_cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Close();
                    con.Close();
                    GC.Collect();
                    return StatusCode(StatusCodes.Status409Conflict, "Client Name Already Exist.");
                }
                else
                {
                    //start creating client 
                    reader.Close();
                    SqlCommand create_client_cmd = new SqlCommand("Insert Into Client(Client_Name,Phone_Number,Address,Bank_Account,Bank_Account_Owner,Contact_Person,Create_Date,Business_Name,IsDeleted)Values(@client_name,@phone_number,@address,@bank_account,@bank_account_owner,@contact_person,@create_date,@business_name,@is_deleted)", con);
                    create_client_cmd.Parameters.AddWithValue("@client_name", clientModel.Client_Name);
                    create_client_cmd.Parameters.AddWithValue("@phone_number", clientModel.Phone_Number);
                    create_client_cmd.Parameters.AddWithValue("@address", clientModel.Address);
                    create_client_cmd.Parameters.AddWithValue("@bank_account", clientModel.Bank_Account);
                    create_client_cmd.Parameters.AddWithValue("@bank_account_owner", clientModel.Bank_Account_Owner);
                    create_client_cmd.Parameters.AddWithValue("@contact_person", clientModel.Contact_Person);
                    create_client_cmd.Parameters.AddWithValue("@business_name", clientModel.Business_Name);
                    create_client_cmd.Parameters.AddWithValue("@is_deleted", '0');
                    create_client_cmd.Parameters.AddWithValue("@create_date", DateTime.Now);
                    create_client_cmd.ExecuteNonQuery();
                    con.Close();
                    GC.Collect();
                    return StatusCode(StatusCodes.Status201Created, "Client Created.");
                }
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,ex.ToString());
            }
        }
        #endregion Create
        #region Update
        [HttpPut]
        [Route("api/data/client")]
        public IActionResult Update_Client([FromBody]ClientModel clientModel)
        {
            try
            {
                string connection = Configuration.GetConnectionString(clientModel.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                //will not consider for duplicate client name in this section
                SqlCommand update_client_cmd = new SqlCommand("Update Client Set Client_Name=@client_name,Phone_Number=@phone_number,Address=@address,Bank_Account=@bank_account,Bank_Account_Owner=@bank_account_owner,Contact_Person=@contact_person,Update_Date=@update_date Where Client_ID=@client_id And Business_Name=@business_name And IsDeleted=@is_deleted", con);
                update_client_cmd.Parameters.AddWithValue("@client_id", clientModel.Client_ID);
                update_client_cmd.Parameters.AddWithValue("@client_name", clientModel.Client_Name);
                update_client_cmd.Parameters.AddWithValue("@phone_number", clientModel.Phone_Number);
                update_client_cmd.Parameters.AddWithValue("@address", clientModel.Address);
                update_client_cmd.Parameters.AddWithValue("@bank_account", clientModel.Bank_Account);
                update_client_cmd.Parameters.AddWithValue("@bank_account_owner", clientModel.Bank_Account_Owner);
                update_client_cmd.Parameters.AddWithValue("@contact_person", clientModel.Contact_Person);
                update_client_cmd.Parameters.AddWithValue("@business_name", clientModel.Business_Name);
                update_client_cmd.Parameters.AddWithValue("@update_date", DateTime.Now);
                update_client_cmd.Parameters.AddWithValue("@is_deleted", '0');
                update_client_cmd.ExecuteNonQuery();
                con.Close();
                GC.Collect();
                return StatusCode(StatusCodes.Status202Accepted, "Client Updated");
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }
        #endregion Update
        #region Delete
        [HttpDelete]
        [Route("api/data/client")]
        public IActionResult Delete_Client(String Client_ID,String Business_Name)
        {
            try
            {
                DefaultModel defaultModel = new DefaultModel();
                string connection = Configuration.GetConnectionString(defaultModel.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                SqlCommand get_client_cmd = new SqlCommand("Select Client_ID from Client Where Client_ID=@client_id And Business_Name=@business_name And IsDeleted=@is_deleted", con);
                get_client_cmd.Parameters.AddWithValue("@client_id", Client_ID);
                get_client_cmd.Parameters.AddWithValue("@business_name", Business_Name);
                get_client_cmd.Parameters.AddWithValue("@is_deleted", '0');
                SqlDataReader reader = get_client_cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    //start updaing isdelete to 1
                    reader.Close();
                    SqlCommand delete_client_cmd = new SqlCommand("Update Client Set IsDeleted=@is_deleted Where Client_ID=@client_id", con);
                    delete_client_cmd.Parameters.AddWithValue("@is_deleted", '1');
                    delete_client_cmd.Parameters.AddWithValue("@client_id", Client_ID);
                    delete_client_cmd.ExecuteNonQuery();
                    reader.Close();
                    con.Close();
                    GC.Collect();
                    return StatusCode(StatusCodes.Status202Accepted, "Client Deleted");
                }
                else
                {
                    reader.Close();
                    con.Close();
                    return StatusCode(StatusCodes.Status406NotAcceptable, "Client Not Exist OR It's Already Deleted.");
                }
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }
        #endregion Delete
    }
}
