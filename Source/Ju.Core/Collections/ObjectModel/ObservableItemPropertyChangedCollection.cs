using Ju.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace Ju.Collections.ObjectModel;

public sealed class ObservableItemPropertyChangedCollection<T> : ObservableCollection<T>, IDisposable
    where T : IObservableNotifyPropertyChanged
{

    public ObservableItemPropertyChangedCollection()
    {
        System.Diagnostics.Debug.WriteLine($"Create {this}.");
    }


    public new void Add(T item)
    {
        subscribings.Add(item, item.PropertyChanged.Subscribe(WhenItemPropertyChanged).AddTo(disposables));
        base.Add(item);
    }

    public void AddRange(IEnumerable<T> collection)
    {
        foreach (var item in collection)
        {
            Add(item);
        }
    }

    public void ReAddRange(IEnumerable<T> collection)
    {
        Clear();
        AddRange(collection);
    }

    public new bool Remove(T item)
    {
        subscribings[item]?.Dispose();
        subscribings.Remove(item);
        return base.Remove(item);
    }

    public new void Clear()
    {
        base.Clear();
        subscribings.Clear();
        disposables.Clear();
    }

    #region ==== CollectionChanged ====

    public new IObservable<NotifyCollectionChangedEventArgs> CollectionChanged =>
        Observable
            .FromEventPattern<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>(
                handler => base.CollectionChanged += handler,
                handler => base.CollectionChanged -= handler)
            .Select(x => x.EventArgs);

    #endregion

    #region ==== ItemPropertyChanged ====

    private readonly Dictionary<T, IDisposable> subscribings = [];

    private event PropertyChangedEventHandler? PrivateItemPropertyChanged;

    private void WhenItemPropertyChanged(PropertyChangedEventArgs e) =>
        PrivateItemPropertyChanged?.Invoke(this, e);

    public IObservable<PropertyChangedEventArgs> ItemPropertyChanged =>
        Observable
            .FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                handler => PrivateItemPropertyChanged += handler,
                handler => PrivateItemPropertyChanged -= handler)
            .Select(x => x.EventArgs);

    #endregion

    #region ==== Implementation of IDisposable ====

    private bool disposed = false;

    private readonly CompositeDisposable disposables = [];

    private void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                disposables.Dispose();
            }
            disposed = true;
        }
    }

    public void Dispose()
    {
        System.Diagnostics.Debug.WriteLine($"Dispose {this}.");
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    #endregion

}
