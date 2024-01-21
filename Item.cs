using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyNote
{
    //class to represent the icon item
    public class IconItem
    {
        public string? iconName { get; set; }
        public string? iconSource { get; set; }
    }

    //class to represent the color item
    public class ColorItem
    {
        public string? colorName { get; set; }
        public string? colorSource { get; set; }
    }

    // class to represent the expense item
    public class ExpenseItem
    {
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
    }

    // class to represent the selected category item
    public class SelectedCategoryItem
    {
        public string? selectedCategoryName { get; set; }
        public string? selectedIconName { get; set; }
        public string? selectedColorName { get; set; }
        public List<ExpenseItem> ExpenseItems { get; set; } = new List<ExpenseItem>();
    }

    public class ItemsService
    {
        public static List<IconItem> iconList = new List<IconItem>();
        public static List<ColorItem> colorList = new List<ColorItem>();
        public static Dictionary<int, SelectedCategoryItem> CategoryItems { get; set; } = new Dictionary<int, SelectedCategoryItem>();
    }
}
