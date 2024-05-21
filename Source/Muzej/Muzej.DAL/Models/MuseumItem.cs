using System;
using System.Collections.Generic;

namespace Muzej.DAL.Models
{
    public partial class MuseumItem
    {
        public int MuseumItemId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int? RoomId { get; set; }
        public string? Period { get; set; }
        public string? MultimediaContentType { get; set; }

        public virtual Room? Room { get; set; }
    }
}
