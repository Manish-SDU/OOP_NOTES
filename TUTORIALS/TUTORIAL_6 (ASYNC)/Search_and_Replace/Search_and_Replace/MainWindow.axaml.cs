using System.IO;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;

namespace Search_and_Replace.Search_and_Replace;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void ReplaceAllHandler(object? sender, RoutedEventArgs e)
    {
        if (TextBoxSearch.Text != null && TextBoxReplace.Text != null && TextBox.Text != null) 
        {
            string searchText = TextBoxSearch.Text;
            string replaceText = TextBoxReplace.Text;
            string originalText = TextBox.Text;
            string newText = originalText.Replace(searchText, replaceText);
            TextBox.Text = newText;
        }
    }
    
    // https://docs.avaloniaui.net/docs/basics/user-interface/file-dialogs
    private async void SaveFileHandler(object? sender, RoutedEventArgs e)
    {
        // Get top level from the current control. Alternatively, you can use Window reference instead.
        var topLevel = GetTopLevel(this);

        // Start async operation to open the dialog.
        var file = await topLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "Save Text File"
        });

        if (file is not null)
        {
            // Open writing stream from the file.
            await using var stream = await file.OpenWriteAsync();
            using var streamWriter = new StreamWriter(stream);
            // Write some content to the file.
            await streamWriter.WriteLineAsync(TextBox.Text);
        }
    }
    
    // https://docs.avaloniaui.net/docs/basics/user-interface/file-dialogs
    private async void OpenFileHandler(object sender, RoutedEventArgs args)
    {
        // Get top level from the current control. Alternatively, you can use Window reference instead.
        var topLevel = GetTopLevel(this);

        // Start async operation to open the dialog.
        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
        {
            Title = "Open Text File",
            AllowMultiple = false
        });

        if (files.Count >= 1)
        {
            // Open reading stream from the first file.
            await using var stream = await files[0].OpenReadAsync();
            using var streamReader = new StreamReader(stream);
            // Reads all the content of file as a text.
            var fileContent = await streamReader.ReadToEndAsync();
            TextBox.Text = fileContent;
        }
    }
}