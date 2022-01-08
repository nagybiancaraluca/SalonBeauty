namespace AutoLotModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Appointment")]
    public partial class Appointment
    {
        [Key]
        public int appId { get; set; }

        public DateTime? Date { get; set; }

        public TimeSpan? Hour { get; set; }

        public int? custId { get; set; }

        public int? servId { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual Service Service { get; set; }
    }
}
