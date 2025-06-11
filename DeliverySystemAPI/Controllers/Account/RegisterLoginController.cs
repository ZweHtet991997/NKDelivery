using DeliverySystemAPI.Model;
using DeliverySystemAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DeliverySystemAPI.Controllers.Account
{
    public class RegisterLoginController : ControllerBase
    {
        private IConfiguration Configuration;
        public RegisterLoginController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        #region Register
        [HttpPost]
        [Route("api/account/register")]
        public IActionResult Index([FromBody]AccountModel account_Model)
        {
            try
            {
                DefaultModel defaultModel = new DefaultModel();
                string connection = Configuration.GetConnectionString(defaultModel.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                //check account already exist
                SqlCommand check_cmd = new SqlCommand("Select User_ID from Users Where Phone_Number=@phone_number And User_Name=@user_name And Business_Name=@business_name And IsDeleted=@isdeleted", con);
                check_cmd.Parameters.AddWithValue("@phone_number", account_Model.Phone_Number);
                check_cmd.Parameters.AddWithValue("@user_name", account_Model.User_Name);
                check_cmd.Parameters.AddWithValue("@business_name", account_Model.Business_Name);
                check_cmd.Parameters.AddWithValue("@isdeleted",'0');
                SqlDataReader reader = check_cmd.ExecuteReader();
                if (reader.Read())
                {
                    GC.Collect();
                    reader.Close();
                    return StatusCode(StatusCodes.Status409Conflict, "Account Already Exist.");
                }
                reader.Close();
                //start creating account
                SqlCommand register_cmd = new SqlCommand("Insert Into Users(User_Name,Phone_Number,Email,Business_Name,Password,User_Role,Client_Name,C_Name,T_Name,Create_Date,IsDeleted) Values(@name,@phone_number,@email,@business_name,@password,@user_role,@client_name,@c_name,@t_name,@create_datetime,@isdeleted)", con);
                register_cmd.Parameters.AddWithValue("@name", account_Model.User_Name);
                register_cmd.Parameters.AddWithValue("@phone_number",account_Model.Phone_Number);
                register_cmd.Parameters.AddWithValue("@email", account_Model.Email);
                register_cmd.Parameters.AddWithValue("@business_name", account_Model.Business_Name);
                register_cmd.Parameters.AddWithValue("@password", account_Model.Password);
                register_cmd.Parameters.AddWithValue("@user_role", account_Model.User_Role);
                register_cmd.Parameters.AddWithValue("@client_name", account_Model.Client_Name);
                register_cmd.Parameters.AddWithValue("@c_name", account_Model.City);
                //looping Township Way Array
                string township = "";
                if (account_Model.Township != null) 
                {
                    for (int i = 0; i < account_Model.Township.Length; i++)
                    {
                        township += account_Model.Township[i].ToString() + ",";
                    }
                }
                
                register_cmd.Parameters.AddWithValue("@t_name",township);
                register_cmd.Parameters.AddWithValue("@create_datetime", DateTime.Now);
                register_cmd.Parameters.AddWithValue("@isdeleted", '0');
                register_cmd.ExecuteNonQuery();
                con.Close();
                GC.Collect();
                return StatusCode(StatusCodes.Status201Created,"Account Created");
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,ex.ToString());
            }
        }
        #endregion Register
        #region Login
        [HttpPost]
        [Route("api/account/login")]
        public IActionResult Login([FromBody]AccountModel account_Model)
        {
            try
            {
                DefaultModel defaultModel = new DefaultModel();
                string connection = Configuration.GetConnectionString(defaultModel.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                //start checking login
                SqlCommand login_cmd = new SqlCommand("Select User_ID,User_Name,Phone_Number,Business_Name,User_Role,C_Name,T_Name,Email,Client_Name From Users Where Password=@password And Phone_Number=@phone_number", con);
                login_cmd.Parameters.AddWithValue("@password", account_Model.Password);
                login_cmd.Parameters.AddWithValue("@phone_number", account_Model.Phone_Number);
                SqlDataReader reader = login_cmd.ExecuteReader();
                if (reader.Read())
                {
                    //Account Exist
                    DataTable dt = new DataTable();
                        dt.Columns.Add("User_ID");
                        dt.Columns.Add("User_Name");
                        dt.Columns.Add("Phone_Number");
                        dt.Columns.Add("Business_Name");
                        dt.Columns.Add("User_Role");
                        dt.Columns.Add("C_Name");
                        dt.Columns.Add("T_Name");
                        dt.Columns.Add("Email");
                        dt.Columns.Add("Client_Name");
                        dt.Columns.Add("Token");
                    GenerateToken generateToken = new GenerateToken();
                    var user_name = reader["User_Name"].ToString();
                    var user_role = reader["User_Role"].ToString();
                    var token = generateToken.Generate_Token(user_name, user_role);
                    dt.Rows.Add(reader["User_ID"].ToString(),reader["User_Name"].ToString(),reader["Phone_Number"].ToString(),reader["Business_Name"].ToString(),reader["User_Role"].ToString(), reader["C_Name"].ToString(), reader["T_Name"].ToString(), reader["Email"].ToString(),reader["Client_Name"].ToString(),token);
                    reader.Close();
                    con.Close();
                    GC.Collect();
                    return StatusCode(StatusCodes.Status202Accepted,JsonConvert.SerializeObject(dt).ToString());
                }
                else
                {
                    reader.Close();
                    con.Close();
                    GC.Collect();
                    return StatusCode(StatusCodes.Status401Unauthorized, "Incorrect Phone Number Or Email and Password.");
                }
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,ex.ToString());
            }
        }
        #endregion Login
        #region GetData
        [HttpGet]
        [Route("api/account")]
        public IActionResult Get_User_Data([FromQuery]String Business_Name)
        {
            try
            {
                DefaultModel defauleModel = new DefaultModel();
                string connection = Configuration.GetConnectionString(defauleModel.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                SqlCommand get_user_cmd = new SqlCommand("Select ROW_NUMBER() OVER (ORDER BY User_ID DESC) AS SrNo, User_ID,User_Name,User_Role,Client_Name,Email,Phone_Number,C_Name,T_Name,Password,Create_Date,Update_Date From Users Where Business_Name=@business_name And IsDeleted=@is_deleted Order By User_ID DESC", con);
                get_user_cmd.Parameters.AddWithValue("@business_name", Business_Name);
                get_user_cmd.Parameters.AddWithValue("@is_deleted", '0');
                SqlDataAdapter da = new SqlDataAdapter(get_user_cmd);
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
        #endregion GetData
        #region Get_Deli_Account
        [HttpGet]
        [Route("api/account/deli")]
        public IActionResult Get_Deli_Account(String C_Name,String T_Name,String Business_Name)
        {
            try
            {
                DefaultModel defauleModel = new DefaultModel();
                string connection = Configuration.GetConnectionString(defauleModel.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                string query="";
                SqlCommand get_user_cmd = null;
                //getting deli men list for way management. 
                if(T_Name=="null" && C_Name == "null")
                {
                    query = "Select User_ID,User_Name,User_Role,Email,Phone_Number,C_Name,T_Name,Password,Create_Date,Update_Date From Users Where User_Role=@user_role And Business_Name=@business_name And IsDeleted=@is_deleted Order By User_ID DESC";
                    get_user_cmd = new SqlCommand(query, con);
                }
                //getting deli men list by collect assign with township selected
                else if (T_Name !="null")
                {
                    query = "Select User_Name From Users Where User_Role=@user_role And T_Name LIKE @t_name And Business_Name=@business_name And IsDeleted=@is_deleted Order By User_ID DESC";
                    get_user_cmd = new SqlCommand(query, con);
                    get_user_cmd.Parameters.AddWithValue("@t_name", "%" + T_Name + "%");
                }
                else if (C_Name != "null")
                {
                    query = "Select User_Name From Users Where User_Role=@user_role And C_Name LIKE @c_name And Business_Name=@business_name And IsDeleted=@is_deleted Order By User_ID DESC";
                    get_user_cmd = new SqlCommand(query, con);
                    get_user_cmd.Parameters.AddWithValue("@c_name", C_Name);
                }
                get_user_cmd.Parameters.AddWithValue("@user_role", "Delivery");
                get_user_cmd.Parameters.AddWithValue("@business_name", Business_Name);
                get_user_cmd.Parameters.AddWithValue("@is_deleted", '0');
                SqlDataAdapter da = new SqlDataAdapter(get_user_cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                con.Close();
                GC.Collect();
                return StatusCode(StatusCodes.Status200OK, JsonConvert.SerializeObject(dt));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }
        #endregion Get_Deli_Account
        #region Update_Account
        [HttpPut]
        [Route("api/account/register")]
        public IActionResult Update_Account([FromBody]AccountModel accountModel)
        {
            try
            {
                DefaultModel defaultModel = new DefaultModel();
                string connection = Configuration.GetConnectionString(defaultModel.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                SqlCommand update_cmd = new SqlCommand("Update Users Set User_Name=@user_name,Phone_Number=@phone_number,User_Role=@user_role,Client_Name=@client_name,Email=@email,C_Name=@c_name,T_Name=@t_name,Password=@password Where User_ID=@user_id", con);
                update_cmd.Parameters.AddWithValue("@user_id", accountModel.User_ID);
                update_cmd.Parameters.AddWithValue("@user_name", accountModel.User_Name);
                update_cmd.Parameters.AddWithValue("@phone_number", accountModel.Phone_Number);
                update_cmd.Parameters.AddWithValue("@user_role", accountModel.User_Role);
                update_cmd.Parameters.AddWithValue("@client_name", accountModel.Client_Name);
                update_cmd.Parameters.AddWithValue("@email", accountModel.Email);
                update_cmd.Parameters.AddWithValue("@c_name", accountModel.City);
                //looping selected townships
                string township = "";
                for (int i = 0; i < accountModel.Township.Length; i++)
                {
                    township += accountModel.Township[i].ToString() + ",";
                }
                update_cmd.Parameters.AddWithValue("@t_name",township);
                update_cmd.Parameters.AddWithValue("@password", accountModel.Password);
                update_cmd.ExecuteNonQuery();
                con.Close();
                GC.Collect();
                return StatusCode(StatusCodes.Status202Accepted, "Account Updated");
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,ex.ToString());
            }

        }
        #endregion Update_Account
        #region Delete_Account
        [HttpDelete]
        [Route("api/account")]
        public IActionResult Delete_Account(String User_ID,String Business_Name)
        {
            try
            {
                DefaultModel defaultModel = new DefaultModel();
                string connection = Configuration.GetConnectionString(defaultModel.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                SqlCommand delete_account_cmd = new SqlCommand("Update Users Set IsDeleted=@isdeleted Where User_ID=@user_id And Business_Name=@business_name", con);
                delete_account_cmd.Parameters.AddWithValue("@isdeleted", '1');
                delete_account_cmd.Parameters.AddWithValue("@user_id", User_ID);
                delete_account_cmd.Parameters.AddWithValue("@business_name", Business_Name);
                delete_account_cmd.ExecuteNonQuery();
                con.Close();
                GC.Collect();
                return StatusCode(StatusCodes.Status202Accepted, "Account Deleted");
            }
            catch(Exception ex)
            {
                GC.Collect();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }
        #endregion Delete_Account
        #region ValidateJWT
        [HttpGet]
        [Route("api/jwt")]
        public IActionResult Validate_JWT_Toke([FromHeader] HttpContext context)
        {
            try
            {
                string authHeader = context.Request.Headers["Authorization"];
                string[] header_and_token = authHeader.Split(' ');
                string header = header_and_token[0];
                string token = header_and_token[1];
                if (header != "Bearer")
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, "Invalid Token");
                }
                else
                {
                    JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                    JwtSecurityToken jwtToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
                    if (jwtToken == null) return StatusCode(StatusCodes.Status401Unauthorized, "Invalid Token");
                    byte[] key = Convert.FromBase64String("nk7863711ksso3111a4e4133zwehtet9");
                    TokenValidationParameters parameters = new TokenValidationParameters()
                    {
                        RequireExpirationTime = true,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    };
                    SecurityToken securityToken;
                    ClaimsPrincipal principal = tokenHandler.ValidateToken(token, parameters, out securityToken);
                    if (principal != null)
                    {
                        return StatusCode(StatusCodes.Status200OK, "Token Valid");
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status401Unauthorized, "Token Invalid");
                    }
                }


            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, "Invalid Token");
            }
        }
        #endregion ValidateJWT
    }
}
