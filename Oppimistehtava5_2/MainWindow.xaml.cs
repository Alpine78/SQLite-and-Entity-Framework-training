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

namespace Oppimistehtava5_2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var populateData = new ShopContext();
            if (populateData.Products.Count() == 0)
            {
                // Tietokannassa ei ole vielä yhtään tuotetta.
                // Lisätään joku tuote ja muita tietoja testausta varten.
                var tuote = new Product
                {
                    Name = "Fujifilm X-H1",
                    Price = 1899.9F
                };
                populateData.Products.Add(tuote);

                var asiakas = new Customer
                {
                    Name = "Ilkka Rytkönen",
                    Address = "Kaihorannankatu 5"
                };
                populateData.Add(asiakas);

                var tilaus = new Order
                {
                    //Name = "Tilauksen nimi",
                    // Tilauksella ei varmaan tarvitse olla nimeä.
                    // Ymmärrän tehtävänannon niin, että nimi
                    // on vaan viittaus tilaajaan tässä yhteydessä.
                    Ordered = DateTime.Now,
                    Status = OrderStatus.Orderd,
                    Customer = asiakas
                };
                populateData.Add(tilaus);

                var tilausrivi = new OrderRow
                {
                    Discount = 0,
                    Order = tilaus,
                    Product = tuote,
                };
                populateData.Add(tilausrivi);

                populateData.SaveChanges();

            }

            showOrders();

            /*
            var context = new ShopContext();
             * 
            
            // LINQ syntax
            var query =
                from c in context.Products
                where c.Name.Contains("some product")
                orderby c.Name
                select c;

            // Extension methods
            var products = context.Products
                .Where(c => c.Name.Contains("something"))
                .OrderBy(c => c.Name);
                */
        }
        private void showOrders()
        {
            this.orderView.Items.Clear();
            using (ShopContext db = new ShopContext())
            {
                foreach (var item in db.Orders)
                {
                    string rivi = $"Tilaus {item.Id} tilattu {item.Ordered}";
                    this.orderView.Items.Add(rivi);
                }
            }
        }

        private void orderView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Joku tilaus on valittu. Pitäisi näyttää tilauksen tiedot.
            var selectedOrder = this.orderView.SelectedItem as Order;
        }
    }
}