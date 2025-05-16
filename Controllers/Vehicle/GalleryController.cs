using CarCareTracker.Filter;
using CarCareTracker.Helper;
using CarCareTracker.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarCareTracker.Controllers
{
    public partial class VehicleController
    {

        [TypeFilter(typeof(CollaboratorFilter))]
        [HttpGet]
        public IActionResult GetGalleryRecordsByVehicleId(int vehicleId)
        {
            var result = _galleryRecordDataAccess.GetGalleryRecordsByVehicleId(vehicleId);

            bool _useDescending = _config.GetUserConfig(User).UseDescending;
            if (_useDescending)
            {
                result = result.OrderByDescending(x => x.Date).ThenByDescending(x => x.Name).ToList();
            }
            else
            {
                result = result.OrderBy(x => x.Date).ThenBy(x => x.Name).ToList();
            }
            return PartialView("_GalleryRecords", result);
        }
        [HttpPost]
        public IActionResult SaveGalleryRecordToVehicleId(GalleryRecordInput galleryRecord)
        {
            //move files from temp.
            galleryRecord.Files = galleryRecord.Files.Select(x => { return new UploadedFiles { Name = x.Name, Location = _fileHelper.MoveFileFromTemp(x.Location, "gallery/") }; }).ToList();
            var result = _galleryRecordDataAccess.SaveGalleryRecordToVehicle(galleryRecord.ToGalleryRecord());
            if (result)
            {
                StaticHelper.NotifyAsync(_config.GetWebHookUrl(), galleryRecord.VehicleId, User.Identity.Name, $"{(galleryRecord.Id == default ? "Created" : "Edited")} Gallery Record");
            }
            return Json(result);
        }
        [HttpGet]
        public IActionResult GetAddGalleryRecordPartialView(int vehicleId)
        {
            return PartialView("_GalleryRecordModal", new GalleryRecordInput() { });
        }
        [HttpGet]
        public IActionResult GetGalleryRecordForEditById(int galleryRecordId)
        {
            var result = _galleryRecordDataAccess.GetGalleryRecordById(galleryRecordId);
            //convert to Input object.
            var convertedResult = new GalleryRecordInput
            {
                Id = result.Id,
                Date = result.Date.ToShortDateString(),
                Name = result.Name,
                Notes = result.Notes,
                VehicleId = result.VehicleId,
                Files = result.Files
            };
            return PartialView("_GalleryRecordModal", convertedResult);
        }
        private bool DeleteGalleryRecordWithChecks(int galleryRecordId)
        {
            var existingRecord = _galleryRecordDataAccess.GetGalleryRecordById(galleryRecordId);
            //security check.
            if (!_userLogic.UserCanEditVehicle(GetUserID(), existingRecord.VehicleId))
            {
                return false;
            }
            var result = _galleryRecordDataAccess.DeleteGalleryRecordById(existingRecord.Id);
            return result;
        }
        [HttpPost]
        public IActionResult DeleteGalleryRecordById(int galleryRecordId)
        {
            var result = DeleteGalleryRecordWithChecks(galleryRecordId);
            if (result)
            {
                StaticHelper.NotifyAsync(_config.GetWebHookUrl(), 0, User.Identity.Name, $"Deleted Gallery Record - Id: {galleryRecordId}");
            }
            return Json(result);
        }
    }
}