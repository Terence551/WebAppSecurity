using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAppSecurity
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        private bool ValidateInput()
        {
            // validation check
            try
            {
                var error = 0;
                // validate email
                if (tb_email.Text == "")
                {
                    lb_finalmsg.Text = "Email/Password is wrong!";
                    lb_finalmsg.ForeColor = Color.Red;
                    error += 1;
                }
                // validate password
                if (tb_pwd.Text == "")
                {
                    lb_finalmsg.Text = "Email/Password is wrong!";
                    lb_finalmsg.ForeColor = Color.Red;
                    error += 1;
                }
                // no error 
                if (error == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        protected void LoginClick(object sender, EventArgs e)
        {
            bool validate = ValidateInput();
            if (validate == true)
            {
                string email = tb_email.Text;
                string pwd = tb_pwd.Text;
                SHA512Managed hashing = new SHA512Managed();
                string dbHash = getDBHash(email);
                string dbSalt = getDBSalt(email);
                Console.WriteLine("Test");

                try
                {

                    Console.WriteLine("Test");
                    if (dbSalt != null && dbSalt.Length > 0 && dbHash != null && dbHash.Length > 0)
                    {

                        Console.WriteLine("Test");
                        string pwdWithSalt = pwd + dbSalt;
                        byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
                        string pwdHash = Convert.ToBase64String(hashWithSalt);
                        if (pwdHash.Equals(dbHash))
                        { 
                            Session["Logged"] = tb_email.Text
                            Response.Redirect("~/", false); 
                        }
                        else
                        {
                            lb_finalmsg.Text = "Email/Password is wrong!";
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
                finally { }
            }
            
        }



        protected string getDBHash(string email)
        {
            string UserconDB = System.Configuration.
                ConfigurationManager.ConnectionStrings["190672BAppSecurity"]
                .ConnectionString;
            string gethash = null;
            SqlConnection connection = new SqlConnection(UserconDB); 
            string sql = "SELECT PasswordHash FROM Userzd WHERE Email=@Email"; 
            SqlCommand command = new SqlCommand(sql, connection); 
            command.Parameters.AddWithValue("@Email", email);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read()) 
                    { 
                        if (reader["PasswordHash"] != null) 
                        { 
                            if (reader["PasswordHash"] != DBNull.Value) 
                            { 
                                gethash = reader["PasswordHash"].ToString(); 
                            } 
                        } 
                    }
                }
            }
            catch (Exception ex) { throw new Exception(ex.ToString()); }
            finally { connection.Close(); }
            return gethash;
        }




        protected string getDBSalt(string email)
        {
            string UserconDB = System.Configuration.
                   ConfigurationManager.ConnectionStrings["190672BAppSecurity"]
                   .ConnectionString;
            string getsalt = null;
            SqlConnection connection = new SqlConnection(UserconDB); 
            string sql = "SELECT PasswordSalt FROM Userzd WHERE Email=@email"; 
            SqlCommand command = new SqlCommand(sql, connection); 
            command.Parameters.AddWithValue("@email", email);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader()) 
                { 
                    while (reader.Read()) 
                    { 
                        if (reader["PasswordSalt"] != null) 
                        { 
                            if (reader["PasswordSalt"] != DBNull.Value) 
                            { 
                                getsalt = reader["PasswordSalt"].ToString(); 
                            } 
                        } 
                    } 
                }
            }
            catch (Exception ex) 
            { 
                throw new Exception(ex.ToString()); 
            }
            finally 
            { 
                connection.Close(); 
            }
            return getsalt;
        }




    }
}