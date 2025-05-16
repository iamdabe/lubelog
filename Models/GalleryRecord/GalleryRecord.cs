namespace CarCareTracker.Models
{
    public class GalleryRecord
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Notes { get; set; }
        public List<UploadedFiles> Files { get; set; } = new List<UploadedFiles>();
    }
}
