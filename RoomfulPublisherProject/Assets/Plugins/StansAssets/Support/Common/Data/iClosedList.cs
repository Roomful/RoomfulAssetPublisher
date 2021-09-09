using System.Collections.Generic;
using net.roomful.api.sa;

namespace SA.Common.Patterns
{
    public interface iClosedList<T>
    {
        SA_iEvent<T> OnItemAdded { get; }
        SA_iEvent<T> OnItemRemoved { get; }

        IEnumerable<T> Items { get; }
    }
}
