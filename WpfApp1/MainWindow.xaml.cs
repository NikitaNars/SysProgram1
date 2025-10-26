using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        private void StartCMD(object sender, RoutedEventArgs e)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = "/c dir",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    WorkingDirectory = Environment.CurrentDirectory,

                    StandardOutputEncoding = Encoding.GetEncoding(866),

                    StandardErrorEncoding = Encoding.GetEncoding(866)

                }
            };
            try
            {
                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                process.WaitForExit();
                if (!string.IsNullOrEmpty(error))
                {
                    Dispatcher.Invoke(() => Error.Text = "Ошибка:\n" + error);

                }
                else
                {
                    Dispatcher.Invoke(() => Output.Text = "Результат:\n" + output);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Ошибка при запуске процесса: {ex.Message}");
            }
            finally
            {

                process.Dispose();
            }

        }
    }
}