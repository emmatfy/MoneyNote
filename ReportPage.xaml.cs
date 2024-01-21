using Microsoft.Maui.Controls.Shapes;


namespace MoneyNote;

public partial class ReportPage : ContentPage
{
    private int? selectedMonth;
    private int? selectedYear;
    private string? categoryName;
    private string? categoryIconName;
    private string? categoryColorName;
    private decimal expense;
    private decimal totalExpense;
    private Label? percentageLabel;
    private Label? expenseLabel;

    public ReportPage()
	{
		InitializeComponent();
        PeriodSelector();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Set the default date to today, month to this month and year to this year
        datePickerForDailyReport.Date = DateTime.Today;
        monthPickerForMonthlyReport.SelectedItem = DateTime.Now.Month;
        yearPickerForMonthlyReport.SelectedItem = DateTime.Now.Year;
        yearPickerForAnnualReport.SelectedItem = DateTime.Now.Year;

        // Trigger the expense calculation for today, this month and this year
        if (datePicker.IsVisible)
        { 
            OnDateSelected(this, new DateChangedEventArgs(DateTime.Today, DateTime.Today));
        }
        else if (monthPicker.IsVisible)
        {
            OnMonthPickerSelectedIndexChanged(monthPickerForMonthlyReport, EventArgs.Empty);
        }
        else if (yearPicker.IsVisible)
        {
            OnYearPickerSelectedIndexChanged(yearPickerForAnnualReport, EventArgs.Empty);
        }
    }

    private void PeriodSelector()
    {
        // set the month picker
        List<int> months = new List<int>();
        for (int month = 1; month <= 12; month++)
        {
            months.Add(month);
        }
        monthPickerForMonthlyReport.ItemsSource = months;

        // set the year picker
        List<int> years = new List<int>();
        for (int year = 1900; year <= 2100; year++)
        {
            years.Add(year);
        }
        yearPickerForMonthlyReport.ItemsSource = years;
        yearPickerForAnnualReport.ItemsSource = years;

        // Subscribe to the selected index changed event
        monthPickerForMonthlyReport.SelectedIndexChanged += OnMonthPickerSelectedIndexChanged!;
        yearPickerForMonthlyReport.SelectedIndexChanged += OnYearPickerSelectedIndexChanged!;
        yearPickerForAnnualReport.SelectedIndexChanged += OnYearPickerSelectedIndexChanged!;      
    }

    private void OnDateSelected(object sender, DateChangedEventArgs e)
    {
        expenseList.Children.Clear();

        // debug
        System.Diagnostics.Debug.WriteLine($"daily report");

            DateTime targetDate = datePickerForDailyReport.Date;

            // kvp = key-value pair
            totalExpense = 0;
            foreach (var kvp in ItemsService.CategoryItems)
            {
                SelectedCategoryItem categoryItem = kvp.Value;
                categoryName = categoryItem.selectedCategoryName;
                categoryIconName = categoryItem.selectedIconName;
                categoryColorName = categoryItem.selectedColorName;

                // Iterate through ExpenseItems in the selected category
                foreach (ExpenseItem expenseItem in categoryItem.ExpenseItems)
                {
                    // Check if the date matches the target date
                    if (expenseItem.Date == targetDate)
                    {
                        // debug
                        System.Diagnostics.Debug.WriteLine($"expense for this date exists");

                        expense = expenseItem.Amount;
                        totalExpense += expenseItem.Amount;

                        // debug
                        System.Diagnostics.Debug.WriteLine($"expense:{expense},totalExpense:{totalExpense}");

                        ListExpense();
                    }
                }
            }
        ExpensePercentage(totalExpense);
    }

