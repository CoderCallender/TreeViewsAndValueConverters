using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace WpfTreeView
{
   
   

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>

        public MainWindow()
        {
            InitializeComponent();
        }


        #endregion

        #region On Loaded
        /// <summary>
        /// when the application first opens
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Get every logical drive on the machine 
           foreach ( var drive in Directory.GetLogicalDrives())
            {
                //create a new item for it
                var item = new TreeViewItem();

                //Set the header and path
                item.Header = drive;
                item.Tag = drive;

                //Add a dummy item
                item.Items.Add(null);

                //Listen out for the item being expanded
                item.Expanded += Folder_Expanded;

                //Add it to the main tree view
                FolderView.Items.Add(item);
            }
        }

        private void Folder_Expanded(object sender, RoutedEventArgs e)
        {
            var item = (TreeViewItem)sender;

            //Check if the item contains the dummy data
            if((item.Items.Count != 1) || (item.Items[0] != null))
            {
                //if it isn't, then return
                return;
            }

            //Clear dummy data 
            item.Items.Clear();

            //get the full path
            var fullPath = (string)item.Tag;

            //Create a blanks list
            var directories = new List<string>();

            //Try and get directories from the folder
            //ignoring any issues that we encounter
            try
            {
                
                var dirs = Directory.GetDirectories(fullPath);

                //try and 
                if (dirs.Length > 0)
                    directories.AddRange(dirs);
            }

            catch { }

            //For each directory...
            directories.ForEach(directoryPath =>
            {
                //Create directory item
                var subItem = new TreeViewItem()
                {
                    //set folder name
                    Header = GetFileFolderName(directoryPath),
                    //set folder path
                    Tag = directoryPath
                };

                //Add dummy item so we can expand the view
                subItem.Items.Add(null);

                //calls this function again if folder is expanded
                subItem.Expanded += Folder_Expanded;

                //add items to the parent
                item.Items.Add(subItem);
            });


        }
        #endregion

        /// <summary>
        /// Find the file or folder name from a ful path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetFileFolderName(string path)
        {
            //if there's nothing in the string, return empty
            if (string.IsNullOrEmpty(path))
                return string.Empty;

            //make all slahes backslashes
            var normalisedPath = path.Replace('/','\\');

            //find the last backslash of any path
            var lastIndex = normalisedPath.LastIndexOf('\\');

            //if we don't find a backslash, return the path itself
            if (lastIndex <= 0)
                return path;

            //return the name after the last backslash
            return path.Substring(lastIndex + 1); 
        }
    }
}
