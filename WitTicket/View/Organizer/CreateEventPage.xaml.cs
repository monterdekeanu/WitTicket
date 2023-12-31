using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Maui.Storage;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using WitTicket.Model;

namespace WitTicket.View.Organizer;

public partial class CreateEventPage : ContentPage
{
    private List<byte[]> selectedImagesData = new List<byte[]>(); // Store selected images in a list
    private List<string> selectedImageFileNames = new List<string>();
    public ObservableCollection<EventClassModel> EventClasses { get; set; }
    public UserModel activeUser = new();
    public EventModel ActiveEvent { get; set; } = new();
    public CreateEventPage(UserModel user)
    {
        InitializeComponent();
        EventClasses = new ObservableCollection<EventClassModel>();
        btnAddEvent.Clicked += OnClickAddEvent;
        activeUser = user;
        BindingContext = this;
    }

    public CreateEventPage(UserModel user, int activeEvent) //Update Event Initialization
    {
        InitializeComponent();
        ActiveEvent = (new Services.Connection().GetEvent(activeEvent));
        EventClasses = ActiveEvent.EventClasses.ToObservableCollection<EventClassModel>();
        activeUser = user;
        
        InitializeActiveEvent();
        BindingContext = this;

    }
    private void InitializeActiveEvent()
    {
        btnAddEvent.Clicked += OnClickSaveEvent;
        btnAddEvent.Text = "Update Event";
        capacityEntry.Text = ActiveEvent.TotalCapacity.ToString();
        eventNameEntry.Text = ActiveEvent.Name;
        startDatePicker.Date = ActiveEvent.StartDate.Date;
        startTimePicker.Time = ActiveEvent.StartDate.TimeOfDay;
        endDatePicker.Date = ActiveEvent.EndDate.Date;
        endTimePicker.Time = ActiveEvent.EndDate.TimeOfDay;
        cityEntry.Text = ActiveEvent.City;
        Street.Text = ActiveEvent.Street;
        txtDescription.Text = ActiveEvent.Description;
    }
    private void UpdateCapacity()
    {
        int totalCapacity = 0;
        EventClasses.ToList().Where(x => !x.IsDeleted).ToList().ForEach(x => totalCapacity += x.ClassMaxQuantity); 
        capacityEntry.Text = totalCapacity.ToString();
        ActiveEvent.TotalCapacity = totalCapacity;
    }

    private void OnPriceChange(object sender, EventArgs e)
{
    int classId = ((EventClassModel)((Entry)sender).BindingContext).ClassId;
    string inputText = ((Entry)sender).Text;

    // Remove non-numeric characters and at most one dot at the end
    string cleanedInput = new string(inputText
        .Where((c, index) => 
            (char.IsDigit(c) || (c == '.' && inputText.Count(ch => ch == '.') == 1)))
        .ToArray());

    if (cleanedInput.Length > 0)
        {
            // Add '0' if the last character is a dot
            if (cleanedInput[cleanedInput.Length - 1] == '.')
            {
                cleanedInput += "0";
            }

            // Update ClassPrice using LINQ
            EventClasses.FirstOrDefault(x => x.ClassId == classId).ClassPrice = double.Parse(cleanedInput);
        }
        else if(cleanedInput.Length == 0)
        {
            EventClasses.FirstOrDefault(x => x.ClassId == classId).ClassPrice = 0;
        }
        else
        {
            EventClasses.FirstOrDefault(x => x.ClassId == classId).ClassPrice = double.Parse(cleanedInput);
        }
    }

    private void OnQuantityChange(object sender, EventArgs e)
    {
        int classId = ((EventClassModel)((Entry)sender).BindingContext).ClassId;
        string inputText = ((Entry)sender).Text;

        // Remove non-numeric characters
        string cleanedInput = new string(inputText.Where(char.IsDigit).ToArray());

        if (int.TryParse(cleanedInput, out int parsedQuantity))
        {
            EventClasses.FirstOrDefault(x => x.ClassId == classId).ClassMaxQuantity = parsedQuantity;
            UpdateCapacity();
        }
        else
        {
            // Handle the case where the input is not a valid integer
            Debug.WriteLine($"Invalid input: {inputText}");
        }
    }