    public void OnMonthSelected()
    {
        expenseList.Children.Clear();

        if (selectedMonth.HasValue && selectedMonth.HasValue)
        {
            // debug
            System.Diagnostics.Debug.WriteLine($"monthly report");

            // kvp = key-value pair
            totalExpense = 0;
            foreach (var kvp in ItemsService.CategoryItems)
            {
                expense = 0;
                SelectedCategoryItem categoryItem = kvp.Value;
                categoryName = categoryItem.selectedCategoryName;
                categoryIconName = categoryItem.selectedIconName;
                categoryColorName = categoryItem.selectedColorName;

                // Iterate through ExpenseItems in the selected category
                foreach (ExpenseItem expenseItem in categoryItem.ExpenseItems)
                {
                    int month = expenseItem.Date.Month;
                    int year = expenseItem.Date.Year;
                    // Check if the month and year matches the selected year
                    if (month == selectedMonth && year == selectedYear)
                    {
                        // debug
                        System.Diagnostics.Debug.WriteLine($"expense for this month exists");

                        expense += expenseItem.Amount;
                        totalExpense += expenseItem.Amount;

                        // debug
                        System.Diagnostics.Debug.WriteLine($"expense:{expense},totalExpense:{totalExpense}");
                    }
                }
                if (expense != 0) { ListExpense(); }
            }
            ExpensePercentage(totalExpense);
        }
    }

    public void OnYearSelected()
    {
        expenseList.Children.Clear();

        if (selectedMonth.HasValue)
        {
            // debug
            System.Diagnostics.Debug.WriteLine($"annual report");

            // kvp = key-value pair
            totalExpense = 0;
            foreach (var kvp in ItemsService.CategoryItems)
            {
                expense = 0;
                SelectedCategoryItem categoryItem = kvp.Value;
                categoryName = categoryItem.selectedCategoryName;
                categoryIconName = categoryItem.selectedIconName;
                categoryColorName = categoryItem.selectedColorName;

                // Iterate through ExpenseItems in the selected category
                foreach (ExpenseItem expenseItem in categoryItem.ExpenseItems)
                {
                    int year = expenseItem.Date.Year;
                    // Check if the year matches the selected year
                    if (year == selectedYear)
                    {
                        // debug
                        System.Diagnostics.Debug.WriteLine($"expense for this month exists");

                        expense += expenseItem.Amount;
                        totalExpense += expenseItem.Amount;

                        // debug
                        System.Diagnostics.Debug.WriteLine($"expense:{expense},totalExpense:{totalExpense}");
                    }
                }
                if(expense != 0) { ListExpense(); }
            }
            ExpensePercentage(totalExpense);
        }
    }

    private void OnMonthPickerSelectedIndexChanged(object sender, EventArgs e)
    {
        if (sender is Picker selectedPicker)
        {
            // Handle the selected month
            selectedMonth = (int)selectedPicker.SelectedItem;

            // debug
            System.Diagnostics.Debug.WriteLine($"Selected Month:{selectedMonth}");

            OnMonthSelected();
        }
    }

    private void OnYearPickerSelectedIndexChanged(object sender, EventArgs e)
    {
        if (sender is Picker selectedPicker)
        {
            // Handle the selected month
            selectedYear = (int)selectedPicker.SelectedItem;
            // debug
            System.Diagnostics.Debug.WriteLine($"Selected Year:{selectedYear}");
            if (monthPicker.IsVisible)
            {
                OnMonthSelected();
            }
            if (yearPicker.IsVisible)
            { 
                OnYearSelected(); 
            }
        }
    }

