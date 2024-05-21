using System;
using System.Collections.Generic;

namespace Muzej.DAL.Models
{
    public partial class Room
    {
        public Room()
        {
            MuseumItems = new HashSet<MuseumItem>();
        }

        public int RoomId { get; set; }
        public string Name { get; set; } = null!;
        public byte Floor { get; set; }
        public string? Description { get; set; }

        public virtual ICollection<MuseumItem> MuseumItems { get; set; }
    }
}
