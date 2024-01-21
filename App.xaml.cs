using System.Text.Json;

namespace MoneyNote
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            ItemsService.CategoryItems = LoadCategoryItems();
            LoadIconsColors();

            MainPage = new AppShell();
        }

        /**protected override void OnSleep()
        {
            // Save data when the app is about to sleep (close)
            SaveCategoryItems();
        }

        private void SaveCategoryItems()
        {
            // Serialize the dictionary to a JSON string
            var jsonString = JsonSerializer.Serialize(ItemsService.CategoryItems);

            // Save the JSON string to Preferences
            Preferences.Set("CategoryItems", jsonString);
        }**/

        private Dictionary<int, SelectedCategoryItem> LoadCategoryItems()
        {
            // Load the JSON string from Preferences
            var jsonString = Preferences.Get("CategoryItems", string.Empty);

            if (!string.IsNullOrEmpty(jsonString))
            {
                // Deserialize the JSON string back to the dictionary
                var deserializedItems = JsonSerializer.Deserialize<Dictionary<int, SelectedCategoryItem>>(jsonString);

                // Return the deserialized items if not null, or a new dictionary otherwise
                return deserializedItems ?? new Dictionary<int, SelectedCategoryItem>();
            }

            // If no data is found, return a new dictionary
            return new Dictionary<int, SelectedCategoryItem>();
        }

        // load the icons and colors
        private void LoadIconsColors()
        {
            // clear the color and icon lists before add the items into them
            ItemsService.colorList.Clear();
            ItemsService.iconList.Clear();

            // get directory path of the icons and colors
            string iconDirectoryPath = "E:\\3.Study\\Master of Software Development\\SWEN 504\\Week 6\\Assignment\\MoneyNote\\Resources\\Images\\CategoryIcons";
            string colorDirectoryPath = "E:\\3.Study\\Master of Software Development\\SWEN 504\\Week 6\\Assignment\\MoneyNote\\Resources\\Images\\CategoryColors";

            // Check if the icon directory exists
            if (Directory.Exists(iconDirectoryPath))
            {
                // Get all files with a specific extension in the directory
                string[] imageFiles = Directory.GetFiles(iconDirectoryPath, "*.jpg");

                // Iterate through each image file and add it to the iconList
                foreach (string imagePath in imageFiles)
                {
                    string fileName = System.IO.Path.GetFileNameWithoutExtension(imagePath);
                    var iconImageSource = new FileImageSource { File = System.IO.Path.Combine(iconDirectoryPath, $"{fileName}.jpg") };
                    ItemsService.iconList.Add(new IconItem { iconName = fileName, iconSource = iconImageSource });
                }
            }

            // Check if the color directory exists
            if (Directory.Exists(colorDirectoryPath))
            {
                // Get all files with a specific extension in the directory
                string[] imageFiles = Directory.GetFiles(colorDirectoryPath, "*.jpg");

                // Iterate through each image file and add it to the iconList
                foreach (string imagePath in imageFiles)
                {
                    string fileName = System.IO.Path.GetFileNameWithoutExtension(imagePath);
                    var colorImageSource = new FileImageSource { File = System.IO.Path.Combine(colorDirectoryPath, $"{fileName}.jpg") };
                    ItemsService.colorList.Add(new ColorItem { colorName = fileName, colorSource = colorImageSource });
                }
            }
        }

    }
}
