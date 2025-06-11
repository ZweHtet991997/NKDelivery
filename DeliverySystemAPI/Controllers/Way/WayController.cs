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

namespace DeliverySystemAPI.Controllers.Way
{
    public class WayController : ControllerBase
    {
        private IConfiguration Configuration;
        public WayController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        #region Get
        [HttpGet]
        [Route("api/manage/way")]
        public IActionResult Get_Ways_Status(String Deli_Men,String Deli_Status,String Deli_Type,String Business_Name,String From_Date,String To_Date)
        {
            try
            {
                DefaultModel defaultModel = new DefaultModel();
                string connection = Configuration.GetConnectionString(defaultModel.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                //SqlCommand get_cmd = new SqlCommand("SELECT ROW_NUMBER() OVER (ORDER BY CAO_ID DESC) AS SrNo,CAO_ID,Deli_Men,ClientOrder.Client_Name,Receiver_Name,C_Name,T_Name,G_Name,G_Price,Item_Price,Expense_Fees,Deli_Price,Deli_Type,CollectAssignOrder.Total_Amount,Deli_Status,Expense_Status,Deli_Men_Remark,Assign_Remark,CollectAssignOrder.Create_Date,Accept_Date,CollectAssignOrder.Update_Date From CollectAssignOrder Full Join ClientOrder On ClientOrder.CO_ID=CollectAssignOrder.CO_ID Where Deli_Men Like @deli_men And Deli_Status Like @deli_status And CollectAssignOrder.Business_Name=@business_name And CollectAssignOrder.IsDeleted=@isdeleted", con);
                SqlCommand get_cmd = null;
                if (From_Date == null && To_Date == null) // means the user want the client order by today date
                {
                    //deli type par yin return item list ko get lote tae request
                    if (Deli_Type == null || Deli_Type=="null")
                    {
                        if (Deli_Men == "%")
                        {

                            get_cmd = new SqlCommand("SELECT ROW_NUMBER() OVER (ORDER BY CollectAssignOrder.Create_Date DESC) AS SrNo,ClientOrder.Client_Name,CAO_ID,Deli_Men,Deli_Status,Receiver_Name,C_Name,T_Name,Deli_Type,Item_Price,G_Name,G_Price,Tan_Sar_Price,Expense_Fees,Return_Item_Amount,Half_Paid_Amount,Deli_Price,CollectAssignOrder.Total_Amount,Deli_Status,Deli_Men_Remark,Complete_Date,CollectAssignOrder.Create_Date,Pending_Date,Accept_Date,Complete_Date From CollectAssignOrder Left Join ClientOrder On CollectAssignOrder.CO_ID=ClientOrder.CO_ID Where CollectAssignOrder.Deli_Men Like @deli_men And CollectAssignOrder.Deli_Status Like @deli_status And  CollectAssignOrder.Business_Name=@business_name And CollectAssignOrder.IsDeleted=@isdeleted And ClientOrder.Business_Name=@business_name And ClientOrder.IsDeleted=@isdeleted", con);
                        }
                        else
                        {
                            get_cmd = new SqlCommand("SELECT ROW_NUMBER() OVER (ORDER BY CollectAssignOrder.Create_Date DESC) AS SrNo,ClientOrder.Client_Name,CAO_ID,Deli_Men,Deli_Status,Receiver_Name,C_Name,T_Name,Deli_Type,Item_Price,G_Name,G_Price,Tan_Sar_Price,Expense_Fees,Return_Item_Amount,Half_Paid_Amount,Deli_Price,CollectAssignOrder.Total_Amount,Deli_Status,Deli_Men_Remark,Complete_Date,CollectAssignOrder.Create_Date,Pending_Date,Accept_Date,Complete_Date From CollectAssignOrder Left Join ClientOrder On CollectAssignOrder.CO_ID=ClientOrder.CO_ID Where CollectAssignOrder.Deli_Men = @deli_men And CollectAssignOrder.Deli_Status Like @deli_status And CollectAssignOrder.Business_Name=@business_name And CollectAssignOrder.IsDeleted=@isdeleted", con);
                        }
                    }
                    else
                    {
                        get_cmd = new SqlCommand("Select ROW_NUMBER() OVER (ORDER BY CollectAssignOrder.Create_Date DESC) AS SrNo,ClientOrder.Client_Name,CAO_ID,Deli_Men,Deli_Status,Receiver_Name,C_Name,T_Name,Deli_Type,Item_Price,G_Name,G_Price,Tan_Sar_Price,Expense_Fees,Return_Item_Amount,Half_Paid_Amount,Deli_Price,CollectAssignOrder.Total_Amount,Deli_Status,Deli_Men_Remark,Complete_Date,CollectAssignOrder.Create_Date,Pending_Date,Accept_Date,Complete_Date From CollectAssignOrder Full Join ClientOrder On (ClientOrder.CO_ID=CollectAssignOrder.CO_ID) Where Rejected_Status<>'Completed' And Deli_Type='Return Item' And CollectAssignOrder.Business_Name=@business_name And CollectAssignOrder.IsDeleted=@isdeleted And ClientOrder.Business_Name=@business_name And ClientOrder.IsDeleted=@isdeleted And Deli_Men Like @deli_man", con);
                        get_cmd.Parameters.AddWithValue("@deli_man", Deli_Men);

                        //if (Deli_Men == "%")
                        //{

                        //    get_cmd = new SqlCommand("SELECT ROW_NUMBER() OVER (ORDER BY CollectAssignOrder.Create_Date DESC) AS SrNo,ClientOrder.Client_Name,CAO_ID,Deli_Men,Deli_Status,Receiver_Name,C_Name,T_Name,Deli_Type,Item_Price,G_Name,G_Price,Tan_Sar_Price,Expense_Fees,Return_Item_Amount,Half_Paid_Amount,Deli_Price,CollectAssignOrder.Total_Amount,Deli_Status,Deli_Men_Remark,Complete_Date,CollectAssignOrder.Create_Date,Pending_Date,Accept_Date From CollectAssignOrder Left Join ClientOrder On CollectAssignOrder.CO_ID=ClientOrder.CO_ID Where Rejected_Status<>'Completed' And CollectAssignOrder.Deli_Men Like @deli_men And CollectAssignOrder.Deli_Status Like @deli_status And Deli_Status<>'Instock' And CollectAssignOrder.Deli_Type Like @deli_type And CollectAssignOrder.Business_Name=@business_name And CollectAssignOrder.IsDeleted=@isdeleted And ClientOrder.Business_Name=@business_name And ClientOrder.IsDeleted=@isdeleted", con);
                        //    get_cmd.Parameters.AddWithValue("@deli_type", Deli_Type);
                        //}
                        //else
                        //{
                        //    get_cmd = new SqlCommand("SELECT ROW_NUMBER() OVER (ORDER BY CollectAssignOrder.Create_Date DESC) AS SrNo,ClientOrder.Client_Name,CAO_ID,Deli_Men,Deli_Status,Receiver_Name,C_Name,T_Name,Deli_Type,Item_Price,G_Name,G_Price,Tan_Sar_Price,Expense_Fees,Return_Item_Amount,Half_Paid_Amount,Deli_Price,CollectAssignOrder.Total_Amount,Deli_Status,Deli_Men_Remark,Complete_Date,CollectAssignOrder.Create_Date,Pending_Date,Accept_Date From CollectAssignOrder Left Join ClientOrder On CollectAssignOrder.CO_ID=ClientOrder.CO_ID Where Rejected_Status<>'Completed' And CollectAssignOrder.Deli_Men = @deli_men And CollectAssignOrder.Deli_Status Like @deli_status And Deli_Status<>'Instock' And CollectAssignOrder.Deli_Type Like @deli_type  And CollectAssignOrder.Business_Name=@business_name And CollectAssignOrder.IsDeleted=@isdeleted", con);
                        //    get_cmd.Parameters.AddWithValue("@deli_type", Deli_Type);
                        //}
                    }

                    
                }
                else // mean the user want the client order by filter date
                {
                    if (Deli_Status == "Assigned")
                    {
                        get_cmd = new SqlCommand("SELECT ROW_NUMBER() OVER (ORDER BY CollectAssignOrder.Create_Date DESC) AS SrNo,CAO_ID,Deli_Men,Deli_Status,ClientOrder.Client_Name,Receiver_Name,C_Name,T_Name,G_Name,G_Price,Tan_Sar_Price,Item_Price,Expense_Fees,Return_Item_Amount,Half_Paid_Amount,Deli_Price,Deli_Type,CollectAssignOrder.Total_Amount,Deli_Status,Expense_Status,Deli_Men_Remark,Assign_Remark,CollectAssignOrder.Create_Date,Accept_Date,CollectAssignOrder.Update_Date,Pending_Date,Accept_Date,Complete_Date From CollectAssignOrder Full Join ClientOrder On ClientOrder.CO_ID=CollectAssignOrder.CO_ID Where Deli_Men Like @deli_men And Deli_Status Like @deli_status And Deli_Status<>'Instock' And CollectAssignOrder.Business_Name=@business_name And CollectAssignOrder.IsDeleted=@isdeleted And cast (CollectAssignOrder.[Create_Date] as date) between @from_date and @to_date", con);
                    }
                    else
                    {
                        get_cmd = new SqlCommand("SELECT ROW_NUMBER() OVER (ORDER BY CollectAssignOrder.Create_Date DESC) AS SrNo,CAO_ID,Deli_Men,Deli_Status,ClientOrder.Client_Name,Receiver_Name,C_Name,T_Name,G_Name,G_Price,Tan_Sar_Price,Item_Price,Expense_Fees,Return_Item_Amount,Half_Paid_Amount,Deli_Price,Deli_Type,CollectAssignOrder.Total_Amount,Deli_Status,Expense_Status,Deli_Men_Remark,Assign_Remark,CollectAssignOrder.Create_Date,Accept_Date,CollectAssignOrder.Update_Date,Pending_Date,Accept_Date,Complete_Date From CollectAssignOrder Full Join ClientOrder On ClientOrder.CO_ID=CollectAssignOrder.CO_ID Where Deli_Men Like @deli_men And Deli_Status Like @deli_status And Deli_Status<>'Instock' And CollectAssignOrder.Business_Name=@business_name And CollectAssignOrder.IsDeleted=@isdeleted And cast([Accept_Date] as date) between @from_date and @to_date", con);
                    }
                    get_cmd.Parameters.AddWithValue("@from_date", From_Date);
                    get_cmd.Parameters.AddWithValue("@to_date", To_Date);                    
                }
                get_cmd.Parameters.AddWithValue("@deli_men", Deli_Men);
                get_cmd.Parameters.AddWithValue("@deli_status", Deli_Status);
                get_cmd.Parameters.AddWithValue("@business_name", Business_Name);
                get_cmd.Parameters.AddWithValue("@isdeleted", '0');
                DataTable dt = new DataTable();
                if (Deli_Status == "Assigned")
                {
                    SqlDataAdapter da = new SqlDataAdapter(get_cmd);
                    da.Fill(dt);
                }
                //if admin want to see by completed or accepted then we need to calculate total amount individually.
                else
                {
                        dt.Columns.Add("CAO_ID");
                        dt.Columns.Add("SrNo");
                        dt.Columns.Add("Client_Name");
                        dt.Columns.Add("Deli_Men");
                        dt.Columns.Add("Receiver_Name");
                        dt.Columns.Add("C_Name");
                        dt.Columns.Add("T_Name");
                        dt.Columns.Add("Deli_Type");
                        dt.Columns.Add("Item_Price");
                        dt.Columns.Add("G_Name");
                        dt.Columns.Add("G_Price");
                        dt.Columns.Add("Deli_Price");
                        dt.Columns.Add("Expense_Fees");
                        dt.Columns.Add("Tan_Sar_Price");
                        dt.Columns.Add("Return_Item_Amount");
                        dt.Columns.Add("Half_Paid_Amount");
                        dt.Columns.Add("Total_Amount");
                        dt.Columns.Add("Deli_Status");
                        dt.Columns.Add("Deli_Men_Remark");
                        dt.Columns.Add("Create_Date");
                        dt.Columns.Add("Pending_Date");
                        dt.Columns.Add("Accept_Date");
                        dt.Columns.Add("Complete_Date");
                    SqlDataReader reader = get_cmd.ExecuteReader();
                        string deli_type = "";
                        int total_amount = 0;
                        while (reader.Read())
                        {
                            total_amount = 0;
                            deli_type = reader["Deli_Type"].ToString();
                            if (deli_type == "OS Paid" || deli_type=="All Paid" || deli_type== "အကုန်ရှင်းပီး")
                            {
                                total_amount += 0;
                            }
                            else if (deli_type == "Item Paid" || deli_type== "ပစ္စည်းဖိုးရှင်းပီး")
                            {
                                if (reader["Deli_Price"].ToString() == "")
                                {
                                    total_amount += 0;
                                }
                                else
                                {
                                    total_amount += Convert.ToInt32(reader["Deli_Price"].ToString());
                                }
                            }
                            else if (deli_type == "No Deli" || deli_type== "Deliမရ")
                            {
                                if (reader["Item_Price"].ToString() == "")
                                {
                                    total_amount += 0;
                                }
                                else
                                {
                                    total_amount += Convert.ToInt32(reader["Item_Price"].ToString());
                                }
                            }
                            else if (deli_type == "Return Item" || deli_type== "ပစ္စည်းနုတ်")
                            {
                                if (reader["Return_Item_Amount"].ToString() == "")
                                {
                                    total_amount += Convert.ToInt32(reader["Total_Amount"].ToString()) - 0;
                                }
                                else
                                {
                                    total_amount += Convert.ToInt32(reader["Total_Amount"].ToString()) - Convert.ToInt32(reader["Return_Item_Amount"].ToString());
                                }
                            }
                            else
                            {
                            //half paid or transfer amount 
                            //previous clien use half paid . now they want to use half paid as transfer 
                            //half paid=transfer.
                                //handling null
                                if (reader["Half_Paid_Amount"].ToString() == "")
                                {
                                    total_amount += Convert.ToInt32(reader["Total_Amount"].ToString()) - 0;
                                }
                                else
                                {
                                    total_amount += Convert.ToInt32(reader["Total_Amount"].ToString()) - Convert.ToInt32(reader["Half_Paid_Amount"].ToString());
                                }
                            }
                            var sr_no = reader["SrNo"].ToString();
                            var cao_id = reader["CAO_ID"].ToString();
                            var client_name = reader["Client_Name"].ToString();
                            var deli_men = reader["Deli_Men"].ToString();
                            var deli_status=reader["Deli_Status"].ToString();
                            var receiver_name = reader["Receiver_Name"].ToString();
                            var c_name = reader["C_Name"].ToString();
                            var t_name = reader["T_Name"].ToString();
                            var item_price = reader["Item_Price"].ToString();
                            var g_name = reader["G_Name"].ToString();
                            var g_price = reader["G_Price"].ToString();
                            var deli_price = reader["Deli_Price"].ToString();
                            var expense_fees = reader["Expense_Fees"].ToString();
                            var tan_sar_price = reader["Tan_Sar_Price"].ToString();
                            var return_item_amount = reader["Return_Item_Amount"].ToString();
                            var half_paid_amount = reader["Half_Paid_Amount"].ToString();
                            var deli_men_remark = reader["Deli_Men_Remark"].ToString();
                            string create_date = reader["Create_Date"].ToString();//same as assign date.
                            var pending_date = reader["Pending_Date"].ToString();
                            var accept_date = reader["Accept_Date"].ToString();
                            var complete_date = reader["Complete_Date"].ToString();
                            dt.Rows.Add(cao_id, sr_no, client_name,deli_men, receiver_name, c_name, t_name, deli_type, item_price, g_name, g_price, deli_price, expense_fees, tan_sar_price, return_item_amount, half_paid_amount, total_amount, deli_status, deli_men_remark, create_date,pending_date,accept_date,complete_date);
                        }
                    }
                con.Close();
                GC.Collect();
                return StatusCode(StatusCodes.Status200OK, JsonConvert.SerializeObject(dt));
            }
            catch(Exception ex)
            {
                GC.Collect();
                return StatusCode(StatusCodes.Status500InternalServerError,ex.ToString());
            }
        }
        #endregion Get
        #region Get_City_Way_Data
        [HttpGet]
        [Route("api/way/citytownship")]
        public IActionResult Get_City_Township_Way_Data(String Category,String Deli_Status,String From_Date,String To_Date,String Business_Name)
        {
            try
            {
                DefaultModel defaultModel = new DefaultModel();
                string connection = Configuration.GetConnectionString(defaultModel.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                //SqlCommand get_cmd = new SqlCommand("SELECT ROW_NUMBER() OVER (ORDER BY CAO_ID DESC) AS SrNo,CAO_ID,Deli_Men,ClientOrder.Client_Name,Receiver_Name,C_Name,T_Name,G_Name,G_Price,Item_Price,Expense_Fees,Deli_Price,Deli_Type,CollectAssignOrder.Total_Amount,Deli_Status,Expense_Status,Deli_Men_Remark,Assign_Remark,CollectAssignOrder.Create_Date,Accept_Date,CollectAssignOrder.Update_Date From CollectAssignOrder Full Join ClientOrder On ClientOrder.CO_ID=CollectAssignOrder.CO_ID Where Deli_Men Like @deli_men And Deli_Status Like @deli_status And CollectAssignOrder.Business_Name=@business_name And CollectAssignOrder.IsDeleted=@isdeleted", con);
                SqlCommand get_cmd = null;
                DataTable dt = new DataTable();
                if (Category == "City")
                {
                    if(From_Date==null && To_Date == null)
                    {
                        get_cmd = new SqlCommand("SELECT ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS SrNo,Deli_Men,Count(CAO_ID) As Total_Ways,SUM(Item_Price) As Item_Price,Sum(Total_Amount) As Total_Amount,SUM(Expense_Fees) As Expense_Fees  From CollectAssignOrder Inner Join Users On(Users.User_Name=CollectAssignOrder.Deli_Men) Where CollectAssignOrder.C_Name<>'' And CollectAssignOrder.T_Name=''  And CollectAssignOrder.Deli_Status Like @deli_status And  CollectAssignOrder.Business_Name=@business_name And CollectAssignOrder.IsDeleted=@isdeleted And Users.Business_Name=@business_name And Users.IsDeleted=@isdeleted Group By Deli_Men,CollectAssignOrder.C_Name,CollectAssignOrder.T_Name", con);
                    }
                    //that is Accepted with date
                    else
                    {
                        get_cmd = new SqlCommand("SELECT ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS SrNo,Deli_Men,Count(CAO_ID) As Total_Ways,SUM(Item_Price) As Item_Price,Sum(Total_Amount) As Total_Amount,SUM(Expense_Fees) As Expense_Fees From CollectAssignOrder Inner Join Users On(Users.User_Name=CollectAssignOrder.Deli_Men) Where CollectAssignOrder.C_Name<>'' And CollectAssignOrder.T_Name=''  And CollectAssignOrder.Deli_Status Like @deli_status And cast([Accept_Date] as date) between @from_date and @to_date And  CollectAssignOrder.Business_Name=@business_name And CollectAssignOrder.IsDeleted=@isdeleted And Users.Business_Name=@business_name And Users.IsDeleted=@isdeleted Group By Deli_Men,CollectAssignOrder.C_Name,CollectAssignOrder.T_Name", con);
                        get_cmd.Parameters.AddWithValue("@from_date", From_Date);
                        get_cmd.Parameters.AddWithValue("@to_date", To_Date);
                    }
                    get_cmd.Parameters.AddWithValue("@deli_status", Deli_Status);
                    get_cmd.Parameters.AddWithValue("@business_name", Business_Name);
                    get_cmd.Parameters.AddWithValue("@isdeleted", '0');
                    SqlDataAdapter da = new SqlDataAdapter(get_cmd);
                    da.Fill(dt);
                }
                //Township
                else
                {
                    if(From_Date==null && To_Date == null)
                    {
                        get_cmd = new SqlCommand("SELECT ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS SrNo,Deli_Men,Count(CAO_ID) As Total_Ways,SUM(Item_Price) As Item_Price,Sum(Total_Amount) As Total_Amount,SUM(Expense_Fees) As Expense_Fees From CollectAssignOrder Inner Join Users On(Users.User_Name=CollectAssignOrder.Deli_Men) Where CollectAssignOrder.C_Name='' And CollectAssignOrder.T_Name<>''  And CollectAssignOrder.Deli_Status Like @deli_status And  CollectAssignOrder.Business_Name=@business_name And CollectAssignOrder.IsDeleted=@isdeleted And Users.Business_Name='7DayDelivery' And Users.IsDeleted='0'  Group By Deli_Men", con);
                    }
                    else
                    {
                        get_cmd = new SqlCommand("SELECT ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS SrNo,Deli_Men,Count(CAO_ID) As Total_Ways,SUM(Item_Price) As Item_Price,Sum(Total_Amount) As Total_Amount,SUM(Expense_Fees) As Expense_Fees From CollectAssignOrder Inner Join Users On(Users.User_Name=CollectAssignOrder.Deli_Men) Where CollectAssignOrder.C_Name='' And CollectAssignOrder.T_Name<>''  And CollectAssignOrder.Deli_Status Like @deli_status And cast([Accept_Date] as date) between @from_date and @to_date And CollectAssignOrder.Business_Name=@business_name And CollectAssignOrder.IsDeleted=@isdeleted And Users.Business_Name='7DayDelivery' And Users.IsDeleted='0' Group By Deli_Men", con);
                        get_cmd.Parameters.AddWithValue("@from_date", From_Date);
                        get_cmd.Parameters.AddWithValue("@to_date", To_Date);
                    }                    
                    get_cmd.Parameters.AddWithValue("@deli_status", Deli_Status);
                    get_cmd.Parameters.AddWithValue("@business_name", Business_Name);
                    get_cmd.Parameters.AddWithValue("@isdeleted", '0');
                    SqlDataAdapter da = new SqlDataAdapter(get_cmd);
                    da.Fill(dt);
                }
                   
                return StatusCode(StatusCodes.Status200OK,JsonConvert.SerializeObject(dt));
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }

        }
        #endregion Get_City_Way_Data
        #region Update
        [HttpPut]
        [Route("api/manage/way")]
        public IActionResult Update_Way([FromBody] WayModel wayModel)
        {
            try
            {
                string connection = Configuration.GetConnectionString(wayModel.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                SqlCommand update_cmd = null;
                //defining query according to deli status
                if (wayModel.Deli_Status == "Assigned" )
                {
                    update_cmd = new SqlCommand("Update CollectAssignOrder Set Deli_Men=@deli_men,Deli_Status=@deli_status,Receiver_Name=@receiver_name,C_Name=@c_name,T_Name=@t_name,Tan_Sar_Price=@tan_sar_price,G_Name=@g_name,G_Price=@g_price,Expense_Fees=@expense_fees,Deli_Type=@deli_type,Deli_Men_Remark=@deli_men_remark,Assign_Remark=@assign_remark,Update_Date=@update_date Where CAO_ID=@cao_id And Business_Name=@business_name And IsDeleted=@isdeleted", con);
                }
                else if(wayModel.Deli_Status == "Pending")
                {
                    update_cmd = new SqlCommand("Update CollectAssignOrder Set Deli_Men=@deli_men,Deli_Status=@deli_status,Receiver_Name=@receiver_name,C_Name=@c_name,T_Name=@t_name,Tan_Sar_Price=@tan_sar_price,G_Name=@g_name,G_Price=@g_price,Expense_Fees=@expense_fees,Deli_Type=@deli_type,Deli_Men_Remark=@deli_men_remark,Assign_Remark=@assign_remark,Create_Date=@create_date,Update_Date=@update_date Where CAO_ID=@cao_id And Business_Name=@business_name And IsDeleted=@isdeleted", con);
                }
                else if (wayModel.Deli_Status == "Completed")
                {
                    update_cmd = new SqlCommand("Update CollectAssignOrder Set Deli_Men=@deli_men,Deli_Status=@deli_status,Receiver_Name=@receiver_name,C_Name=@c_name,T_Name=@t_name,Tan_Sar_Price=@tan_sar_price,G_Name=@g_name,G_Price=@g_price,Expense_Fees=@expense_fees,Deli_Type=@deli_type,Return_Item_Amount=@return_item_amount,Half_Paid_Amount=@half_paid_amount,Deli_Men_Remark=@deli_men_remark,Assign_Remark=@assign_remark,Update_Date=@update_date,Complete_Date=@complete_date Where CAO_ID=@cao_id And Business_Name=@business_name And IsDeleted=@isdeleted", con);
                }
                else if (wayModel.Deli_Status == "Accepted")
                {
                    update_cmd = new SqlCommand("Update CollectAssignOrder Set Deli_Men=@deli_men,Deli_Status=@deli_status,Expense_Status=@expense_status,Receiver_Name=@receiver_name,C_Name=@c_name,T_Name=@t_name,Tan_Sar_Price=@tan_sar_price,G_Name=@g_name,G_Price=@g_price,Expense_Fees=@expense_fees,Deli_Type=@deli_type,Deli_Men_Remark=@deli_men_remark,Assign_Remark=@assign_remark,Update_Date=@update_date,Accept_Date=@accept_date Where CAO_ID=@cao_id And Business_Name=@business_name And IsDeleted=@isdeleted", con);
                }
                else if (wayModel.Deli_Status == "Rejected")
                {
                    update_cmd = new SqlCommand("Update CollectAssignOrder Set Deli_Men=@deli_men,Deli_Status=@deli_status,Rejected_Status=@rejected_status,Receiver_Name=@receiver_name,C_Name=@c_name,T_Name=@t_name,Tan_Sar_Price=@tan_sar_price,G_Name=@g_name,G_Price=@g_price,Expense_Fees=@expense_fees,Deli_Type=@deli_type,Return_Item_Amount=@return_item_amount,Half_Paid_Amount=@half_paid_amount,Deli_Men_Remark=@deli_men_remark,Assign_Remark=@assign_remark,Update_Date=@update_date,Complete_Date=@complete_date Where CAO_ID=@cao_id And Business_Name=@business_name And IsDeleted=@isdeleted", con);
                    update_cmd.Parameters.AddWithValue("@rejected_status", "Pending");
                    update_cmd.Parameters.AddWithValue("@return_item_amount", wayModel.Return_Item_Amount);
                    update_cmd.Parameters.AddWithValue("@half_paid_amount", wayModel.Half_Paid_Amount);
                    update_cmd.Parameters.AddWithValue("@complete_date", DateTime.Now);
                }
                else
                {
                    update_cmd = new SqlCommand("Update CollectAssignOrder Set Deli_Men=@deli_men,Deli_Status=@deli_status,Receiver_Name=@receiver_name,C_Name=@c_name,T_Name=@t_name,Tan_Sar_Price=@tan_sar_price,G_Name=@g_name,G_Price=@g_price,Expense_Fees=@expense_fees,Deli_Type=@deli_type,Deli_Men_Remark=@deli_men_remark,Assign_Remark=@assign_remark,Update_Date=@update_date,Instock_Date=@instock_date Where CAO_ID=@cao_id And Business_Name=@business_name And IsDeleted=@isdeleted", con);
                }
                //update_cmd = new SqlCommand("Update CollectAssignOrder Set Deli_Men=@deli_men,Deli_Status=@deli_status,Receiver_Name=@receiver_name,C_Name=@c_name,T_Name=@t_name,G_Name=@g_name,G_Price=@g_price,Expense_Fees=@expense_fees,Deli_Type=@deli_type,Deli_Men_Remark=@deli_men_remark,Assign_Remark=@assign_remark,Create_Date=@create_date,Update_Date=@update_date,Accept_Date=@accept_date Where CAO_ID=@cao_id And Business_Name=@business_name And IsDeleted=@isdeleted",con);  
                update_cmd.Parameters.AddWithValue("@cao_id", wayModel.CAO_ID);
                update_cmd.Parameters.AddWithValue("@deli_men", wayModel.Deli_Men);
                update_cmd.Parameters.AddWithValue("@deli_status", wayModel.Deli_Status);
                update_cmd.Parameters.AddWithValue("@receiver_name", wayModel.Receiver_Name);
                if (wayModel.C_Name == null || wayModel.C_Name == "")
                {
                    update_cmd.Parameters.AddWithValue("@c_name", "");
                }
                else
                {
                    update_cmd.Parameters.AddWithValue("@c_name", wayModel.C_Name);
                }
                if (wayModel.T_Name == null || wayModel.T_Name == "")
                {
                    update_cmd.Parameters.AddWithValue("@t_name", "");
                }
                else
                {
                    update_cmd.Parameters.AddWithValue("@t_name", wayModel.T_Name);
                }
                if (wayModel.Assign_Remark == null)
                {
                    update_cmd.Parameters.AddWithValue("@assign_remark", "");

                }
                else
                {
                    update_cmd.Parameters.AddWithValue("@assign_remark",wayModel.Assign_Remark);
                }
                update_cmd.Parameters.AddWithValue("@g_name", wayModel.G_Name);
                update_cmd.Parameters.AddWithValue("@g_price", wayModel.G_Price);
                update_cmd.Parameters.AddWithValue("@expense_fees", wayModel.Expense_Fees);
                update_cmd.Parameters.AddWithValue("@deli_type", wayModel.Deli_Type);

                if (wayModel.Deli_Men_Remark == null)
                {
                    update_cmd.Parameters.AddWithValue("@deli_men_remark", "");
                }
                else
                {
                    update_cmd.Parameters.AddWithValue("@deli_men_remark", wayModel.Deli_Men_Remark);
                }
                if (wayModel.Deli_Status == "Pending")
                {
                    //update_cmd.Parameters.AddWithValue("@create_date","");
                    update_cmd.Parameters.AddWithValue("@create_date", DateTime.Now);
                }
                //if deli status is assigned . 
/*                else 
                {
                    update_cmd.Parameters.AddWithValue("@create_date",DateTime.Now);
                }*/
                if (wayModel.Deli_Status == "Completed")
                {
                    update_cmd.Parameters.AddWithValue("@half_paid_amount", wayModel.Half_Paid_Amount);
                    update_cmd.Parameters.AddWithValue("@return_item_amount", wayModel.Return_Item_Amount);
                    //completed return item soe yin ayin loe rejected status ko pending htae ko mae htae taut vuu .
                    if(wayModel.Deli_Type=="Return Item")
                    {
                        //update_cmd.Parameters.AddWithValue("@rejected_status","");
                        update_cmd.Parameters.AddWithValue("@rejected_status", "");
                    }
                    //else
                    //{
                    //    update_cmd.Parameters.AddWithValue("@rejected_status", "");
                    //}
                    update_cmd.Parameters.AddWithValue("@complete_date", DateTime.Now);
                }
                else if (wayModel.Deli_Status == "Accepted")
                {
                    update_cmd.Parameters.AddWithValue("@expense_status","Paid");
                    update_cmd.Parameters.AddWithValue("@accept_date", DateTime.Now);
                }
                else if(wayModel.Deli_Status!="Rejected")
                {
                    update_cmd.Parameters.AddWithValue("@instock_date", DateTime.Now);
                }
                update_cmd.Parameters.AddWithValue("@tan_sar_price", wayModel.Tan_Sar_Price);
                update_cmd.Parameters.AddWithValue("@update_date", DateTime.Now);
                update_cmd.Parameters.AddWithValue("@business_name", wayModel.Business_Name);
                update_cmd.Parameters.AddWithValue("@isdeleted",'0');
                update_cmd.ExecuteNonQuery();
                con.Close();
                GC.Collect();
                return StatusCode(StatusCodes.Status202Accepted, "Updated Way");
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,ex.ToString());
            }
        }
        #endregion Update
        #region Delete
        [HttpDelete]
        [Route("api/manage/way")]
        public IActionResult Delete_Way(String CAO_ID,String Business_Name)
        {
            try
            {
                DefaultModel defaultModel = new DefaultModel();
                string connection = Configuration.GetConnectionString(defaultModel.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                SqlCommand delete_cmd = new SqlCommand("Update CollectAssignOrder Set IsDeleted=@isdeleted Where CAO_ID=@cao_id And Business_Name=@business_name", con);
                delete_cmd.Parameters.AddWithValue("@cao_id", CAO_ID);
                delete_cmd.Parameters.AddWithValue("@business_name", Business_Name);
                delete_cmd.Parameters.AddWithValue("@isdeleted", '1');
                delete_cmd.ExecuteNonQuery();
                con.Close();
                GC.Collect();
                return StatusCode(StatusCodes.Status202Accepted, "Deleted Way");
            }
            catch(Exception ex)
            {
                GC.Collect();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }
        #endregion Delete
        #region AcceptWayByDeliMen
        [HttpPut]
        [Route("api/manage/way/accept")]
        public IActionResult AcceptWay_By_DeliMen(String Deli_Men,String Business_Name)
        {
            try
            {
                DefaultModel defaultModel = new DefaultModel();
                string connection = Configuration.GetConnectionString(defaultModel.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                SqlCommand update_cmd = new SqlCommand("Update CollectAssignOrder Set Deli_Status=@update_deli_status,Expense_Status=@expense_status,Accept_Date=@accept_date Where Deli_Men Like @deli_men And Deli_Status=@previous_deli_status And Business_Name=@business_name And IsDeleted=@isdeleted", con);
                update_cmd.Parameters.AddWithValue("@update_deli_status", "Accepted");
                update_cmd.Parameters.AddWithValue("@previous_deli_status", "Completed");
                update_cmd.Parameters.AddWithValue("@expense_status", "Paid");
                update_cmd.Parameters.AddWithValue("@deli_men", Deli_Men);
                update_cmd.Parameters.AddWithValue("@accept_date", DateTime.Now);
                update_cmd.Parameters.AddWithValue("@business_name", Business_Name);
                update_cmd.Parameters.AddWithValue("@isdeleted", '0');
                update_cmd.ExecuteNonQuery();
                con.Close();
                GC.Collect();
                return StatusCode(StatusCodes.Status202Accepted, "Accepted Ways");
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }
        #endregion AcceptWayByDeliMen

        #region AssignWayByDeliMen
        [HttpPut]
        [Route("api/manage/way/assign")]
        public IActionResult AssignWay_By_DeliMen(String Deli_Men, String Business_Name)
        {
            try
            {
                DefaultModel defaultModel = new DefaultModel();
                string connection = Configuration.GetConnectionString(defaultModel.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                SqlCommand update_cmd = new SqlCommand("Update CollectAssignOrder Set Deli_Status=@update_deli_status,Expense_Status=@expense_status,Create_Date=@create_date,Update_Date=@update_date Where Deli_Men Like @deli_men And Deli_Status=@previous_deli_status And Business_Name=@business_name And IsDeleted=@isdeleted", con);
                update_cmd.Parameters.AddWithValue("@update_deli_status", "Assigned");
                update_cmd.Parameters.AddWithValue("@previous_deli_status", "Pending");
                update_cmd.Parameters.AddWithValue("@expense_status", "UnPaid");
                if (Deli_Men == "All")
                {
                    Deli_Men = "%";
                }
                update_cmd.Parameters.AddWithValue("@deli_men", Deli_Men);
                update_cmd.Parameters.AddWithValue("@create_date", DateTime.Now);
                update_cmd.Parameters.AddWithValue("@update_date", DateTime.Now);
                update_cmd.Parameters.AddWithValue("@business_name", Business_Name);
                update_cmd.Parameters.AddWithValue("@isdeleted", '0');
                update_cmd.ExecuteNonQuery();
                con.Close();
                GC.Collect();
                return StatusCode(StatusCodes.Status202Accepted, "Assigned Ways");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }
        #endregion AssignWayByDeliMen
        #region All_Complete_Ways_By_Admin
        [HttpPut]
        [Route("api/manage/way/complete")]
        public IActionResult All_Complete_Ways_By_Admin(String Deli_Men, String Business_Name)
        {
            try
            {
                DefaultModel defaultModel = new DefaultModel();
                string connection = Configuration.GetConnectionString(defaultModel.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                SqlCommand update_cmd = new SqlCommand("Update CollectAssignOrder Set Deli_Status=@update_deli_status,Update_Date=@update_date,Complete_Date=@complete_date Where Deli_Men Like @deli_men And Deli_Status=@previous_deli_status And Business_Name=@business_name And IsDeleted=@isdeleted", con);
                update_cmd.Parameters.AddWithValue("@update_deli_status", "Completed");
                update_cmd.Parameters.AddWithValue("@previous_deli_status", "Assigned");                
                update_cmd.Parameters.AddWithValue("@deli_men", Deli_Men);
                update_cmd.Parameters.AddWithValue("@complete_date", DateTime.Now);
                update_cmd.Parameters.AddWithValue("@update_date", DateTime.Now);
                update_cmd.Parameters.AddWithValue("@business_name", Business_Name);
                update_cmd.Parameters.AddWithValue("@isdeleted", '0');
                update_cmd.ExecuteNonQuery();
                con.Close();
                GC.Collect();
                return StatusCode(StatusCodes.Status202Accepted, "Changed Assigned To Completed Ways");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }
        #endregion All_Complete_Ways_By_Admin
        #region Get_AllPaid_Transfer_Amount
        [HttpGet]
        [Route("api/manage/way/allpaidtransfer")]
        public IActionResult Get_AllPaid_Transfer_Amount(String Deli_Men,String Business_Name)
        {
            try
            {
                DefaultModel defaultModel = new DefaultModel();
                string connection = Configuration.GetConnectionString(defaultModel.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                SqlCommand get_cmd = new SqlCommand("Select Total_Amount From CollectAssignOrder Where Deli_Men=@deli_men And Deli_Status=@deli_status And Deli_Type=@deli_type And Business_Name=@business_name And IsDeleted=@isdeleted", con);
                get_cmd.Parameters.AddWithValue("@deli_men", Deli_Men);
                get_cmd.Parameters.AddWithValue("@deli_status", "Completed");
                get_cmd.Parameters.AddWithValue("@deli_type", "All Paid");
                get_cmd.Parameters.AddWithValue("@business_name", Business_Name);
                get_cmd.Parameters.AddWithValue("@isdeleted", '0');
                SqlDataReader allpaidReader = get_cmd.ExecuteReader();
                int allPaidAmount = 0;
                while (allpaidReader.Read())
                {
                    allPaidAmount += Convert.ToInt32(allpaidReader["Total_Amount"].ToString());
                }
                allpaidReader.Close();
                SqlCommand get_cmd_transfer = new SqlCommand("Select Half_Paid_Amount from CollectAssignOrder Where  Deli_Men=@deli_men And Deli_Status=@deli_status And Deli_Type=@deli_type And Business_Name=@business_name And IsDeleted=@isdeleted",con);
                get_cmd_transfer.Parameters.AddWithValue("@deli_men", Deli_Men);
                get_cmd_transfer.Parameters.AddWithValue("@deli_status", "Completed");
                get_cmd_transfer.Parameters.AddWithValue("@deli_type", "Transfer");
                get_cmd_transfer.Parameters.AddWithValue("@business_name", Business_Name);
                get_cmd_transfer.Parameters.AddWithValue("@isdeleted", '0');
                SqlDataReader transferReader = get_cmd_transfer.ExecuteReader();
                int transferAmount = 0;
                while (transferReader.Read())
                {
                    transferAmount += Convert.ToInt32(transferReader["Half_Paid_Amount"].ToString());
                }
                transferReader.Close();
                int totalAllPaidTransferAmount = allPaidAmount + transferAmount;
                DataTable dt = new DataTable();
                dt.Columns.Add("Total_AllPaid_Transfer_Amount");
                dt.Rows.Add(totalAllPaidTransferAmount);
                con.Close();
                GC.Collect();
                return StatusCode(StatusCodes.Status200OK, JsonConvert.SerializeObject(dt));
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
            return Ok();
        }
        #endregion
    }
}
