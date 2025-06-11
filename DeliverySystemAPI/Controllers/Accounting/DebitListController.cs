using DeliverySystemAPI.Model;
using DeliverySystemAPI.Services.OverAll;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DeliverySystemAPI.Controllers.Accounting
{
    public class DebitListController : ControllerBase
    {
        private IConfiguration Configuration;
        public DebitListController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        #region Get
        [HttpGet]
        [Route("api/accounting/debit")]
        public IActionResult Get_Debit_Data(String Client_Name, String Debit_Type, String Payment_Status, String From_Date, String To_Date, String Business_Name)
        {
            try
            {
                DefaultModel defaultModel = new DefaultModel();
                string connection = Configuration.GetConnectionString(defaultModel.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                DataTable dt = new DataTable();
                if (Debit_Type == "To Get")
                {
                    dt.Columns.Add("SrNo");
                    dt.Columns.Add("CO_ID");
                    dt.Columns.Add("CAO_ID");
                    dt.Columns.Add("Client_Name");
                    dt.Columns.Add("Receiver_Name");
                    dt.Columns.Add("Deli_Men");
                    dt.Columns.Add("C_Name");
                    dt.Columns.Add("T_Name");
                    dt.Columns.Add("Tan_Sar_Price");
                    dt.Columns.Add("Deli_Type");
                    dt.Columns.Add("Total_Amount");
                    /*                    dt.Columns.Add("Item_Price");
                                        dt.Columns.Add("Expense_Fees");
                                        dt.Columns.Add("Half_Paid_Amount");
                                        dt.Columns.Add("Return_Item_Amount");*/
                    dt.Columns.Add("Create_Date");
                    dt.Columns.Add("Payment_Date");
                    dt.Columns.Add("Complete_Date");
                    //start getting co_id by client Name
                    //SqlCommand get_cmd = new SqlCommand("Select ROW_NUMBER() OVER (ORDER BY CAO_ID DESC) As SrNo,CO_ID,CAO_ID,Receiver_Name,Deli_Men,C_Name,T_Name,Tan_Sar_Price,Total_Amount,Item_Price,Expense_Fees,Half_Paid_Amount,Return_Item_Amount,Deli_Type,Create_Date  From CollectAssignOrder Where CO_ID in (Select Client_Name From ClientOrder Where Client_Name Like @client_name And Business_Name=@business_name And IsDeleted=@isdeleted) And Deli_Type<>@deli_type1 And Deli_Type<>@deli_type2 And Deli_Type<>@deli_type3 And Deli_Type<>@deli_type4 And Deli_Type<>@deli_type5 And Payment_Status=@payment_status", con);
                    //SqlCommand get_cmd = new SqlCommand("Select ROW_NUMBER() OVER (ORDER BY CAO_ID DESC) As SrNo,CollectAssignOrder.CO_ID,CAO_ID,ClientOrder.Client_Name,Receiver_Name,Deli_Men,C_Name,T_Name,Tan_Sar_Price,CollectAssignOrder.Total_Amount,Item_Price,Expense_Fees,Half_Paid_Amount,Return_Item_Amount,Deli_Type,CollectAssignOrder.Create_Date,Payment_Date  From CollectAssignOrder Full Join ClientOrder On ClientOrder.CO_ID=CollectAssignOrder.CO_ID And ClientOrder.Client_Name=@client_name And ClientOrder.Business_Name=@business_name And ClientOrder.IsDeleted=@isdeleted  Where Deli_Type<>@deli_type1 And Deli_Type<>@deli_type2 And Deli_Type<>@deli_type3 And Deli_Type<>@deli_type4 And Deli_Type<>@deli_type5 And Payment_Status=@payment_status", con);
                    //SqlCommand get_cmd = new SqlCommand("Select ROW_NUMBER() OVER (ORDER BY CAO_ID DESC) As SrNo,CollectAssignOrder.CO_ID,CAO_ID,ClientOrder.Client_Name,Receiver_Name,Deli_Men,C_Name,T_Name,Tan_Sar_Price,CollectAssignOrder.Total_Amount,Item_Price,Expense_Fees,Half_Paid_Amount,Return_Item_Amount,Deli_Type,CollectAssignOrder.Create_Date,Payment_Date  From CollectAssignOrder Full Join ClientOrder On ClientOrder.Client_Name Like @client_name And ClientOrder.CO_ID=CollectAssignOrder.CO_ID And ClientOrder.Business_Name=@business_name And ClientOrder.IsDeleted=@isdeleted And Deli_Type<>@deli_type1 And Deli_Type<>@deli_type2 And Deli_Type<>@deli_type3 And Deli_Type<>@deli_type4 And Deli_Type<>@deli_type5 And Payment_Status=@payment_status", con);
                    //SqlCommand get_cmd=new SqlCommand("Select ROW_NUMBER() OVER (ORDER BY CAO_ID DESC) As SrNo,CollectAssignOrder.CO_ID,CAO_ID,ClientOrder.Client_Name,Receiver_Name,Deli_Men,C_Name,T_Name,Tan_Sar_Price,CollectAssignOrder.Total_Amount,Item_Price,Expense_Fees,Half_Paid_Amount,Return_Item_Amount,Deli_Type,CollectAssignOrder.Create_Date,Payment_Date From CollectAssignOrder Inner Join ClientOrder On ClientOrder.Client_Name='Tauras htun' And ClientOrder.Business_Name='7DayDelivery' And ClientOrder.IsDeleted='0' And CollectAssignOrder.Deli_Type<>'Cash' And CollectAssignOrder.Deli_Type<>'UnPaid' And Deli_Type<>'All Paid' And Deli_Type<>'Only Deli' And Deli_Type<>@deli And Payment_Status=@payment_status")
                    SqlCommand get_cmd = null;
                    if (From_Date == null && To_Date == null)
                    {
                        get_cmd = new SqlCommand("Select ROW_NUMBER() OVER (ORDER BY Complete_Date DESC) As SrNo,ClientOrder.Client_Name,CollectAssignOrder.Business_Name,CollectAssignOrder.IsDeleted,CollectAssignOrder.CO_ID,CAO_ID,Receiver_Name,Deli_Men,C_Name,T_Name,Tan_Sar_Price,CollectAssignOrder.Total_Amount,Item_Price,Deli_Price,Expense_Fees,Half_Paid_Amount,Return_Item_Amount,Deli_Type,CollectAssignOrder.Create_Date,CollectAssignOrder.Payment_Date,Complete_Date From CollectAssignOrder Inner Join  ClientOrder On(ClientOrder.CO_ID=CollectAssignOrder.CO_ID) And CollectAssignOrder.CO_ID in (Select CO_ID From ClientOrder Where Client_Name Like @client_name And Business_Name=@business_name And IsDeleted=@isdeleted)  And Deli_Type<>@deli_type1 And Deli_Type<>@deli_type2 And Deli_Type<>@deli_type3 And Deli_Type<>@deli_type4 And Deli_Type<>@deli_type5 And Deli_Type<>@deli_type6 And CollectAssignOrder.Payment_Status=@payment_status", con);
                    }
                    //getting Client To Get data by date for showing in overall.
                    else
                    {
                        get_cmd = new SqlCommand("Select ROW_NUMBER() OVER (ORDER BY Complete_Date DESC) As SrNo,ClientOrder.Client_Name,CollectAssignOrder.Business_Name,CollectAssignOrder.IsDeleted,CollectAssignOrder.CO_ID,CAO_ID,Receiver_Name,Deli_Men,C_Name,T_Name,Tan_Sar_Price,CollectAssignOrder.Total_Amount,Item_Price,Deli_Price,Expense_Fees,Half_Paid_Amount,Return_Item_Amount,Deli_Type,CollectAssignOrder.Create_Date,CollectAssignOrder.Payment_Date,Complete_Date From CollectAssignOrder Inner Join  ClientOrder On(ClientOrder.CO_ID=CollectAssignOrder.CO_ID) And CollectAssignOrder.CO_ID in (Select CO_ID From ClientOrder Where Client_Name Like @client_name And Business_Name=@business_name And IsDeleted=@isdeleted)  And Deli_Type<>@deli_type1 And Deli_Type<>@deli_type2 And Deli_Type<>@deli_type3 And Deli_Type<>@deli_type4 And Deli_Type<>@deli_type5  And CollectAssignOrder.Payment_Status=@payment_status And cast(CollectAssignOrder.[Payment_Date] as date) between @from_date And @to_date", con);
                        get_cmd.Parameters.AddWithValue("@from_date", From_Date);
                        get_cmd.Parameters.AddWithValue("@to_date", To_Date);
                    }

                    get_cmd.Parameters.AddWithValue("@client_name", Client_Name);
                    get_cmd.Parameters.AddWithValue("@deli_type1", "Cash");
                    get_cmd.Parameters.AddWithValue("@deli_type2", "UnPaid");
                    get_cmd.Parameters.AddWithValue("@deli_type3", "OS Paid");
                    get_cmd.Parameters.AddWithValue("@deli_type4", "Only Deli");
                    get_cmd.Parameters.AddWithValue("@deli_type5", "Deli Free");
                    get_cmd.Parameters.AddWithValue("@deli_type6", "Return Item");
                    get_cmd.Parameters.AddWithValue("@payment_status", Payment_Status);
                    get_cmd.Parameters.AddWithValue("@business_name", Business_Name);
                    get_cmd.Parameters.AddWithValue("@isdeleted", '0');
                    /*                    SqlDataAdapter da = new SqlDataAdapter(get_cmd);
                                        da.Fill(dt);*/
                    //using sql data reader to sum amount by deli type and showing in total amount
                    SqlDataReader reader = get_cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        int total_amount = 0;
                        string deli_type = reader["Deli_Type"].ToString();
                        if (deli_type == "All Paid")
                        {
                            if (reader["Total_Amount"].ToString() == "")
                            {
                                total_amount += 0;
                            }
                            else
                            {
                                total_amount += Convert.ToInt32(reader["Total_Amount"].ToString());
                            }

                        }
                        else if (deli_type == "Item Paid")
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
                        else if (deli_type == "No Deli")
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
                        else if (deli_type == "Return Item")
                        {
                            if (reader["Return_Item_Amount"].ToString() == "")
                            {
                                total_amount += 0;
                            }
                            else
                            {
                                total_amount += Convert.ToInt32(reader["Return_Item_Amount"].ToString());
                            }
                        }
                        /*                        else if(deli_type=="Tan Sar")
                                                {
                                                    if (reader["Tan_Sar_Price"].ToString() == "")
                                                    {
                                                        total_amount += 0;
                                                    }
                                                    else
                                                    {
                                                        total_amount += Convert.ToInt32(reader["Tan_Sar_Price"].ToString());
                                                    }
                                                }*/
                        else
                        {
                            //handling null
                            if (reader["Half_Paid_Amount"].ToString() == "")
                            {
                                total_amount += 0;
                            }
                            else
                            {
                                total_amount += Convert.ToInt32(reader["Half_Paid_Amount"].ToString());
                            }

                        }
                        if (total_amount == 0 && deli_type == "Tan Sar")
                        {
                            var sr_no = reader["SrNo"].ToString();
                            var co_id = reader["CO_ID"].ToString();
                            var cao_id = reader["CAO_ID"].ToString();
                            var client_name = reader["Client_Name"].ToString();
                            var receiver_name = reader["Receiver_Name"].ToString();
                            var deli_men = reader["Deli_Men"].ToString();
                            var c_name = reader["C_Name"].ToString();
                            var t_name = reader["T_Name"].ToString();
                            var tan_sar_price = reader["Tan_Sar_Price"].ToString();
                            var create_date = reader["Create_Date"].ToString();//same as assign date.
                            var payment_date = reader["Payment_Date"].ToString();
                            var complete_date = reader["Complete_Date"].ToString();
                            dt.Rows.Add(sr_no, co_id, cao_id, client_name, receiver_name, deli_men, c_name, t_name, tan_sar_price, deli_type, total_amount, create_date, payment_date, complete_date);
                        }
                        else if (total_amount != 0)
                        {
                            var sr_no = reader["SrNo"].ToString();
                            var co_id = reader["CO_ID"].ToString();
                            var cao_id = reader["CAO_ID"].ToString();
                            var client_name = reader["Client_Name"].ToString();
                            var receiver_name = reader["Receiver_Name"].ToString();
                            var deli_men = reader["Deli_Men"].ToString();
                            var c_name = reader["C_Name"].ToString();
                            var t_name = reader["T_Name"].ToString();
                            var tan_sar_price = reader["Tan_Sar_Price"].ToString();
                            var create_date = reader["Create_Date"].ToString();//same as assign date.
                            var payment_date = reader["Payment_Date"].ToString();
                            var complete_date = reader["Complete_Date"].ToString();
                            dt.Rows.Add(sr_no, co_id, cao_id, client_name, receiver_name, deli_men, c_name, t_name, tan_sar_price, deli_type, total_amount, create_date, payment_date, complete_date);
                        }
                    }//while loop finished reading data
                    reader.Close();
                }
                else if (Debit_Type == "To Pay")
                {
                    AccountingDebitOverall call_service = new AccountingDebitOverall();
                    Object result = call_service.Get_Accounting_To_Pay(Client_Name, Payment_Status, From_Date, To_Date, Business_Name, con);
                    GC.Collect();
                    return StatusCode(StatusCodes.Status200OK, result);
                }
                else if (Debit_Type == "Return Item")
                {
                    AccountingDebitOverall call_service = new AccountingDebitOverall();
                    Object result = call_service.Get_Return_Item_Data(Client_Name, Payment_Status, From_Date, To_Date, Business_Name, con);
                    GC.Collect();
                    return StatusCode(StatusCodes.Status200OK, result);
                }
                con.Close();
                GC.Collect();
                return StatusCode(StatusCodes.Status200OK, JsonConvert.SerializeObject(dt));
            }
            catch (Exception ex)
            {
                GC.Collect();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }
        #endregion Get

        #region Update_Payment_Status
        [HttpPut]
        [Route("api/accounting/debit")]
        public IActionResult Update_Payment_Status(String ID, String Client_Name, String Debit_Type, String Business_Name)
        {
            try
            {
                DefaultModel defaultModel = new DefaultModel();
                string connection = Configuration.GetConnectionString(defaultModel.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                //SqlCommand update_cmd = new SqlCommand("Update CollectAssignOrder Set Payment_Status=@payment_status,Payment_Date=@payment_date,Update_Date=@update_date Where Client_Name in (Select Client_Name from ClientOrder Where Client_Name Like @client_name and Business_Name=@business_name and IsDeleted=@isdeleted) And CAO_ID Like @cao_id And Payment_Status=@previous_payment_status And Business_Name=@business_name And IsDeleted=@isdeleted", con);
                SqlCommand update_cmd = null;
                if (Debit_Type == "To Get")
                {
                    //when user click complete payment button
                    if (ID == "%")
                    {
                        //when selecting client is all
                        if (Client_Name == "%")
                        {
                            update_cmd = new SqlCommand("Update CollectAssignOrder Set Payment_Status=@payment_status,Payment_Date=@payment_date,Update_Date=@update_date Where Deli_Type<>@deli_type1 And Deli_Type<>@deli_type2 And Deli_Type<>@deli_type3 And Deli_Type<>@deli_type4 And Deli_Type<>@deli_type5 And CollectAssignOrder.Payment_Status=@previous_payment_status And Business_Name=@business_name and IsDeleted=@isdeleted", con);
                            update_cmd.Parameters.AddWithValue("@deli_type1", "Cash");
                            update_cmd.Parameters.AddWithValue("@deli_type2", "UnPaid");
                            update_cmd.Parameters.AddWithValue("@deli_type3", "All Paid");
                            update_cmd.Parameters.AddWithValue("@deli_type4", "Only Deli");
                            update_cmd.Parameters.AddWithValue("@deli_type5", "Deli Free");
                            update_cmd.Parameters.AddWithValue("@previous_payment_status", "Pending");
                            update_cmd.Parameters.AddWithValue("@payment_status", "Completed");
                            update_cmd.Parameters.AddWithValue("@payment_date", DateTime.Now);
                            update_cmd.Parameters.AddWithValue("@business_name", Business_Name);
                            update_cmd.Parameters.AddWithValue("@update_date", DateTime.Now);
                            update_cmd.Parameters.AddWithValue("@isdeleted", '0');
                            update_cmd.ExecuteNonQuery();
                        }
                        //when client is selected.
                        else
                        {
                            update_cmd = new SqlCommand("Update CollectAssignOrder Set Payment_Status=@payment_status,Payment_Date=@payment_date,Update_Date=@update_date Where CollectAssignOrder.CO_ID in (Select CO_ID From ClientOrder Where Client_Name=@client_name And Business_Name=@business_name And  IsDeleted=@isdeleted) And Business_Name=@business_name And IsDeleted=@isdeleted", con);
                            update_cmd.Parameters.AddWithValue("@client_name", Client_Name);
                            update_cmd.Parameters.AddWithValue("@payment_status", "Completed");
                            update_cmd.Parameters.AddWithValue("@payment_date", DateTime.Now);
                            update_cmd.Parameters.AddWithValue("@business_name", Business_Name);
                            update_cmd.Parameters.AddWithValue("@update_date", DateTime.Now);
                            update_cmd.Parameters.AddWithValue("@isdeleted", '0');
                        }

                    }
                    //when user want to change complete payment by individual.
                    else
                    {
                        update_cmd = new SqlCommand("Update CollectAssignOrder Set Payment_Status=@payment_status,Payment_Date=@payment_date,Update_Date=@update_date Where CAO_ID=@cao_id And Business_Name=@business_name And IsDeleted=@isdeleted", con);
                        update_cmd.Parameters.AddWithValue("@cao_id", ID);
                        update_cmd.Parameters.AddWithValue("@payment_status", "Completed");
                        update_cmd.Parameters.AddWithValue("@payment_date", DateTime.Now);
                        update_cmd.Parameters.AddWithValue("@business_name", Business_Name);
                        update_cmd.Parameters.AddWithValue("@update_date", DateTime.Now);
                        update_cmd.Parameters.AddWithValue("@isdeleted", '0');
                    }
                }
                else if (Debit_Type == "Return Item")
                {
                    update_cmd = new SqlCommand("Update CollectAssignOrder Set Payment_Status=@payment_status,Payment_Date=@payment_date,Update_Date=@update_date Where CAO_ID=@cao_id And Business_Name=@business_name And IsDeleted=@isdeleted", con);
                    update_cmd.Parameters.AddWithValue("@cao_id", ID);
                    update_cmd.Parameters.AddWithValue("@payment_status", "Completed");
                    update_cmd.Parameters.AddWithValue("@payment_date", DateTime.Now);
                    update_cmd.Parameters.AddWithValue("@business_name", Business_Name);
                    update_cmd.Parameters.AddWithValue("@update_date", DateTime.Now);
                    update_cmd.Parameters.AddWithValue("@isdeleted", '0');
                }
                //For To Pay 
                else if (Debit_Type == "To Pay")
                {
                    //when user click complete payment button
                    if (ID == "%")
                    {
                        //when selecting client is all
                        if (Client_Name == "%")
                        {
                            update_cmd = new SqlCommand("Update ClientOrder Set Payment_Status=@payment_status,Payment_Date=@payment_date,Update_Date=@update_date Where Payment_Status=@previous_payment_status And Business_Name=@business_name and IsDeleted=@isdeleted", con);
                            update_cmd.Parameters.AddWithValue("@previous_payment_status", "Pending");
                            update_cmd.Parameters.AddWithValue("@payment_status", "Completed");
                            update_cmd.Parameters.AddWithValue("@payment_date", DateTime.Now);
                            update_cmd.Parameters.AddWithValue("@business_name", Business_Name);
                            update_cmd.Parameters.AddWithValue("@update_date", DateTime.Now);
                            update_cmd.Parameters.AddWithValue("@isdeleted", '0');
                            update_cmd.ExecuteNonQuery();
                        }
                        //when client is selected.
                        else
                        {
                            update_cmd = new SqlCommand("Update ClientOrder Set Payment_Status=@payment_status,Payment_Date=@payment_date,Update_Date=@update_date Where Client_Name=@client_name And Payment_Status=@previous_payment_status And Business_Name=@business_name And IsDeleted=@isdeleted", con);
                            update_cmd.Parameters.AddWithValue("@client_name", Client_Name);
                            update_cmd.Parameters.AddWithValue("@previous_payment_status", "Pending");
                            update_cmd.Parameters.AddWithValue("@payment_status", "Completed");
                            update_cmd.Parameters.AddWithValue("@payment_date", DateTime.Now);
                            update_cmd.Parameters.AddWithValue("@business_name", Business_Name);
                            update_cmd.Parameters.AddWithValue("@update_date", DateTime.Now);
                            update_cmd.Parameters.AddWithValue("@isdeleted", '0');
                        }

                    }
                    //when user want to change complete payment by individual.
                    else
                    {
                        update_cmd = new SqlCommand("Update ClientOrder Set Payment_Status=@payment_status,Payment_Date=@payment_date,Update_Date=@update_date Where CO_ID=@co_id And Business_Name=@business_name And IsDeleted=@isdeleted", con);
                        update_cmd.Parameters.AddWithValue("@co_id", ID);
                        update_cmd.Parameters.AddWithValue("@payment_status", "Completed");
                        update_cmd.Parameters.AddWithValue("@payment_date", DateTime.Now);
                        update_cmd.Parameters.AddWithValue("@business_name", Business_Name);
                        update_cmd.Parameters.AddWithValue("@update_date", DateTime.Now);
                        update_cmd.Parameters.AddWithValue("@isdeleted", '0');
                    }
                }
                //SqlCommand update_cmd = new SqlCommand("Update CollectAssignOrder Set Payment_Status=@payment_status,Payment_Date=@payment_date,Update_Date=@update_date Full Join ClientOrder On Client_Name=@client_Name Where CAO_ID Like @cao_id And Payment_Status=@previous_payment_status And Business_Name=@business_name And IsDeleted=@isdeleted", con);
                update_cmd.ExecuteNonQuery();
                con.Close();
                GC.Collect();
                return StatusCode(StatusCodes.Status202Accepted, "Payment Status Changed");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }

        }
        #endregion Update_Payment_Status
        #region Get_Today_Rejected_Complete_Amount
        [HttpGet]
        [Route("api/accounting/debit/overall/rejected")]
        public IActionResult Get_Rejected_Amount_For_Overall(String Client, String Business_Name)
        {
            try
            {
                DefaultModel dm = new DefaultModel();
                string connection = Configuration.GetConnectionString(dm.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                SqlCommand get_cmd = new SqlCommand("Select SUM(Item_Price) As Rejected_Amount From CollectAssignOrder Where CollectAssignOrder.CO_ID in (Select ClientOrder.CO_ID From ClientOrder Where Client_Name Like @client_name And Business_Name=@business_name And IsDeleted=@isdeleted)  And Rejected_Status=@rejected_status And cast([Rejected_Complete_Date] as date) between @from_date And @to_date And Business_Name=@business_name And IsDeleted=@isdeleted", con);
                get_cmd.Parameters.AddWithValue("@client_name", Client);
                get_cmd.Parameters.AddWithValue("@rejected_status", "Completed");
                get_cmd.Parameters.AddWithValue("@from_date", DateTime.Now.ToString("yyyy-MM-dd"));
                get_cmd.Parameters.AddWithValue("@to_date", DateTime.Now.ToString("yyyy-MM-dd"));
                get_cmd.Parameters.AddWithValue("@business_name", Business_Name);
                get_cmd.Parameters.AddWithValue("@isdeleted", '0');
                SqlDataAdapter da = new SqlDataAdapter(get_cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                con.Close();
                GC.Collect();
                return StatusCode(StatusCodes.Status200OK, JsonConvert.SerializeObject(dt));
            }
            catch (Exception ex)
            {
                GC.Collect();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }
        #endregion Get_Today_Rejected_Complete_Amount
        #region Client_To_Pay_Paid_Amount
        [HttpPut]
        [Route("api/accounting/debit/topay")]
        public IActionResult Add_Client_To_Pay_Paid_Amount(int CO_ID, int To_Pay_Paid_Amount, String Business_Name)
        {
            try
            {
                DefaultModel dm = new DefaultModel();
                string connection = Configuration.GetConnectionString(dm.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                SqlCommand update_cmd = new SqlCommand("Update ClientOrder Set To_Pay_Paid_Amount=@paid_amount,Update_Date=@update_date Where CO_ID=@cao_id And Business_Name=@business_name And IsDeleted=@isdeleted", con);
                update_cmd.Parameters.AddWithValue("@paid_amount", To_Pay_Paid_Amount);
                update_cmd.Parameters.AddWithValue("@cao_id", CO_ID);
                update_cmd.Parameters.AddWithValue("@update_date", DateTime.Now);
                update_cmd.Parameters.AddWithValue("@business_name", Business_Name);
                update_cmd.Parameters.AddWithValue("@isdeleted", '0');
                update_cmd.ExecuteNonQuery();
                con.Close();
                GC.Collect();
                return StatusCode(StatusCodes.Status202Accepted, "Updated To Pay Paid Amount");
            }
            catch (Exception ex)
            {
                GC.Collect();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }
        #endregion Client_To_Pay_Paid_Amount
    }
}
