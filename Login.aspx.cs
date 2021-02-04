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
using System.Timers;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Data;

namespace WebAppSecurity
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        // check email exist
        public int CheckEmail(string email)
        {
            int result;
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["190672BAppSecurity"].ConnectionString);
            DataTable dt = new DataTable();
            con.Open();
            string sql = "SELECT * FROM Userzd WHERE Email = @paraEmail";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@paraEmail", email);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            if (dt.Rows.Count < 1)
            {
                result = 1;
            }
            else
            {
                result = 0;
            }
            con.Close();
            return result;
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
                else
                {
                    // check database (email must be unique)
                    int result = CheckEmail(tb_email.Text.Trim());
                    if (result != 0)
                    {
                        error += 1;
                        lb_finalmsg.Text += "Email not exist! Try another.";
                        lb_finalmsg.ForeColor = Color.Red;
                    }
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
            catch
            {
                lb_finalmsg.Text = "There is an error with the submission, please try again!";
                return false;
            }
        }

        // lockout
        protected void putLogDB(int dbLog, string email)
        {
            string UserconDB = System.Configuration.
                ConfigurationManager.ConnectionStrings["190672BAppSecurity"]
                .ConnectionString;
            SqlConnection connection = new SqlConnection(UserconDB);
            string sql = "UPDATE Userzd SET LoggedOut=@Logged WHERE Email=@Email";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Email", email);
            command.Parameters.AddWithValue("@Logged", dbLog);
            try
            {
                connection.Open();
                int result = command.ExecuteNonQuery();
            }
            catch
            {
                lb_finalmsg.Text = "Failed to update lockout";
            }
            finally
            {
                connection.Close();
            }
        }

        // event click
        protected void LoginClick(object sender, EventArgs e)
        {
            // validate
            bool validateC = validateCaptcha();
            bool validateI = ValidateInput();
            if (validateI == true && validateC == true)
            {
                string email = HttpUtility.HtmlEncode(tb_email.Text);
                string pwd = HttpUtility.HtmlEncode(tb_pwd.Text);
                SHA512Managed hashing = new SHA512Managed();
                // hash and salt retrieve from db
                string dbHash = getDBHash(email);
                string dbSalt = getDBSalt(email);

                try
                {
                    if (dbSalt != null && dbSalt.Length > 0 && dbHash != null && dbHash.Length > 0)
                    {
                        string pwdWithSalt = pwd + dbSalt;
                        byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
                        string pwdHash = Convert.ToBase64String(hashWithSalt);
                        if (pwdHash.Equals(dbHash))
                        {
                            // lockout retrieve from db
                            int dbLog = getDBLogged(email);
                            if (dbLog >= 3)
                            {
                                lb_finalmsg.Text = "THIS ACCOUNT IS LOCKED!";
                            }
                            else
                            {
                                
                                // session
                                Session["LoggedIn"] = tb_email.Text.ToString();
                                // cookie
                                string guid = Guid.NewGuid().ToString();
                                Session["AuthToken"] = guid;
                                Response.Cookies.Add(new HttpCookie("AuthToken", guid));
                                // lockout
                                putLogDB(0, email);
                                Response.Redirect("shop", false);

                            }
                        }
                        else
                        {
                            //lockout
                            int dbLog = getDBLogged(email);
                            dbLog += 1;

                            putLogDB(dbLog, email);

                            int leftlog = 3-dbLog;
                            if(leftlog <= 0)
                            {
                                lb_finalmsg.Text = "This account has been locked";
                            }
                            else
                            {
                                lb_finalmsg.Text = "Email/Password is wrong! " + leftlog + " try left";
                            }

                        }
                    }
                }
                catch { lb_finalmsg.Text = "Error in verifying account"; }
                finally { }
            }
            else
            {
                if (validateC != true)
                {
                    lb_finalmsg.Text = "Bot detected!";
                }
                else
                {
                    lb_finalmsg.Text = "Input have error";
                }
            }
            
        }

        // get log from database
        protected int getDBLogged(string email)
        {
            int getLogged = 0;
            string UserconDB = System.Configuration.
                ConfigurationManager.ConnectionStrings["190672BAppSecurity"]
                .ConnectionString;
            SqlConnection connection = new SqlConnection(UserconDB);
            string sql = "SELECT LoggedOut FROM Userzd WHERE Email=@Email";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Email", email);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["LoggedOut"] != null)
                        {
                            if (reader["LoggedOut"] != DBNull.Value)
                            {
                                getLogged = Convert.ToInt32(reader["LoggedOut"]);
                            }
                        }
                    }
                }

            }
            catch
            { lb_finalmsg.Text = "Error in accessing Database"; }
            finally { }


            return getLogged;
        }

        // get hash from database
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
            catch { lb_finalmsg.Text = "Error in accessing Database"; }
            finally { connection.Close(); }
            return gethash;
        }



        // get salt from database
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
            catch { lb_finalmsg.Text = "Error in accessing Database"; }
            finally 
            { 
                connection.Close(); 
            }
            return getsalt;
        }




        // captcha v3
        // Site client key -->> 6LfpPkgaAAAAAM-Is4yw2dLLGS6lOdsBMwNLSB-G
        // Secret server key -->> 6LfpPkgaAAAAAPA-xeHYSjW6iQQ5MXD_LoxvHr9H
        public class CaptchaResponse
        {
            public bool Success { get; set; }
            public double Score { get; set; }
            public string Action { get; set; }
            public string Hostname { get; set; }
            [JsonProperty(PropertyName = "error-code")]
            public IEnumerable<string> ErrorCodes { get; set; }
            [JsonProperty(PropertyName = "challenge_ts")]
            public DateTime ChallengeTime { get; set; }
        }
        public bool validateCaptcha()
        {
            bool result;

            string captchaResponse = Request.Form["g-recaptcha-response"];

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create
                ("https://www.google.com/recaptcha/api/siteverify?secret=6LfpPkgaAAAAAPA-xeHYSjW6iQQ5MXD_LoxvHr9H &response=" + captchaResponse);
            try
            {
                using (WebResponse wResponse = req.GetResponse())
                {
                    using (StreamReader readStream = new StreamReader(wResponse.GetResponseStream()))
                    {
                        string jsonResponse = readStream.ReadToEnd();

                        lb_gscore.Text = jsonResponse.ToString();

                        JavaScriptSerializer js = new JavaScriptSerializer();

                        CaptchaResponse jsonObject = js.Deserialize<CaptchaResponse>(jsonResponse);

                        result = Convert.ToBoolean(jsonObject.Success);
                    }
                }
                return result;
            }
            catch { lb_finalmsg.Text = "Error in google captcha";
                return false;
            }
        }


    }
}