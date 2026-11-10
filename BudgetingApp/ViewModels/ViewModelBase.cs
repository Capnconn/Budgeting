using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BudgetingApp.ViewModels;

public class ViewModelBase : ObservableObject, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected string GetAssemblyResource(string name)
    {
        using var stream = AssetLoader.Open(new Uri(name));
        using StreamReader reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    protected bool RaiseAndSetIfChanged<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
        {
            return false;
        }

        field = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        return true;
    }

    protected void RaisePropertyChanged(string propName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}
