using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace WpfTreeView
{
    /// <summary>
    /// Converts a full path to a specific image
    /// </summary>
   
    [ValueConversion(typeof(string), typeof(BitmapImage))]
    public class HeaderToImageconverter : IValueConverter
    {
        public static HeaderToImageconverter Instance = new HeaderToImageconverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var path = (string)value;

            //if the path is empty, retur
            if (path == null)
                return null;

            //get the name of the file/folder
            var name = MainWindow.GetFileFolderName(path);

            //by default, expect an image
            var image = "Image/file.png";

            //If the name is blank, we presume it is a drive as we cannot have a blank file or folder name
            if (string.IsNullOrEmpty(name))
                image = "Image/drive.png";
            else if (new FileInfo(path).Attributes.HasFlag(FileAttributes.Directory))
                image = "Image/folder-closed.png";


            return new BitmapImage(new Uri($"pack://application:,,,/{image}"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
