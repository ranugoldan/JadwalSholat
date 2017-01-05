using RestSharp;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using PAPPK.Model;
using System.Globalization;

namespace PAPPK
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<Kota> listProvinsi { get; set; }
        public List<Kota> listKota { get; set; }
        public List<Kota> listKecamatan { get; set; }
        public List<Kota> listKelurahan { get; set; }
        public PrayerTime PrayerTime { get; set; }
        private List<Lokasi> lokasi = new List<Lokasi>();

        public MainWindow()
        {
            PrayerTime = new PrayerTime();
            DataContext = PrayerTime;
            

            Debug.WriteLine(PrayerTime.fajr);

            InitializeComponent();
            initializeProvinsi();
            dataGrid.ItemsSource = lokasi;

            button.IsEnabled = false;
            comboBoxKota.IsEnabled = false;
            comboBoxKecamatan.IsEnabled = false;
            comboBoxKelurahan.IsEnabled = false;
            subuh.IsEnabled = false;
            terbit.IsEnabled = false;
            dzuhur.IsEnabled = false;
            ashar.IsEnabled = false;
            magrib.IsEnabled = false;
            isya.IsEnabled = false;

            Binding myBinding = new Binding();
            myBinding.Source = listProvinsi;
            comboBoxProvinsi.SetBinding(ComboBox.ItemsSourceProperty, myBinding);
            comboBoxProvinsi.DisplayMemberPath = "nama";
            comboBoxProvinsi.SelectedValuePath = "id";
            initializeProvinsi();
        }
        private void initializeProvinsi()
        {
            RestClient client = new RestClient("http://api.masbie.com/lokasi/");
            RestRequest request = new RestRequest(Method.GET);
            RestResponse<List<Kota>> response = (RestResponse<List<Kota>>)client.Execute<List<Kota>>(request);
            listProvinsi = response.Data;
            Debug.WriteLine("data"+response.Data);
        }

        private void initializeKota()
        {
            RestClient client = new RestClient("http://api.masbie.com/lokasi/index.php?provinsi="+comboBoxProvinsi.SelectedValue);
            RestRequest request = new RestRequest(Method.GET);
            RestResponse<List<Kota>> response = (RestResponse<List<Kota>>)client.Execute<List<Kota>>(request);
            listKota = response.Data;
        }

        private void initializeKecamatan()
        {
            RestClient client = new RestClient("http://api.masbie.com/lokasi/index.php?provinsi=" + comboBoxProvinsi.SelectedValue+ "&&kabupatenkota=" + comboBoxKota.SelectedValue);
            RestRequest request = new RestRequest(Method.GET);
            RestResponse<List<Kota>> response = (RestResponse<List<Kota>>)client.Execute<List<Kota>>(request);
            listKecamatan = response.Data;
        }

        private void initializeKelurahan()
        {
            RestClient client = new RestClient("http://api.masbie.com/lokasi/index.php?provinsi=" + comboBoxProvinsi.SelectedValue + "&&kabupatenkota=" + comboBoxKota.SelectedValue+"&&kecamatan="+comboBoxKecamatan.SelectedValue);
            RestRequest request = new RestRequest(Method.GET);
            RestResponse<List<Kota>> response = (RestResponse<List<Kota>>)client.Execute<List<Kota>>(request);
            listKelurahan = response.Data;
        }

        private void ProvinsiSelected(object sender, SelectionChangedEventArgs e)
        {
            Debug.WriteLine("selected provinsi: "+comboBoxProvinsi.SelectedValue);
            initializeKota();
            Binding myBinding = new Binding();
            myBinding.Source = listKota;

            comboBoxKota.IsEnabled = true;
            comboBoxKota.SetBinding(ComboBox.ItemsSourceProperty, myBinding);
            comboBoxKota.DisplayMemberPath = "nama";
            comboBoxKota.SelectedValuePath = "id";
        }

        private void KotaSelected(object sender, SelectionChangedEventArgs e)
        {
            Debug.WriteLine("selected kota : " + comboBoxKota.SelectedValue);
            initializeKecamatan();
            Binding myBinding = new Binding();
            myBinding.Source = listKecamatan;

            comboBoxKecamatan.IsEnabled = true;
            comboBoxKecamatan.SetBinding(ComboBox.ItemsSourceProperty, myBinding);
            comboBoxKecamatan.DisplayMemberPath = "nama";
            comboBoxKecamatan.SelectedValuePath = "id";
        }

        private void KecamatanSelected(object sender, SelectionChangedEventArgs e)
        {
            Debug.WriteLine("selected kecamatan : " + comboBoxKecamatan.SelectedValue);
            initializeKelurahan();
            Binding myBinding = new Binding();
            myBinding.Source = listKelurahan;

            comboBoxKelurahan.IsEnabled = true;
            comboBoxKelurahan.SetBinding(ComboBox.ItemsSourceProperty, myBinding);
            comboBoxKelurahan.DisplayMemberPath = "nama";
            comboBoxKelurahan.SelectedValuePath = "id";
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            lokasi.Add(new Lokasi { id = (lokasi.Count+1), Provinsi = ((Kota)comboBoxProvinsi.SelectedItem).nama, KabupatenKota = ((Kota)comboBoxKota.SelectedItem).nama, Kecamatan = ((Kota)comboBoxKecamatan.SelectedItem).nama, Kelurahan = ((Kota)comboBoxKelurahan.SelectedItem).nama });
            dataGrid.Items.Refresh();

            getWaktuSholat();
        }
        private void getWaktuSholat()
        {
            String lokasi = ((Kota)comboBoxKelurahan.SelectedItem).nama + ", ";
            lokasi += ((Kota)comboBoxKecamatan.SelectedItem).nama + ", ";
            lokasi += ((Kota)comboBoxKota.SelectedItem).nama + ", ";
            lokasi += ((Kota)comboBoxProvinsi.SelectedItem).nama;
            RestClient client = new RestClient("https://muslimsalat.p.mashape.com/"+lokasi+".json");
            RestRequest request = new RestRequest(Method.GET);
            request.AddHeader("X-Mashape-Key", "YLeo9wsBkSmshWdvWHwh5ZTtSeEjp16gCKEjsnYDgcdkiIVANZ");
            request.AddHeader("Accept", "application/json");
            RestResponse response = (RestResponse)client.Execute(request);
            Debug.WriteLine("Data : " + response.Content);
            JObject data = JObject.Parse(response.Content);
            JObject prayer = (JObject)data.SelectToken("items[0]");
            if (prayer != null)
            {
                DateTime parser;
                Debug.WriteLine("jobject prayer" + prayer.ToString());
                parser = DateTime.ParseExact((string)prayer.SelectToken("fajr"), "h:mm tt", CultureInfo.InvariantCulture);
                PrayerTime.fajr = parser.ToString("t");
                parser = DateTime.ParseExact((string)prayer.SelectToken("shurooq"), "h:mm tt", CultureInfo.InvariantCulture);
                PrayerTime.shurooq = parser.ToString("t");
                parser = DateTime.ParseExact((string)prayer.SelectToken("dhuhr"), "h:mm tt", CultureInfo.InvariantCulture);
                PrayerTime.dhuhr = parser.ToString("t");
                parser = DateTime.ParseExact((string)prayer.SelectToken("asr"), "h:mm tt", CultureInfo.InvariantCulture);
                DateTime a = parser.AddHours(1);
                a = a.AddMinutes(10);
                PrayerTime.asr = a.ToString("t");
                parser = DateTime.ParseExact((string)prayer.SelectToken("maghrib"), "h:mm tt", CultureInfo.InvariantCulture);
                PrayerTime.maghrib = parser.ToString("t");
                parser = DateTime.ParseExact((string)prayer.SelectToken("isha"), "h:mm tt", CultureInfo.InvariantCulture);
                PrayerTime.isha = parser.ToString("t");
            }
            Debug.WriteLine("prayeritme asr " + PrayerTime.asr);

            //Debug.WriteLine("Json : " + data["items"]);
            //foreach (var d in data["items"])
            //{
            //    Debug.WriteLine("Json : " + d["date_for"]);
            //    subuh.Text = d["fajr"]+"";
            //    terbit.Text = d["shurooq"] + "";
            //    dzuhur.Text = d["dhuhr"] + "";
            //    ashar.Text = d["asr"] + "";
            //    magrib.Text = d["maghrib"] + "";
            //    isya.Text = d["isha"] + "";
            //}
        }

        private void KelurahanSelected(object sender, SelectionChangedEventArgs e)
        {
            button.IsEnabled = true;
        }
    }
}