    private void OnClassNameChange(object sender, EventArgs e)
    {
        int classId = ((EventClassModel)((Entry)sender).BindingContext).ClassId;
        EventClasses.FirstOrDefault(x => x.ClassId == classId).ClassName = ((Entry)sender).Text;
    }
    private void OnClickAddClass(object sender, EventArgs e)
    {
        ObservableCollection<EventModel> events = (new Services.Connection()).GetEventsList();
        int eventId = 0;
        if(events.Count>0)
        {
            eventId = events.Last().EventId + 1;
        }
        try
        {
            EventClasses.Add(new EventClassModel(EventClasses.Count, txtClassName.Text, double.Parse(txtPrice.Text), Int32.Parse(txtQuantity.Text), eventId));
            txtClassName.Text = "";
            txtPrice.Text = "";
            txtQuantity.Text = "";
            UpdateCapacity();
        }
        catch(Exception err)
        {
            DisplayAlert("Invalid Input", "Input numeric price value only.", "OK");
            Console.WriteLine(err);
        }
    }
    private void OnClickRemoveClass (object sender, EventArgs e)
    {
        EventClasses.FirstOrDefault(x => x.ClassId == Int32.Parse(((Button)sender).AutomationId)).IsDeleted = true;
        ((Grid)((Button)sender).Parent).IsVisible = false;
        //EventClasses.Remove((EventClassModel)((Button)sender).BindingContext);
        //for(int i = 0; i < EventClasses.Count; i++) //update index
        //{
        //    EventClasses[i].ClassId = i;
        //}
        UpdateCapacity();
    }
    private async void OnClickUploadImage(object sender, EventArgs e)
    {
        selectedImagesData.Clear();
        var options = new PickOptions
        {
            PickerTitle = "Select an Image",
            FileTypes = FilePickerFileType.Images,
        };

        var selectedMedia = await FilePicker.PickMultipleAsync(options);

        if (selectedMedia != null)
        {
            foreach (var media in selectedMedia)
            {
                string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(media.FileName);
                selectedImageFileNames.Add(uniqueFileName);
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
                HeightRequest = 200, // Set the desired image height
                WidthRequest = 200, // Set the desired image width
                MaximumHeightRequest = 200,
                MaximumWidthRequest = 200,
                Margin = 2,
                Aspect = Aspect.Fill // Fill the image box without distorting the image
            };

            imageStackLayout.Children.Add(image);
        }
    }

