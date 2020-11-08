using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
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
using System.Windows.Shapes;

namespace WpfApp5
{
    /// <summary>
    /// Логика взаимодействия для OverviewWindow.xaml
    /// </summary>
    public partial class OverviewWindow : Window
    {
        ObservableCollection<Book> books;

        BookPage1 firstPage;
        BookPage1 previousPage;
        BookPage1 nextPage;
        BookPage1 lastPage;

        //public BitmapImage Convert(Bitmap src)
        //{
        //    MemoryStream ms = new MemoryStream();
        //    (src).Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
        //    BitmapImage image = new BitmapImage();
        //    image.BeginInit();
        //    ms.Seek(0, SeekOrigin.Begin);
        //    image.StreamSource = ms;
        //    image.EndInit();
        //    return image;
        //}

        public OverviewWindow(ObservableCollection<Book> books)
        {
            InitializeComponent();
            this.books = books;



            firstPage = new BookPage1(books.First());
            lastPage = new BookPage1(books.Last());
            filmsFrame.Content = firstPage;

            if (books.Count == 1)
            {
                nextPage = firstPage;
                previousPage = firstPage;
            }
            else if (books.Count == 2)
            {
                nextPage = lastPage;
                previousPage = lastPage;
            }
            else
            {
                nextPage = new BookPage1(books.ElementAt(books.IndexOf(firstPage.book) + 1));

                if (books.IndexOf(firstPage.book) - 1 < 0)
                {
                    previousPage = lastPage;
                }
                else
                {
                    previousPage = new BookPage1(books.ElementAt(books.IndexOf(firstPage.book) - 1));
                }

              
            }





        }

        private void First_Click(object sender, RoutedEventArgs e)
        {
            filmsFrame.Content = firstPage;

            Thread firstPageThread = new Thread(new ThreadStart(FirstPageDo));
            firstPageThread.SetApartmentState(ApartmentState.STA);
            firstPageThread.IsBackground = true;
            firstPageThread.Start();
        }

        private void Previous_Click(object sender, RoutedEventArgs e)
        {
            filmsFrame.Content = previousPage;

            Thread previousPageThread = new Thread(new ThreadStart(PreviousPageDo));
            previousPageThread.SetApartmentState(ApartmentState.STA);
            previousPageThread.IsBackground = true;
            previousPageThread.Start();
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            filmsFrame.Content = nextPage;

            Thread nextPagesThread = new Thread(new ThreadStart(NextPageDo));
            nextPagesThread.SetApartmentState(ApartmentState.STA);
            nextPagesThread.IsBackground = true;
            nextPagesThread.Start();
        }

        private void Last_Click(object sender, RoutedEventArgs e)
        {
            filmsFrame.Content = lastPage;

            Thread lastPagesThread = new Thread(new ThreadStart(LastPageDo));
            lastPagesThread.SetApartmentState(ApartmentState.STA);
            lastPagesThread.IsBackground = true;
            lastPagesThread.Start();
        }

        private void FirstPageDo()
        {
            int nextIndex = books.IndexOf(firstPage.book) + 1;
            int previousIndex = books.IndexOf(firstPage.book) - 1;

            if (nextIndex > books.Count - 1)
            {
                nextIndex = 0;
            }
            if (previousIndex < 0)
            {
                previousIndex = books.Count - 1;
            }

            Dispatcher.Invoke(() => nextPage = new BookPage1(books.ElementAt(nextIndex)));
            Dispatcher.Invoke(() => previousPage = new BookPage1(books.ElementAt(previousIndex)));
        }

        private void NextPageDo()
        {

            int nextIndex = books.IndexOf(nextPage.book) + 1;
            int previousIndex = books.IndexOf(nextPage.book) - 1;

            if (nextIndex > books.Count - 1)
            {
                nextIndex = 0;
            }
            if (previousIndex < 0)
            {
                previousIndex = 0;
            }

            Dispatcher.Invoke(() => nextPage = new BookPage1(books.ElementAt(nextIndex)));
            Dispatcher.Invoke(() => previousPage = new BookPage1(books.ElementAt(previousIndex)));

        }

        private void PreviousPageDo()
        {
            int nextIndex = books.IndexOf(previousPage.book) + 1;
            int previousIndex = books.IndexOf(previousPage.book) - 1;
            if (nextIndex > books.Count - 1)
            {
                nextIndex = 0;
            }
            if (previousIndex < 0)
            {
                previousIndex = books.Count - 1;
            }

            Dispatcher.Invoke(() => nextPage = new BookPage1(books.ElementAt(nextIndex)));
            Dispatcher.Invoke(() => previousPage = new BookPage1(books.ElementAt(previousIndex)));
        }

        private void LastPageDo()
        {
            int previousIndex = books.IndexOf(lastPage.book) - 1;

            if (previousIndex < 0)
            {
                previousIndex = books.Count - 1;
            }

            nextPage = firstPage;
            Dispatcher.Invoke(() => previousPage = new BookPage1(books.ElementAt(previousIndex)));
        }

    }
}
