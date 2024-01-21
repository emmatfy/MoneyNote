using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using System.Globalization;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace MoneyNote
{
    public partial class MainPage : ContentPage
    {
        private string? selectedCategoryName;

        public MainPage()
        {
            InitializeComponent();

            DisplayCategories();
        }

        private void DisplayCategories()
        {
            foreach (var categoryItem in ItemsService.CategoryItems)
            {
                // Create Image with Border for category icon
                Border categoryIconImageBorder = new Border()
                {
                    StrokeThickness = 3,
                    HeightRequest = 58,
                    WidthRequest = 58,
                    Stroke = Color.FromArgb(categoryItem.Value.selectedColorName),
                    HorizontalOptions = LayoutOptions.Center,
                    StrokeShape = new RoundRectangle
                    {
                        CornerRadius = new CornerRadius(4, 4, 4, 4)
                    },
                    // Create a new Image
                    Content = new Microsoft.Maui.Controls.Image
                    {
                        Source = GetIconImagePath(categoryItem.Value.selectedIconName),
                        WidthRequest = 53,
                        HeightRequest = 53,
                        Aspect = Aspect.AspectFill
                    }
                };

                // Create Label for category name
                Label categoryNameLabel = new Label
                {
                    Text = categoryItem.Value.selectedCategoryName,
                    VerticalTextAlignment = TextAlignment.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    FontSize = 18
                };

                // Create HorizontalStackLayout
                VerticalStackLayout verticalStackLayout = new VerticalStackLayout()
                {
                    Children = { categoryIconImageBorder, categoryNameLabel },
                    Margin = new Thickness(0, 5, 0, 5),
                    Spacing = 1
                };

                // Create border for HorizontalStackLayout
                Border categoryBorder = new Border()
                {
                    Content = verticalStackLayout,
                    StrokeThickness = 3,
                    HeightRequest = 100,
                    WidthRequest = 160,
                    Margin = new Thickness(5, 5, 5, 0),
                    Stroke = Color.FromArgb("#969696"),
                    HorizontalOptions = LayoutOptions.Center,
                    StrokeShape = new RoundRectangle
                    {
                        CornerRadius = new CornerRadius(4, 4, 4, 4)
                    },

                };

                // Attach the OnCategorySelected event handler
                categoryBorder.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    Command = new Command(() => OnCategorySelected(categoryBorder)),
                });

                categoryContainer.Children.Add(categoryBorder);
            }
        }

        private ImageSource GetIconImagePath(string? iconName)
        {
            var iconItem = ItemsService.iconList.FirstOrDefault(item => item.iconName == iconName);
            return iconItem?.iconSource ?? "edit.jpg";
        }


        private void OnCategorySelected(Border selectedCategoryBorder)
        {
            // Get the selected category name
            if (selectedCategoryBorder.Content is VerticalStackLayout selectedVerticalStackLayout)
            {
                var label = selectedVerticalStackLayout.Children.FirstOrDefault(child => child is Label) as Label;

                // Check if the Label is found
                if (label != null)
                {
                    selectedCategoryName = label.Text;

                    // debug
                    System.Diagnostics.Debug.WriteLine($"Tapped Category: {selectedCategoryName}");
                }

                // Reset border colors for all icons
                foreach (View child in categoryContainer.Children)
                {
                    if (child is Border categoryBorder)
                    {
                        categoryBorder.Stroke = Color.FromArgb("#969696");
                        }
                 }

                // set border color for selected category
                selectedCategoryBorder.Stroke = Color.FromArgb("#000000"); 
            }
        }

        private async void OnSubmitButtonClicked(object sender, EventArgs e)
        {
            // Check if the category name is input as well as an icon and color are selected
            if (!string.IsNullOrEmpty(amount.Text) && !string.IsNullOrEmpty(selectedCategoryName))
            {
                // debug
                System.Diagnostics.Debug.WriteLine($"Selected Category: {selectedCategoryName}");

                // Create a new SelectedCategoryItem
                ExpenseItem expenseItem = new ExpenseItem
                {
                    Date = datePicker.Date,
                    Amount= decimal.Parse(amount.Text)
                };

                var matchingEntry = ItemsService.CategoryItems.FirstOrDefault(entry => entry.Value.selectedCategoryName == selectedCategoryName);
                if (matchingEntry.Value != null)
                {
                    // Update the list associated with the name
                    matchingEntry.Value.ExpenseItems.Add(expenseItem);

                    amount.Text = "";
                    await DisplayAlert("Success", "Expense has been saved!", "OK");
                }
            }
            else
            {
                // Display an error message
                await DisplayAlert("Error", "Please fill in all fields!", "OK");
            }
        }

        private async void OnSettingButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CategoryPage());
        }


        private void OnAmountTextChanged(object sender, TextChangedEventArgs e)
        {
            var entry = (Entry)sender;

            // Remove non-numeric characters and ensure at least one digit before the dot
            string cleanText = new string(e.NewTextValue.Where(c => char.IsDigit(c) || c == '.').ToArray());

            // Split the string by the dot
            string[] parts = cleanText.Split('.');

            // Ensure at least one digit before the dot
            if (parts.Length > 1 && parts[0].Length == 0)
            {
                entry.Text = "0" + cleanText;
                return;
            }

            // Ensure two characters after the dot
            if (parts.Length > 1 && parts[1].Length > 2)
            {
                entry.Text = parts[0] + "." + parts[1].Substring(0, 2);
            }
            else
            {
                entry.Text = cleanText;
            }
        }

        private async void NavigateToInputPage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage());
        }

        private async void NavigateToReportPage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ReportPage());
        }

    }
}
