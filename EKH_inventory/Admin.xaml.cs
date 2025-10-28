using System;
using System.Linq;
using System.Windows;
using EKH_inventory.data;
using EKH_inventory.model;
using Microsoft.EntityFrameworkCore;

namespace EKH_inventory
{
    public partial class Admin : Window
    {
        private Contextt context = new Contextt();

        public Admin()
        {
            InitializeComponent();
            LoadDataGrids();
            LoadProductsForUpdate();
        }

        private void LoadDataGrids()
        {
            try
            {
                var Allproducts = context.Product.Include(p => p.Supplier).ToList();
                data.ItemsSource = Allproducts;

                var Lowquantity = context.Product.Include(p => p.Supplier).Where(p => p.Pquantity < 10).ToList();
                data2.ItemsSource = Lowquantity;
            }
            catch (Exception eee)
            {
                MessageBox.Show(eee.Message);
            }
        }

        private void LoadProductsForUpdate()
        {
            try
            {
                var products = context.Product.ToList();
                updateComboBox.ItemsSource = products;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading products: {ex.Message}");
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (updateComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a product!");
                return;
            }

            if (!int.TryParse(quantityTextBox.Text, out int newQuantity) || newQuantity < 0)
            {
                MessageBox.Show("Please enter a valid quantity!");
                return;
            }

            var selectedProduct = (Product)updateComboBox.SelectedItem;

            try
            {
                var productInDb = context.Product
                    .FirstOrDefault(p => p.PID == selectedProduct.PID);

                if (productInDb != null)
                {
                    productInDb.Pquantity = newQuantity;
                    context.SaveChanges();

                    MessageBox.Show($"Quantity updated successfully for {productInDb.Pname}!");

 
                    LoadDataGrids();
                    quantityTextBox.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating quantity: {ex.Message}");
            }
        }
    }
}