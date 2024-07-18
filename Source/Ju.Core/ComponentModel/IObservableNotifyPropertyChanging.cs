using System.ComponentModel;

namespace Ju.ComponentModel;

public interface IObservableNotifyPropertyChanging : INotifyPropertyChanging
{
    new IObservable<PropertyChangingEventArgs> PropertyChanging { get; }
}
