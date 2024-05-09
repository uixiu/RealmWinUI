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

            _localRealm = Realm.GetInstance(config);

            CatsQuery = _localRealm.All<Cat>(); //.AsQueryable();
        }

        private Realm _localRealm;

        [ObservableProperty] 
        private IQueryable<Cat> _catsQuery;
        
        public Cat? SelectedCat { get; set; }

        [RelayCommand]
        private void RemoveCat()
        {
            if (SelectedCat == null)
                return;

            _localRealm.Write(() => { _localRealm.Remove(SelectedCat); });
        }

        [RelayCommand]
        public void Populate()
        {
            Cat ninja = new() { Name = "Ninja", Age = 1, Breed = "Angora" };
            Cat nounou = new() { Name = "Nounou", Age = 2, Breed = "Siamese" };

          
            _localRealm.Write(() => _localRealm.Add(nounou));
            _localRealm.Write(() => _localRealm.Add(ninja));
        }
    }
}
