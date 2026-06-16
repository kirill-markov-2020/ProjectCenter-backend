using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCenter.Core.Entities
{
    public class TypeNotification
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}
