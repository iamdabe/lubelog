using CarCareTracker.External.Interfaces;
using CarCareTracker.Models;
using CarCareTracker.Helper;
using LiteDB;

namespace CarCareTracker.External.Implementations
{
    public class GalleryRecordDataAccess : IGalleryRecordDataAccess
    {
        private ILiteDBHelper _liteDB { get; set; }
        private static string tableName = "galleryrecords";
        public GalleryRecordDataAccess(ILiteDBHelper liteDB)
        {
           _liteDB = liteDB;
        }
        public List<GalleryRecord> GetGalleryRecordsByVehicleId(int vehicleId)
        {
            var db = _liteDB.GetLiteDB();
            var table = db.GetCollection<GalleryRecord>(tableName);
            var GalleryRecords = table.Find(Query.EQ(nameof(GalleryRecord.VehicleId), vehicleId));
            return GalleryRecords.ToList() ?? new List<GalleryRecord>();
        }
        public GalleryRecord GetGalleryRecordById(int GalleryRecordId)
        {
            var db = _liteDB.GetLiteDB();
            var table = db.GetCollection<GalleryRecord>(tableName);
            return table.FindById(GalleryRecordId);
        }
        public bool DeleteGalleryRecordById(int GalleryRecordId)
        {
            var db = _liteDB.GetLiteDB();
            var table = db.GetCollection<GalleryRecord>(tableName);
            table.Delete(GalleryRecordId);
            db.Checkpoint();
            return true;
        }
        public bool SaveGalleryRecordToVehicle(GalleryRecord GalleryRecord)
        {
            var db = _liteDB.GetLiteDB();
            var table = db.GetCollection<GalleryRecord>(tableName);
            table.Upsert(GalleryRecord);
            db.Checkpoint();
            return true;
        }
        public bool DeleteAllGalleryRecordsByVehicleId(int vehicleId)
        {
            var db = _liteDB.GetLiteDB();
            var table = db.GetCollection<GalleryRecord>(tableName);
            var GalleryRecords = table.DeleteMany(Query.EQ(nameof(GalleryRecord.VehicleId), vehicleId));
            db.Checkpoint();
            return true;
        }
    }
}
