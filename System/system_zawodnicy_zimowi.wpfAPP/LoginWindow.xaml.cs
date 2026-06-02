using System.Windows;
using system_zawodnicy_zimowi.core.Domain.Entities;
using system_zawodnicy_zimowi.Data;

namespace system_zawodnicy_zimowi.wpfAPP
{
    public partial class LoginWindow : Window
    {
        private ManagerDanych manager;

        public LoginWindow()
        {
            InitializeComponent();
            manager = new ManagerDanych();
        }

      
        private void BtnZaloguj_Click(object sender, RoutedEventArgs e)
        {
            string login = txtLogin.Text;
            string haslo = txtHaslo.Password;
          

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(haslo))
            {
                lblKomunikat.Text = "Wpisz login i hasło!";
                return;
            }

            bool sukces = manager.ZalogujUzytkownika(login, haslo);

            if (sukces)
            {
                MainWindow glowneOkno = new MainWindow();
                glowneOkno.Show();
                Application.Current.MainWindow = glowneOkno;

                this.Close();
            }
            else
            {
                lblKomunikat.Foreground = System.Windows.Media.Brushes.Red;
                lblKomunikat.Text = "Błędny login lub hasło.";
                txtHaslo.Clear();
            }
        }
        private void BtnRejestruj_Click(object sender, RoutedEventArgs e)
        {
            string login = txtLogin.Text;
            string haslo = txtHaslo.Password;
            string kodAuth = txtKodAuth.Password;


            if (kodAuth != "Admin123")
            {
                lblKomunikat.Foreground = System.Windows.Media.Brushes.Red;
                lblKomunikat.Text = "Błędny kod administratora! Brak uprawnień.";
                return; 
            }

            try
            {
               
                manager.ZarejestrujUzytkownika(login, haslo);

               
                lblKomunikat.Foreground = System.Windows.Media.Brushes.Green;
                lblKomunikat.Text = "Konto utworzone! Możesz się zalogować.";
                txtKodAuth.Clear();
                txtHaslo.Clear();
            }
            catch (Exception ex)
            {
              
                lblKomunikat.Foreground = System.Windows.Media.Brushes.Red;
                lblKomunikat.Text = ex.Message;
            }
        }

            public void ZarejestrujUzytkownika(string login, string haslo)
            {
            using (var context = new AppDbContext())
            {
                
                if (context.Uzytkownicy.Any(u => u.Login == login))
                {
                    
                    throw new Exception("Ten login jest już zajęty.");
                }

               
                var nowyUzytkownik = new Uzytkownik(login, haslo);

                context.Uzytkownicy.Add(nowyUzytkownik);
                context.SaveChanges();
            }
        }
    }
}