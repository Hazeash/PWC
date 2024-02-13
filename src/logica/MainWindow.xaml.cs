using PWC.src.httpd;
using PWC.src.mysql;
using PWC.src.control;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PWC.src.logica {
    public partial class MainWindow : Window {
        private HttpdServer httpdServer = HttpdServer.GetInstance();
        private MysqlServer mysqlServer = MysqlServer.GetInstance();

        public MainWindow() {
            InitializeComponent();
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

        [STAThread]
        public static void Main() {
            Application app = new Application();
            MainWindow mainWindow = new MainWindow();
            app.Run(mainWindow);
        }
    }
}