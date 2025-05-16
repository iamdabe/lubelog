using CarCareTracker.Models;

namespace CarCareTracker.External.Interfaces
{
    public interface IGalleryRecordDataAccess
    {
        public List<GalleryRecord> GetGalleryRecordsByVehicleId(int vehicleId);
        public GalleryRecord GetGalleryRecordById(int GalleryRecordId);
        public bool DeleteGalleryRecordById(int GalleryRecordId);
        public bool SaveGalleryRecordToVehicle(GalleryRecord GalleryRecord);
        public bool DeleteAllGalleryRecordsByVehicleId(int vehicleId);
    }
}
