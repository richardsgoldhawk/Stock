using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stock
{
    public partial class Products : Form
    {
        public Products()
        {
            InitializeComponent();
            LoadData();
        }

        private void Products_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // to do Check log in username and password
            SqlConnection con = new SqlConnection("Data Source=RICHARDS\\SQLEXPRESS;Initial Catalog=Stock;Integrated Security=True");
            //insert logic
            con.Open();
            bool status = false;
            if (comboBox1.SelectedIndex == 0) {
                status = true;
            }
            else
            {
                status = false;
            }

            var sqlQuery = "";
            if (IfProductsExists(con, textBox1.Text))
            {
                sqlQuery = @"UPDATE [Products] SET [ProductName] = '" + textBox2.Text + "',[ProductStatus] = '" + status + "'WHERE [ProductCode] ='" + textBox1.Text + "'";
            }
            else
            {
                sqlQuery = @"INSERT INTO[Stock].[dbo].[Products]([ProductCode],[ProductName],[ProductStatus])VALUES
                           ('" + textBox1.Text + "','" + textBox2.Text + "','" + status + "')";
            }
            
            SqlCommand cmd = new SqlCommand(sqlQuery, con);
            cmd.ExecuteNonQuery();
            con.Close();

            LoadData();
            
        }

    private bool IfProductsExists(SqlConnection con, string productCode)
        {
            // Reading Data
            SqlDataAdapter sda = new SqlDataAdapter("Select 1 From [Stock].[dbo].[Products] Where [ProductCode] = '" + productCode +"'", con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;

        }

    public void LoadData()
    {
        // to do Check log in username and password
        SqlConnection con = new SqlConnection("Data Source=RICHARDS\\SQLEXPRESS;Initial Catalog=Stock;Integrated Security=True");
        // Reading Data
        SqlDataAdapter sda = new SqlDataAdapter("Select * From [Stock].[dbo].[Products]", con);
        DataTable dt = new DataTable();
        sda.Fill(dt);
        dataGridView1.Rows.Clear();
        foreach (DataRow item in dt.Rows)
        {
            int n = dataGridView1.Rows.Add();
            dataGridView1.Rows[n].Cells[0].Value = item["ProductCode"].ToString();
            dataGridView1.Rows[n].Cells[1].Value = item["ProductName"].ToString();
            if ((bool)item["ProductStatus"])
            {
                dataGridView1.Rows[n].Cells[2].Value = "Active";
            }
            else
            {
                dataGridView1.Rows[n].Cells[2].Value = "Deactive";
            }
        }
    }

        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            textBox1.Text = dataGridView1.SelectedRows[0].Cells[0].ToString();
            textBox2.Text = dataGridView1.SelectedRows[0].Cells[1].ToString();
            if ((dataGridView1.SelectedRows[0].Cells[2].ToString() == "Active"))
            {
                comboBox1.SelectedIndex = 0;
            }
            else
            {
                comboBox1.SelectedIndex = 1;
            }
             
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // to do Check log in username and password
            SqlConnection con = new SqlConnection("Data Source=RICHARDS\\SQLEXPRESS;Initial Catalog=Stock;Integrated Security=True");


            var sqlQuery = "";
            if (IfProductsExists(con, textBox1.Text))
            {
                con.Open();
                sqlQuery = @"DELETE From [Products] WHERE [ProductCode] ='" + textBox1.Text + "'";
                SqlCommand cmd = new SqlCommand(sqlQuery, con);
                cmd.ExecuteNonQuery();
                con.Close();
            }
            else
            {
                MessageBox.Show("Record Not Exists ... !");
            }
            //Reading Data
            LoadData();
        }
    }
}
