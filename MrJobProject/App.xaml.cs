using System.IO;
using System.Reflection;
using System.Windows;

namespace MrJobProject
{
    /// <summary>
    /// Logika okna dialogowego InfoOK
    /// </summary>
    /// <remarks>
    /// Laczy z baza danych
    /// </remarks>
    public partial class App : Application
    {
        private static string databaseName = "AppDataBase.db";
        private static string folderPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public static string databasePath = System.IO.Path.Combine(folderPath, databaseName);
    }
}