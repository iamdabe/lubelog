function showAddGalleryRecordModal() {
    $.get('/Vehicle/GetAddGalleryRecordPartialView', function (data) {
        if (data) {
            $("#galleryRecordModalContent").html(data);
            //initiate datepicker
            initDatePicker($('#galleryRecordDate'));
            initTagSelector($("#galleryRecordTag"));
            $('#galleryRecordModal').modal('show');
        }
    });
}
function showEditGalleryRecordModal(galleryRecordId, nocache) {
    if (!nocache) {
        var existingContent = $("#galleryRecordModalContent").html();
        if (existingContent.trim() != '') {
            //check if id is same.
            var existingId = getGalleryRecordModelData().id;
            if (existingId == galleryRecordId && $('[data-changed=true]').length > 0) {
                $('#galleryRecordModal').modal('show');
                $('.cached-banner').show();
                return;
            }
        }
    }
    $.get(`/Vehicle/GetGalleryRecordForEditById?galleryRecordId=${galleryRecordId}`, function (data) {
        if (data) {
            $("#galleryRecordModalContent").html(data);
            //initiate datepicker
            initDatePicker($('#galleryRecordDate'));
            initTagSelector($("#galleryRecordTag"));
            $('#galleryRecordModal').modal('show');
            bindModalInputChanges('galleryRecordModal');
            $('#galleryRecordModal').off('shown.bs.modal').on('shown.bs.modal', function () {
                if (getGlobalConfig().useMarkDown) {
                    toggleMarkDownOverlay("galleryRecordNotes");
                }
            });
        }
    });
}
function hideAddGalleryRecordModal() {
    $('#galleryRecordModal').modal('hide');
}
function deleteGalleryRecord(galleryRecordId) {
    $("#workAroundInput").show();
    Swal.fire({
        title: "Confirm Deletion?",
        text: "Deleted gallery photos cannot be restored.",
        showCancelButton: true,
        confirmButtonText: "Delete",
        confirmButtonColor: "#dc3545"
    }).then((result) => {
        if (result.isConfirmed) {
            $.post(`/Vehicle/DeleteGalleryRecordById?galleryRecordId=${galleryRecordId}`, function (data) {
                if (data) {
                    hideAddGalleryRecordModal();
                    successToast("Gallery Photo Deleted");
                    var vehicleId = GetVehicleId().vehicleId;
                    getVehicleGalleryRecords(vehicleId);
                } else {
                    errorToast(genericErrorMessage());
                }
            });
        } else {
            $("#workAroundInput").hide();
        }
    });
}
function saveGalleryRecordToVehicle(isEdit) {
    //get values
    var formValues = getAndValidateGalleryRecordValues();
    //validate
    if (formValues.hasError) {
        errorToast("Please check the form data");
        return;
    }
    //save to db.
    $.post('/Vehicle/SaveGalleryRecordToVehicleId', { galleryRecord: formValues }, function (data) {
        if (data) {
            successToast(isEdit ? "Gallery Photo Updated" : "Gallery Photo Added.");
            hideAddGalleryRecordModal();
            saveScrollPosition();
            getVehicleGalleryRecords(formValues.vehicleId);
            if (formValues.addReminderRecord) {
                setTimeout(function () { showAddReminderModal(formValues); }, 500);
            }
        } else {
            errorToast(genericErrorMessage());
        }
    })
}
function getAndValidateGalleryRecordValues() {
    var galleryRecordName = $("#galleryRecordName").val();
    var galleryRecordDate = $("#galleryRecordDate").val();
    var galleryRecordNotes = $("#galleryRecordNotes").val();
    var vehicleId = GetVehicleId().vehicleId;
    var galleryRecordId = getGalleryRecordModelData().id;

    //validation
    var hasError = false;


    if (galleryRecordDate.trim() == '') { //eliminates whitespace.
        hasError = true;
        $("#galleryRecordDate").addClass("is-invalid");
    } else {
        $("#galleryRecordDate").removeClass("is-invalid");
    }
    return {
        id: galleryRecordId,
        hasError: hasError,
        vehicleId: vehicleId,
        name: galleryRecordName,
        date: galleryRecordDate,
        notes: galleryRecordNotes,
        files: uploadedFiles
    }
}