    private void ListExpense()
    {
        // Create GridLayout
        Grid grid = new Grid
        {
            Margin = new Thickness(0, 3, 15, 0),
        };
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(4, GridUnitType.Star) }); 
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(4, GridUnitType.Star) });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.5, GridUnitType.Star) });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(38) });
        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(5) });

        // Create Image with Border for category icon
        Border categoryIconImageBorder = new Border()
        {
            StrokeThickness = 3,
            HeightRequest = 35,
            WidthRequest = 35,
            Stroke = Color.FromArgb(categoryColorName),
            HorizontalOptions = LayoutOptions.Start,
            StrokeShape = new RoundRectangle
            {
                CornerRadius = new CornerRadius(4, 4, 4, 4)
            },
            // Create a new Image
            Content = new Image
            {
                Source = GetIconImagePath(categoryIconName),
                WidthRequest = 30,
                HeightRequest = 30,
                Aspect = Aspect.AspectFill
            }
        };

        // Label of category name
        Label categoryNameLabel = new Label
        {
            Text = categoryName,
            VerticalTextAlignment = TextAlignment.Center,
            HorizontalOptions = LayoutOptions.Start,
            FontSize = 18
        };

        // Label of category expense
        expenseLabel = new Label
        {
            Text = $"{expense:F2}",
            VerticalTextAlignment = TextAlignment.Center,
            HorizontalOptions = LayoutOptions.End,
            AutomationId = "expenseLabel",
            FontSize = 18
        };

        // Label of currency symbol
        Label currencySymbol = new Label
        {
            Text = "$",
            VerticalTextAlignment = TextAlignment.Center,
            HorizontalOptions = LayoutOptions.Center,
            FontSize = 18
        };


        // Label of category expense percentage
        percentageLabel = new Label
        {
            VerticalTextAlignment = TextAlignment.Center,
            HorizontalOptions = LayoutOptions.End,
            AutomationId = "percentageLabel",
            FontSize = 16
        };

        BoxView line = new BoxView
        {
            Color = Colors.LightGray,
            HeightRequest = 1,
            WidthRequest = 800,
            HorizontalOptions = LayoutOptions.Start
        };

        // Add controls to HorizontalStackLayout
        grid.Add(categoryIconImageBorder, 0, 0);
        grid.Add(categoryNameLabel, 1, 0);
        grid.Add(expenseLabel, 2, 0);
        grid.Add(currencySymbol, 3, 0);
        grid.Add(percentageLabel, 4, 0);

        Grid.SetRow(line, 1);
        Grid.SetColumnSpan(line, 5);
        grid.Add(line);

        expenseList.Children.Add(grid);
    }

    // calculate and display the percentage
    private void ExpensePercentage(decimal totalExpense)
    {
        totalAmountForReport.Text = $"{totalExpense:F2}";

        foreach (var child in expenseList.Children)
        {
            if (child is Grid grid)
            {
                // Iterate through the children of the Grid
                foreach (var gridChild in grid.Children)
                {
                    if (gridChild is Label label)
                    {
                        if (label.AutomationId == "expenseLabel")
                        {
                            expenseLabel = label;
                            expense = Decimal.Parse(expenseLabel.Text); 
                        }
                        if (label.AutomationId == "percentageLabel")
                        {
                            percentageLabel = label;
                            if (totalExpense != 0)
                            {
                                decimal expensePercentage = (expense / totalExpense) * 100;
                                percentageLabel.Text = $"{expensePercentage:F2}%";
                            }
                        }
                    }
                }
            }
        } 
    }

    private ImageSource GetIconImagePath(string? iconName)
    {
        var iconItem = ItemsService.iconList.FirstOrDefault(item => item.iconName == iconName);
        return iconItem?.iconSource ?? "edit.jpg";
    }

    private void Daily_Clicked(object sender, EventArgs e)
    {
        if (!datePicker.IsVisible)
        { 
            datePicker.IsVisible = true;
            monthPicker.IsVisible = false;
            yearPicker.IsVisible = false;
           
            daily.TextColor = Color.FromArgb("#000000");
            monthly.TextColor = Color.FromArgb("#808080");
            annual.TextColor = Color.FromArgb("#808080");

        }
        expenseList.Children.Clear();
        OnAppearing();
    }

    private void Monthly_Clicked(object sender, EventArgs e)
    {
        if (!monthPicker.IsVisible)
        {
            datePicker.IsVisible = false;
            monthPicker.IsVisible = true;
            yearPicker.IsVisible = false;

            daily.TextColor = Color.FromArgb("#808080");
            monthly.TextColor = Color.FromArgb("#000000");
            annual.TextColor = Color.FromArgb("#808080");
        }
        expenseList.Children.Clear();
        OnAppearing();
    }

     private void Annual_Clicked(object sender, EventArgs e)
    {
        if (!yearPicker.IsVisible)
        {
            datePicker.IsVisible = false;
            monthPicker.IsVisible = false;
            yearPicker.IsVisible = true;

            daily.TextColor = Color.FromArgb("#808080");
            monthly.TextColor = Color.FromArgb("#808080");
            annual.TextColor = Color.FromArgb("#000000");
        }
        expenseList.Children.Clear();
        OnAppearing();
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