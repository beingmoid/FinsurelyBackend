using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramaBackend.Data.CatalogDb
{
    public class Enums
    {
        public enum SubscriptionStatus
        {
            Active = 1,
            Canceled,
            PastDue,
            Expired
        }

        public enum DocumentCategoryType
        {
            Personal = 1,
            Client
        }


        public enum NotificationType
        {
            Alert = 1,
            Event,
            Action
        }
    }
}
