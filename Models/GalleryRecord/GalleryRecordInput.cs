namespace CarCareTracker.Models
{
    public class GalleryRecordInput
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public string Name { get; set; }
        public string Date { get; set; } = DateTime.Now.ToShortDateString();
        public string Notes { get; set; }
        public List<UploadedFiles> Files { get; set; } = new List<UploadedFiles>();
        public GalleryRecord ToGalleryRecord() { return new GalleryRecord { Id = Id, VehicleId = VehicleId, Name = Name, Date = DateTime.Parse(Date), Notes = Notes, Files = Files }; }
    }
}
