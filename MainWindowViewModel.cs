using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MongoDB.Bson;
using Realms;
using Realms.Exceptions;

namespace RealmWinUI
{
    public partial class MainWindowViewModel : ObservableObject
    {
        public MainWindowViewModel() 
        {
            var config = new RealmConfiguration(@"d:\temp\cats.realm");

            try
            {
                Realm.DeleteRealm(config);
            }
            catch 
            {
                // ignore
            }

            _localRealm = Realm.GetInstance(config);
           
        }

        private Realm _localRealm;

      
        private IQueryable<Cat> _catsQuery;
        public IQueryable<Cat> CatsQuery
        {
            get => _catsQuery ??= _localRealm.All<Cat>();
            set => SetProperty(ref _catsQuery, value);
        }
   
        public Cat? SelectedCat { get; set; }


        private RelayCommand _removeCommand;
        public RelayCommand RemoveCommand => _removeCommand ??= new RelayCommand(RemoveCat);
        private void RemoveCat()
        {
            if (SelectedCat == null)
                return; 
    
            
            try
            {
                _localRealm.Write(() => {
                    _localRealm.Remove(SelectedCat);

                });
            }
            catch (Exception ex)
            {
                // the selected cat is not removed from the datagrid.
                // In the case of binding to a listbox. The deleted object still shows but it rises exception if selected.
                Debug.WriteLine($@"Error deleting the selected cat. {ex.Message}");
            }
           

        }

        private RelayCommand _populateCommand;
        public RelayCommand PopulateCommand => _populateCommand ??= new RelayCommand(Populate);
        private void Populate()
        {
            Cat ninja = new() { Name = "Ninja", Age = 1, Breed = "Angora" };
            Cat nounou = new() { Name = "Nounou", Age = 2, Breed = "Siamese" };
            Cat leila = new() { Name = "Leila", Age = 3, Breed = "Local" };

            try
            {
                _localRealm.Write(() => _localRealm.Add(nounou));
                _localRealm.Write(() => _localRealm.Add(ninja));
                _localRealm.Write(() => _localRealm.Add(leila));
            }
            catch (RealmFileAccessErrorException ex)
            {
                Debug.WriteLine($@"Error writeing to the realm file. {ex.Message}");
            }
        }

    }
}
