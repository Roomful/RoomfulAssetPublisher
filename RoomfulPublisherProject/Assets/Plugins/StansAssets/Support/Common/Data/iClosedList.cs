using System.Collections.Generic;
using SA.Foundation.Events;

namespace SA.Common.Patterns
{
    public interface iClosedList<T>
    {
        SA_iEvent<T> OnItemAdded { get; }
        SA_iEvent<T> OnItemRemoved { get; }

        IEnumerable<T> Items { get; }
    }
}
