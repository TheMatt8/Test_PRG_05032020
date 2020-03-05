using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
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
using _18022020.Model;
using Microsoft.Win32;

namespace _18022020
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Bit bitmapModel = new Bit();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.DefaultExt = ".jpg";
            dlg.Filter = "Obrázek (.jpg)|*.jpg|Všechny soubory|*.*";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                string filename = dlg.FileName;
                bitmapModel.src = new BitmapImage();
                bitmapModel.src.BeginInit();
                bitmapModel.src.UriSource = new Uri(filename, UriKind.Relative);
                bitmapModel.src.CacheOption = BitmapCacheOption.OnLoad;
                bitmapModel.src.EndInit();

                imgCanvas.Source = bitmapModel.src;
            }
        }

        private void RotateButton_Click(object sender, RoutedEventArgs e)
        {
            bitmapModel.width = bitmapModel.src.PixelWidth;
            bitmapModel.height = bitmapModel.src.PixelHeight;

            bitmapModel.BitmapImageToArray2D();

            for (int x = 0; x < bitmapModel.height; x++)
            {
                for (int y = 0; y < bitmapModel.width; y++)
                {
                    bitmapModel.array2D[x, y] = bitmapModel.array2D[x, y] & 0xFF0000;
                }
            }

            imgCanvas.Source = bitmapModel.Array2DToBitmapImage();
        }

        private void GreenButton_Click(object sender, RoutedEventArgs e)
        {
            bitmapModel.width = bitmapModel.src.PixelWidth;
            bitmapModel.height = bitmapModel.src.PixelHeight;

            bitmapModel.BitmapImageToArray2D();

            Stopwatch stopky = new Stopwatch();
            stopky.Start();
            for (int x = 0; x < bitmapModel.height; x++)
            {
                for (int y = 0; y < bitmapModel.width; y++)
                {
                    bitmapModel.array2D[x, y] = bitmapModel.array2D[x, y] & 0x00FF00;
                }
            }

            stopky.Stop();
            Console.WriteLine(stopky.ElapsedMilliseconds);

            imgCanvas.Source = bitmapModel.Array2DToBitmapImage();
        }

        private void GreenMultiButton_Click(object sender, RoutedEventArgs e)
        {
            bitmapModel.width = bitmapModel.src.PixelWidth;
            bitmapModel.height = bitmapModel.src.PixelHeight;

            bitmapModel.BitmapImageToArray2D();

            Stopwatch stopky = new Stopwatch();
            stopky.Start();

            Thread t1 = new Thread(() =>
            {
               for (int x = 0; x < bitmapModel.height; x++)
               {
                   for (int y = 0; y < bitmapModel.width / 2; y++)
                   {
                       bitmapModel.array2D[x, y] = bitmapModel.array2D[x, y] & 0x00FF00;
                   }
               }
            });

            Thread t2 = new Thread(() =>
            {
                for (int x = 0; x < bitmapModel.height; x++)
                {
                    for (int y = bitmapModel.width / 2; y < bitmapModel.width ; y++)
                    {
                        bitmapModel.array2D[x, y] = bitmapModel.array2D[x, y] & 0x00FF00;
                    }
                }
            });

            t1.Start();
            t2.Start();

            t1.Join();
            t2.Join();

            stopky.Stop();
            Console.WriteLine(stopky.ElapsedMilliseconds);

            imgCanvas.Source = bitmapModel.Array2DToBitmapImage();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (bitmapModel.src != null) imgCanvas.Source = bitmapModel.src;
        }

        private void GrayButton_Click(object sender, RoutedEventArgs e)
        {
            bitmapModel.width = bitmapModel.src.PixelWidth;
            bitmapModel.height = bitmapModel.src.PixelHeight;
            int blue = 0;
            int red = 0;
            int green = 0;
            int pixels = 0;

            bitmapModel.BitmapImageToArray2D();

            for (int x = 0; x < bitmapModel.height; x++)
            {
                for (int y = 0; y < bitmapModel.width; y++)
                {
                    blue += (byte)((bitmapModel.array2D[x, y] & 0xFF));
                    red += (byte)((bitmapModel.array2D[x, y] & 0xFF0000));
                    green += (byte)((bitmapModel.array2D[x, y] & 0xFF00));
                    pixels = x * y;
                }
            }

            blue = blue / pixels;
            red = red / pixels;
            green = green / pixels;

            byte[] data = { (byte)red, (byte)green, (byte)blue };

            string hex = BitConverter.ToString(data);
            hex = BitConverter.ToString(data).Replace("-", string.Empty);

            for (int x = 0; x < bitmapModel.height; x++)
            {
                for (int y = 0; y < bitmapModel.width; y++)
                {
                    bitmapModel.array2D[x, y] = bitmapModel.array2D[x, y] & Convert.ToInt32(hex, 16); //Nemám ponětí jak z hex stringu udělat hex number pro operaci s &
                }
            }

            imgCanvas.Source = bitmapModel.Array2DToBitmapImage();
        }

        private void BlueButton_Click(object sender, RoutedEventArgs e)
        {
            bitmapModel.width = bitmapModel.src.PixelWidth;
            bitmapModel.height = bitmapModel.src.PixelHeight;

            bitmapModel.BitmapImageToArray2D();

            for (int x = 0; x < bitmapModel.height; x++)
            {
                for (int y = 0; y < bitmapModel.width; y++)
                {
                    bitmapModel.array2D[x, y] = bitmapModel.array2D[x, y] & 0xFF;
                }
            }

            imgCanvas.Source = bitmapModel.Array2DToBitmapImage();
        }
    }
}
