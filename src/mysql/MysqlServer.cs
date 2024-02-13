using System.Diagnostics;
using System.IO;

namespace PWC.src.mysql {
    public class MysqlServer {
        private static MysqlServer? instance;
        private Process mysqlProcess = new Process();

        private MysqlServer() { }

        public static MysqlServer GetInstance() {
            return instance ??= new MysqlServer();
        }

        private const string mysqlVersion = "mysql-8.0.36-winx64";
        private bool condicion = true;

        private string currentDirectory = "";
        private string projectDirectory = "";
        private string mysqlBin = "";
        private string myCnfPath = "";
        private string dataDirectory = "";
        private string logFilePath = "";

        private string mysqlBinConsole = "";
        private string logFilePathConsole = "";

        public void Init() {
            currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            projectDirectory = Directory.GetParent(currentDirectory)?.Parent?.Parent?.Parent?.FullName ?? string.Empty;
            Environment.CurrentDirectory = projectDirectory;
            mysqlBin = Path.Combine(projectDirectory, "dependencias", mysqlVersion, "bin", "mysqld.exe");
            myCnfPath = Path.Combine(projectDirectory, "dependencias", mysqlVersion, "my.cnf");
            dataDirectory = Path.Combine(projectDirectory, "dependencias", mysqlVersion, "data");
            logFilePath = Path.Combine(projectDirectory, "logs", "MySQL", "mysql_error.log");
            mysqlBinConsole = Path.Combine(projectDirectory, "dependencias", "mysql-8.0.36-winx64", "bin", "mysql.exe");
            logFilePathConsole = Path.Combine(projectDirectory, "logs", "MySQL", "mysql_console_error_auto.log");

            /*mysqlBin = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dependencias", mysqlVersion, "bin", "mysqld.exe");
            myCnfPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dependencias", mysqlVersion, "my.cnf");
            dataDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dependencias", mysqlVersion, "data");
            logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", "MySQL", "mysql_error.log");
            mysqlBinConsole = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dependencias", "mysql-8.0.36-winx64", "bin", "mysql.exe");*/
            //logFilePathConsole = Path.Combine("dependencias", "mysql-8.0.36-winx64", "logs", "mysql_console_error_auto.log");
        }

        public void Condicion() {
            if(condicion) {
                Start();
                condicion = false;
            } else {
                Stop();
                condicion = true;
            }
        }

        public void Start() {
            Init();
            Console.WriteLine("Se ha establecido el directorio de trabajo de MySQL en -> " + Environment.CurrentDirectory);
            Console.WriteLine("Iniciando ejecución en -> " + mysqlBin);
            Console.WriteLine("Configuración encontrada en -> " + myCnfPath);
            Console.WriteLine("Directorio de datos establecido en -> " + dataDirectory);
            Console.WriteLine("Creando logs en -> " + logFilePath);
            try {
                Thread mysqlThread = new Thread(() => {
                    try {
                        DirectoryInfo dataDir = new DirectoryInfo(dataDirectory);

                        List<string> command = new List<string> {
                            mysqlBin,
                            $"--defaults-file={myCnfPath}",
                            $"--datadir={dataDirectory}",
                            $"--log-error={logFilePath}",
                            "--log-error-verbosity=3"
                        };

                        if(!dataDir.Exists) {
                            dataDir.Create();
                            command.Add("--initialize-insecure");
                        }

                        ProcessStartInfo startInfo = new ProcessStartInfo(command[0]) {
                            RedirectStandardOutput = true,
                            RedirectStandardError = true,
                            UseShellExecute = false,
                            CreateNoWindow = true,
                            Arguments = string.Join(" ", command.Skip(1))
                        };

                        mysqlProcess = new Process { StartInfo = startInfo };

                        mysqlProcess.Start();

                        mysqlProcess.WaitForExit();
                        Console.WriteLine("MySQL ha generado el código de salida: " + mysqlProcess.ExitCode);
                    } catch(Exception ex) {
                        Console.WriteLine("Error al intentar iniciar MySQL: " + ex.Message);
                    }
                });

                mysqlThread.Start();

                Console.WriteLine("El servidor MySQL se ejecutará en segundo plano...");
            } catch(Exception ex) {
                Console.WriteLine("Error al intentar iniciar MySQL: " + ex.Message);
            }
        }

        public void Stop() {
            try {
                if(mysqlProcess != null && !mysqlProcess.HasExited) {
                    mysqlProcess.Kill();
                    Console.WriteLine("El servidor MySQL se ha detenido.");
                } else {
                    Console.WriteLine("El servidor MySQL no se está ejecutando");
                }
            } catch(Exception e) {
                Console.Error.WriteLine("No se ha podido detener el servidor MySQL: " + e.Message);
            }
        }

        public void Restart() {
            Stop();
            mysqlProcess.WaitForExit();
            Start();
        }

        public void OpenConsole() {
            try {
                ProcessStartInfo startInfo = new ProcessStartInfo {
                    FileName = "cmd",
                    Arguments = $"/c start cmd /k \"{mysqlBinConsole}\" --user=root --password=",
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                Process process = new Process {
                    StartInfo = startInfo
                };

                process.Start();
                Console.WriteLine("Abriendo la consola de MySQL...");

                process.WaitForExit();
                Console.WriteLine("Se ha abierto el terminal");
            } catch(Exception e) {
                Console.Error.WriteLine("Error al abrir la consola de MySQL: " + e.Message);
            }
        }
    }
}