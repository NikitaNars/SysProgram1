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

namespace SysProgram1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            threadInfo.Text = $"Главный поток (UI): {Thread.CurrentThread.ManagedThreadId}";
            statusText.Text = "Приложение запущено. Нажмите кнопку для запуска задачи";
        }
        private async void StartNotepad(object sender, RoutedEventArgs e)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "notepad.exe"
                }
            };
            try
            {
                process.Start();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при запуске Notepad: {ex.Message}");

            }
            await Task.Delay(3000);
           process.Kill();

        }

        private void Update(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                int backgroundThreadId = Thread.CurrentThread.ManagedThreadId;
                try
                {
                    statusText.Text = $"Обновлено из фонового потока (ID: {backgroundThreadId})";
                }
                catch (Exception ex)
                {

                    Dispatcher.Invoke(() =>
                    {
                        errorText.Text = $"ОШИБКА: {ex.Message}\n" +
                                         $"Код ошибки: {ex.HResult}\n" +
                                         $"Источник: {ex.Source}\n" +
                                         $"Стек вызовов:\n{ex.StackTrace}";
                        statusText.Text = "Ошибка при обновлении UI";
                    });
                }
                bool canAccess = statusText.CheckAccess();
                Dispatcher.Invoke(() =>
                {
                    statusText.Text = "Обновлено из фонового потока через Dispatcher";
                    threadInfo.Text = $"Текущий поток (через Dispatcher): {Thread.CurrentThread.ManagedThreadId}";
                });

            });

        }
    }
}