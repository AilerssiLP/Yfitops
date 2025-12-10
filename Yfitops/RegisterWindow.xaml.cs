using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Yfitops.Services;
using Yfitops.Models;
using System.Windows.Controls;

namespace Yfitops;

public partial class RegisterWindow : Window
{
    private readonly UserService _userService;

    public RegisterWindow()
    {
        InitializeComponent();
        _userService = App.Services.GetRequiredService<UserService>();
    }

    private async void Register_Click(object sender, RoutedEventArgs e)
    {
        string username = UsernameBox.Text;
        string password = PasswordBox.Password;

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            MessageBox.Show("Username and password cannot be empty.");
            return;
        }

        if (RoleBox.SelectedItem is not ComboBoxItem selected)
        {
            MessageBox.Show("Please select a role.");
            return;
        }

        string roleString = selected.Tag!.ToString()!;
        UserRole role = roleString == "Musician" ? UserRole.Musician : UserRole.User;

        bool ok = await _userService.Register(username, password, role);

        if (ok)
        {
            MessageBox.Show("Registration successful!");
            Close();
        }
        else
        {
            MessageBox.Show("Username already exists.");
        }
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
