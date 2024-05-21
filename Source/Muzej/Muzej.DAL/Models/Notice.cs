using System;
using System.Collections.Generic;

namespace Muzej.DAL.Models
{
    public partial class Notice
    {
        public int NoticeId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDateTime { get; set; }
        public string NoticeText { get; set; } = null!;
    }
}
