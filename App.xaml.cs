using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace TetrisC
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Chargez et affichez le menu principal
            MainWindow mainWindow = new MainWindow();
            mainWindow.ShowMenu(); // Assurez-vous que la méthode ShowMenu est bien accessible.
            mainWindow.Show();
        }
    }
}
