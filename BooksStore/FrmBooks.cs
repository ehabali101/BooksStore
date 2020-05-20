using BooksStore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BooksStore
{
    public partial class FrmBooks : Form
    {
        public FrmBooks()
        {
            InitializeComponent();
        }

        private void FrmBooks_Load(object sender, EventArgs e)
        {

        }

        private void BindGrid(List<string> bindingList)
        {
            //Title;Author;Year;Price;In Stock;Binding;Description

            DataGridViewTextBoxColumn Title = new DataGridViewTextBoxColumn();
            Title.HeaderText = "Title";
            Title.Name = "colTitle";
            Title.DataPropertyName = "Title";

            DataGridViewTextBoxColumn Author = new DataGridViewTextBoxColumn();
            Author.HeaderText = "Author";
            Author.Name = "colAuthor";
            Author.DataPropertyName = "Author";

            DataGridViewTextBoxColumn Year = new DataGridViewTextBoxColumn();
            Year.HeaderText = "Year";
            Year.Name = "colYear";
            Year.DataPropertyName = "Year";

            DataGridViewTextBoxColumn Price = new DataGridViewTextBoxColumn();
            Price.HeaderText = "Price";
            Price.Name = "colPrice";
            Price.DataPropertyName = "Price";

            DataGridViewCheckBoxColumn InStock = new DataGridViewCheckBoxColumn();
            InStock.HeaderText = "In Stock";
            InStock.Name = "colInStock";
            InStock.DataPropertyName = "InStock";

            DataGridViewComboBoxColumn Binding = new DataGridViewComboBoxColumn();
            Binding.HeaderText = "Binding";
            Binding.Name = "colBinding";
            Binding.DataPropertyName = "Binding";
            foreach (string b in bindingList)
                Binding.Items.Add(b);

            DataGridViewButtonColumn Description = new DataGridViewButtonColumn();
            Description.HeaderText = "Description";
            Description.Name = "colDescription";
            Description.DataPropertyName = "Description";
            Description.Text = string.Empty;
            Description.FlatStyle = FlatStyle.Popup;
            //Description.DefaultCellStyle.BackColor = Description.DefaultCellStyle.ForeColor;

            dgvBooks.Columns.Add(Title);
            dgvBooks.Columns.Add(Author);
            dgvBooks.Columns.Add(Year);
            dgvBooks.Columns.Add(Price);
            dgvBooks.Columns.Add(InStock);
            dgvBooks.Columns.Add(Binding);
            dgvBooks.Columns.Add(Description);
            dgvBooks.AutoGenerateColumns = false;
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                Title = "Browse csv Files",
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = "csv",
                Filter = "CSV files (*.csv)|*.csv",
                RestoreDirectory = true,
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtFileName.Text = openFileDialog1.FileName;

                dgvBooks.Columns.Clear();
                List<Book> books = CsvImport.GetBooks(txtFileName.Text, ";");
                List<string> bindingList = books.Select(b => b.Binding).Distinct().ToList();
                BindGrid(bindingList);
                dgvBooks.DataSource = books;

            }
        }

        private void dgvBooks_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
             if (dgvBooks.Columns[e.ColumnIndex].Name.Equals("colDescription"))
            {
                e.CellStyle.ForeColor = e.CellStyle.BackColor;
                e.CellStyle.SelectionBackColor = e.CellStyle.BackColor;
            }
            else if (dgvBooks.Columns[e.ColumnIndex].Name.Equals("colInStock"))
            {
                if (!Convert.ToBoolean( dgvBooks.Rows[e.RowIndex].Cells["colInStock"].Value))
                    dgvBooks.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Yellow;
            }      
            else if (dgvBooks.Columns[e.ColumnIndex].Name.Equals("colPrice"))
            {
                List<Book> books = (List<Book>)dgvBooks.DataSource;
                var maxPrice = books.Max(p => p.Price);
                double minPrice = books.Min(p => p.Price);
                double doubleValue = Convert.ToDouble(e.Value);

                e.CellStyle.BackColor = GetColorOf(doubleValue, minPrice, maxPrice);
            }
        }

        public Color GetColorOf(double value, double minValue, double maxValue)
        {
            if (value == 0 || minValue == maxValue) return Color.White;

            var g = (int)(240 * value / maxValue);
            var r = (int)(240 * value / minValue);

            return (value > 0
                ? Color.FromArgb(240 - g, 255 - (int)(g * ((255 - 155) / 240.0)), 240 - g)
                : Color.FromArgb(255 - (int)(r * ((255 - 230) / 240.0)), 240 - r, 240 - r));
        }

        private void dgvBooks_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                string message = Convert.ToString(senderGrid.Rows[e.RowIndex].Cells["colDescription"].Value);
                MessageBox.Show(this, message, "Description", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btn_Remove_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show(this, "Are you sure you want to delete?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                List<Book> books = (List<Book>)dgvBooks.DataSource;
                books = books.Where(b => b.InStock == true).ToList();
                dgvBooks.DataSource = books;
            }
        }
    }
}
