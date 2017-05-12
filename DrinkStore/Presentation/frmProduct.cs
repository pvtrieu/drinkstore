﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DrinkStore.Entities;
using DrinkStore.BUS;
using System.IO;

namespace DrinkStore.Presentation
{
    public partial class frmProduct : Form
    {
        
        public frmProduct()
        {
            InitializeComponent();
            productBindingSource.DataSource = new Product();
        }
        
        // Load database to view
        private void initLoad()
        {
            productTBBindingSource.DataSource = ProductBUS.getAll();
            categoryBindingSource.DataSource = CategoryBUS.getAll();
            brandBindingSource.DataSource = BrandBUS.getAll();
            productBindingSource.DataSource = new Product();
            dgvProduct.ClearSelection();
        }

        // Load datbase to view without change in category and brand
        private void onLoad()
        {
            productTBBindingSource.DataSource = ProductBUS.getAll();
            productBindingSource.DataSource = new Product();
            dgvProduct.ClearSelection();
        }


        private void frmProduct_Load(object sender, EventArgs e)
        {
            initLoad();
                      
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
           
            ProductBUS.insert(productBindingSource.Current as Product);
            onLoad();
        }

        private void dgvProduct_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            productBindingSource.DataSource = productTBBindingSource.Current;           
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            ProductBUS.update(productBindingSource.Current as Product);
            onLoad();

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Message", "Are you sure?", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                ProductBUS.delete(productBindingSource.Current as Product);
                onLoad();
            };
            
           
        }

        //Pass the error on datagridview combobox
        private void dgvProduct_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        
        public byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            return ms.ToArray();
        }

        public Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }

        private void btnImg_Click(object sender, EventArgs e)
        {
            Product product = productBindingSource.Current as Product;
            OpenFileDialog dialog= new OpenFileDialog();
            dialog.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                product.Avatar = imageToByteArray(Image.FromFile(dialog.FileName));
                picProImg.Image = byteArrayToImage(product.Avatar);
            }
            productBindingSource.DataSource = product;
        }

        private void btnAddCate_Click(object sender, EventArgs e)
        {
            frmCategory _frmCate = new frmCategory();
            if (_frmCate.ShowDialog() == DialogResult.Cancel)
            {
                categoryBindingSource.DataSource = CategoryBUS.getAll();
            }
        }

        

        private void btnAddBrand_Click(object sender, EventArgs e)
        {
            frmBrand _frmBrand = new frmBrand();
            if (_frmBrand.ShowDialog() == DialogResult.Cancel)
            {
                brandBindingSource.DataSource = BrandBUS.getAll();
            }
        }

      

        private void txtPBox_KeyUp(object sender, KeyEventArgs e)
        {
            int input;
            if (int.TryParse(txtPBox.Text, out input))
            {
                Category _cate = CategoryBUS.getById((int)cboCate.SelectedValue);
                int output = (input + _cate.Unit - 1) / _cate.Unit;
                txtPUnit.Text = output.ToString();
            }
        }

        private void btnSearchName_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtName.Text))
                productTBBindingSource.DataSource = ProductBUS.getAll();
            else            
                productTBBindingSource.DataSource = ProductBUS.searchByName(txtName.Text);
        }

        //Reload form if right click
        private void frmProduct_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                onLoad();
        }
    }
}
