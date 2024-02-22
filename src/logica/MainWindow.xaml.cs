using PWC.src.httpd;
using PWC.src.mysql;
using PWC.src.control;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;

namespace PWC.src.logica {
    public partial class MainWindow : Window {
        private HttpdServer httpdServer = HttpdServer.GetInstance();
        private MysqlServer mysqlServer = MysqlServer.GetInstance();

        public MainWindow() {
            InitializeComponent();
            CheckAndInstallDependencies();
            Console.SetOut(new TextBlockWriter(Consola));
            Closing += MainWindow_Closing;
        }

        private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e) {
            httpdServer.Stop();
            mysqlServer.Stop();
        }

        private void Mysql_Click(object sender, MouseButtonEventArgs e) {
            mysqlServer.OpenConsole();
        }

        private void ApacheButton_Click(object sender, RoutedEventArgs e) {
            httpdServer.Condicion();
        }

        private void NginxButton_Click(object sender, RoutedEventArgs e) {
        }

        private void MySQLButton_Click(object sender, RoutedEventArgs e) {
            mysqlServer.Condicion();
        }

        private void PostgreSQLButton_Click(object sender, RoutedEventArgs e) {
        }

        private void CheckAndInstallDependencies() {
            if(!IsVCRedistInstalled()) {
                InstallVCRedist();
            }
        }

        private bool IsVCRedistInstalled() {
            try {
                using(RegistryKey? key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\VisualStudio\14.0\VC\Runtimes\x64", false)) {
                    return key != null;
                }
            } catch {
                return false;
            }
        }

        private void InstallVCRedist() {
            try {
                string redistributableInstallerPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "VC_redist.x64.exe");
                if(File.Exists(redistributableInstallerPath)) {
                    this.Topmost = true;
                    Process installerProcess = new Process();
                    installerProcess.StartInfo.FileName = redistributableInstallerPath;
                    installerProcess.Start();
                    installerProcess.WaitForExit();
                    this.Topmost = false;
                } else {
                    Console.WriteLine("Error: No se encontró el instalador del paquete redistribuible.");
                }
            } catch(Exception ex) {
                Console.WriteLine($"Error al intentar instalar el paquete redistribuible: {ex.Message}");
            }
        }

        [STAThread]
        public static void Main() {
            Application app = new Application();
            MainWindow mainWindow = new MainWindow();
            app.Run(mainWindow);
        }
    }
}