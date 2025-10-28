using EKH_inventory.model;
using System;
using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using EKH_inventory.data;

namespace EKH_inventory
{
    public partial class Stock : Window
    {
        private Contextt context = new Contextt();
        private Product selectedProduct;
        private Supplier selectedSupplier;

        public Stock()
        {
            InitializeComponent();
            LoadAllData();
        }

        private void LoadAllData()
        {
            try
            {
                // Load products
                var products = context.Product.Include(p => p.Supplier).ToList();
                productsComboBox.ItemsSource = products;
                productsDataGrid.ItemsSource = products;

                // Load suppliers for combobox
                var suppliers = context.Supplier.ToList();
                productSupplierCmb.ItemsSource = suppliers;
                suppliersDataGrid.ItemsSource = suppliers;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}");
            }
        }

        // Existing Ship Out Method
        private void ShipOutButton_Click(object sender, RoutedEventArgs e)
        {
            if (productsComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a product!");
                return;
            }

            if (!int.TryParse(quantityTextBox.Text, out int quantityToShip) || quantityToShip <= 0)
            {
                MessageBox.Show("Please enter a valid quantity!");
                return;
            }

            var selectedProduct = (Product)productsComboBox.SelectedItem;

            try
            {
                var productInDb = context.Product
                    .FirstOrDefault(p => p.PID == selectedProduct.PID);

                if (productInDb != null)
                {
                    if (quantityToShip > productInDb.Pquantity)
                    {
                        MessageBox.Show($"Not enough quantity! Available: {productInDb.Pquantity}");
                        return;
                    }

                    productInDb.Pquantity -= quantityToShip;
                    context.SaveChanges();

                    MessageBox.Show($"Successfully shipped out {quantityToShip} of {productInDb.Pname}");
                    LoadAllData();
                    quantityTextBox.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error shipping out product: {ex.Message}");
            }
        }

        // Product Selection
        private void ProductsDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            selectedProduct = productsDataGrid.SelectedItem as Product;
            if (selectedProduct != null)
            {
                productNameTxt.Text = selectedProduct.Pname;
                productDescTxt.Text = selectedProduct.Pdescription;
                productQtyTxt.Text = selectedProduct.Pquantity.ToString();
                productPriceTxt.Text = selectedProduct.Price.ToString();
                productSupplierCmb.SelectedValue = selectedProduct.PSID;
            }
        }

