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

namespace DeliverySystemAPI.Controllers.DeliMen
{
    public class DeliMenController : ControllerBase
    {
        private IConfiguration Configuration;
        public DeliMenController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        #region Get
        [HttpGet]
        [Route("api/delimen")]
        public IActionResult Get_Deli_Ways(String Deli_Men,String Deli_Status,String Deli_Type,String Business_Name,String From_Date, String To_Date)
        {
            try
            {
                DefaultModel dm = new DefaultModel();
                string connection = Configuration.GetConnectionString(dm.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                SqlCommand get_cmd =null;
                if (From_Date == null && To_Date == null) // means the user want the client order by today date
                {
                    if (Deli_Type == null || Deli_Type == "All")
                    {
                        Deli_Type = "%";
                    }
                    get_cmd = new SqlCommand("SELECT ROW_NUMBER() OVER (ORDER BY CAO_ID ASC) AS SrNo,ClientOrder.Client_Name,CAO_ID,Receiver_Name,C_Name,T_Name,Deli_Type,Item_Price,G_Name,G_Price,Tan_Sar_Price,Deli_Price,Expense_Fees,Return_Item_Amount,Half_Paid_Amount,CollectAssignOrder.Total_Amount,Deli_Status,Deli_Men_Remark,Assign_Remark,Complete_Date,CollectAssignOrder.Create_Date From CollectAssignOrder Left Join ClientOrder On CollectAssignOrder.CO_ID=ClientOrder.CO_ID Where CollectAssignOrder.Deli_Men=@deli_men And CollectAssignOrder.Deli_Status=@deli_status And CollectAssignOrder.Deli_Type Like @deli_type And CollectAssignOrder.Business_Name=@business_name And CollectAssignOrder.IsDeleted=@isdeleted", con);
                    get_cmd.Parameters.AddWithValue("@deli_type", Deli_Type);
                }
                else // mean the user want the client order by filter date
                {
                    if (Deli_Status == "Assigned")
                    {
                        get_cmd = new SqlCommand("SELECT ROW_NUMBER() OVER (ORDER BY CollectAssignOrder.Create_Date DESC) AS SrNo,CAO_ID,Deli_Men,Deli_Status,ClientOrder.Client_Name,Receiver_Name,C_Name,T_Name,G_Name,G_Price,Tan_Sar_Price,Item_Price,Expense_Fees,Return_Item_Amount,Half_Paid_Amount,Deli_Price,Deli_Type,CollectAssignOrder.Total_Amount,Deli_Status,Expense_Status,Deli_Men_Remark,Assign_Remark,CollectAssignOrder.Create_Date,Accept_Date,CollectAssignOrder.Update_Date,Pending_Date,Accept_Date From CollectAssignOrder Full Join ClientOrder On ClientOrder.CO_ID=CollectAssignOrder.CO_ID Where Deli_Men Like @deli_men And Deli_Status Like @deli_status And Deli_Status<>'Instock' And CollectAssignOrder.Business_Name=@business_name And CollectAssignOrder.IsDeleted=@isdeleted And cast (CollectAssignOrder.[Create_Date] as date) between @from_date and @to_date", con);
                    }
                    else
                    {
                        get_cmd = new SqlCommand("SELECT ROW_NUMBER() OVER (ORDER BY CollectAssignOrder.Create_Date DESC) AS SrNo,CAO_ID,Deli_Men,Deli_Status,ClientOrder.Client_Name,Receiver_Name,C_Name,T_Name,G_Name,G_Price,Tan_Sar_Price,Item_Price,Expense_Fees,Return_Item_Amount,Half_Paid_Amount,Deli_Price,Deli_Type,CollectAssignOrder.Total_Amount,Deli_Status,Expense_Status,Deli_Men_Remark,Assign_Remark,CollectAssignOrder.Create_Date,Accept_Date,CollectAssignOrder.Update_Date,Pending_Date,Accept_Date From CollectAssignOrder Full Join ClientOrder On ClientOrder.CO_ID=CollectAssignOrder.CO_ID Where Deli_Men Like @deli_men And Deli_Status Like @deli_status And Deli_Status<>'Instock' And CollectAssignOrder.Business_Name=@business_name And CollectAssignOrder.IsDeleted=@isdeleted And cast([Accept_Date] as date) between @from_date and @to_date", con);
                    }
                    get_cmd.Parameters.AddWithValue("@from_date", From_Date);
                    get_cmd.Parameters.AddWithValue("@to_date", To_Date);
                    /*  get_cmd = new SqlCommand("SELECT ROW_NUMBER() OVER (ORDER BY CAO_ID ASC) AS SrNo,ClientOrder.Client_Name,CAO_ID,Receiver_Name,C_Name,T_Name,Deli_Type,Item_Price,G_Name,G_Price,Tan_Sar_Price,Deli_Price,Expense_Fees,Return_Item_Amount,Half_Paid_Amount,CollectAssignOrder.Total_Amount,Deli_Status,Deli_Men_Remark,Assign_Remark,CollectAssignOrder.Create_Date From CollectAssignOrder Left Join ClientOrder On CollectAssignOrder.CO_ID=ClientOrder.CO_ID Where CollectAssignOrder.Deli_Men=@deli_men And CollectAssignOrder.Deli_Status=@deli_status And CollectAssignOrder.Business_Name=@business_name And CollectAssignOrder.IsDeleted=@isdeleted And cast([Accept_Date] as date) between @from_date and @to_date", con);
                      get_cmd.Parameters.AddWithValue("@from_date", From_Date);
                      get_cmd.Parameters.AddWithValue("@to_date", To_Date);*/
                }
                get_cmd.Parameters.AddWithValue("@deli_men", Deli_Men);
                get_cmd.Parameters.AddWithValue("@deli_status", Deli_Status);
                get_cmd.Parameters.AddWithValue("@business_name", Business_Name);
                get_cmd.Parameters.AddWithValue("@isdeleted", '0');
                DataTable dt = new DataTable();
                if (Deli_Status=="Assigned")
                {
                    SqlDataAdapter da = new SqlDataAdapter(get_cmd);
                    da.Fill(dt);
                }
                //if deli men want to see by completed or accepted then we need to calculate total amount individually.
                else
                {
                    dt.Columns.Add("CAO_ID");
                    dt.Columns.Add("SrNo");
                    dt.Columns.Add("Client_Name");
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
                    SqlDataReader reader = get_cmd.ExecuteReader();
                    string deli_type = "";
                    int total_amount = 0;
                    while (reader.Read())
                    {
                        total_amount = 0;
                        deli_type = reader["Deli_Type"].ToString();
                        if (deli_type == "OS Paid" || deli_type == "All Paid" || deli_type== "အကုန်ရှင်းပီး")
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
                                total_amount +=Convert.ToInt32(reader["Total_Amount"].ToString())-0;
                            }
                            else
                            {
                                total_amount += Convert.ToInt32(reader["Total_Amount"].ToString()) - Convert.ToInt32(reader["Return_Item_Amount"].ToString());
                            }
                        }
                        else
                        {
                            //handling null
                            if (reader["Half_Paid_Amount"].ToString() == "")
                            {
                                total_amount += Convert.ToInt32(reader["Total_Amount"].ToString()) -0;
                            }
                            else
                            {
                                total_amount += Convert.ToInt32(reader["Total_Amount"].ToString()) - Convert.ToInt32(reader["Half_Paid_Amount"].ToString());
                            }
                        }
                        var sr_no = reader["SrNo"].ToString();
                        var cao_id = reader["CAO_ID"].ToString();
                        var client_name = reader["Client_Name"].ToString();
                        var receiver_name = reader["Receiver_Name"].ToString();
                        var c_name = reader["C_Name"].ToString();
                        var t_name = reader["T_Name"].ToString();
                        var item_price = reader["Item_Price"].ToString();
                        var g_name = reader["G_Name"].ToString();
                        var g_price = reader["G_Price"].ToString();
                        var deli_price = reader["Expense_Fees"].ToString();
                        var expense_fees = reader["Expense_Fees"].ToString();
                        var tan_sar_price = reader["Tan_Sar_Price"].ToString();
                        var return_item_amount = reader["Return_Item_Amount"].ToString();
                        var half_paid_amount = reader["Half_Paid_Amount"].ToString();
                        var deli_men_remark = reader["Deli_Men_Remark"].ToString();
                        string create_date = reader["Create_Date"].ToString();//same as assign date.
                        dt.Rows.Add(cao_id, sr_no, client_name, receiver_name, c_name, t_name, deli_type, item_price, g_name, g_price, deli_price,expense_fees, tan_sar_price,return_item_amount,half_paid_amount, total_amount, Deli_Status, deli_men_remark, create_date);
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
        #region Update
        [HttpPut]
        [Route("api/delimen")]
        public IActionResult Update_Deli_Status([FromBody] DeliMenModel deliMenModel)
        {
            try
            {
                DefaultModel dm = new DefaultModel();
                string connection = Configuration.GetConnectionString(dm.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                SqlCommand update_cmd = new SqlCommand("Update CollectAssignOrder Set Deli_Status=@deli_status,Deli_Type=@deli_type,Rejected_Status=@rejected_status,Return_Item_Amount=@return_item_amount,Half_Paid_Amount=@half_paid_amount,Tan_Sar_Price=@tan_sar_price,Deli_Men_Remark=@deli_men_remark,Payment_Status=@payment_status,Complete_Date=@complete_date,Update_Date=@update_date Where CAO_ID=@cao_id And Business_Name=@business_name And IsDeleted=@isdeleted", con);
                update_cmd.Parameters.AddWithValue("@cao_id", deliMenModel.CAO_ID);
                update_cmd.Parameters.AddWithValue("@deli_status", deliMenModel.Deli_Status);
                update_cmd.Parameters.AddWithValue("@tan_sar_price", deliMenModel.Tan_Sar_Price);
                if (deliMenModel.Deli_Status == "Completed")
                {
                    update_cmd.Parameters.AddWithValue("@complete_date",DateTime.Now);                    
                    if (deliMenModel.Deli_Type=="Return Item" || deliMenModel.Deli_Type=="Transfer" || deliMenModel.Deli_Type=="All Paid")
                    {
                        update_cmd.Parameters.AddWithValue("@payment_status", "Pending");
                    }
                    else
                    {
                        update_cmd.Parameters.AddWithValue("@payment_status", "");
                    }
                }
                else
                {
                    update_cmd.Parameters.AddWithValue("@payment_status", "");
                    update_cmd.Parameters.AddWithValue("@complete_date","");
                }
                update_cmd.Parameters.AddWithValue("@deli_type", deliMenModel.Deli_Type);
                //Check if deli type is return item then auto move to rejected status pending
                //if(deliMenModel.Deli_Type=="Return Item")
                //{
                //    update_cmd.Parameters.AddWithValue("@rejected_status","Pending");
                //}
                //completed return item soe yin ayin loe rejected status ko pending htae ko mae htae taut vuu .
                if (deliMenModel.Deli_Type == "Return Item")
                {
                    //update_cmd.Parameters.AddWithValue("@rejected_status","");
                    update_cmd.Parameters.AddWithValue("@rejected_status", "");
                }
                else
                {
                    update_cmd.Parameters.AddWithValue("@rejected_status", "");
                }
                //else
                //{
                //    update_cmd.Parameters.AddWithValue("@rejected_status","");
                //}
                int return_item_amount = 0, half_paid_amount = 0;
                if(deliMenModel.Deli_Type=="Return Item")
                {
                    return_item_amount = deliMenModel.Return_Or_Half_Amount;
                }
                //Deli Type Half Paid=Transfer
                else if(deliMenModel.Deli_Type == "Transfer")
                {
                    half_paid_amount= deliMenModel.Return_Or_Half_Amount;
                }
                update_cmd.Parameters.AddWithValue("@return_item_amount",return_item_amount);
                update_cmd.Parameters.AddWithValue("@half_paid_amount",half_paid_amount);
                update_cmd.Parameters.AddWithValue("@deli_men_remark", deliMenModel.Deli_Men_Remark);
                update_cmd.Parameters.AddWithValue("@update_date", DateTime.Now);
                //if (deliMenModel.Deli_Status == "Pending")
                //{
                //    update_cmd.Parameters.AddWithValue("@instock_date",DateTime.Now);
                //}
                //else
                //{
                //    update_cmd.Parameters.AddWithValue("@instock_date",DateTime.Now);
                //}
                update_cmd.Parameters.AddWithValue("@business_name", deliMenModel.Business_Name);
                update_cmd.Parameters.AddWithValue("@isdeleted", '0');
                update_cmd.ExecuteNonQuery();
                con.Close();
                GC.Collect();
                return StatusCode(StatusCodes.Status202Accepted, "Deli Status Updated");
            }
            catch(Exception ex)
            {
                GC.Collect();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }
        #endregion Update
    }
}
