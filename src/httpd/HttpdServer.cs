using System.Diagnostics;
using System.IO;

namespace PWC.src.httpd {
    public class HttpdServer {
        private static HttpdServer? instance;
        private Process apacheProcess = new Process();

        private HttpdServer() { }

        public static HttpdServer GetInstance() {
            return instance ??= new HttpdServer();
        }

        private const string apacheVersion = "httpd-2.4.58-win64-VS17";
        private bool condicion = true;

        private string currentDirectory = "";
        private string projectDirectory = "";
        private string apacheExecutable = "";
        private string configFile = "";

        public void Init() {
            currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            projectDirectory = Directory.GetParent(currentDirectory)?.Parent?.Parent?.Parent?.FullName ?? string.Empty;
            Environment.CurrentDirectory = projectDirectory;
            apacheExecutable = Path.Combine(projectDirectory, "dependencias", apacheVersion, "Apache24", "bin", "httpd.exe");
            configFile = Path.Combine(projectDirectory, "dependencias", apacheVersion, "Apache24", "conf", "httpd.conf");

            /*apacheExecutable = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dependencias", apacheVersion, "Apache24", "bin", "httpd.exe");
            configFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dependencias", apacheVersion, "Apache24", "conf", "httpd.conf");*/
        }

        public void Condicion() {
            if (condicion) {
                Start();
                condicion = false;
            } else { 
                Stop();
                condicion= true;
            }
        }

        public void Start() {
            Init();
            Console.WriteLine("Se ha establecido el directorio de trabajo de MySQL en -> " + Environment.CurrentDirectory);
            Console.WriteLine("Iniciando ejecución en -> " + apacheExecutable);
            Console.WriteLine("Configuración encontrada en -> " + configFile);
            try {
                Thread apacheThread = new Thread(() => {
                    try {
                        ProcessStartInfo startInfo = new ProcessStartInfo(apacheExecutable) {
                            RedirectStandardOutput = true,
                            RedirectStandardError = true,
                            UseShellExecute = false,
                            CreateNoWindow = true,
                            Arguments = $"-f {configFile}"
                        };

                        apacheProcess = new Process { StartInfo = startInfo };

                        apacheProcess.Start();

                        apacheProcess.OutputDataReceived += (sender, e) => {
                            if(!string.IsNullOrEmpty(e.Data)) {
                                Console.WriteLine(e.Data);
                            }
                        };

                        apacheProcess.ErrorDataReceived += (sender, e) => {
                            if(!string.IsNullOrEmpty(e.Data)) {
                                Console.WriteLine(e.Data);
                            }
                        };

                        apacheProcess.BeginOutputReadLine();
                        apacheProcess.BeginErrorReadLine();

                        apacheProcess.WaitForExit();
                        Console.WriteLine("Apache ha generado el código de salida: " + apacheProcess.ExitCode);
                    } catch(Exception ex) {
                        Console.WriteLine("Error al intentar iniciar Apache: " + ex.Message);
                    }
                });

                apacheThread.Start();

                Console.WriteLine("El servidor Apache se ejecutará en segundo plano...");

            } catch(Exception ex) {
                Console.WriteLine("Error al intentar iniciar Apache: " + ex.Message);
            }
        }

        public void Stop() {
            try {
                if(apacheProcess != null && !apacheProcess.HasExited) {
                    apacheProcess.Kill();
                    Console.WriteLine("El servidor Apache se ha detenido.");
                } else {
                    Console.WriteLine("El servidor Apache no se está ejecutando");
                }
            } catch(Exception e) {
                Console.Error.WriteLine("No se ha podido detener el servidor Apache: " + e.Message);
            }
        }

        public void Restart() {
            Stop();
            apacheProcess.WaitForExit();
            Start();
        }
    }
}