using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
// validate
using System.Text.RegularExpressions;
using System.Drawing;
// database
using System.Data.SqlClient;
// Hash
using System.Configuration;
using System.Data;
using System.Text;
using System.Security.Cryptography;

namespace WebAppSecurity
{
    public partial class Register : System.Web.UI.Page
    {
        static string finalHash;
        static string salt;
        byte[] Key;
        byte[] IV;

        protected void Page_Load(object sender, EventArgs e)
        {
            calendar.Visible = false;
        }
        // calendarBtn
        protected void calendarCall(object sender, EventArgs e)
        {
            if (calendar.Visible)
            {
                calendar.Visible = false;
            }
            else
            {
                calendar.Visible = true;
            }
            calendar.Attributes.Add("style", "position:absolute");
        }
        // calendar_Select
        protected void calendar_select(object sender, EventArgs e)
        {
            tb_dob.Text = calendar.SelectedDate.ToString("dd/MM/yyyy");
            calendar.Visible = false;
        }
        // calendar_render
        protected void calendar_render(object sender, DayRenderEventArgs e)
        {
            if (e.Day.IsOtherMonth)
            {
                e.Day.IsSelectable = false;
            }
        }

        // email must be unique
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
        //Ctrl k + Ctrl c
        private bool ValidateInput()
        {
            // validation check
            try
            {
                var error = 0;
                // validate fname
                if (tb_fname.Text == "")
                {
                    lb_fname.Text = "Is Empty!";
                    lb_fname.ForeColor = Color.Red;
                    error += 1;
                }
                if (Regex.IsMatch(tb_fname.Text, "^[a-zA-Z]+$") != true)
                {
                    lb_fname.Text = "Invalid First name!";
                    lb_fname.ForeColor = Color.Red;
                    error += 1;
                }
                // validate lname
                if (tb_lname.Text == "")
                {
                    lb_lname.Text = "Is Empty!";
                    lb_lname.ForeColor = Color.Red;
                    error += 1;
                }
                if (Regex.IsMatch(tb_lname.Text, "^[a-zA-Z]+$") != true)
                {
                    lb_lname.Text = "Invalid Last name!";
                    lb_lname.ForeColor = Color.Red;
                    error += 1;
                }
                // validate cc
                if (tb_cc.Text == "")
                {
                    error += 1;
                    lb_cc.Text = "Is Empty!";
                    lb_cc.ForeColor = Color.Red;
                }
                else if (Regex.IsMatch(tb_cc.Text, "^[0-9]+$") != true)
                {
                    lb_cc.Text = "Invalid Credit Card. Numbers only";
                    lb_cc.ForeColor = Color.Red;
                    error += 1;
                }
                // validate email
                if (tb_email.Text == "")
                {
                    lb_email.Text = "Is Empty!";
                    lb_email.ForeColor = Color.Red;
                    error += 1;
                }
                else if (Regex.IsMatch(tb_email.Text, "^[a-zA-Z0-9]+@[a-zA-Z]+\\.[a-zA-Z]+$") != true)
                {
                    lb_email.Text = "Invalid Email Address!";
                    lb_email.ForeColor = Color.Red;
                    error += 1;
                }
                else
                {
                    // check database (email must be unique)
                    int result = CheckEmail(tb_email.Text.Trim());
                    if (result != 1)
                    {
                        error += 1;
                        lb_email.Text += "Email used! Try another.";
                        lb_email.ForeColor = Color.Red;
                    }
                }
                // validate password
                var passmsg = "Must have ";
                if (tb_pwd.Text == "")
                {
                    lb_pwd.Text = "Is Empty!";
                    lb_pwd.ForeColor = Color.Red;
                    error += 1;
                }
                if (tb_pwd.Text.Length < 8)
                {
                    passmsg += "8 Characters ";
                    error += 1;
                }
                if (Regex.IsMatch(tb_pwd.Text, "[a-z]") != true)
                {
                    passmsg += "1 lowercase ";
                    error += 1;
                }
                if (Regex.IsMatch(tb_pwd.Text, "[A-Z]") != true)
                {
                    passmsg += "1 uppercase ";
                    error += 1;
                }
                if (Regex.IsMatch(tb_pwd.Text, "[0-9]") != true)
                {
                    passmsg += "Missing 1 number ";
                    error += 1;
                }
                if (Regex.IsMatch(tb_pwd.Text, "[^a-zA-Z0-9]") != true)
                {
                    passmsg += "1 symbol ";
                    error += 1;
                }
                if (passmsg != "Must have ")
                {
                    lb_pwd.Text = passmsg;
                    lb_pwd.ForeColor = Color.Red;
                }
                // validate confirm password
                if (tb_confirmpassword.Text == "")
                {
                    lb_confirmpassword.Text = "Is Empty!";
                    lb_confirmpassword.ForeColor = Color.Red;
                    error += 1;
                }
                if (tb_confirmpassword.Text != tb_pwd.Text)
                {
                    lb_confirmpassword.Text = "Password do not match!";
                    lb_confirmpassword.ForeColor = Color.Red;
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

        // Add User to database
        public void AddUser()
        {
            // add object to database
            string UserconDB = System.Configuration.
                ConfigurationManager.ConnectionStrings["190672BAppSecurity"]
                .ConnectionString;
            SqlConnection con = new SqlConnection(UserconDB);
            string sql ="INSERT INTO Userzd VALUES(@Id,@Firstname,@Lastname,@Creditcard,@Email,@PasswordHash,@PasswordSalt,@Dob,@IV,@Key,@LoggedOut)";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@Id", Guid.NewGuid().ToString());
            cmd.Parameters.AddWithValue("@Firstname", tb_fname.Text);
            cmd.Parameters.AddWithValue("@Lastname", tb_lname.Text);
            cmd.Parameters.AddWithValue("@PasswordSalt", salt);
            cmd.Parameters.AddWithValue("@PasswordHash", finalHash);
            cmd.Parameters.AddWithValue("@Creditcard", Convert.ToBase64String(encryptData(tb_cc.Text)));
            cmd.Parameters.AddWithValue("@Email", tb_email.Text);
            cmd.Parameters.AddWithValue("@Dob", tb_dob.Text);
            cmd.Parameters.AddWithValue("@IV", Convert.ToBase64String(IV));
            cmd.Parameters.AddWithValue("@Key", Convert.ToBase64String(Key));
            cmd.Parameters.AddWithValue("@LoggedOut", 0);
            try
            {
                con.Open();
                int result = cmd.ExecuteNonQuery();
                if (result == 1)
                {
                    lb_finalmsg.Text = "Completed";
                    Response.Redirect("~/", false);
                }
                else
                {
                    lb_finalmsg.Text = "Failed AddUser";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                con.Close();

            }
        }
        protected void addUserClick(object sender, EventArgs e)
        {
            bool validate = ValidateInput();
            if (validate == true)
            {
                string pwd = tb_pwd.Text.ToString().Trim(); ;
                //Generate random "salt" 
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider(); 
                byte[] saltByte = new byte[8];
                //Fills array of bytes with random values. 
                rng.GetBytes(saltByte); 
                salt = Convert.ToBase64String(saltByte);
                SHA512Managed hashing = new SHA512Managed();
                string pwdWithSalt = pwd + salt; 
                byte[] plainHash = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwd)); 
                byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
                finalHash = Convert.ToBase64String(hashWithSalt);
                RijndaelManaged cipher = new RijndaelManaged(); 
                cipher.GenerateKey(); 
                Key = cipher.Key; 
                IV = cipher.IV;
                
                AddUser();
                
            }

        }

        protected byte[] encryptData(string data)
        {
            byte[] cipherText = null; 
            try
            {
                RijndaelManaged cipher = new RijndaelManaged(); 
                cipher.IV = IV; 
                cipher.Key = Key; 
                ICryptoTransform encryptTransform = cipher.CreateEncryptor(); 
                //ICryptoTransform decryptTransform = cipher.CreateDecryptor(); 
                byte[] plainText = Encoding.UTF8.GetBytes(data); 
                cipherText = encryptTransform.TransformFinalBlock(plainText, 0, plainText.Length);
            }
            catch (Exception ex) 
            { 
                throw new Exception(ex.ToString()); 
            }
            finally { }
            return cipherText;
        }








    }
}