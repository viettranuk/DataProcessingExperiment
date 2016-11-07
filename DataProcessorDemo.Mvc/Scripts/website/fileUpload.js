(function (FileUploadDemo, $, undefined) {

    "use strict";

    FileUploadDemo.prototype = {

        handleFileUpload: function () {

            $("#uploadResult").hide();
            $("#progressIndicator").hide();

            var filesList = [],
                paramNames = [],
                acceptFileTypes = /(\.|\/)(text\/csv|application\/vnd\.ms-excel)$/i,
                uploader = $("form").fileupload({

                url: DataProcessorDemo.getRootUrl() + "/Home/Upload",
                autoUpload: false,
                fileInput: $("input:file")

            }).on("fileuploadadd", function (e, data) {

                var selectedElement = "#" + e.delegatedEvent.target.id,
                    nameInputElement = $(selectedElement).parents(".file-input").find(".file-name").first(),
                    selectedFileName = data.originalFiles[0]["name"],
                    selectedFileType = data.originalFiles[0]["type"];

                if (!selectedFileName.toLowerCase().endsWith(".csv") &&
                    selectedFileType.length && !acceptFileTypes.test(selectedFileType)) {

                    $("#fileUpload").prop("disabled", true);

                    $(nameInputElement).val("Error: file type not supported. Only .csv files allowed. Please try again");

                } else {

                    while (filesList.length > 0) { filesList.pop(); }

                    filesList.push(data.originalFiles[0]);
                    paramNames.push(e.delegatedEvent.target.name);
                    $(nameInputElement).val(selectedFileName);

                    $("#fileUpload").prop("disabled", false);
                }                

            }).on("fileuploadfail", function (e, data) {

                $("#uploadResult").html("");
                $("#progressIndicator").hide();
                $("#uploadResult").removeClass();
                $("#uploadResult").addClass("text-danger");
                $("#uploadResult").html("&#9658; Sorry, there was an error whilst processing your request. Please try again later");
                $("#uploadResult").show();

                $("#fileUpload").prop("disabled", true);
                $("#fileSelector").prop("disabled", false);

            }).on("fileuploaddone", function (e, data) {

                $("#uploadResult").html("");
                $("#progressIndicator").hide();
                $("#allFileNames").val("");
                $("#uploadResult").removeClass();
                $("#uploadResult").addClass("text-success");
                $("#uploadResult").html("&#9658; File has been uploaded");
                $("#uploadResult").show().delay(5000).fadeOut();

                $("#fileUpload").prop("disabled", true);
                $("#fileSelector").prop("disabled", false);
            });

            $("#fileUpload").click(function (e) {

                e.preventDefault();
                e.stopImmediatePropagation();

                $("#uploadResult").hide();
                $("#progressIndicator").show();

                if (filesList.length == 0) {

                    $("#uploadResult").html("");
                    $("#progressIndicator").hide();
                    $("#uploadResult").removeClass();
                    $("#uploadResult").addClass("text-danger");
                    $("#uploadResult").html("&#9658; Please select a file before proceeding");
                    $("#uploadResult").show();

                    return false;
                }

                $("#fileUpload").prop("disabled", true);
                $("#fileSelector").prop("disabled", true);

                $(uploader).fileupload("send", { files: filesList, paramName: paramNames });
            })
        }
    };

}(window.FileUploadDemo = window.FileUploadDemo || {}, jQuery));