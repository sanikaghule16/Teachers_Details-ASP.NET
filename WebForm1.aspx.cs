using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace Employee_Details
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["TeachersConnectionString"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) BindGrid();
        }
        private void BindGrid(string query = "SELECT * FROM Teachers")
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
        }


        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            ExecuteQuery("INSERT INTO Teachers VALUES(@ID, @Name, @Des, @Date, @Mob, @Gen)");
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            ExecuteQuery("UPDATE Teachers SET Name=@Name, Designation=@Des, JoiningDate=@Date, Mobile=@Mob, Gender=@Gen WHERE Teacher_ID=@ID");
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            ExecuteQuery("DELETE FROM Teachers WHERE Teacher_ID=@ID");
        }

        protected void Button4_Click(object sender, EventArgs e)
        {
            string query = "SELECT * FROM Teachers WHERE Name LIKE '%" + TextBox6.Text + "%'";
            BindGrid(query);
        }
        private void ExecuteQuery(string query)
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ID", TextBox2.Text);
                cmd.Parameters.AddWithValue("@Name", TextBox3.Text);
                cmd.Parameters.AddWithValue("@Des", txtDesig.Text);
                cmd.Parameters.AddWithValue("@Date", txtDate.Text);
                cmd.Parameters.AddWithValue("@Mob", txtMob.Text);
                cmd.Parameters.AddWithValue("@Gen", DropDownList1.SelectedValue);
                con.Open();
                cmd.ExecuteNonQuery();
                BindGrid(); // Refresh
            }
        }

        protected void GridView1_SelectedIndexChanged1(object sender, EventArgs e)
        {
            GridViewRow row = GridView1.SelectedRow;
            TextBox2.Text = row.Cells[1].Text;
            TextBox3.Text = row.Cells[2].Text;
            txtDesig.Text = row.Cells[3].Text;
            txtDate.Text = Convert.ToDateTime(row.Cells[4].Text).ToString("yyyy-MM-dd");
            txtMob.Text = row.Cells[5].Text;
            string genderValue = row.Cells[6].Text.Trim();

            if (DropDownList1.Items.FindByValue(genderValue) != null)
            {
                DropDownList1.SelectedValue = genderValue;
            }
        }
    }

}
