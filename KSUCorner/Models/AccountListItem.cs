using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KSUCorner.Models
{
    public class AccountListItem
    {
        public int AccountID { get; set; }

        public string UserName { get; set; }

        public string Info { get; set; }

        public bool EmailConfirmed { get; set; }

        public bool IsActivated { get; set; }

        public bool IsLockedOut { get; set; }

        public string LastLockedOutPeriod { get; set; }

        public string LastLockedOutReason { get; set; }

        public string Restrictions { get; set; }
    }
}