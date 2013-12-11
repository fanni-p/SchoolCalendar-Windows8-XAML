using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;
using TimetableApp.Behavior;
using TimetableApp.Common;
using TimetableApp.Data;
using TimetableApp.Models;
using TimetableApp.Services;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Media.Capture;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TimetableApp.ViewModel
{
    public class HomePageViewModel: ViewModelBase
    {
        private const int MinUsernameLength = 5;
        private const int MaxUsernameLength = 30;
        private double popupWidth;
        private bool isGridVisible;
        private bool isOpen = true;
        private bool isToastNotificated;
        private string username;
        private string message;
        private IEnumerable<LessonModel> currentDay;
        private IEnumerable<HomeworkModel> nextHomeworks;
        private bool noInternetFormIsOpen = false;
        private bool isMessageVisible = false;
        private bool isNoLessonsTextVisible = false;
        private DataTransferManager dataTransferManager;

        public HomePageViewModel()
        {
            this.CheckIsUserLoggedIn();
            this.TakePicture = new RelayCommand(this.TakePictureCommand);
            this.LoginCommand = new RelayCommand(this.HandleLoginCommand);
            this.RegisterCommand = new RelayCommand(this.HandleRegisterCommand);
            this.RetryCommand = new RelayCommand(this.HandleRetryCommand);
            this.NavigateToSubjectsPage = new RelayCommand(this.HandleNavigateToSubjectsPage);
            this.NavigateToHomeworksPage = new RelayCommand(this.HandleNavigateToHomeworksPage);
            this.NavigateToWeekSchedulePage = new RelayCommand(this.HandleNavigateToWeekSchedulePage);
            this.AddSubjectCommand = new RelayCommand(this.HandleAddSubjectCommand);
            this.AddHomeworkCommand = new RelayCommand(this.HandleAddHomeworkCommand);
            this.AddLessonCommand = new RelayCommand(this.HandleAddLessonCommand);
        }

        public ICommand TakePicture { get; private set; }
        public ICommand LoginCommand { get; private set; }
        public ICommand RegisterCommand { get; private set; }
        public ICommand RetryCommand { get; private set; }
        public ICommand AddSubjectCommand { get; private set; }
        public ICommand AddHomeworkCommand { get; private set; }
        public ICommand AddLessonCommand { get; private set; }
        public ICommand NavigateToSubjectsPage { get; private set; }
        public ICommand NavigateToHomeworksPage { get; private set; }
        public ICommand NavigateToWeekSchedulePage { get; private set; }

        public bool IsOpen
        {
            get
            {
                return this.isOpen;
            }

            set
            {
                this.isOpen = value;
                this.OnPropertyChanged("IsOpen");
            }

        }

        public bool NoInternetFormIsOpen
        {
            get
            {
                return this.noInternetFormIsOpen;
            }

            set
            {
                this.noInternetFormIsOpen = value;
                this.OnPropertyChanged("NoInternetFormIsOpen");
            }

        }

        public string Username
        {
            get
            {
                return this.username;
            }

            set
            {
                this.username = value;
                this.OnPropertyChanged("Username");
            }

        }

        public bool IsGridVisible
        {
            get
            {
                return this.isGridVisible;
            }

            set
            {
                this.isGridVisible = value;
                this.OnPropertyChanged("IsGridVisible");
            }

        }

        public double PopupWidth
        {
            get
            {
                return this.popupWidth;
            }

            set
            {
                this.popupWidth = value;
                this.OnPropertyChanged("PopupWidth");
            }
        }

        public string Message
        {
            get
            {
                return this.message;
            }

            set
            {
                this.message = value;
                this.OnPropertyChanged("Message");
            }
        }

        public bool IsMessageVisible
        {
            get
            {
                return this.isMessageVisible;
            }

            set
            {
                this.isMessageVisible = value;
                this.OnPropertyChanged("IsMessageVisible");
            }
        }

        public bool IsNoLessonsTextVisible
        {
            get
            {
                return this.isNoLessonsTextVisible;
            }

            set
            {
                this.isNoLessonsTextVisible = value;
                this.OnPropertyChanged("IsNoLessonsTextVisible");
            }
        }

        public IEnumerable<LessonModel> CurrentDay
        {
            get
            {
                return this.currentDay;
            }

            set
            {
                this.currentDay = value;
                this.OnPropertyChanged("CurrentDay");
            }
        }

        public IEnumerable<HomeworkModel> NextHomeworks
        {
            get
            {
                return this.nextHomeworks;
            }

            set
            {
                this.nextHomeworks = value;
                this.OnPropertyChanged("NextHomeworks");
            }
        }

        public override void LoadState(object navParameter, Dictionary<string, object> viewModelState)
        {
            if (viewModelState != null)
            {
                if (viewModelState.ContainsKey("accessToken"))
                {
                    App.AccessToken = (string)viewModelState["accessToken"];
                }
                if (viewModelState.ContainsKey("toast"))
                {
                    this.isToastNotificated = (bool)viewModelState["toast"];
                }
            }

            this.dataTransferManager = DataTransferManager.GetForCurrentView();
            this.dataTransferManager.DataRequested +=
                new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(this.OnDataRequested);
        }

        public override void SaveState(Dictionary<string, object> viewModelState)
        {
            viewModelState["accessToken"] = App.AccessToken;
            viewModelState["toast"] = isToastNotificated;

            this.dataTransferManager.DataRequested -=
                new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(this.OnDataRequested);
        }

        public void CheckIsUserLoggedIn()
        {
            if (App.AccessToken == null)
            {
                IsOpen = true;
                IsGridVisible = false;
                PopupWidth = Window.Current.Bounds.Width;
            }
            else
            {
                IsOpen = false;
                IsGridVisible = true;
                this.GetData();
            }
        }

        private async void GetData()
        {
            try
            {
                var allLessons = await DataPersister.GetLessonsPerWeek();
                var allHomeworks = await DataPersister.GetAllHomeworks();
                CurrentDay = allLessons.Where(x => x.Day == DateTime.Today.DayOfWeek.ToString());

                if (CurrentDay.Count() == 0)
                {
                    this.IsNoLessonsTextVisible = true;
                }

                this.NextHomeworks = allHomeworks.Where(h => h.SubmitDate >= DateTime.Today && h.SubmitDate <= DateTime.Today.AddDays(3));
                var homeworksPerDay = allHomeworks.Where(h => h.SubmitDate == DateTime.Today).Count();
                this.LoadToastNotification(homeworksPerDay);
                this.NoInternetFormIsOpen = false;
            }
            catch
            {
                this.IsOpen = false;
                this.IsGridVisible = false;
                this.NoInternetFormIsOpen = true;
                this.PopupWidth = Window.Current.Bounds.Width;
            }
        }

        private async void HandleLoginCommand(object parameter)
        {
            var passwordBox = parameter as PasswordBox;
            var password = passwordBox.Password;

            this.ValidateUsername(this.Username);
            this.ValidatePassword(password);

            var authenticationCode = this.GetAuthenticationCode(this.Username, password);

            var username = await DataPersister.LoginUser(this.Username, authenticationCode);

            StorageManager.Write("username", username);

            if (!string.IsNullOrEmpty(username))
            {
                this.CheckIsUserLoggedIn();
            }
        }

        private void ValidateUsername(string name)
        {
            if (name == null || name.Length < MinUsernameLength || MaxUsernameLength < name.Length)
            {
               this.Message = String.Format("Username must be between {0} and {1} characters",
                                 MinUsernameLength, MaxUsernameLength);
               IsMessageVisible = true;
            }

            Regex r = new Regex("[^A-Za-z0-9._@ ]$");
            if (r.IsMatch(name))
            {
               this.Message = "Username contains invalid characters";
               IsMessageVisible = true;
            }
        }

        private void ValidatePassword(string password)
        {
            if (String.IsNullOrEmpty(password) || password.Length < MinUsernameLength || MaxUsernameLength < password.Length)
            {
                this.Message = String.Format("Password must be between {0} and {1} characters",
                                  MinUsernameLength, MaxUsernameLength);
                IsMessageVisible = true;
            }

            Regex r = new Regex("[^A-Za-z0-9._@ ]$");
            if (r.IsMatch(password))
            {
                this.Message = "Password contains invalid characters";
                IsMessageVisible = true;
            }
        }

        private void HandleRegisterCommand(object parameter)
        {
            var passwordBox = parameter as PasswordBox;
            var password = passwordBox.Password;

            this.ValidateUsername(this.Username);
            this.ValidatePassword(password);

            var authenticationCode = this.GetAuthenticationCode(this.Username, password);

            DataPersister.RegisterUser(this.Username, authenticationCode);

            this.HandleLoginCommand(parameter);
        }

        private void HandleRetryCommand(object parameter)
        {
            this.CheckIsUserLoggedIn();
        }

        private void HandleNavigateToSubjectsPage(object parameter)
        {
            NavigationService.Navigate(ViewType.SubjectsPage);
        }

        private void HandleNavigateToHomeworksPage(object parameter)
        {
            NavigationService.Navigate(ViewType.HomeworksPage);
        }

        private void HandleNavigateToWeekSchedulePage(object parameter)
        {
            NavigationService.Navigate(ViewType.WeekSchedulePage);
        }

        private void HandleAddSubjectCommand(object parameter)
        {
            NavigationService.Navigate(ViewType.AddNewSubjectPage);
        }

        private void HandleAddHomeworkCommand(object parameter)
        {
            NavigationService.Navigate(ViewType.AddNewHomeworkPage);
        }

        private void HandleAddLessonCommand(object parameter)
        {
            NavigationService.Navigate(ViewType.AddLessonPage);
        }

        private string GetAuthenticationCode(string username, string password)
        {
            var str = username + password;
            IBuffer buffer = CryptographicBuffer.ConvertStringToBinary(str, BinaryStringEncoding.Utf8);
            HashAlgorithmProvider hashAlgorithm = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Sha1);
            IBuffer hashBuffer = hashAlgorithm.HashData(buffer);

            var hashHex = CryptographicBuffer.EncodeToHexString(hashBuffer);

            return hashHex;
        }

        private void OnDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            DataRequest request = args.Request;
            request.Data.Properties.ApplicationName = "School Timetable";
            request.Data.Properties.Title = "Daily schedule";
            request.Data.Properties.Description = "My school schedule for today.";
            string textToShare = this.ShareMyDailySchedule();
            string textAsHtml = HtmlFormatHelper.CreateHtmlFormat(textToShare);
            request.Data.SetHtmlFormat(textAsHtml);
        }

        private string ShareMyDailySchedule()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(DateTime.Today.DayOfWeek.ToString() + "</br>");
            foreach (var item in CurrentDay)
            {
                sb.Append("Subject: " + item.Subject + "</br>");
                sb.Append("Room: " + item.LessonRoom + "</br>");
                sb.Append("Begin: " + item.StartTimeString + "</br>");
                sb.Append("End: " + item.EndTimeString + "</br>");
                sb.Append("</br>");
            }

            return sb.ToString();
        }

        private void LoadToastNotification(int homeworksPerDay)
        {
            var username = StorageManager.Read("username");
            if (username != null)
            {
                if (!this.isToastNotificated)
                {
                    ToastNotificator.RaiseBackupToastNotification(username,homeworksPerDay);
                    isToastNotificated = true;
                }
            }
        }

        private async void TakePictureCommand(object parameter)
        {
            var cameraCapture = new CameraCaptureUI();
            cameraCapture.PhotoSettings.CroppedAspectRatio = new Size(4, 3);

            var file = await cameraCapture.CaptureFileAsync(CameraCaptureUIMode.Photo);

            if (file != null)
            {
               this.SavePicture(file);
            }
        }

        private async void SavePicture(StorageFile file)
        {
            byte[] buffer;
            Stream stream = await file.OpenStreamForReadAsync();
            buffer = new byte[stream.Length];
            await stream.ReadAsync(buffer, 0, (int)stream.Length);

            var savePicker = new FileSavePicker
            {
                SuggestedStartLocation = PickerLocationId.PicturesLibrary,
                SuggestedSaveFile = file,
                SuggestedFileName = file.Name
            };

            savePicker.FileTypeChoices.Add("JPEG-Image", new List<string>() { ".jpg" });
            savePicker.FileTypeChoices.Add("PNG-Image", new List<string>() { ".png" });

            var fileForSave = await savePicker.PickSaveFileAsync();
            if (fileForSave != null)
            {
                CachedFileManager.DeferUpdates(fileForSave);
                await FileIO.WriteBytesAsync(fileForSave, buffer);
                CachedFileManager.CompleteUpdatesAsync(fileForSave);
            }
        }
    }
}
