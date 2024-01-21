using Microsoft.Maui.Controls.Shapes;
namespace MoneyNote;

public partial class NewCategoryPage : ContentPage
{
    private int? selectedCategoryID;
    private string? selectedCategoryName;
    private string? selectedIconName;
    private string? selectedColorName;

    public NewCategoryPage(int? selectedCategoryID = null, string? selectedCategoryName = null, string? selectedIconName = null, string? selectedColorName = null)
	{
		InitializeComponent();

        // Set the values received from CategoryPage
        this.selectedCategoryID = selectedCategoryID;
        this.selectedCategoryName = selectedCategoryName;
        this.selectedIconName = selectedIconName;
        this.selectedColorName = selectedColorName;

        DisplayIconsColors();

        // Set the categoryName to the inputName label if provided
        if (!string.IsNullOrEmpty(selectedCategoryName))
        {
            inputName.Text = selectedCategoryName;
        }
    }

    // load the icons and colors from the directory and add them into the lists
    

    // display the icons and colors on UI
    private void DisplayIconsColors()
    {
        // display icons
        foreach (var iconItem in ItemsService.iconList)
        {
            Border imageBorder = new Border
            {
                StrokeThickness = 3,
                WidthRequest = 60,
                HeightRequest = 60,
                Margin = new Thickness(5, 5, 0, 0),
                HorizontalOptions = LayoutOptions.Center,
                StrokeShape = new RoundRectangle
                {
                    CornerRadius = new CornerRadius(4, 4, 4, 4)
                },
                // Create a new Image
                Content = new Image
                {
                    Source = iconItem.iconSource,
                    WidthRequest = 55,
                    HeightRequest = 55,
                    Aspect = Aspect.AspectFill
                }
            };

            // set border color of image            
            if (!string.IsNullOrEmpty(iconItem.iconName) && iconItem.iconName.Equals(selectedIconName) && !string.IsNullOrEmpty(selectedColorName) && !string.IsNullOrEmpty(selectedIconName))
            {
                imageBorder.Stroke = Color.FromArgb(selectedColorName);
            }
            else
            {
                imageBorder.Stroke = Color.FromArgb("#969696");
            }

            // Attach the OnIconSelected event handler
            imageBorder.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => OnIconSelected(imageBorder)),
            });

            // Add the Image control to the FlexLayout
            iconFlexLayout.Children.Add(imageBorder);
        }


        // display colors
        foreach (var colorItem in ItemsService.colorList)
        {
            Border imageBorder = new Border
            {
                StrokeThickness = 3,
                WidthRequest = 90,
                HeightRequest = 40,
                Margin = new Thickness(5, 5, 0, 0),
                HorizontalOptions = LayoutOptions.Center,
                StrokeShape = new RoundRectangle
                {
                    CornerRadius = new CornerRadius(4, 4, 4, 4)
                },
                // Create a new Image
                Content = new Image
                {
                    Source = colorItem.colorSource,
                    WidthRequest = 85,
                    HeightRequest = 35,
                    Aspect = Aspect.AspectFill
                }
            };

            // set border color of image            
            if (!string.IsNullOrEmpty(colorItem.colorName) && colorItem.colorName.Equals(selectedColorName) && !string.IsNullOrEmpty(selectedColorName))
            {
                imageBorder.Stroke = Color.FromArgb("#000000");
            }
            else
            {
                imageBorder.Stroke = Color.FromArgb(colorItem.colorName);
            }

            // Attach the OnColorSelected event handler
            imageBorder.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => OnColorSelected(imageBorder)),
            });

            // Add the Image control to the FlexLayout
            colorFlexLayout.Children.Add(imageBorder);
        }
    }

    private void OnIconSelected(Border selectedImageBorder)
    {
        // Get the selected icon name
        if (selectedImageBorder.Content is Image selectedImageView && selectedImageView.Source is FileImageSource fileImageSource)
        {
            string imagePath = fileImageSource.File;
            selectedIconName = ItemsService.iconList.Find(item => item.iconSource == imagePath)?.iconName;

            // Reset border colors for all icons
            foreach (View child in iconFlexLayout.Children)
            {
                if (child is Border imageBorder)
                {
                    imageBorder.Stroke = Color.FromArgb("#969696");
                }
            }

            // set border color for selected icon
            if (!string.IsNullOrEmpty(selectedColorName))
            {
                selectedImageBorder.Stroke = Color.FromArgb(selectedColorName);
            }
            else
            {
                selectedImageBorder.Stroke = Color.FromArgb("#000000");
            }
        }
    }

    private void OnColorSelected(Border selectedImageBorder)
    {
        // Get the selected color name
        if (selectedImageBorder.Content is Image selectedImageView && selectedImageView.Source is FileImageSource fileImageSource)
        {
            string imagePath = fileImageSource.File;
            selectedColorName = ItemsService.colorList.Find(item => item.colorSource == imagePath)?.colorName;

            // Reset border colors for all icons
            foreach (View child in colorFlexLayout.Children)
            {
                if (child is Border imageBorder)
                {
                    // Get the Source of the Image within the Border
                    if (imageBorder.Content is Image image)
                    {
                        // Find the corresponding colorItem in the colorList based on the Source
                        var colorItem = ItemsService.colorList.FirstOrDefault(item => item.colorSource == (image.Source as FileImageSource)?.File);

                        // Check if the colorItem is found
                        if (colorItem != null)
                        {
                            string? colorName = colorItem.colorName;
                            imageBorder.Stroke = Color.FromArgb(colorName);
                        }
                    }
                }
            }
            // set border color for selected color
            selectedImageBorder.Stroke = Color.FromArgb("#000000");

            // if there is an icon selected
            if (!string.IsNullOrEmpty(selectedIconName))
            {
                foreach (View child in iconFlexLayout.Children)
                {
                    if (child is Border imageBorder)
                    {
                        if (imageBorder.Content is Image image)
                        {
                            // Find the corresponding iconItem in the iconList based on the Source
                            var iconItem = ItemsService.iconList.FirstOrDefault(item => item.iconSource == (image.Source as FileImageSource)?.File);

                            // Check if the colorItem is found
                            if (iconItem != null)
                            {
                                string? iconName = iconItem.iconName;
                                if (iconName != null && iconName.Equals(selectedIconName))
                                {
                                    imageBorder.Stroke = Color.FromArgb(selectedColorName);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

        private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CategoryPage());
    }

    // limit the category name length
    private void OnEntryTextChanged(object sender, TextChangedEventArgs e)
    {
        const int maxLength = 16;

        if (e.NewTextValue.Length > maxLength)
        {
            // Truncate the text to the maximum character limit
            inputName.Text = e.NewTextValue.Substring(0, maxLength);
        }
    }

    private async void OnSaveButtonClicked(object sender, EventArgs e)
    {       
        if (selectedCategoryID.HasValue && !string.IsNullOrEmpty(inputName.Text))
        {
            bool exist = false;
            foreach (var categoryItem in ItemsService.CategoryItems)
            {
                if (categoryItem.Value?.selectedCategoryName != null && categoryItem.Value.selectedCategoryName.Equals(inputName.Text) && categoryItem.Key != selectedCategoryID)
                {
                    exist = true;
                    break;
                }
            }
            if (exist)
            {
                await DisplayAlert("Error", "This category name has been used. Please choose another one.", "OK");
            }
            else
            {
                ItemsService.CategoryItems[(int)selectedCategoryID].selectedCategoryName = inputName.Text;
                ItemsService.CategoryItems[(int)selectedCategoryID].selectedIconName = selectedIconName;
                ItemsService.CategoryItems[(int)selectedCategoryID].selectedColorName = selectedColorName;
                await Navigation.PushAsync(new CategoryPage());
            }  
        }
        else
        {
            // Check if the category name is input as well as an icon and color are selected
            if (!string.IsNullOrEmpty(inputName.Text) && !string.IsNullOrEmpty(selectedIconName) && !string.IsNullOrEmpty(selectedColorName))
            {
                selectedCategoryName = inputName.Text;
                //bool isIconNameContained = ItemsService.CategoryItems.Values.Any(item => item.selectedIconName == selectedIconName);
                bool isCategoryNameContained = ItemsService.CategoryItems.Values.Any(item => item.selectedCategoryName == selectedCategoryName);

                // check if the category exists
                if (isCategoryNameContained)
                {
                    await DisplayAlert("Error", "This category name has been used! Please choose another one.", "OK");
                }
                else
                {
                    // Create a new SelectedCategoryItem
                    SelectedCategoryItem selectedCategoryItem = new SelectedCategoryItem
                    {
                        selectedCategoryName = selectedCategoryName,
                        selectedIconName = selectedIconName,
                        selectedColorName = selectedColorName
                    };

                    // assign the ID to new category item
                    int categoryID = 0;
                    if (ItemsService.CategoryItems.Count==0) {
                        categoryID = 1;
                    }
                    else
                    {
                        categoryID = ItemsService.CategoryItems.Keys.Max()+1;
                    }
                    // Add the selected item to the dictionary
                    ItemsService.CategoryItems.Add(categoryID, selectedCategoryItem);

                    await Navigation.PushAsync(new CategoryPage()); 
                }
            }
            else
            {
                // Display an error message
                await DisplayAlert("Error", "Please fill in all fields!", "OK");
            }
        }
    }
}