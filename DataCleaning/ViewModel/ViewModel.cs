using DataCleaning_Preprocessing;
using DataCleaning_Preprocessing.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace DataCleaning_Preprocessing
{
    public class ViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<WebsiteTrafficData> RawData { get; set; }
        
        private ObservableCollection<WebsiteTrafficData> cleanData;
        public ObservableCollection<WebsiteTrafficData> CleanedData
        {
            get { return cleanData; }
            set
            {
                cleanData = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CleanedData"));
            }
        }

        private bool isBusy;
        public bool IsBusy
        {
            get
            {
                return isBusy;
            }

            set
            {
                isBusy = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsBusy"));
            }
        }

        public ViewModel()
        {
            IsBusy = false;

            RawData = new ObservableCollection<WebsiteTrafficData>()
            {
                new WebsiteTrafficData{ DateTime = new DateTime(2024, 07, 01, 00, 00, 00), Visitors = 150 },
                new WebsiteTrafficData{ DateTime = new DateTime(2024, 07, 01, 01, 00, 00), Visitors = 160 },
                new WebsiteTrafficData{ DateTime = new DateTime(2024, 07, 01, 02, 00, 00), Visitors = 155 },
                new WebsiteTrafficData{ DateTime = new DateTime(2024, 07, 01, 03, 00, 00), Visitors = double.NaN }, // Missing data
                new WebsiteTrafficData{ DateTime = new DateTime(2024, 07, 01, 04, 00, 00), Visitors = 170 },
                new WebsiteTrafficData{ DateTime = new DateTime(2024, 07, 01, 05, 00, 00), Visitors = 175 },
                new WebsiteTrafficData{ DateTime = new DateTime(2024, 07, 01, 06, 00, 00), Visitors = 145 }, // Missing data
                new WebsiteTrafficData{ DateTime = new DateTime(2024, 07, 01, 07, 00, 00), Visitors = 180 },
                new WebsiteTrafficData{ DateTime = new DateTime(2024, 07, 01, 08, 00, 00), Visitors = 190 },
                new WebsiteTrafficData{ DateTime = new DateTime(2024, 07, 01, 09, 00, 00), Visitors = 185 },
                new WebsiteTrafficData{ DateTime = new DateTime(2024, 07, 01, 10, 00, 00), Visitors = 200 },
                new WebsiteTrafficData{ DateTime = new DateTime(2024, 07, 01, 11, 00, 00), Visitors = double.NaN }, // Missing data
                new WebsiteTrafficData{ DateTime = new DateTime(2024, 07, 01, 12, 00, 00), Visitors = 220 },
                new WebsiteTrafficData{ DateTime = new DateTime(2024, 07, 01, 13, 00, 00), Visitors = 230 },
                new WebsiteTrafficData{ DateTime = new DateTime(2024, 07, 01, 14, 00, 00), Visitors = double.NaN }, // Missing data
                new WebsiteTrafficData{ DateTime = new DateTime(2024, 07, 01, 15, 00, 00), Visitors = 250 },
                new WebsiteTrafficData{ DateTime = new DateTime(2024, 07, 01, 16, 00, 00), Visitors = 260 },
                new WebsiteTrafficData{ DateTime = new DateTime(2024, 07, 01, 17, 00, 00), Visitors = 270 },
                new WebsiteTrafficData{ DateTime = new DateTime(2024, 07, 01, 18, 00, 00), Visitors = double.NaN }, // Missing data
                new WebsiteTrafficData{ DateTime = new DateTime(2024, 07, 01, 19, 00, 00), Visitors = 280 },
                new WebsiteTrafficData{ DateTime = new DateTime(2024, 07, 01, 20, 00, 00), Visitors = 290 },
                new WebsiteTrafficData{ DateTime = new DateTime(2024, 07, 01, 21, 00, 00), Visitors = 300 },
                new WebsiteTrafficData{ DateTime = new DateTime(2024, 07, 01, 22, 00, 00), Visitors = double.NaN }, // Missing data
                new WebsiteTrafficData{ DateTime = new DateTime(2024, 07, 01, 23, 00, 00), Visitors = 320 },
            };

            CleanedData = new ObservableCollection<WebsiteTrafficData>();
            //_ = LoadCleanedDataAsync();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        internal async Task LoadCleanedDataAsync()
        {
            var service = new AzureOpenAIService("Your_AI_Key");
            CleanedData = await service.GetCleanedData(RawData);
            IsBusy = false;
        }
    }
}