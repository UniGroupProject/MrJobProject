using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace MrJobProject
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static string databaseName = "AppDataBase.db";
        static string folderPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public static string databasePath = System.IO.Path.Combine(folderPath, databaseName);
    }
}
