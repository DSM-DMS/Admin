using System;
using System.Collections.Generic;
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
using System.Windows.Threading;

namespace DMS
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            timer.Interval = TimeSpan.FromTicks(100000);
            timer.Tick += new EventHandler(Timer_Tick);
            this.Loaded += (s, e) =>
            {
                Matrix m = PresentationSource.FromVisual(this).CompositionTarget.TransformToDevice;
                ScaleTransform dpiTransform = new ScaleTransform(1 / m.M11, 1 / m.M22);
                if (dpiTransform.CanFreeze)
                    dpiTransform.Freeze();
                this.LayoutTransform = dpiTransform;
            };
        }

        DispatcherTimer timer = new DispatcherTimer();

        int before_click_panel = 0;
        int after_click_panel = 0;
        int timer_cnt = 0;
        double timer_n = 6;

        void Timer_Tick(object sender, EventArgs e)
        {
            Button p = point_Button;
            switch (after_click_panel)
            {
                case (0):
                    p = point_Button;
                    break;
                case (1):
                    p = mpoint_Button;
                    point_canvas.Visibility = Visibility.Hidden;
                    break;
                case (2):
                    p = download_Button;
                    break;
                case (3):
                    p = notice_Button;
                    break;
                case (4):
                    p = chat_Button;
                    break;
            }

            timer_cnt++;
            int n = Math.Abs(after_click_panel - before_click_panel);

            switch (n)
            {
                case (3):
                    n = 4;
                    break;
                case (4):
                    n = 5;
                    break;
            }


            if (mark_Border.Margin.Top + 16 < p.Margin.Top)
            {
                if (timer_cnt < 6 * n)
                {
                    timer_n = 10 + n;
                }
                else if (timer_cnt < 14 * n)
                {
                    timer_n = 6;
                }

                if(p.Margin.Top - mark_Border.Margin.Top + 16 < 10)
                {
                    timer_n = 1;
                }
                mark_Border.Margin = new Thickness(0, (int)Math.Round(mark_Border.Margin.Top) + timer_n, 0, 0);
            }
            else if (mark_Border.Margin.Top + 16 > p.Margin.Top)
            {
                if (timer_cnt < 6 * n)
                {
                    timer_n = 10 + n;
                }
                else if (timer_cnt < 14 * n)
                {
                    timer_n = 6;
                }
                
                if(mark_Border.Margin.Top + 16 - p.Margin.Top < 10)
                {
                    timer_n = 1;
                }
                mark_Border.Margin = new Thickness(0, (int)Math.Round(mark_Border.Margin.Top) - timer_n, 0, 0);
            }
            else
            {
                point_Button.Opacity = 0.7;
                mpoint_Button.Opacity = 0.7;
                download_Button.Opacity = 0.7;
                notice_Button.Opacity = 0.7;
                chat_Button.Opacity = 0.7;
                Console.WriteLine("1");
                p.Opacity = 1;
                before_click_panel = after_click_panel;
                timer.Stop();
            }
        }

        private void point_click(object sender, RoutedEventArgs e)
        {
            if (mark_Border.Margin.Top != point_Button.Margin.Top && after_click_panel != 0)
            {
                before_click_panel = after_click_panel;
                timer.Stop();
                after_click_panel = 0;
                timer_cnt = 0;
                timer_n = 6;
                timer.Start();
            }
        }

        private void mpoint_click(object sender, RoutedEventArgs e)
        {
            if (mark_Border.Margin.Top != mpoint_Button.Margin.Top && after_click_panel != 1)
            {
                before_click_panel = after_click_panel;
                timer.Stop();
                after_click_panel = 1;
                timer_cnt = 0;
                timer_n = 6;
                timer.Start();
            }
        }

        private void download_click(object sender, RoutedEventArgs e)
        {
            if (mark_Border.Margin.Top != download_Button.Margin.Top && after_click_panel != 2)
            {
                before_click_panel = after_click_panel;
                timer.Stop();
                after_click_panel = 2;
                timer_cnt = 0;
                timer_n = 6;
                timer.Start();
            }
        }

        private void notice_click(object sender, RoutedEventArgs e)
        {
            if (mark_Border.Margin.Top != notice_Button.Margin.Top && after_click_panel != 3)
            {
                before_click_panel = after_click_panel;
                timer.Stop();
                after_click_panel = 3;
                timer_cnt = 0;
                timer_n = 6;
                timer.Start();
            }
        }

        private void chat_click(object sender, RoutedEventArgs e)
        {
            if (mark_Border.Margin.Top != chat_Button.Margin.Top && after_click_panel != 4)
            {
                before_click_panel = after_click_panel;
                timer.Stop();
                after_click_panel = 4;
                timer_cnt = 0;
                timer_n = 6;
                timer.Start();
            }
        }

        private void textbox1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            search_TextBox.Text = "";
        }

        private void All_Grade_Click(object sender, RoutedEventArgs e)
        {
            var bc = new BrushConverter();
            Console.WriteLine("a");

            All_Grade_Button.Background = (Brush)bc.ConvertFrom("#FFFFFFFF");
            First_Grade_Button.Background = (Brush)bc.ConvertFrom("#FF2F5FD2");
            Second_Grade_Button.Background = (Brush)bc.ConvertFrom("#FF2F5FD2");
            Third_Grade_Button.Background = (Brush)bc.ConvertFrom("#FF2F5FD2");
            All_Grade_Button.Foreground = (Brush)bc.ConvertFrom("#FF2F5FD2");
            First_Grade_Button.Foreground = (Brush)bc.ConvertFrom("#FFFFFFFF");
            Second_Grade_Button.Foreground = (Brush)bc.ConvertFrom("#FFFFFFFF");
            Third_Grade_Button.Foreground = (Brush)bc.ConvertFrom("#FFFFFFFF");
        }

        private void First_Grade_Click(object sender, RoutedEventArgs e)
        {
            var bc = new BrushConverter();
            Console.WriteLine("1");
            All_Grade_Button.Background = (Brush)bc.ConvertFrom("#FF2F5FD2");
            First_Grade_Button.Background = (Brush)bc.ConvertFrom("#FFFFFFFF");
            Second_Grade_Button.Background = (Brush)bc.ConvertFrom("#FF2F5FD2");
            Third_Grade_Button.Background = (Brush)bc.ConvertFrom("#FF2F5FD2");
            All_Grade_Button.Foreground = (Brush)bc.ConvertFrom("#FFFFFFFF");
            First_Grade_Button.Foreground = (Brush)bc.ConvertFrom("#FF2F5FD2");
            Second_Grade_Button.Foreground = (Brush)bc.ConvertFrom("#FFFFFFFF");
            Third_Grade_Button.Foreground = (Brush)bc.ConvertFrom("#FFFFFFFF");
        }

        private void Second_Grade_Click(object sender, RoutedEventArgs e)
        {
            var bc = new BrushConverter();

            All_Grade_Button.Background = (Brush)bc.ConvertFrom("#FF2F5FD2");
            First_Grade_Button.Background = (Brush)bc.ConvertFrom("#FF2F5FD2");
            Second_Grade_Button.Background = (Brush)bc.ConvertFrom("#FFFFFFFF");
            Third_Grade_Button.Background = (Brush)bc.ConvertFrom("#FF2F5FD2");
            All_Grade_Button.Foreground = (Brush)bc.ConvertFrom("#FFFFFFFF");
            First_Grade_Button.Foreground = (Brush)bc.ConvertFrom("#FFFFFFFF");
            Second_Grade_Button.Foreground = (Brush)bc.ConvertFrom("#FF2F5FD2");
            Third_Grade_Button.Foreground = (Brush)bc.ConvertFrom("#FFFFFFFF");
        }

        private void Third_Grade_Click(object sender, RoutedEventArgs e)
        {
            var bc = new BrushConverter();

            All_Grade_Button.Background = (Brush)bc.ConvertFrom("#FF2F5FD2");
            First_Grade_Button.Background = (Brush)bc.ConvertFrom("#FF2F5FD2");
            Second_Grade_Button.Background = (Brush)bc.ConvertFrom("#FF2F5FD2");
            Third_Grade_Button.Background = (Brush)bc.ConvertFrom("#FFFFFFFF");
            All_Grade_Button.Foreground = (Brush)bc.ConvertFrom("#FFFFFFFF");
            First_Grade_Button.Foreground = (Brush)bc.ConvertFrom("#FFFFFFFF");
            Second_Grade_Button.Foreground = (Brush)bc.ConvertFrom("#FFFFFFFF");
            Third_Grade_Button.Foreground = (Brush)bc.ConvertFrom("#FF2F5FD2");
        }
    }
}
