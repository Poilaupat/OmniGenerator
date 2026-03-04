using OmniGenerator.Lib.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Lib.Interfaces
{
    public interface INotifier<TNotificationData> where TNotificationData : notnull
    {
        Notifier<TNotificationData> Notifier { get; }
    }
}
