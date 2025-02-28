using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfCore
{
    public enum NotificationTypes
    {
        TaskAssigned,
        PRApproved,
        PRRejected,
        PRCommented,
        ScrumMeetingScheduled
    }
}
