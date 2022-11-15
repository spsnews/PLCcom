using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCcom.ViewModel
{
    public partial class OneViewModel : ObservableObject
    {

        [RelayCommand]
        Task BackDetail() => Shell.Current.GoToAsync("../SetupPage");

    }
}
