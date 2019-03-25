$(document).ready(function () {
    //Upload dữ liệu vào Ebook
    $("#upload").click(function () {
        var files = $("#fileInput").get(0).files;
        var fileData = new FormData();

        for (var i = 0; i < files.length; i++) {
            fileData.append("fileInput", files[i]);
        }
        //cái này là lấy thông tin trên text box  title là key tiltle.val() là giá trị
        fileData.append("title", $("#title").val());
        fileData.append("describe", $("#describe").val());
        fileData.append("author", $("#author").val());
        fileData.append("year", $("#year").val());
        fileData.append("userid", $("#userid").text());
        fileData.append("subid", $("#subject option:selected").val());

        console.log(fileData);
        $.ajax({
            type: "POST",
            url: "http://localhost:52385/api/FileAPI/UploadFiles",
            dataType: "json",
            contentType: false, // Not to set any content header
            processData: false, // Not to process data
            data: fileData,
            success: function (result) {
                alert(result);
            }
        });

    });


});
//lấy môn học của ebook
$(document).ready(function () {
    //Ẩn hiện form
    $("#btn_upload").hide();
    $("#loadEbook").hide();

    $.ajax({
        type: "GET",
        url: "/api/FileAPI/GetSubjectEbook",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            $.each(result, function (i, v) {
                //alert(v.name);
                //load các môn của ebook lên thanh danh mục bên trái
                $("#loadSubjectOfEbook").append("<li id='ebook_" + v.id + "'><a>" + v.name + "</a></li >");
                //load môn học lên combobox trong form upload file

                //
                //khi click vào môn học sẽ load các file trong môn đó
                $("#ebook_" + v.id).click(function () {
                    //Ẩn hiện form upload file ở trang chỉ hay uplaod file trong môn học
                    $("#btn_upload").show();
                    $("#loadEbook").show();
                    $("#subject").append("<option value='" + v.id + "'>" + v.name + "</option>");
                    $("#loadEbook").empty();
                    $.ajax({
                        type: "GET",
                        url: "/api/FileAPI/GetEbook",
                        data: { id: v.id },
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (result) {

                            $.each(result, function (i, v) {


                                var data = "<div class='col-lg-4 col-md-6 mb-4' data-bind='attr: { id: id }'>";
                                data += "<div class='card h-100'>";
                                data += "<a href='#'><img class='card-img-top' src='http://placehold.it/700x400' alt=''></a>";
                                data += "<div class='card-body'>";
                                data += "<h4 class='card-title'>";
                                data += "<a href='#' data-bind='text:title'>" + v.title + "</a> </h4>";
                                data += "<h5>tác giả: " + v.author + "</h5>";
                                data += "<p class='card-text' data-bind='text:author'></p>";
                                data += "<div class='card-footer'></div>";
                                data += "<small class='text-muted'>&#9733; &#9733; &#9733; &#9733; &#9733;</small>";
                                data += " </div></div></div>";

                                $("#loadEbook").append(data);
                            });
                        }
                    });
                });
            });
        }
    });

    $("#create_sub_ebook").click(function () {
        var subject = {};
        subject.name = $("#nameSujectEbook").val();
        $.ajax({
            type: "POST",
            url: "/api/FileAPI/CreateSubject",
            data: JSON.stringify(subject),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                //$("#loadSubjectOfEbook").append("<li id='" + result.id + "'><a>" + result.name + "</a></li >");
                alert(result);

                $("#nameSujectEbook").empty();
            }
        });
    });
});
//SIGNALR
$(function () {
    $.connection.hub.logging = true;
    var chat = $.connection.myHub;

    //SignalR thêm môn học vào Ebook
    chat.client.postToClient = function (id, name) {
        $("#loadSubjectOfEbook").append("<li id='" + id + "'><a>" + name + "</a></li >");
    };
    //SignalR thêm file
    chat.client.postFileEbookToClient = function (id, title, author, describe, filename,
        date_upload) {
        var data = "<div class='col-lg-4 col-md-6 mb-4' data-bind='attr: { id: id }'>";
        data += "<div class='card h-100'>";
        data += "<a href='#'><img class='card-img-top' src='http://placehold.it/700x400' alt=''></a>";
        data += "<div class='card-body'>";
        data += "<h4 class='card-title'>";
        data += "<a href='#' data-bind='text:title'>" + title + "</a> </h4>";
        data += "<h5>tác giả: " + author + "</h5>";
        data += "<p class='card-text' data-bind='text:author'></p>";
        data += "<div class='card-footer'></div>";
        data += "<small class='text-muted'>&#9733; &#9733; &#9733; &#9733; &#9733;</small>";
        data += " </div></div></div>";

        $("#loadEbook").append(data);
    };
    $.connection.hub.start().done(function () {
        //alert("started");
    }).fail(function (result) {
        alert(result);
    });

});  
