using System.Collections.Generic;
using System.Collections.ObjectModel;
using Assignment.Connection;
using System.Linq;
using Assignment.Model; // Import the UserDisplayModel namespace
using System;

namespace Assignment.ViewModels
{
    public class UserListViewModel : ViewModelBase
    {
        private ObservableCollection<UserDisplayModel> _data;
        public ObservableCollection<UserDisplayModel> Data
        {
            get { return _data; }
            set
            {
                _data = value;
                OnPropertyChanged(nameof(Data));
            }
        }

        private ObservableCollection<UserDisplayModel> _filteredData;
        public ObservableCollection<UserDisplayModel> FilteredData
        {
            get { return _filteredData; }
            set
            {
                _filteredData = value;
                OnPropertyChanged(nameof(FilteredData));
            }
        }

        private DatabaseConnection _dbConnection;

        public UserListViewModel()
        {
            Data = new ObservableCollection<UserDisplayModel>();
            FilteredData = new ObservableCollection<UserDisplayModel>();
            _dbConnection = new DatabaseConnection(); // Initialize the database connection
            LoadUserData();
        }

        // Method to load user data from the database
        private void LoadUserData()
        {
            List<UserDisplayModel> usersFromDb = _dbConnection.GetUserDetails();

            foreach (var user in usersFromDb)
            {
                Data.Add(user);
                FilteredData.Add(user); // Initially, filtered data is the same as Data
            }
        }

        // Search query property
        private string _searchQuery;
        public string SearchQuery
        {
            get { return _searchQuery; }
            set
            {
                _searchQuery = value;
                OnPropertyChanged(nameof(SearchQuery));
                FilterUsers(); // Automatically filter users when search query changes
            }
        }

        // Search method to filter the list based on the search query
        private void FilterUsers()
        {
            if (string.IsNullOrWhiteSpace(SearchQuery))
            {
                // If search query is empty, show all users
                FilteredData = new ObservableCollection<UserDisplayModel>(Data);
            }
            else
            {
                // Make sure the search query is not null
                string query = SearchQuery.ToLower();

                // Filter users based on FirstName, LastName, or Email
                FilteredData = new ObservableCollection<UserDisplayModel>(
                    Data.Where(u =>
                        (!string.IsNullOrEmpty(u.FirstName) && u.FirstName.ToLower().Contains(query)) ||
                        (!string.IsNullOrEmpty(u.LastName) && u.LastName.ToLower().Contains(query)) ||
                        (!string.IsNullOrEmpty(u.Email) && u.Email.ToLower().Contains(query))
                    )
                );
            }
        }

    }

}




