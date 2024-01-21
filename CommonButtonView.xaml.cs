using System.Text.Json;

namespace MoneyNote;

public partial class CommonButtonView : ContentView
{
	public CommonButtonView()
	{
		InitializeComponent();
	}
    private void OnSaveAndExitClicked(object sender, EventArgs e)
    {
        // Save data
        SaveCategoryItems();

        // Exit the app
        if (Microsoft.Maui.Controls.Application.Current != null)
        {
            MainThread.BeginInvokeOnMainThread(() => Microsoft.Maui.Controls.Application.Current.Quit());
        }
    }

    private void SaveCategoryItems()
    {
        // Serialize the dictionary to a JSON string
        var jsonString = JsonSerializer.Serialize(ItemsService.CategoryItems);

        // Save the JSON string to Preferences
        Preferences.Set("CategoryItems", jsonString);
    }
}