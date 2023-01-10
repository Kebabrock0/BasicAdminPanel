using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
using Renci.SshNet;

namespace MysqlConnection
{
    public partial class Form1 : Form
    {
        string connStr = "server=167.71.14.131;port=3306;user=rashit;database=ecomm;password=1649";
        MySqlDataAdapter dp;
        MySqlCommand cmd;
        DataTable table, dtProduct, dtUser, dtCategory, dtSubCategory,dtContact;
        public Form1()
        
        {
            InitializeComponent();
            //this.WindowState = FormWindowState.Maximized;
        }

        public DataTable getData(string query)  
        {
            MySqlConnection conn = new MySqlConnection(connStr);
            cmd = new MySqlCommand(query, conn);
            dp = new MySqlDataAdapter(cmd);
            table = new DataTable();
            dp.Fill(table);
            return table;
        }


        public void textBoxTemizle()
        {

            Action<Control.ControlCollection> func = null;

            func = (controls) =>
            {
                foreach (Control control in controls)
                {
                    if (control is TextBox)
                    {
                        (control as TextBox).Clear();
                    }
                    else
                    {
                        func(control.Controls);
                    }
                }
            };
            func(Controls);

            func = (controls) =>
            {
                foreach (Control control in controls)
                {
                    if (control is RichTextBox)
                    {
                        (control as RichTextBox).Clear();
                    }
                    else
                    {
                        func(control.Controls);
                    }
                }
            };
            func(Controls);
        }


        private void getUsers()
        {

          
            string sqlUser = "SELECT * FROM users";
            dtUser = getData(sqlUser);
            dataGridView2.DataSource = dtUser;
 
        }

        private void getCategory()
        {
            string query = "SELECT * FROM categories";
            dtCategory = getData(query);
            categoryDataGrid.DataSource = dtCategory;

        }

        private void getContact()
        {
            string query = "SELECT name, email, mobile, comment, added_on FROM contact_us";
            dtContact = getData(query);
            contactGrid.DataSource = dtContact;

        }


        private void getSubCategory()
        {

            string query = "SELECT * FROM sub_categories";
            dtSubCategory = getData(query);
            subCategoryGrid.DataSource = dtSubCategory;

        }