        // Supplier Selection
        private void SuppliersDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            selectedSupplier = suppliersDataGrid.SelectedItem as Supplier;
            if (selectedSupplier != null)
            {
                supplierNameTxt.Text = selectedSupplier.Sname;
                supplierPhoneTxt.Text = selectedSupplier.Sphone.ToString();
                supplierEmailTxt.Text = selectedSupplier.Semail;
            }
        }

        // Product Management Methods
        private void AddProductBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateProductInputs()) return;

            try
            {
                var newProduct = new Product
                {
                    Pname = productNameTxt.Text.Trim(),
                    Pdescription = productDescTxt.Text.Trim(),
                    Pquantity = int.Parse(productQtyTxt.Text),
                    Price = int.Parse(productPriceTxt.Text),
                    PSID = (int)productSupplierCmb.SelectedValue
                };

                context.Product.Add(newProduct);
                context.SaveChanges();

                MessageBox.Show("Product added successfully!");
                ClearProductFields();
                LoadAllData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding product: {ex.Message}");
            }
        }

        private void UpdateProductBtn_Click(object sender, RoutedEventArgs e)
        {
            if (selectedProduct == null)
            {
                MessageBox.Show("Please select a product to update!");
                return;
            }

            if (!ValidateProductInputs()) return;

            try
            {
                var productInDb = context.Product.FirstOrDefault(p => p.PID == selectedProduct.PID);
                if (productInDb != null)
                {
                    productInDb.Pname = productNameTxt.Text.Trim();
                    productInDb.Pdescription = productDescTxt.Text.Trim();
                    productInDb.Pquantity = int.Parse(productQtyTxt.Text);
                    productInDb.Price = int.Parse(productPriceTxt.Text);
                    productInDb.PSID = (int)productSupplierCmb.SelectedValue;

                    context.SaveChanges();
                    MessageBox.Show("Product updated successfully!");
                    LoadAllData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating product: {ex.Message}");
            }
        }

        private void DeleteProductBtn_Click(object sender, RoutedEventArgs e)
        {
            if (selectedProduct == null)
            {
                MessageBox.Show("Please select a product to delete!");
                return;
            }

            var result = MessageBox.Show($"Are you sure you want to delete {selectedProduct.Pname}?",
                "Confirm Delete", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    var productInDb = context.Product.FirstOrDefault(p => p.PID == selectedProduct.PID);
                    if (productInDb != null)
                    {
                        context.Product.Remove(productInDb);
                        context.SaveChanges();
                        MessageBox.Show("Product deleted successfully!");
                        ClearProductFields();
                        LoadAllData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting product: {ex.Message}");
                }
            }
        }

        private void ClearProductBtn_Click(object sender, RoutedEventArgs e)
        {
            ClearProductFields();
            selectedProduct = null;
        }

        // Supplier Management Methods
        private void AddSupplierBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateSupplierInputs()) return;

            try
            {
                var newSupplier = new Supplier
                {
                    Sname = supplierNameTxt.Text.Trim(),
                    Sphone = int.Parse(supplierPhoneTxt.Text),
                    Semail = supplierEmailTxt.Text.Trim()
                };

                context.Supplier.Add(newSupplier);
                context.SaveChanges();

                MessageBox.Show("Supplier added successfully!");
                ClearSupplierFields();
                LoadAllData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding supplier: {ex.Message}");
            }
        }

        private void UpdateSupplierBtn_Click(object sender, RoutedEventArgs e)
        {
            if (selectedSupplier == null)
            {
                MessageBox.Show("Please select a supplier to update!");
                return;
            }

            if (!ValidateSupplierInputs()) return;

            try
            {
                var supplierInDb = context.Supplier.FirstOrDefault(s => s.SID == selectedSupplier.SID);
                if (supplierInDb != null)
                {
                    supplierInDb.Sname = supplierNameTxt.Text.Trim();
                    supplierInDb.Sphone = int.Parse(supplierPhoneTxt.Text);
                    supplierInDb.Semail = supplierEmailTxt.Text.Trim();

                    context.SaveChanges();
                    MessageBox.Show("Supplier updated successfully!");
                    LoadAllData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating supplier: {ex.Message}");
            }
        }

        private void DeleteSupplierBtn_Click(object sender, RoutedEventArgs e)
        {
            if (selectedSupplier == null)
            {
                MessageBox.Show("Please select a supplier to delete!");
                return;
            }

            var result = MessageBox.Show($"Are you sure you want to delete {selectedSupplier.Sname}?",
                "Confirm Delete", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    var supplierInDb = context.Supplier.FirstOrDefault(s => s.SID == selectedSupplier.SID);
                    if (supplierInDb != null)
                    {
                        context.Supplier.Remove(supplierInDb);
                        context.SaveChanges();
                        MessageBox.Show("Supplier deleted successfully!");
                        ClearSupplierFields();
                        LoadAllData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting supplier: {ex.Message}");
                }
            }
        }

        private void ClearSupplierBtn_Click(object sender, RoutedEventArgs e)
        {
            ClearSupplierFields();
            selectedSupplier = null;
        }

        // Helper Methods
        private bool ValidateProductInputs()
        {
            if (string.IsNullOrWhiteSpace(productNameTxt.Text) ||
                string.IsNullOrWhiteSpace(productDescTxt.Text) ||
                !int.TryParse(productQtyTxt.Text, out int qty) || qty < 0 ||
                !int.TryParse(productPriceTxt.Text, out int price) || price <= 0 ||
                productSupplierCmb.SelectedValue == null)
            {
                MessageBox.Show("Please fill all product fields with valid data!");
                return false;
            }
            return true;
        }

        private bool ValidateSupplierInputs()
        {
            if (string.IsNullOrWhiteSpace(supplierNameTxt.Text) ||
                !int.TryParse(supplierPhoneTxt.Text, out int phone) || phone <= 0 ||
                string.IsNullOrWhiteSpace(supplierEmailTxt.Text))
            {
                MessageBox.Show("Please fill all supplier fields with valid data!");
                return false;
            }
            return true;
        }

        private void ClearProductFields()
        {
            productNameTxt.Clear();
            productDescTxt.Clear();
            productQtyTxt.Clear();
            productPriceTxt.Clear();
            productSupplierCmb.SelectedIndex = -1;
        }

        private void ClearSupplierFields()
        {
            supplierNameTxt.Clear();
            supplierPhoneTxt.Clear();
            supplierEmailTxt.Clear();
        }
    }
}