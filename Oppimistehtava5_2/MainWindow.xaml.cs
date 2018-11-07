using Microsoft.EntityFrameworkCore;
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
            insertDummyData();
            showOrders();
        }
        private void showOrders()
        {
            this.orderView.Items.Clear();
            using (ShopContext db = new ShopContext())
            {
                foreach (var item in db.Orders)
                {
                    this.orderView.Items.Add(item);
                }
            }
        }

        private void orderView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Joku tilaus on valittu. Pitäisi näyttää tilauksen tiedot.
            var selectedOrder = this.orderView.SelectedItem as Order;
            using (var context = new ShopContext())
            {
                // selectedOrders-muuttujasta puuttuu asiakas.
                // Kaivetaan se muulla tavalla esiin.
                var orderId = selectedOrder.Id;
                var wholeOrder = context.Orders
                    .Include(customer => customer.Customer)
                    .Include(orderRow => orderRow.OrderRows)
                    .Where(c => c.Id.Equals(orderId))
                    .ToList();

                // Ladataan asiakkaan tiedot käyttöliittymään
                customerNameTB.Text = wholeOrder[0].Customer.Name;
                customerAddressTB.Text = wholeOrder[0].Customer.Address;

                var orderRows = context.OrderRows
                    .Include(product => product.Product)
                    .Where(c => c.Order.Equals(selectedOrder))
                    .ToList();

                float summa = 0;
                float alennusSumma = 0;
                orderDetailsView.Items.Clear();

                foreach (var item in orderRows)
                {
                    summa += item.Product.Price;
                    alennusSumma += item.Product.Price * (1 - (item.Discount / 100));
                    string detailsString = $"{item.Product.Name}, hinta {item.Product.Price}€, alennus {item.Discount}%, loppuhinta " + item.Product.Price * (1 - (item.Discount / 100)) + "€";
                    orderDetailsView.Items.Add(detailsString);
                    total.Text = summa + " € yhteensä";
                    totaldiscount.Text = alennusSumma + " € yhteensä alennusten kanssa";
                }
            }
        }

        private void insertDummyData()
        {
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
                    Status = OrderStatus.Tilattu,
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


                var tuote2 = new Product
                {
                    Name = "Fujifilm X-T3",
                    Price = 1499.9F
                };
                populateData.Products.Add(tuote2);

                var tuote3 = new Product
                {
                    Name = "Fujifilm XF200mmF2",
                    Price = 5999.9F
                };
                populateData.Products.Add(tuote3);

                var asiakas2 = new Customer
                {
                    Name = "Aku Ankka",
                    Address = "Ankanpolku 1"
                };
                populateData.Add(asiakas2);

                var tilaus2 = new Order
                {
                    Ordered = DateTime.Now,
                    Status = OrderStatus.Tilattu,
                    Customer = asiakas2
                };
                populateData.Add(tilaus2);

                var tilausrivi2 = new OrderRow
                {
                    Discount = 0,
                    Order = tilaus2,
                    Product = tuote2,
                };
                populateData.Add(tilausrivi2);

                var tilausrivi3 = new OrderRow
                {
                    Discount = 0,
                    Order = tilaus2,
                    Product = tuote3,
                };
                populateData.Add(tilausrivi3);

                var tuote4 = new Product
                {
                    Name = "Fujifilm XF8.-16mmF2.8 LM WR",
                    Price = 1999.9F
                };
                populateData.Products.Add(tuote4);

                var tuote5 = new Product
                {
                    Name = "Fujifilm XF16-55mmF2.8 WR",
                    Price = 1099.9F
                };
                populateData.Products.Add(tuote5);

                var asiakas3 = new Customer
                {
                    Name = "Roope Ankka",
                    Address = "Rahasäiliönkuja 10"
                };
                populateData.Add(asiakas3);

                var tilaus3 = new Order
                {
                    Ordered = DateTime.Now,
                    Status = OrderStatus.Toimitettu,
                    Customer = asiakas3
                };
                populateData.Add(tilaus3);

                var tilausrivi4 = new OrderRow
                {
                    Discount = 10,
                    Order = tilaus3,
                    Product = tuote4,
                };
                populateData.Add(tilausrivi4);

                var tilausrivi5 = new OrderRow
                {
                    Discount = 20,
                    Order = tilaus3,
                    Product = tuote5,
                };
                populateData.Add(tilausrivi5);

                populateData.SaveChanges();
            }
        }
    }
}