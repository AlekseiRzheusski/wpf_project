using System;
using System.Collections.Generic;
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
using System.Collections;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Runtime.Serialization.Json;
using System.ComponentModel;
using PlaginInterface;
using System.Drawing;
using System.IO.Compression;
using WpfApp5.Classes;
using System.Reflection;
using System.Linq;
//using WpfApp5.Classes;

namespace WpfApp5
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IEnumerable
    {


        long k = 0;
        //string tmp = "";
        //string temp = "";
        int i = 0;
        public string tmp_poster;
        public bool ifPoster = false;
        public bool IfOpen = false;
        public string tmp_change_poster;
        bool isascending = false;
        bool isdescending = false;
        //bool isloaded = false;


        public ObservableCollection<Book> Books = new ObservableCollection<Book>();
        public ObservableCollection<Book> SortedBooks = new ObservableCollection<Book>();
        public ObservableCollection<Author> _authors = new ObservableCollection<Author>();
        public ObservableCollection<Author> Tmp_authors = new ObservableCollection<Author>();
        public ObservableCollection<Publisher> publisers = new ObservableCollection<Publisher>();
        public ObservableCollection<Book> SBook1 = new ObservableCollection<Book>();
        public ObservableCollection<Author> Authors;
        public AllInf allInf = new AllInf();

        BinaryFormatter formatter = new BinaryFormatter();
        BinaryFormatter formatter1 = new BinaryFormatter();

        BackgroundWorker bw = new BackgroundWorker();


        Dictionary<string, IPlugin> plugins;
        ICollection<IPlugin> pluginsCollection;

        private readonly string pluginPath = System.IO.Path.Combine(Environment.CurrentDirectory, Properties.Settings.Default.Directory_Plugins);


        public BitmapImage Convert1(Bitmap src)
        {
            MemoryStream ms = new MemoryStream();
            (src).Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();
            return image;
        }


        public MainWindow()
        {

            InitializeComponent();

            plugins = new Dictionary<string, IPlugin>();

            DatePicker1.DisplayDateEnd = DateTime.Today;
            DatePickerAut.DisplayDateEnd = DateTime.Today;
            DatePicker2.DisplayDateEnd = DateTime.Today;
            DatePicker3.DisplayDateEnd = DateTime.Today;


            Author author = new Author();
            author.AutName = "kazuhiro otomo";
            author.BirthDate = "14.04.1954";
            _authors.Add(author);
            Tmp_authors.Add(author);

            Author author1 = new Author();
            author1.AutName = "kazuhiro otomo1";
            author1.BirthDate = "14.04.1954";
            _authors.Add(author1);
            Tmp_authors.Add(author1);



            Publisher publisher = new Publisher();
            publisher.PubName = "Kodansha";
            publisher.PubCity = "Tokio";
            publisers.Add(publisher);

            Book book1 = new Book();
            book1.Name = "Akira vol.1";
            book1.NumberOfPages = 32;
            book1.ISBN = 1389467835621;
            book1.releaseDate = "06.12.1982";
            book1.ImagePath = "D:/обложки/7.jpg";
            book1.Authors = Tmp_authors;
            book1.Publ = publisher.PubName + ", " + publisher.PubCity;
            book1.Publish = publisher;
            Books.Add(book1);

            BookList.ItemsSource = Books;
            AuthorsComboBox.ItemsSource = _authors;
            PubComboBox.ItemsSource = publisers;
            PublDelete.ItemsSource = publisers;
            PublChange.ItemsSource = publisers;

            bw.DoWork += bwDoWork;
            bw.RunWorkerCompleted += bwRunWorkerCompleted;
            bw.WorkerReportsProgress = true;
            bw.WorkerSupportsCancellation = true;
            bw.RunWorkerAsync();

            fileIcon.Source = Convert1(Properties.Resources.Fiile);
            ascending_image.Source = Convert1(Properties.Resources.sortirowkaaz);
            descending_image.Source=Convert1(Properties.Resources.sortirowkaza);
            saveIcon.Source = Convert1(Properties.Resources.save);
            
            DataContext = this;





        }

      

        private void bwDoWork(object sender, DoWorkEventArgs e)
        {
            plugins.Clear();

            DirectoryInfo pluginDirectory = new DirectoryInfo(pluginPath);

            if (!pluginDirectory.Exists)
            {
                pluginDirectory.Create();
            }

            pluginsCollection = GenericPluginLoader<IPlugin>.LoadPlugins(pluginPath);
        }

        private void bwRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (pluginsCollection.Count != 0)
            {
                MenuItem pluginsMenuItem = new MenuItem();
                pluginsMenuItem.Header = "Плагины";
                FileMenu.Items.Add(pluginsMenuItem);

                foreach (var item in pluginsCollection)
                {
                    plugins.Add(item.Name, item);
                    MenuItem menuItem = new MenuItem();
                    menuItem.Header = item.Name;
                    menuItem.Click += menuItem_Click;
                    pluginsMenuItem.Items.Add(menuItem);
                }
            }
        }


        private void menuItem_Click(object sender, RoutedEventArgs e)
        {
            Book SaveBook = (Book)BookList.SelectedItem;
            MenuItem b = sender as MenuItem;
            if (b != null)
            {
                string key = b.Header.ToString();
                if (plugins.ContainsKey(key))
                {
                    IPlugin plugin = plugins[key];
                    plugin.Do(SaveBook);
                }
            }
        }



        public IEnumerator GetEnumerator()
        {
            return Books.GetEnumerator();
        }

        public void Button_Click_AddImage(object sender, RoutedEventArgs e)
        {
            OpenFileDialog myDialog = new OpenFileDialog();
            myDialog.Filter = "Картинки(*.JPG;*.GIF)|*.JPG;*.GIF" + "|Все файлы (*.*)|*.* ";
            myDialog.CheckFileExists = true;
            myDialog.Multiselect = true;
            if (myDialog.ShowDialog() == true)
            {
                tmp_poster = myDialog.FileName;
                ifPoster = true;
            }
        }

        public void Change_Image(object sender, RoutedEventArgs e)
        {
            OpenFileDialog myDialog = new OpenFileDialog();
            Book Tmp_add_book = (Book)BookList.SelectedItem;
            myDialog.Filter = "Картинки(*.JPG;*.GIF)|*.JPG;*.GIF" + "|Все файлы (*.*)|*.* ";
            myDialog.CheckFileExists = true;
            myDialog.Multiselect = true;
            if (myDialog.ShowDialog() == true)
            {
                Tmp_add_book.ImagePath = myDialog.FileName;
                ifPoster = true;
            }
            
        }



        public void Button_Click_Add(object sender, RoutedEventArgs e)
        {
            if (NameText.Text == "" || ISBNText.Text == "" || NumberOfPagesText.Text == "" || DatePicker1.Text == "")
            {
                MessageBox.Show("Введите значения");
                return;
            }
            Author unknown = new Author();
            unknown.AutName = "Неизвестный автор";
            unknown.BirthDate = "0.00.0001";
            Authors = new ObservableCollection<Author>();
            Publisher Tmp_publisher = (Publisher)PubComboBox.SelectedItem;
            Authors.Add(unknown);
            Book add_book = new Book();
            this.DataContext = add_book;
            string tmp_name = NameText.Text;
            long tmp_pages = long.Parse(NumberOfPagesText.Text);
            long tmp_isbn = long.Parse(ISBNText.Text);
            string tmp_releaseDate = DatePicker1.Text;

            add_book.Name = tmp_name;
            add_book.NumberOfPages = tmp_pages;
            add_book.ISBN = tmp_isbn;
            add_book.releaseDate = tmp_releaseDate;
            add_book.Publish = Tmp_publisher;
            add_book.ImagePath = tmp_poster;
            add_book.Authors = Authors;


            foreach (var inf in Books)
            {
                if (add_book.Name == inf.Name && add_book.ISBN == inf.ISBN)
                {
                    MessageBox.Show("Такая книга уже существует");
                    NameText.Text = null;
                    ISBNText.Text = null;
                    return;
                }
            }

            Books.Add(add_book);
            if (isascending)
            {
                Book_Sort();
            }
            if (isdescending)
            {
                Book_Sort_Descrnding();
            }


        }

        public void Button_Click_Add_Author(object sender, RoutedEventArgs e)
        {
            if (NameAutText.Text == "" || DatePickerAut.Text == "")
            {
                MessageBox.Show("Введите значения");
                NameAutText.Text = null;
                DatePickerAut = null;
                return;
            }
            string name = NameAutText.Text;
            string tmp_BirthDate = DatePickerAut.Text;
            Author auth = new Author();
            auth.AutName = name;
            auth.BirthDate = tmp_BirthDate;
            Book Tmp_add_book = (Book)BookList.SelectedItem;
            Author remove = (Author)AuthorList.SelectedItem;
            foreach (var inf in _authors)
            {
                if (auth.AutName == inf.AutName)
                {
                    MessageBox.Show("Такой автор уже существует");
                    NameAutText.Text = null;
                    return;
                }
            }
            Tmp_add_book.Authors.Add(auth);
            _authors.Add(auth);
            foreach (var a in Tmp_add_book.Authors)
            {
                if (a.AutName == "Неизвестный автор")
                {
                    Tmp_add_book.Authors.Remove(a);
                    return;
                }
            }
        }

        public void Button_Click_Add_Exist_Author(object sender, RoutedEventArgs e)
        {
            Author exist = (Author)AuthorsComboBox.SelectedItem;
            Book Tmp_add_book = (Book)BookList.SelectedItem;
            foreach (var a in Tmp_add_book.Authors)
            {

                if (a.AutName == exist.AutName)
                {
                    MessageBox.Show("Такой автор уже существует");
                    NameAutText.Text = null;
                    return;
                }
            }
            Tmp_add_book.Authors.Add(exist);
            Author remove = (Author)AuthorList.SelectedItem;
            foreach (var a in Tmp_add_book.Authors)
            {

                if (a.AutName == "Неизвестный автор")
                {
                    Tmp_add_book.Authors.Remove(a);
                    return;
                }
            }

        }
        public void Button_Click_Delete(object sender, RoutedEventArgs e)
        {
            string sMessageBoxText = "Вы действительно хотите удалить?";
            string sCaption = "Удаление";

            MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
            MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

            MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);

            switch (rsltMessageBox)
            {
                case MessageBoxResult.Yes:
                    Book Delete_Book = (Book)BookList.SelectedItem;
                    foreach(var a in Delete_Book.Authors)
                    {
                        _authors.Remove(a);
                    }
                    Books.Remove((Book)BookList.SelectedItem);

                    break;

                case MessageBoxResult.No:
                    /* ... */
                    break;
            }
            if (isascending)
            {
                Book_Sort();
            }
            if (isdescending)
            {
                Book_Sort_Descrnding();
            }

        }

        public void Button_Click_AutDelete(object sender, RoutedEventArgs e)
        {
            Book Tmp_add_book = (Book)BookList.SelectedItem;
            string sMessageBoxText = "Вы действительно хотите удалить?";
            string sCaption = "Удаление";

            MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
            MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

            MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);

            switch (rsltMessageBox)
            {
                case MessageBoxResult.Yes:
                    Author del_author = ((Author)AuthorList.SelectedItem);
                    _authors.Remove(del_author);
                    Tmp_add_book.Authors.Remove((Author)AuthorList.SelectedItem);
                    
                    break;

                case MessageBoxResult.No:
                    /* ... */
                    break;
            }

        }

        public void Button_Click_Add_Publisher(object sender, RoutedEventArgs e)
        {
            Publisher tmp_publisher = new Publisher();
            tmp_publisher.PubName = TextPubName.Text;
            tmp_publisher.PubCity = TextPubCity.Text;
            publisers.Add(tmp_publisher);
        }

        public void Serealize_Book(object sender, RoutedEventArgs e)
        {
            if (BookList.SelectedItem == null)
            {
                MessageBox.Show("Выберите книгу");
            }
            else
            {
                k++;
                if (k >= 2)
                {
                    string sMessageBoxText = "Вы действительно хотите перезаписать?";
                    string sCaption = "Перезаписать";

                    MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
                    MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

                    MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);

                    switch (rsltMessageBox)
                    {
                        case MessageBoxResult.Yes:
                            Book save_book = (Book)BookList.SelectedItem;

                            using (FileStream fs = new FileStream("book.dat", FileMode.OpenOrCreate))
                            {
                                formatter.Serialize(fs, save_book);
                            }
                            break;

                        case MessageBoxResult.No:
                          
                            break;
                    }

                }
                else
                {
                    Book save_book = (Book)BookList.SelectedItem;
                    using (FileStream fs = new FileStream("people.dat", FileMode.OpenOrCreate))
                    {
                        formatter.Serialize(fs, save_book);
                        MessageBox.Show("Книга успешно сохранена");
                    }
                }
            }
        }

        public void Deserealize_Book(object sender, RoutedEventArgs e)
        {

            OpenFileDialog myDialog = new OpenFileDialog();
            myDialog.Filter = "Информация(*.dat)|*.dat" + "|Все файлы (*.*)|*.* ";
            myDialog.CheckFileExists = true;
            myDialog.Multiselect = true;
            if (myDialog.ShowDialog() == true)
            {
                using (FileStream fs = new FileStream(myDialog.FileName, FileMode.OpenOrCreate))
                {
                    Book newBook = (Book)formatter.Deserialize(fs);
                    foreach (var inf in Books)
                    {
                        if (newBook.Name == inf.Name && newBook.ISBN == inf.ISBN)
                        {
                            MessageBox.Show("Такая книга уже существует");
                            NameText.Text = null;
                            ISBNText.Text = null;
                            return;
                        }
                    }
                    Books.Add(newBook);
                   

                }
                
                IfOpen = true;
            }

        }

        

        public void SaveAs(object sender, RoutedEventArgs e)
        {
            if (BookList.SelectedItem == null)
            {
                MessageBox.Show("Выберите книгу");
            }
            else
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.FileName = "*";
                saveFileDialog.DefaultExt = ".dat";
                saveFileDialog.Filter = "Text documents (.dat)|*.dat";



                Nullable<bool> result = saveFileDialog.ShowDialog();

                if (result == true)
                {

                    Book save_book = (Book)BookList.SelectedItem;
                    using (FileStream fs = new FileStream(saveFileDialog.FileName, FileMode.OpenOrCreate))
                    {
                        formatter.Serialize(fs, save_book);
                        MessageBox.Show("Книга успешно сохранена");
                    }
                }
            }



        }

        private void Book_Pages_TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        

       

        [DataContract]
        public class Sereal_Books : IEnumerable
        {
            [DataMember]
            public ObservableCollection<Book> Ser_Book { get; set; }
           
            public Sereal_Books()
            {
                Ser_Book = new ObservableCollection<Book>();
            }

            public IEnumerator GetEnumerator()
            {
                return Ser_Book.GetEnumerator();
            }


        }

        public void SaveAll_reserve(object sender, RoutedEventArgs e)
        {
            //Copy();
            DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(Sereal_Books));
            Sereal_Books sb = new Sereal_Books();
            sb.Ser_Book = Books;
            using (FileStream fs = new FileStream("copyBooks.json", FileMode.Create))
            {
                jsonFormatter.WriteObject(fs, sb);
                MessageBox.Show("Проект успешно сохранен");

            }
        }

        public void Open_reserve_Copy(object sender, RoutedEventArgs e)
        {
            DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(Sereal_Books));
            Sereal_Books sb = new Sereal_Books();
            using (FileStream fs = new FileStream("copyBook.json", FileMode.OpenOrCreate))
            {
                sb = (Sereal_Books)jsonFormatter.ReadObject(fs);
                Books.Clear();
                foreach (var a in sb.Ser_Book)
                {
                    Books.Add(a);
                }
                BookList.ItemsSource = Books;
                isascending = false;
                isdescending = false;


            }
        }

        public void SaveAll(object sender, RoutedEventArgs e)
        {
            Copy();
            DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(Sereal_Books));
            Sereal_Books sb = new Sereal_Books();
            sb.Ser_Book = Books;
            using (FileStream fs = new FileStream("books.json", FileMode.Create))
            {
                jsonFormatter.WriteObject(fs, sb);
                MessageBox.Show("Проект успешно сохранен");

            }




        }

        //public void Copy()
        //{
        //    if (File.Exists("copyBook.json"))
        //        File.Delete("copyBook.json");
        //    File.Copy("books.json", "copyBook.json");
        //}

        public void OpenAll(object sender, RoutedEventArgs e)
        {
            OpenFileDialog myDialog = new OpenFileDialog();
            myDialog.Filter = "Информация(*.json)|*.json" + "|Все файлы (*.*)|*.* ";
            myDialog.CheckFileExists = true;
            myDialog.Multiselect = true;
            if (myDialog.ShowDialog() == true)
            {
                DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(Sereal_Books));
                Sereal_Books sb = new Sereal_Books();
                using (FileStream fs = new FileStream(myDialog.FileName, FileMode.OpenOrCreate))
                {
                    sb = (Sereal_Books)jsonFormatter.ReadObject(fs);
                    Books.Clear();
                    foreach (var a in sb.Ser_Book)
                    {
                        Books.Add(a);
                    }
                    BookList.ItemsSource = Books;
                    isascending = false;
                    isdescending = false;
                }
            }
                
              
            
           

        }

        public void Zip_Compress(object sender, RoutedEventArgs e)
        {
            using (FileStream sourceStream = new FileStream("books.json", FileMode.OpenOrCreate))
            {
                using (FileStream targetStream = File.Create("data.gz"))
                {
                    using (GZipStream compressionStream = new GZipStream(targetStream, CompressionMode.Compress))
                    {
                        sourceStream.CopyTo(compressionStream);
                    }
                }
            }
        }

        public void Delete_Publisher(object sender, RoutedEventArgs e)
        {
            Publisher pDelete=(Publisher)PublDelete.SelectedItem;
            publisers.Remove(pDelete);
        }

        private void Book_Pages_TextBox_PreviewTextInput1(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-7]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        public void Book_Date_Picker_Changed(object sender, SelectionChangedEventArgs e)
        {
            string date = Convert.ToString(DateTime.Today);
            if (DatePicker1.DisplayDate > DateTime.Today)
            {
                DatePicker1.Text = date;
            }
        }
        public void Aut_Date_Picker_Changed(object sender, SelectionChangedEventArgs e)
        {
            string date = Convert.ToString(DateTime.Today);
            if (DatePickerAut.DisplayDate > DateTime.Today)
            {
                DatePickerAut.Text = date;
            }
        }

        public void Bookcng_Date_Picker_Changed(object sender, SelectionChangedEventArgs e)
        {
            string date = Convert.ToString(DateTime.Today);
            if (DatePicker2.DisplayDate > DateTime.Today)
            {
                DatePicker2.Text = date;
            }
        }

        public void Autcng_Date_Picker_Changed(object sender, SelectionChangedEventArgs e)
        {
            string date = Convert.ToString(DateTime.Today);
            if (DatePicker3.DisplayDate > DateTime.Today)
            {
                DatePicker3.Text = date;
            }
        }

        public void Book_Sort_ascending(object sender, RoutedEventArgs e)
        {
            Book_Sort();
            isascending=true;
            isdescending = false;
        }

        public void Book_Sort_descending(object sender, RoutedEventArgs e)
        {
            Book_Sort_Descrnding();
            isdescending = true;
            isascending = false;
        }

        public void Book_Sort()
        {
            SortedBooks.Clear();
            var sorted_books = from u in Books
                               orderby u.Name ,u.releaseDate

                               select u;

            foreach (Book u in sorted_books)
            {
                SortedBooks.Add(u);
            }
            BookList.ItemsSource = SortedBooks;
        }

        public void Book_Sort_Descrnding()
        {
            SortedBooks.Clear();
            var sortedBook = Books.OrderByDescending(u => u.Name).ThenBy(u=>u.releaseDate);
            foreach(Book u in sortedBook)
            {
                SortedBooks.Add(u);
            }
            BookList.ItemsSource = SortedBooks;

        }

        public void Return_List(object sender, RoutedEventArgs e)
        {
            BookList.ItemsSource = Books;
            isascending = false;
            isdescending = false;
            SortedBooks.Clear();

        }

        private void Parallel_Sort(object sender, RoutedEventArgs e)
        {
            Books = new ObservableCollection<Book>(from u in Books.AsParallel()
                                                   orderby u.Name
                                                   select u);
        }

        private void Search(object sender, RoutedEventArgs e)
        {
            SBook1.Clear();
            var serchedBook = Books.Where(u => u.Name == SearchItem.Text && u.releaseDate == SearchPicker.Text);

            foreach (var a in serchedBook)
            {
                SBook1.Add(a);

            }
            BookList.ItemsSource = SBook1;
            if (SBook1 == null) { MessageBox.Show("Такой книги не существует"); };
        }

        public void Groupping(object sender, RoutedEventArgs e)
        {
            string output = "";
            var BookGroups = from Book in Books
                              group Book by Book.releaseDate;

            foreach (IGrouping<string, Book> g in BookGroups)
            {
                output = output+ g.Key+": ";
               
                foreach (var t in g)
                {
                    output =output+"\n"+"  "+ t.Name;
                }
                
                output += "\n";

            }
            MessageBox.Show(output);

        }

        public void Agregate(object sender, RoutedEventArgs e)
        {
            string output = "";
            
            long min = Books.Min(n => n.NumberOfPages);
            long max = Books.Max(n => n.NumberOfPages);
            long sum = Books.Sum(n => n.NumberOfPages);

            output += "Максимальное кол-во страниц: " + max + "\nМинимальное кол-во страниц: " + min + "\nСумма всех страниц: " + sum;
            MessageBox.Show(output);

        }

 

        public void opn_overviw(object sender, RoutedEventArgs e)
        {
            OverviewWindow ow = new OverviewWindow(Books);
            ow.ShowDialog();
        }

       

        private void Copy()
        {
            if (File.Exists("copyBook.json"))
                File.Delete("copyBook.json");
            if (File.Exists("books.json")) File.Copy("books.json", "copyBook.json");
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            if (File.Exists("books.json"))
            {
                if (File.Exists("books2.json"))
                    File.Delete("books2.json");
                File.Move("books.json", "books2.json");
            }
        }

        private ActionCommand command;
        public ActionCommand TabSelect
        {
            get
            {
                return (command = new ActionCommand(obj => {
                    TabItem ti = obj as TabItem;
                    ti.IsSelected = true;
                }));
            }
        }

        
    }

    
}
