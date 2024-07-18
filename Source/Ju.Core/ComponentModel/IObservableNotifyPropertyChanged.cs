using System.ComponentModel;

namespace Ju.ComponentModel;

public interface IObservableNotifyPropertyChanged : INotifyPropertyChanged
{
    new IObservable<PropertyChangedEventArgs> PropertyChanged { get; }
}
