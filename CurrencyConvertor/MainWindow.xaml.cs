using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;

namespace CurrencyConvertor;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private string _urlCurrency = "https://www.cbr-xml-daily.ru/daily_json.js";
    private dynamic _jsonData;
    private List<string> _collectionFROM = new List<string>();
    private List<string> _collectionTO = new List<string>();

    public MainWindow()
    {
        InitializeComponent();
        _jsonData = GetCurrencies();
        SetCurrenciesToFields();
    }

    private dynamic GetCurrencies()
    {
        var client = new HttpClient();
        var response = client.GetAsync(_urlCurrency).Result;
        response.EnsureSuccessStatusCode();
        string responseBody = response.Content.ReadAsStringAsync().Result;
        
        return JsonConvert.DeserializeObject(responseBody);
    }

    private void SetCurrenciesToFields()
    {
        List<string> collection = new List<string>();
        foreach (var item in _jsonData.Valute)
        {
            string temp = item.Name + " - " + item.Value.Name;
            collection.Add(temp);
            Console.WriteLine($"{item.Name} - {item.Value}");
        }
        collection.Add("RUB - Российский рубль");
        _collectionFROM = collection;
        _collectionTO = collection;

        SetLists();
    }

    private void fromComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (cmbFromCurrency.SelectedItem?.ToString() == cmbToCurrency.SelectedItem?.ToString())
        {
            Console.WriteLine(cmbFromCurrency.SelectedItem?.ToString());
            var tempTo = cmbToCurrency.SelectedIndex;
            var tempFrom = cmbFromCurrency.SelectedIndex;
            cmbToCurrency.SelectedItem = cmbFromCurrency.Items[tempFrom];
            cmbFromCurrency.SelectedItem = cmbFromCurrency.Items[tempTo];
        }
    }
    private void toComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (cmbToCurrency.SelectedItem?.ToString() == cmbFromCurrency.SelectedItem?.ToString())
        {
            Console.WriteLine(cmbToCurrency.SelectedItem?.ToString());
            var tempTo = cmbToCurrency.SelectedIndex;
            var tempFrom = cmbFromCurrency.SelectedIndex;
            cmbToCurrency.SelectedItem = cmbFromCurrency.Items[tempFrom];
            cmbFromCurrency.SelectedItem = cmbFromCurrency.Items[tempTo];
        }
    }

    private void Convert_Click(object sender, RoutedEventArgs e)
    {
        GetConvertedValue();
    }

    private void GetConvertedValue()
    {
        string res = "";
        double inputNumber = Double.Parse(txtCurrency.Text);
        
        double num1 = 0;
        double num2 = 0;
        foreach (var item in _jsonData.Valute)
        {
            string temp = item.Name + " - " + item.Value.Name;
            if (temp == cmbFromCurrency.SelectedItem.ToString()) num1 = Math.Round((double)item.Value.Value, 2);
            if (temp == cmbToCurrency.SelectedItem.ToString()) num2 = Math.Round((double)item.Value.Value, 2);
        }
        if (cmbFromCurrency.Items[cmbFromCurrency.SelectedIndex]?.ToString() == "RUB - Российский рубль")
        {
            res = (inputNumber / num2).ToString();
        }
        else if (cmbToCurrency.Items[cmbToCurrency.SelectedIndex]?.ToString() == "RUB - Российский рубль")
        {
            res = (inputNumber * num1).ToString();
        }
        else
        {
            res = (num1*Math.Round(inputNumber/num2, 2)).ToString();
        }

        lblCurrency.Content = res;
    }

    private void SetLists()
    {
        cmbFromCurrency.ItemsSource = _collectionFROM;
        cmbToCurrency.ItemsSource = _collectionTO;
    }
    
}