using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using EKH_inventory.data;
using EKH_inventory.model;
using Microsoft.EntityFrameworkCore;

namespace EKH_inventory
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Contextt cont = new Contextt();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(text1.Text) && !string.IsNullOrWhiteSpace(text2.Text))
            {
                var user = text1.Text.Trim();
                var pass = text2.Text.Trim();

                using (var context = new Contextt())
                {
                    var valid = context.Users.FirstOrDefault(y => y.Username == user && y.Upassword == pass);

                    if (valid != null && user == "fayza" && pass == "koto")
                    {
                        Stock stock = new Stock();
                        stock.Show();
                        this.Close();
                        
                    }
                    else if (valid != null)
                    {
                        Admin admin = new Admin();
                        admin.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Invalid User Name OR Password");
                    }
                }
            }
            else
            {
                MessageBox.Show("Insert All Data yalaa ");
            }
        }
    }
}
