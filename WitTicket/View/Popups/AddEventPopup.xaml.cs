using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Views;
using WitTicket.Model;

namespace WitTicket.View.Popups;

public partial class AddEventPopup : Popup
{
    private List<byte[]> selectedImagesData = new List<byte[]>(); // Store selected images in a list
    UserModel activeUser = new();
	public AddEventPopup(UserModel user)
	{
		InitializeComponent();
        activeUser = user;
	}

    private async void OnClickUploadImage(object sender, EventArgs e)
	{
        selectedImagesData.Clear();
        var options = new PickOptions
        {
            PickerTitle = "Select an Image",
            FileTypes = FilePickerFileType.Images
        };

        var selectedMedia = await FilePicker.PickMultipleAsync(options);

        if (selectedMedia != null)
        {
            foreach (var media in selectedMedia)
            {
                // Read the selected image into a byte array
                using (var stream = await media.OpenReadAsync())
                using (var memoryStream = new MemoryStream())
                {
                    await stream.CopyToAsync(memoryStream);
                    selectedImagesData.Add(memoryStream.ToArray());
                }
            }

            // Display the selected images
            UpdateImageDisplay();
        }

    }
    private void UpdateImageDisplay()
    {
        // Clear existing images
        imageStackLayout.Children.Clear();

        // Display selected images
        foreach (var imageData in selectedImagesData)
        {
            var image = new Image
            {
                Source = ImageSource.FromStream(() => new MemoryStream(imageData)),
                HeightRequest = 64, // Set the desired image height
                WidthRequest = 64 // Set the desired image width
            };

            imageStackLayout.Children.Add(image);
        }
    }

    private void OnClickClosePopup(object sender, EventArgs e)
    {
        Close();
    }
}