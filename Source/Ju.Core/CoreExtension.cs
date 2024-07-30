using System.Collections.ObjectModel;

namespace Ju;

public static class CoreExtension
{

    public static T AddTo<T>(this T self, ICollection<IDisposable> container)
        where T : IDisposable
    {
        container.Add(self);
        return self;
    }

    public static void AddRange<T>(this ObservableCollection<T> self, IEnumerable<T> collection)
    {
        foreach (var item in collection)
        {
            self.Add(item);
        }
    }

    public static void ReAddRange<T>(this ObservableCollection<T> self, IEnumerable<T> collection)
    {
        self.Clear();
        self.AddRange(collection);
    }

}
