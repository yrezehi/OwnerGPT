using OwnerGPT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwnerGPT.Core.Services
{
    public class NotificationCenter
    {

        public NotificationCenter() { }

        public Notification GetUnreadNotificationsFor(int userId)
        {
            throw new NotImplementedException();
        }

        public void PushNotificationToAll()
        {
            throw new NotImplementedException();
        }

        public void PushNotificationTo(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