    private void OnClickClosePopup(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }
    private async void OnClickAddEvent(object sender, EventArgs e)
    {
        if (ValidateInput())
        {
            ObservableCollection<EventModel> events = (new Services.Connection()).GetEventsList();
            
            EventModel eventModel = new EventModel(
                eventClasses: EventClasses.ToList<EventClassModel>(),
                eventId: 0, // You can set this to an appropriate value GET LIST FIRST THEN UPDATE AS IT AAAAAAAAAAAA TABANG MGA LANGIT
                name: eventNameEntry.Text,
                city: cityEntry.Text,
                street: Street.Text,
                startDate: startDatePicker.Date + startTimePicker.Time,
                endDate: endDatePicker.Date + endTimePicker.Time,
                price: EventClasses[0].ClassPrice,//decimal.Parse(priceEntry.Text),
                totalCapacity: int.Parse(capacityEntry.Text),
                organizer: activeUser.FirstName + " " + activeUser.LastName, // Replace with the actual organizer's name
                organizerId: activeUser.AccountId,
                description: txtDescription.Text
            );
            if (events.Count > 0)
            {
                 eventModel.EventId= events.Last().EventId + 1; //updated eventID here
            }

            
            await SaveImagesToResourcesAsync();
            

            // Add the selected images to the event
            foreach(string fileName in selectedImageFileNames)
            {
                eventModel.AddImage(fileName);
            }

            // Add the event to the list of events
            events.Add(eventModel);

            // Clear the list of selected image file names and data

            selectedImageFileNames.Clear();
            selectedImagesData.Clear();

            (new Services.Connection()).AddEvents(events);
            await DisplayAlert("Success", "Event created successfully.", "OK");
            await Navigation.PushAsync(new DashboardOrganizer(activeUser));
        }
    }
    private async void OnClickSaveEvent(object sender, EventArgs e)
    {
        if (ValidateInput())
        {
            ObservableCollection<EventModel> events = (new Services.Connection()).GetEventsList();

            EventModel eventModel = new EventModel(
                eventClasses: EventClasses.ToList<EventClassModel>(),
                eventId: ActiveEvent.EventId, // You can set this to an appropriate value GET LIST FIRST THEN UPDATE AS IT AAAAAAAAAAAA TABANG MGA LANGIT
                name: eventNameEntry.Text,
                city: cityEntry.Text,
                street: Street.Text,
                startDate: startDatePicker.Date + startTimePicker.Time,
                endDate: endDatePicker.Date + endTimePicker.Time,
                price: EventClasses[0].ClassPrice,//decimal.Parse(priceEntry.Text),
                totalCapacity: int.Parse(capacityEntry.Text),
                organizer: activeUser.FirstName + " " + activeUser.LastName, // Replace with the actual organizer's name
                organizerId: activeUser.AccountId,
                description: txtDescription.Text
            );
            eventModel.Images = ActiveEvent.Images;

            await SaveImagesToResourcesAsync();


            // Add the selected images to the event
            //foreach (string fileName in selectedImageFileNames)
            //{
            //    eventModel.AddImage(fileName);
            //}

            // Save the event to the list of events
            for(int i = 0; i < events.Count; i++)
            {
                if (events[i].EventId == ActiveEvent.EventId)
                {
                    events[i] = eventModel;
                    break;
                }
            }
            

            (new Services.Connection()).AddEvents(events);
            await DisplayAlert("Success", "Event created successfully.", "OK");
            await Navigation.PushAsync(new DashboardOrganizer(activeUser));
            // Clear the list of selected image file names and data

            selectedImageFileNames.Clear();
            selectedImagesData.Clear();
        }
    }
    private async Task SaveImagesToResourcesAsync()
    {
        try
        {
            string imagesFolderPath = Environment.GetEnvironmentVariable("DATABASE_EVENT_IMAGES",EnvironmentVariableTarget.Process);
            

            foreach (var fileName in selectedImageFileNames)
            {
                // Get the image data
                byte[] imageData = selectedImagesData[selectedImageFileNames.IndexOf(fileName)];

                // Combine the destination path with the file name
                string destinationPath = Path.Combine(imagesFolderPath, fileName);

                // Create the destination directory if it doesn't exist
                Directory.CreateDirectory(Path.GetDirectoryName(destinationPath));

                // Save the image data to the destination file
                using (var outputStream = File.OpenWrite(destinationPath))
                {
                    await outputStream.WriteAsync(imageData, 0, imageData.Length);
                }
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions here
            await DisplayAlert("Image Saving Error", ex.Message, "OK");
            Console.WriteLine($"Image saving error: {ex.Message}");
        }
    }

    private async void OnClickAutoGenerate(object sender, EventArgs e)
    {
        string response = await new Services.AutoGenerate().GenerateContentAsync("how old is chatgpt?");
        await DisplayAlert("Haha", response, "OK");
    }
    private bool ValidateInput()
    {
        if (string.IsNullOrWhiteSpace(eventNameEntry.Text))
        {
            DisplayAlert("Error", "Event Name cannot be empty.", "OK");
            return false;
        }

        if (string.IsNullOrWhiteSpace(txtDescription.Text))
        {
            DisplayAlert("Error", "Event Description cannot be empty.", "OK");
            return false;
        }

        if (string.IsNullOrWhiteSpace(cityEntry.Text))
        {
            DisplayAlert("Error", "City cannot be empty.", "OK");
            return false;
        }

        if (string.IsNullOrWhiteSpace(Street.Text))
        {
            DisplayAlert("Error", "Street cannot be empty.", "OK");
            return false;
        }

        

        if (!int.TryParse(capacityEntry.Text, out int totalCapacity) || totalCapacity <= 0)
        {
            DisplayAlert("Error", "Invalid or empty Total Capacity value.", "OK");
            return false;
        }

        DateTime startDate = startDatePicker.Date + startTimePicker.Time;
        DateTime endDate = endDatePicker.Date + endTimePicker.Time;

        if (startDate >= endDate)
        {
            DisplayAlert("Error", "End Date and Time must be after Start Date and Time.", "OK");
            return false;
        }

        return true;
    }
}