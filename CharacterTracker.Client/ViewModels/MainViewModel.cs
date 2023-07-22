using CharacterTracker.Client.AddonDataParser.Models;
using DynamicData;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Linq;

namespace CharacterTracker.Client.ViewModels;

public class MainViewModel : ViewModelBase
{
    private ObservableCollection<ESOCharacter> _characters = new();
    public ObservableCollection<ESOCharacter> Characters
    {
        get => _characters;
        set => this.RaiseAndSetIfChanged(ref _characters, value);
    }

    private ObservableCollection<ESOAccount> _accounts = new();
    public ObservableCollection<ESOAccount> Accounts
    {
        get => _accounts;
        set => this.RaiseAndSetIfChanged(ref _accounts, value);
    }

    private ESOAccount _selectedAccount = null!;
    public ESOAccount SelectedAccount
    {
        get => _selectedAccount;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedAccount, value);
            
            Characters.Clear();
            Characters.AddRange(value.Characters);
        }
    }

    public MainViewModel()
    {
        var data = AddonDataParser.AddonDataParser.ParseAddonData();
        if (data == null)
            return;

        Accounts.AddRange(data);
        if (_accounts.Count > 0)
            SelectedAccount = Accounts.First();
    }
}
