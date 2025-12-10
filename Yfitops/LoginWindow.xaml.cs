using System.Windows;
using Yfitops.Services;
using Yfitops.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;
using Yfitops;

namespace Yfitops;

public partial class LoginWindow : Window
{
    private readonly UserService _userService;

    public LoginWindow()
    {
        InitializeComponent();
        _userService = App.Services.GetRequiredService<UserService>();
    }

    private async void Login_Click(object sender, RoutedEventArgs e)
    {
        string username = UsernameBox.Text;
        string password = PasswordBox.Password;

        bool ok = await _userService.Login(username, password);

        if (!ok)
        {
            MessageBox.Show("Invalid username or password.");
            return;
        }

        MessageBox.Show($"Welcome {username}!");

        switch (_userService.CurrentUser!.Role)
        {
            case UserRole.Admin:
                new AdminWindow().Show();
                break;

            case UserRole.Musician:
                new MusicianWindow().Show();
                break;

            case UserRole.User:
                new UserWindow().Show();
                break;
        }

        this.Close();
    }

    private async void Register_Click(object sender, RoutedEventArgs e)
    {
        string username = UsernameBox.Text;
        string password = PasswordBox.Password;

        bool ok = await _userService.Register(username, password, UserRole.User);

        if (ok)
            MessageBox.Show("Registration successful!");
        else
            MessageBox.Show("Username already exists.");
    }
}
