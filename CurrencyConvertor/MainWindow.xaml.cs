using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
using Newtonsoft.Json;

namespace CurrencyConvertor;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private string _urlCurrency = "https://www.cbr-xml-daily.ru/daily_json.js";
    private dynamic _jsonData;

    public MainWindow()
    {
        InitializeComponent();
        GetCurrencies();
    }

    private void GetCurrencies()
    {
        var client = new HttpClient();
        var response = client.GetAsync(_urlCurrency).Result;
        response.EnsureSuccessStatusCode();
        string responseBody = response.Content.ReadAsStringAsync().Result;
        _jsonData = JsonConvert.DeserializeObject(responseBody);
        SetCurrenciesToFields();
    }

    private void SetCurrenciesToFields()
    {
        List<string> collection = new List<string>();
        foreach (var item in _jsonData.Valute)
        {
            collection.Add(item.Name);
            Console.WriteLine($"{item.Name} - {item.Value}");
        }

        cmbFromCurrency.ItemsSource = collection;
        cmbToCurrency.ItemsSource = collection;
    }

    private void fromComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        //Console.WriteLine(cmbFromCurrency.SelectedValue?.ToString());
    }
    private void toComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        //Console.WriteLine(cmbToCurrency.SelectedValue?.ToString());
    }

    private void Convert_Click(object sender, RoutedEventArgs e)
    {
        GetConvertedValue();
    }

    private void GetConvertedValue()
    {
        double num1 = 0;
        double num2 = 0;
        foreach (var item in _jsonData.Valute)
        {
            if (item.Name == cmbFromCurrency.SelectedItem) num1 = (double)item.Value.Value;
            if (item.Name == cmbToCurrency.SelectedItem) num2 = (double)item.Value.Value;
        }
        lblCurrency.Content = num1*Double.Parse(txtCurrency.Text)/num2;
    }
}