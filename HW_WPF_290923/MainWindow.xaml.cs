using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HW_WPF_290923
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			LoadImages();
		}

		void AddImageClick(object sender, RoutedEventArgs e)
		{
			Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
			openFileDialog.Filter = "Image Files|*.jpg;*.png;*.gif;*.bmp|All Files|*.*";

			if (openFileDialog.ShowDialog() == true) {
				string sourceFilePath = openFileDialog.FileName;

				//	У меня чего-то не получалось по человеческому сделать, переделал так
				string relativeDestinationFolderPath = @"res\images";
				string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
				string destinationFolderPath = baseDirectory + relativeDestinationFolderPath;

				try {
					// Проверяем существует ли файл.
					if (System.IO.File.Exists(sourceFilePath))
					{
						// На всякий случай создаем дерикторию если она не существует
						System.IO.Directory.CreateDirectory(destinationFolderPath);

						// Функция которая достает имя файла из пути к нему
						string fileName = System.IO.Path.GetFileName(sourceFilePath);

						// Создаем полный путь
						string destinationFilePath = System.IO.Path.Combine(destinationFolderPath, fileName);

						// Копируем файл
						System.IO.File.Copy(sourceFilePath, destinationFilePath, true);

						// Проверяем, скопировался ли файл правильно
						if (System.IO.File.Exists(destinationFilePath)) {
							AddButtonToStackPanel(fileName);

						} else {
							MessageBox.Show("Ошибка при копировании.");
						}
					} else {
						MessageBox.Show("Файла не существует.");
					}
				} catch (Exception ex) {
					MessageBox.Show(ex.Message);
				}
			}
		}

		private void AddButtonToStackPanel(string name)
		{
			Button newButton = new Button();
			newButton.Content = name;
			newButton.Height = 25;
			newButton.FontSize = 9;
			newButton.Margin = new Thickness(10, 5, 10, 5);

			Color buttonBackgroundColor = (Color)ColorConverter.ConvertFromString("#FF607178");
			Color buttonBorderBrushColor = (Color)ColorConverter.ConvertFromString("#FF4B585E");

			newButton.Background = new SolidColorBrush(buttonBackgroundColor);
			newButton.BorderBrush = new SolidColorBrush(buttonBorderBrushColor);

			newButton.Click += OpenImageClick;

			// Добавляю кнопку в стек панель
			ImagesStackPanel.Children.Add(newButton);
		}

		private void OpenImageClick(object sender, RoutedEventArgs e)
		{
			Button clickedButton = (Button)sender;
			string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
			string imageName = clickedButton.Content.ToString();
			string imagePath = baseDirectory + @"res\images\" + imageName;

			BitmapImage image = new BitmapImage(new Uri(imagePath));
			imagePlace.Source = image;
		}

		private void LoadImages()
		{
			string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

			string[] fileNames = Directory.GetFiles(baseDirectory + @"res\images");

			foreach (string fileName in fileNames) {
				string name = System.IO.Path.GetFileName(fileName);
				AddButtonToStackPanel(name);
			}
		}
	}
}
