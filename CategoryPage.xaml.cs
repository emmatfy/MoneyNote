using Microsoft.Maui;
using Microsoft.Maui.Controls;

using Microsoft.Maui.Controls.Shapes;

using System.Linq.Expressions;

namespace MoneyNote;

public partial class CategoryPage : ContentPage
{

    public CategoryPage()
	{
		InitializeComponent();
        DisplayCategories();
    }

    private void DisplayCategories()
    {
        foreach (var categoryItem in ItemsService.CategoryItems)
        {

            // Create GridLayout
            Grid grid = new Grid
            {
                Margin = new Thickness(0, 3, 15, 0),
            };
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.6, GridUnitType.Star) }); // Delete button
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }); // Category icon
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) }); // Category name
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(8, GridUnitType.Star) }); // Enter button
            grid.RowDefinitions.Add(new RowDefinition { Height = new  GridLength(38) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(5) });

            // Create Delete button
            ImageButton deleteButton = new ImageButton
            {
                Source = "delete.jpg",
                WidthRequest = 15,
                HeightRequest = 15,
                Aspect = Aspect.AspectFit,
                HorizontalOptions = LayoutOptions.Start,
                // Add click event handler for delete action
                CommandParameter = categoryItem.Key,
                Command = new Command((selectedCategoryID) => OnDeleteButtonClicked((int)selectedCategoryID))
            };

            // Create Image with Border for category icon
            Border categoryIconImageBorder = new Border()
            {
                StrokeThickness = 3,
                HeightRequest = 35,
                WidthRequest = 35,
                Stroke = Color.FromArgb(categoryItem.Value.selectedColorName),
                HorizontalOptions = LayoutOptions.Start,
                StrokeShape = new RoundRectangle
                {
                    CornerRadius = new CornerRadius(4, 4, 4, 4)
                },
                // Create a new Image
                Content = new Image
                {
                    Source = GetIconImagePath(categoryItem.Value.selectedIconName),
                    WidthRequest = 30,
                    HeightRequest = 30,
                    Aspect = Aspect.AspectFill
                }
            };

            // Create Label for category name
            Label categoryNameLabel = new Label
            {
                Text = categoryItem.Value.selectedCategoryName,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.Start,
                FontSize = 18
            };

            // Create Enter button
            ImageButton enterButton = new ImageButton
            {
                Source = "enter.jpg",
                WidthRequest = 20,
                HeightRequest = 20,
                Aspect = Aspect.AspectFit,
                HorizontalOptions = LayoutOptions.End,
                // Add click event handler for entering another page
                Command = new Command(() => OnEnterButtonClicked(categoryItem.Key, categoryItem.Value.selectedCategoryName,categoryItem.Value.selectedIconName, categoryItem.Value.selectedColorName))
            };

            BoxView line = new BoxView
            {
                Color = Colors.LightGray,
                HeightRequest = 1,
                WidthRequest = 800,
                HorizontalOptions = LayoutOptions.Start
            };

            grid.Add(deleteButton, 0, 0);
            grid.Add(categoryIconImageBorder, 1, 0);
            grid.Add(categoryNameLabel, 2, 0);
            grid.Add(enterButton, 3, 0);

            Grid.SetRow(line, 1);
            Grid.SetColumnSpan(line, 4);
            grid.Add(line);

            categoryList.Children.Add(grid);
        }
    }

    private async void OnEnterButtonClicked(int selectedCategoryID, string? selectedCategoryName, string? selectedIconName, string? selectedColorName)
    {
        await Navigation.PushAsync(new NewCategoryPage(selectedCategoryID, selectedCategoryName, selectedIconName, selectedColorName));
    }

    private ImageSource GetIconImagePath(string? iconName)
    {
        // search for an item in the iconList collection.
        // The FirstOrDefault method is used to retrieve the first element that matches the specified condition (item.iconName == iconName). 
        var iconItem = ItemsService.iconList.FirstOrDefault(item => item.iconName == iconName);

        // iconItem?.iconSource: This expression uses the null-conditional operator to safely access the iconSource property of iconItem.
        // If iconItem is null, this expression evaluates to null. If iconItem is not null, it returns the value of iconItem.iconSource.
        // ?? "default_icon.jpg": The null - coalescing operator is used here.
        // If the result of the previous expression is null, it returns the default value "default_icon.jpg".
        // Otherwise, if the result is not null, it returns the result.
        return iconItem?.iconSource ?? "edit.jpg";
    }

    private void OnDeleteButtonClicked(int selectedCategoryID)
    {
        // Remove the category from the dictionary
        ItemsService.CategoryItems.Remove(selectedCategoryID);
   
            // update the list
            categoryList.Children.Clear();
            DisplayCategories();

    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MainPage());
    }

    private async void OnEnterButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new NewCategoryPage());
    }

}