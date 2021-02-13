namespace P01_HospitalDatabase.Data.Models
{
    public class PatientMedicament
    {
        //mapping class between Patients and Medicaments

        public int PatientId { get; set; }
        public Patient Patient { get; set; }


        public int MedicamentId { get; set; }
        public Medicament Medicament { get; set; }

    }
}