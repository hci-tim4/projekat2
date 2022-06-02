using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace railway.model
{
    public class DrivingLine
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        [ForeignKey("Train")]
        public int TrainId { get; set; }
        public virtual Train Train { get; set; }

        public bool deleted { get; set; }

        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }

    }
}
