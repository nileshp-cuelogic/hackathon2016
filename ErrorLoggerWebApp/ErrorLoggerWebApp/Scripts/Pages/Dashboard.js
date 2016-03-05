/// <reference path="../jquery-1.10.2.min.js" />
$(document).ready(function () {
    $("#applicationList").change(function () {
        var applicationId = $(this).val();

        var select = $("#moduleList");
        if (!applicationId) {
            select.empty();
            select.append($('<option/>', {
                value: '',
                text: "Select Module"
            }));
            return;
        }
        $.getJSON("LoadApplicationModules", { applicationId: applicationId },
               function (data) {
                   select.empty();
                   select.append($('<option/>', {
                       value: '',
                       text: "Select Module"
                   }));
                   $.each(data, function (index, itemData) {
                       select.append($('<option/>', {
                           value: itemData.Value,
                           text: itemData.Text
                       }));
                   });
               });
    });

    $("#btnSubmit").click(function () {
        var applicationId = $("#applicationList").val();
        var moduleName = $("#moduleList").val();
        if (!applicationId) {
            applicationId = 0;
        }
        if (!moduleName)
            moduleName = null;

        $.getJSON("Dashboard/LoadErrorSummary", { applicationId: applicationId, moduleName: moduleName },
               function (data) {
                   console.log(data);
               });
    });


});