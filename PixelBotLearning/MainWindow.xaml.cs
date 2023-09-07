using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using MessageBox = System.Windows.MessageBox;



namespace PixelBotLearning
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const UInt32 MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const UInt32 MOUSEEVENTF_LEFTUP = 0x0004;

        [DllImport("user32.dll")]
        private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, uint dwExtraInf);

        [DllImport("user32.dll")]
        private static extern void SetCursorPos(int x, int y);

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnButtonSearchPixelClick(object sender, RoutedEventArgs e)
        {
            string inputHexColorCode = TextBoxHexColor.Text;
            SearchPixel(inputHexColorCode);
        }

        private bool SearchPixel(string hexCode)
        {
            //empty bitmap with size of a all screens for 1 screen use Screen.PrimaryScreen
            Bitmap bitmap = new Bitmap(SystemInformation.VirtualScreen.Width, SystemInformation.VirtualScreen.Height);

            //new graphics objects that can capture the screen
            Graphics graphics = Graphics.FromImage(bitmap as Image);
            graphics.CopyFromScreen(0, 0, 0, 0, bitmap.Size);

            //translate color code to a color object
            Color desiredPixelColor = ColorTranslator.FromHtml(hexCode);

            for (int x = 0; x < SystemInformation.VirtualScreen.Width; x++)
            {
                for (int y = 0; y < SystemInformation.VirtualScreen.Height; y++)
                {
                    //get the current color
                    Color currentPixelColor = bitmap.GetPixel(x, y);

                    if(desiredPixelColor == currentPixelColor)
                    {
                        MessageBox.Show(String.Format($"Found pixel at {x}, {y} - Set mouse coursor"));

                        DoubleClickAtPosition(x, y);
                        return true;
                    }
                }
            }
            MessageBox.Show(String.Format("No Pixel found"));
            return false;
        }

        private void Click()
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);

        }

        private void DoubleClickAtPosition(int posX, int posY)
        {
            SetCursorPos(posX, posY);
            Click();
            System.Threading.Thread.Sleep(250);
            Click();
        }

    }
}
