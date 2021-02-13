using System;
using System.ComponentModel.DataAnnotations;

namespace P01_HospitalDatabase.Data.Models
{
    public class Visitation
    {
    //    	VisitationId
    //	Date
    //	Comments(up to 250 characters, unicode)
    //	Patient
    
        public int VisitationId { get; set; }

        public DateTime Date { get; set; }

        public string Comments { get; set; }

        public int PatientId { get; set; }
        public Patient Patient { get; set; }

        public int? DoctorId { get; set; }
        public Doctor Doctor { get; set; }




    }
}