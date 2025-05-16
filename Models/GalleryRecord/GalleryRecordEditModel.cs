namespace CarCareTracker.Models
{
    public class GalleryRecordEditModel
    {
        public List<int> RecordIds { get; set; } = new List<int>();
        public GalleryRecord EditRecord { get; set; } = new GalleryRecord();
    }
}