        public void getProducts()
        {
            string sqlProduct = "SELECT * FROM product INNER JOIN categories ON product.categories_id = categories.id";
            dtProduct = getData(sqlProduct);
            dataGridView1.DataSource = dtProduct;

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {

                getProducts();
                getUsers();
                getCategory();
                getSubCategory();
                categoryLoad();
                getContact();
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
        }

      
        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (idTextBox.Text == "")
                {
                    MessageBox.Show("You can't leave text boxes empty", "Please no empyt space", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MySqlConnection conn = new MySqlConnection(connStr);
                    
                    string sql = "UPDATE `product` SET qty=@qty,categories_id=@category,sub_categories_id=@subcat,name=@name,mrp=@mrp,price=@price,image=@image," +
                        "short_desc=@shortdesc,description=@desc,best_seller=@bestseller,meta_title=@metatitle,meta_desc=@metadesc,meta_keyword=@metakey,added_by=@addedby,status=@status WHERE id = @id;";
                    cmd = new MySqlCommand(sql, conn);

                    cmd.Parameters.AddWithValue("@id", idTextBox.Text);
                    cmd.Parameters.AddWithValue("@qty", qtyTextBox.Text);
                    cmd.Parameters.AddWithValue("@category", categoryComboBox.SelectedValue);
                    cmd.Parameters.AddWithValue("@subcat", subCatComboBox.SelectedValue);
                    cmd.Parameters.AddWithValue("@name", nameTextBox.Text);
                    cmd.Parameters.AddWithValue("@mrp", mrpTextBox.Text);
                    cmd.Parameters.AddWithValue("@image", txtImage.Text);
                    cmd.Parameters.AddWithValue("@bestseller", cmbBestSeller.Text);
                    cmd.Parameters.AddWithValue("@price", priceTextBox.Text);
                    cmd.Parameters.AddWithValue("@addedby", cmbAddedBy.Text);
                    cmd.Parameters.AddWithValue("@status", cmbStatus.Text);
                    cmd.Parameters.AddWithValue("@metakey", metaKeywordTextBox.Text);
                    cmd.Parameters.AddWithValue("@metatitle", rtxtMetaTitle.Text);
                    cmd.Parameters.AddWithValue("@metadesc", rtxtMetaDesc.Text);
                    cmd.Parameters.AddWithValue("@shortdesc", rtxtShortDesc.Text);
                    cmd.Parameters.AddWithValue("@desc", rtxtDesc.Text);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    textBoxTemizle();
                    MessageBox.Show("Update Successfull", "Product Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }


        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            idTextBox.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            categoryComboBox.SelectedValue = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            subCatComboBox.SelectedValue = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            nameTextBox.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            mrpTextBox.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            qtyTextBox.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();
            metaKeywordTextBox.Text = dataGridView1.CurrentRow.Cells[13].Value.ToString();
            priceTextBox.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
            cmbBestSeller.Text = dataGridView1.CurrentRow.Cells[10].Value.ToString();
            rtxtMetaTitle.Text = dataGridView1.CurrentRow.Cells[11].Value.ToString();
            rtxtMetaDesc.Text = dataGridView1.CurrentRow.Cells[12].Value.ToString();
            txtImage.Text = dataGridView1.CurrentRow.Cells[7].Value.ToString();
            rtxtDesc.Text = dataGridView1.CurrentRow.Cells[9].Value.ToString();
            rtxtShortDesc.Text = dataGridView1.CurrentRow.Cells[8].Value.ToString();
            cmbAddedBy.Text = dataGridView1.CurrentRow.Cells[14].Value.ToString();
            cmbStatus.Text = dataGridView1.CurrentRow.Cells[15].Value.ToString();
            sub_categoryLoad();

        }

        private void addButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (qtyTextBox.Text == "" || categoryComboBox.Text == "" || subCatComboBox.Text == "" || nameTextBox.Text == "" || rtxtShortDesc.Text == "" 
                    || rtxtDesc.Text == "" || mrpTextBox.Text == "" || priceTextBox.Text == "" || cmbAddedBy.Text == "" || cmbBestSeller.Text == "" || cmbStatus.Text == "")
                {
                    MessageBox.Show("You can't leave text boxes empty","Please no empyt space", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MySqlConnection conn = new MySqlConnection(connStr);
                    conn.Open();
                    string sql = "insert into `product` (qty,categories_id,sub_categories_id,name,mrp,price,image,short_desc,description,best_seller,meta_title,meta_desc," +
                        "meta_keyword,added_by,status) values(@qty,@category,@subcat,@name,@mrp,@price,@image,@shortdesc,@desc,@bestseller,@metatitle,@metadesc,@metakey,@addedby,@status);";
                    cmd = new MySqlCommand(sql, conn);

                    cmd.Parameters.AddWithValue("@qty", qtyTextBox.Text);
                    cmd.Parameters.AddWithValue("@category", categoryComboBox.SelectedValue);
                    cmd.Parameters.AddWithValue("@subcat", subCatComboBox.SelectedValue);
                    cmd.Parameters.AddWithValue("@name", nameTextBox.Text);
                    cmd.Parameters.AddWithValue("@image", txtImage); 
                    cmd.Parameters.AddWithValue("@mrp", mrpTextBox.Text);
                    cmd.Parameters.AddWithValue("@bestseller", cmbBestSeller.Text);
                    cmd.Parameters.AddWithValue("@price", priceTextBox.Text);
                    cmd.Parameters.AddWithValue("@addedby", cmbAddedBy.Text);
                    cmd.Parameters.AddWithValue("@status", cmbStatus.Text);
                    cmd.Parameters.AddWithValue("@metakey", metaKeywordTextBox.Text);
                    cmd.Parameters.AddWithValue("@metatitle", rtxtMetaTitle.Text);
                    cmd.Parameters.AddWithValue("@metadesc", rtxtMetaDesc.Text);
                    cmd.Parameters.AddWithValue("@shortdesc", rtxtShortDesc.Text);
                    cmd.Parameters.AddWithValue("@desc", rtxtDesc.Text);


                    cmd.ExecuteNonQuery();
                    conn.Close();

                    textBoxTemizle();
                    MessageBox.Show("Register Successfull", "Product Added", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void categoryLoad()
        {
            string query = "SELECT `id`, `categories` FROM `categories` ";
            pCatCombo.DataSource = categoryComboBox.DataSource = getData(query);
            categoryComboBox.DisplayMember = "categories";
            categoryComboBox.ValueMember = "id";
            
            pCatCombo.DisplayMember = "categories";
            pCatCombo.ValueMember = "id";
        }
        private void sub_categoryLoad()
        {
            string query = $"SELECT `id`, `categories_id`, `sub_categories` FROM `sub_categories` WHERE categories_id = {categoryComboBox.SelectedValue}";
            subCatComboBox.DataSource = getData(query);
            subCatComboBox.DisplayMember = "sub_categories";
            subCatComboBox.ValueMember = "id";
        }

        private void categoryComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void categoryComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            sub_categoryLoad();
        }

        private void refreshBtn_Click(object sender, EventArgs e)
        {
            getProducts();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            textBoxTemizle();
        }

        string filePath;

        private void btnUploadImage_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "Image files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg;";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                filePath = openFileDialog.FileName;
                String[] filePathDizi = filePath.Split('\\');
                string filePathRaw = filePathDizi[filePathDizi.Length - 1];
                //Read the contents of the file into a stream
                var fileStream = openFileDialog.OpenFile();
                Image image = Image.FromFile(filePath);
                byte[] data = ImageToByteArray(image);
                using (SftpClient client = new SftpClient(new PasswordConnectionInfo("167.71.14.131", 22, "root", "1649MrasitO")))
                {
                    client.Connect();
                    if (client.IsConnected)
                    {
                        Random rnd = new Random();
                        int id = rnd.Next(11111111, 999999999);
                        using (Stream stream = new MemoryStream(data))
                
                        {
                            client.BufferSize = (uint)stream.Length; // bypass Payload error large files
                            client.UploadFile(stream, @"/var/www/html/Ecomm/media/product/" + id + "_" + filePathRaw);
                            txtImage.Text = id + "_" + filePathRaw;
                        }
                        client.Disconnect();
                    }
                    else
                    {
                        MessageBox.Show("I couldn't connect");
                    }
                    
                }
            }
           
        }

        private void txtArama_TextChanged(object sender, EventArgs e)
        {
            DataView dv = dtProduct.DefaultView;
            dv.RowFilter = "name LIKE '" + txtArama.Text + "%'";
            dataGridView1.DataSource = dv;
        }

        private void btnDeleteProduct_Click(object sender, EventArgs e)
        {
            try
            {
                if (idTextBox.Text == "")
                {
                    MessageBox.Show("First select a product", "There is not selected item", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    DialogResult onay = MessageBox.Show($@"Selected item will deleted permanently ?", "Item will be deleted permanently", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (onay == DialogResult.Yes)
                    {
                        MySqlConnection conn = new MySqlConnection(connStr);
                        String sql = "DELETE FROM product WHERE id=@productid";
                        cmd = new MySqlCommand(sql, conn);
                        cmd.Parameters.AddWithValue("@productid", idTextBox.Text);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        getProducts();
                        textBoxTemizle();
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtUserID.Text = dataGridView2.CurrentRow.Cells[0].Value.ToString();
            txtUserName.Text = dataGridView2.CurrentRow.Cells[1].Value.ToString();
            txtUserPassword.Text = dataGridView2.CurrentRow.Cells[2].Value.ToString();
            txtUserMail.Text = dataGridView2.CurrentRow.Cells[3].Value.ToString();
            txtUserMobile.Text = dataGridView2.CurrentRow.Cells[4].Value.ToString();
            datePicker.Value = (DateTime)dataGridView2.CurrentRow.Cells[5].Value;

        }

        private void btnUserDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtUserID.Text == "")
                {
                    MessageBox.Show("First select a user", "There is not selected item", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    DialogResult onay = MessageBox.Show($@"Selected user will deleted permanently?", "Item will be deleted permanently", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (onay == DialogResult.Yes)
                    {
                        MySqlConnection conn = new MySqlConnection(connStr);
                        String sql = "DELETE FROM users WHERE id=@userid";
                        cmd = new MySqlCommand(sql, conn);
                        cmd.Parameters.AddWithValue("@userid", txtUserID.Text);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        getUsers();
                        textBoxTemizle();
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void btnCatRefresh_Click(object sender, EventArgs e)
        {
            getCategory();
            getSubCategory();
            textBoxTemizle();
        }

        private void btnUserRefresh_Click(object sender, EventArgs e)
        {
            getUsers();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBoxTemizle();
        }

        private void btnCatAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtCatName.Text == "" || comboCatStatus.Text == "")
                {
                    MessageBox.Show("You can't leave text boxes empty", "Please dont leave empyt space", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MySqlConnection conn = new MySqlConnection(connStr);
                    conn.Open();
                    string sql = "insert into `categories` (categories,status) values(@category,@status);";
                    cmd = new MySqlCommand(sql, conn);

                    cmd.Parameters.AddWithValue("@category", txtCatName.Text);
                    cmd.Parameters.AddWithValue("@status", comboCatStatus.Text);

                    cmd.ExecuteNonQuery();
                    conn.Close();

                    textBoxTemizle();
                    MessageBox.Show("Register Successfull", "Category Added", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void btnSubCatAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (subCatName.Text == "" || pCatCombo.Text == "" || comboSubCat.Text == "")
                {
                    MessageBox.Show("You can't leave text boxes empty", "Please dont leave empyt space", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MySqlConnection conn = new MySqlConnection(connStr);
                    conn.Open();
                    string sql = "insert into `sub_categories` (sub_categories,categories_id,status) values(@scategory,@pcategory,@status);";
                    cmd = new MySqlCommand(sql, conn);

                    cmd.Parameters.AddWithValue("@scategory", subCatName.Text);
                    cmd.Parameters.AddWithValue("@status", comboSubCat.Text);
                    cmd.Parameters.AddWithValue("@pcategory", pCatCombo.SelectedValue);


                    cmd.ExecuteNonQuery();
                    conn.Close();

                    textBoxTemizle();
                    MessageBox.Show("Register Successfull", "Category Added", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void btnCatUpgrade_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtCatId.Text == "")
                {
                    MessageBox.Show("You can't leave text boxes empty", "Please no empyt space", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MySqlConnection conn = new MySqlConnection(connStr);

                    string sql = "UPDATE `categories` SET categories=@name,status=@status WHERE id = @id;";
                    cmd = new MySqlCommand(sql, conn);

                    cmd.Parameters.AddWithValue("@id", txtCatId.Text);
                    cmd.Parameters.AddWithValue("@name", txtCatName.Text);
                    cmd.Parameters.AddWithValue("@status", comboCatStatus.Text);


                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    textBoxTemizle();
                    MessageBox.Show("Update Successfull", "Category Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void btnSubCatUpgrade_Click(object sender, EventArgs e)
        {
            try
            {
                if (subCatId.Text == "")
                {
                    MessageBox.Show("You can't leave text boxes empty", "Please no empyt space", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MySqlConnection conn = new MySqlConnection(connStr);

                    string sql = "UPDATE `sub_categories` SET categories_id=@categoryid,status=@status,sub_categories=@subcat WHERE id = @id;";
                    cmd = new MySqlCommand(sql, conn);

                    cmd.Parameters.AddWithValue("@id", subCatId.Text);
                    cmd.Parameters.AddWithValue("@subcat", subCatName.Text);
                    cmd.Parameters.AddWithValue("@status", comboSubCat.Text);
                    cmd.Parameters.AddWithValue("@categoryid", pCatCombo.SelectedValue);



                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    textBoxTemizle();
                    MessageBox.Show("Update Successfull", "Category Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }

        }

        private void btnSubCatDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (subCatId.Text == "")
                {
                    MessageBox.Show("First select a product", "There is not selected item", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    DialogResult onay = MessageBox.Show($@"Selected item will deleted permanently ?", "Item will be deleted permanently", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (onay == DialogResult.Yes)
                    {
                        MySqlConnection conn = new MySqlConnection(connStr);
                        String sql = "DELETE FROM sub_categories WHERE id=@subcatid";
                        cmd = new MySqlCommand(sql, conn);
                        cmd.Parameters.AddWithValue("@subcatid", subCatId.Text);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        getProducts();
                        textBoxTemizle();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }

        }

        private void btnCatDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtCatId.Text == "")
                {
                    MessageBox.Show("First select a product", "There is not selected item", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    DialogResult onay = MessageBox.Show($@"Selected item will deleted permanently ?", "Item will be deleted permanently", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (onay == DialogResult.Yes)
                    {
                        MySqlConnection conn = new MySqlConnection(connStr);
                        String sql = "DELETE FROM categories WHERE id=@categoryid";
                        cmd = new MySqlCommand(sql, conn);
                        cmd.Parameters.AddWithValue("@categoryid", txtCatId.Text);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        getProducts();
                        textBoxTemizle();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void btnCategoryClear_Click(object sender, EventArgs e)
        {
            textBoxTemizle();
        }

        private void contactGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            contactName.Text = contactGrid.CurrentRow.Cells[0].Value.ToString();
            contactMail.Text = contactGrid.CurrentRow.Cells[1].Value.ToString();
            contactMobile.Text = contactGrid.CurrentRow.Cells[2].Value.ToString();
            contactQuery.Text = contactGrid.CurrentRow.Cells[3].Value.ToString();
            contactDate.Text = contactGrid.CurrentRow.Cells[4].Value.ToString();
            contactDate.Visible = true;
            contactMail.Visible = true;
            contactName.Visible = true;
            contactQuery.Visible = true;
            contactMobile.Visible = true;
        }

        private void userSearch_TextChanged(object sender, EventArgs e)
        {
            DataView dv = dtUser.DefaultView;
            dv.RowFilter = "name LIKE '" + userSearch.Text + "%'";
            dataGridView2.DataSource = dv;
        }

        private void tableLayoutPanel10_Paint(object sender, PaintEventArgs e)
        {

        }

        private void categoryDataGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtCatId.Text = categoryDataGrid.CurrentRow.Cells[0].Value.ToString();
            comboCatStatus.Text = categoryDataGrid.CurrentRow.Cells[2].Value.ToString();
            txtCatName.Text = categoryDataGrid.CurrentRow.Cells[1].Value.ToString();
        }

        private void subCategoryGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            subCatId.Text = subCategoryGrid.CurrentRow.Cells[0].Value.ToString();
            pCatCombo.SelectedValue = subCategoryGrid.CurrentRow.Cells[1].Value;
            subCatName.Text = subCategoryGrid.CurrentRow.Cells[2].Value.ToString();
            comboSubCat.Text = subCategoryGrid.CurrentRow.Cells[3].Value.ToString();
        }

        private void btnUserUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtUserID.Text == "")
                {
                    MessageBox.Show("You can't leave text boxes empty", "Please no empyt space", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MySqlConnection conn = new MySqlConnection(connStr);

                    string sql = "UPDATE `users` SET name=@name,email=@mail,password=@password,mobile=@mobile WHERE id = @id;";
                    cmd = new MySqlCommand(sql, conn);

                    cmd.Parameters.AddWithValue("@id", txtUserID.Text);
                    cmd.Parameters.AddWithValue("@mail", txtUserMail.Text);
                    cmd.Parameters.AddWithValue("@password", txtUserPassword.Text);
                    cmd.Parameters.AddWithValue("@mobile", txtUserMobile.Text);
                    cmd.Parameters.AddWithValue("@name", txtUserName.Text);


                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    textBoxTemizle();
                    MessageBox.Show("Update Successfull", "Product Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }

        }

        private void btnUserAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtUserMobile.Text == "" || txtUserMail.Text == "" || txtUserName.Text == "" || txtUserPassword.Text == "" )
                {
                    MessageBox.Show("You can't leave text boxes empty", "Please dont leave empyt space", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MySqlConnection conn = new MySqlConnection(connStr);
                    conn.Open();
                    string sql = "insert into `users` (name,password,email,mobile,added_on) values(@name,@password,@email,@mobile,@date);";
                    cmd = new MySqlCommand(sql, conn);

                    cmd.Parameters.AddWithValue("@mobile", txtUserMobile.Text);
                    cmd.Parameters.AddWithValue("@email", txtUserMail.Text);
                    cmd.Parameters.AddWithValue("@name", txtUserName.Text);
                    cmd.Parameters.AddWithValue("@password", txtUserPassword.Text);
                    cmd.Parameters.AddWithValue("@date", datePicker.Value);
             


                    cmd.ExecuteNonQuery();
                    conn.Close();

                    textBoxTemizle();
                    MessageBox.Show("Register Successfull", "Product Added", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }
        public Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }
        public byte[] ImageToByteArray(Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, imageIn.RawFormat);
                return ms.ToArray();
            }
        }
    }
}
