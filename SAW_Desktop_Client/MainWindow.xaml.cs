using System.Windows;
using SAW_Deskopt.Models;
using SAW_Deskopt.Tools;

namespace SAW_Deskopt;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        // Loaded += async (s, e) => await loadDataTask();
    }

    // private async Task loadDataTask()
    // {
    //     APIClient apiClient = new APIClient();
    //     //var z = await apiClient.GetAsync("event/1");
    //     
    // }

    //
    private async void onSearchEventClickButton(object sender, RoutedEventArgs e)
    {
        var eventId = SearchEventTextbox.Text;
        using (APIClient apiClient = new APIClient())
        {
            var result = await apiClient.GetAsync<EventDTO>($"event/{eventId}");
            var resultList = new List<EventDTO>();
            resultList.Add(result);
            EventDataGrid.ItemsSource = resultList;
        }
    }
}