
function clearFormFile() {
    $("input[class='title']").val("");
    $("input[class='describe']").val("");
    $("input[class='author']").val("");
    $("input[class='year']").val("");
    $("input[class='Files']").val("");
}
//Up file ebook
$(document).ready(function () {
    //Upload dữ liệu vào Ebook
    $("#upload").click(function () {
        clearFormFile();
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
        fileData.append("userid", @Session["user_id"]);
    fileData.append("subid", $("#subject option:selected").val());
    //fileData.append("subid", $("#subject").val());
    console.log(fileData);
    $.ajax({
        type: "POST",
        url: "/api/AdminAPI/UploadFiles",
        dataType: "json",
        contentType: false, // Not to set any content header
        processData: false, // Not to process data
        data: fileData,
        success: function (result) {
            if (result.isSuccess) {
                alert("Upload thành công.");
            }
            else {
                alert("Upload không thành công.");
            }
            clearFormFile();
        }
    });
});
});
//lấy môn học của ebook
$(document).ready(function () {
    //Ẩn hiện form
    $("#btn_upload").hide();
    $("#btn_upload1").hide();
    $("#btn_upload2").hide();
    //$("#loadEbook").hide();
    // hàm trả về html thông tin file ebooks
    function simpleTemplating(result) {
        var data = "";
        $.each(result, function (i, v) {
            data += "<div class='col-lg-4 col-md-6 mb-4' data-bind='attr: { id: id }'>";
            data += "<div class='card h-100'>";
            data += "<a href='#'><img class='card-img-top' src='http://placehold.it/700x400' alt=''></a>";
            data += "<div class='card-body'>";
            data += "<h4 class='card-title'>";//
            data += "<a href='/Details/Details?ebook_id=" + v.ebook_id + "'" + "data-bind='text:title'>" + v.title + "</a> </h4>";
            data += "<h5>tác giả: " + v.author + "</h5>";
            //data += "<p class='card-text' data-bind='text:author'></p>";
            //data += "<div class='card-footer'>";
            data += "<small class='text-muted'>&#9733; &#9733; &#9733; &#9733; &#9733;</small>";
            data += " </div></div></div></div>";
        });
        //data += "</div>";
        //debugger;
        return data;
    }
    //load môn học ebooks
    $.ajax({
        type: "GET",
        url: "/api/AdminAPI/GetSubjectEbook",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            $.each(result, function (i, v) {
                //load các môn của ebook lên thanh danh mục bên trái
                $("#loadSubjectOfEbook").append("<li id='ebook_" + v.id + "'><a>" + v.name + "</a></li >");
                //click môn học load file
                $("#ebook_" + v.id).click(function (e) {
                    //Ẩn hiện form upload file ở trang chỉ hay uplaod file trong môn học
                    $("#btn_upload").show();
                    $("#btn_upload1").hide();
                    $("#btn_upload2").hide();
                    $("#essay").hide();
                    $("#thesis").hide();
                    $("#loadEssay").hide();
                    $("#loadThesis").hide();
                    $("#ebook").show();
                    $("#loadEbook").show();
                    $("#subject").append("<option value='" + v.id + "'>" + v.name + "</option>"); subjectE
                    //$("#subject").val(v.name);
                    $("#loadEbook").empty();
                    $.ajax({
                        type: "GET",
                        url: "/api/AdminAPI/GetEbook",
                        data: { id: v.id },
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (result) {
                            console.log(result);

                            var tag = $(e.currentTarget).closest('.mainmenu-item');

                            var tagText = $(tag).find('.mainmenu-item-text');
                            $('.breadcrumb-item-2').css("d isplay", "block");
                            $('.breadcrumb-item-3').css("display", "block");
                            $('#breadcrumb-2').text($(tagText).text());
                            $('#breadcrumb-3').text(v.name);
                            $.each(result, function (i, v) {
                                console.log("ebookid", v.ebook_id);
                                var data = "<div class='col-lg-4 col-md-6 mb-4' data-bind='attr: { id: id }'>";
                                data += "<div class='card h-100'>";
                                data += "<a href='#'><img class='card-img-top' src='http://placehold.it/700x400' alt=''></a>";
                                data += "<div class='card-body'>";
                                data += "<h4 class='card-title'>";//
                                data += "<a href='/Details/Details?ebook_id=" + v.ebook_id + "'" + "data-bind='text:title'>" + v.title + "</a> </h4>";
                                data += "<h5>tác giả: " + v.author + "</h5>";
                                data += "<p class='card-text' data-bind='text:author'></p>";
                                data += "<div class='card-footer'></div>";
                                data += "<small class='text-muted'>&#9733; &#9733; &#9733; &#9733; &#9733;</small>";
                                data += " </div></div></div>";

                                //$("#loadEbook").append(data);
                            });
                            $('#pagination-container').pagination({
                                dataSource: function (done) {

                                    $.ajax({
                                        type: 'GET',
                                        url: '/api/AdminAPI/GetEbookPaging',
                                        data: { id: v.id },
                                        contentType: "application/json; charset=utf-8",
                                        dataType: "json",
                                        success: function (response) {
                                            done(response);
                                        }
                                    });
                                },
                                pageSize: 9,
                                callback: function (data, pagination) {
                                    var html = simpleTemplating(data);
                                    $('#loadEbook').html(html);
                                }
                            })
                        }
                    });
                });
            });
        }
    });


    //tạo môn ebook
    $("#create_sub_ebook").click(function () {
        var subject = {};
        subject.name = $("#nameSujectEbook").val();
        $.ajax({
            type: "POST",
            url: "/api/AdminAPI/CreateSubject",
            data: JSON.stringify(subject),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
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
//TIỂU LUẬN

function simpleTemplating1(result) {
    var data = "";
    $.each(result, function (i, v) {
        data += "<div class='col-lg-4 col-md-6 mb-4' data-bind='attr: { id: id }'>";
        data += "<div class='card h-100'>";
        data += "<a href='#'><img class='card-img-top' src='http://placehold.it/700x400' alt=''></a>";
        data += "<div class='card-body'>";
        data += "<h4 class='card-title'>";//
        data += "<a href='/Details/DetailsEssay?essay_id=" + v.essay_id + "'" + "data-bind='text:title'>" + v.title + "</a> </h4>";
        data += "<h5>GVHD: " + v.instructor + "</h5>";
        data += "<p class='card-text' data-bind='text:author'></p>";
        data += "<div class='card-footer'></div>";
        data += "<small class='text-muted'>&#9733; &#9733; &#9733; &#9733; &#9733;</small>";
        data += " </div></div></div>";
    });
    return data;
}

// load môn tiểu luận
$.ajax({
    type: "GET",
    url: "/api/AdminAPI/GetSubjectEssay",
    contentType: "application/json; charset=utf-8",
    dataType: "json",
    success: function (result) {
        $.each(result, function (i, v) {
            //load các môn của ebook lên thanh danh mục bên trái
            $("#loadSubjectOfEssay").append("<li id='essay_" + v.id + "'><a>" + v.name + "</a></li >");
            //click môn học load file
            $("#essay_" + v.id).click(function (e) {
                //Ẩn hiện form upload file ở trang chỉ hay uplaod file trong môn học
                $("#btn_upload1").show();
                $("#btn_upload2").hide();
                $("#btn_upload").hide();
                $("#essay").show();
                $("#thesis").hide();
                $("#loadThesis").hide();
                $("#ebook").hide();
                $("#loadEbook").hide();
                $("#loadEssay").show();
                $("#subjectE").append("<option value='" + v.id + "'>" + v.name + "</option>");
                //$("#subjectE").val(v.name);
                $("#loadThesis").empty();
                $("#loadEbook").empty();
                $("#loadEssay").empty();
                $.ajax({
                    type: "GET",
                    url: "/api/AdminAPI/GetEssay",
                    data: { id: v.id },
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        console.log(v);

                        var tag = $(e.currentTarget).closest('.mainmenu-item');

                        var tagText = $(tag).find('.mainmenu-item-text');
                        $('.breadcrumb-item-2').css("display", "block");
                        $('.breadcrumb-item-3').css("display", "block");
                        $('#breadcrumb-2').text($(tagText).text());
                        $('#breadcrumb-3').text(v.name);

                        //$("#loadEbook").append(data);
                        //});
                        $('#pagination-container').pagination({
                            dataSource: function (done) {
                                $.ajax({
                                    type: 'GET',
                                    url: '/api/AdminAPI/GetEssayPaging',
                                    data: { id: v.id },
                                    contentType: "application/json; charset=utf-8",
                                    dataType: "json",
                                    success: function (response) {
                                        done(response);
                                    }
                                });
                            },
                            callback: function (data, pagination) {
                                var html = simpleTemplating1(data);
                                $('#loadEssay').html(html);
                            }
                        })
                    }
                });
            });
        });
    }
});
//tạo môn ebook
$("#create_sub_ebook").click(function () {
    var subject = {};
    subject.name = $("#nameSujectEbook").val();
    $.ajax({
        type: "POST",
        url: "/api/AdminAPI/UploadFileEssay",
        dataType: "json",
        contentType: false, // Not to set any content header
        processData: false, // Not to process data
        data: fileData,
        success: function (result) {
            if (result.isSuccess) {
                alert("Upload thành công.");
            }
            else {
                alert("Upload không thành công.");
            }
            clearFormFile();
        }
    });
});

//SIGNALR
$(function () {
    $.connection.hub.logging = true;
    var chat = $.connection.myHub;
    //SignalR thêm file
    chat.client.postFileEssayToClient = function (id, title, instructor, executor1, executor1,
        describe, filename, date_upload, user, subject) {
        var data = "<div class='col-lg-4 col-md-6 mb-4' data-bind='attr: { id: id }'>";
        data += "<div class='card h-100'>";
        data += "<a href='#'><img class='card-img-top' src='http://placehold.it/700x400' alt=''></a>";
        data += "<div class='card-body'>";
        data += "<h4 class='card-title'>";
        data += "<a href='/Details/DetailsEssay?essay_id=" + v.essay_id + "'" + "data-bind='text:title'>" + title + "</a> </h4>";
        data += "<h5>GVHD: " + instructor + "</h5>";
        data += "<p class='card-text' data-bind='text:author'></p>";
        data += "<div class='card-footer'></div>";
        data += "<small class='text-muted'>&#9733; &#9733; &#9733; &#9733; &#9733;</small>";
        data += " </div></div></div>";

        $("#loadEssay").append(data);
    };
    $.connection.hub.start().done(function () {
        //alert("started");
    }).fail(function (result) {
        alert(result);
    });

});
//KHÓA LUẬN-- >
function simpleTemplating2(result) {
    var data = "";
    $.each(result, function (i, v) {
        data += "<div class='col-lg-4 col-md-6 mb-4' data-bind='attr: { id: id }'>";
        data += "<div class='card h-100'>";
        data += "<a href='#'><img class='card-img-top' src='http://placehold.it/700x400' alt=''></a>";
        data += "<div class='card-body'>";
        data += "<h4 class='card-title'>";//
        data += "<a href='/Details/DetailsThesis?thesis_id=" + v.thesis_id + "'" + "data-bind='text:title'>" + v.title + "</a> </h4>";
        data += "<h5>giáo viên hd: " + v.instructor + "</h5>";
        data += "<p class='card-text' data-bind='text:author'></p>";
        //data += "<div class='card-footer'></div>";
        data += "<small class='text-muted'>&#9733; &#9733; &#9733; &#9733; &#9733;</small>";
        data += " </div></div></div>";
    });
    //data += "</div>";
    //debugger;
    return data;
}

// load môn khóaluận
$.ajax({
    type: "GET",
    url: "/api/AdminAPI/GetSubjectThesis",
    contentType: "application/json; charset=utf-8",
    dataType: "json",
    success: function (result) {
        $.each(result, function (i, v) {
            //load các môn của ebook lên thanh danh mục bên trái
            $("#loadSubjectOfThesis").append("<li id='thesis_" + v.id + "'><a>" + v.name + "</a></li >");
            //click môn học load file
            $("#thesis_" + v.id).click(function (e) {
                //Ẩn hiện form upload file ở trang chỉ hay uplaod file trong môn học
                $("#btn_upload2").show();
                $("#btn_upload1").hide();
                $("#btn_upload").hide();
                $("#loadThesis").show();
                $("#subjectT").append("<option value='" + v.id + "'>" + v.name + "</option>");
                $("#essay").hide();
                $("#thesis").show();
                $("#loadThesis").show();
                $("#ebook").hide();
                $("#loadEbook").hide();
                $("#loadEssay").hide();
                $("#loadEssay").empty();
                $("#loadEbook").empty();
                $.ajax({
                    type: "GET",
                    url: "/api/AdminAPI/GetThesis",
                    data: { id: v.id },
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        console.log(v);

                        var tag = $(e.currentTarget).closest('.mainmenu-item');

                        var tagText = $(tag).find('.mainmenu-item-text');
                        $('.breadcrumb-item-2').css("display", "block");
                        $('.breadcrumb-item-3').css("display", "block");
                        $('#breadcrumb-2').text($(tagText).text());
                        $('#breadcrumb-3').text(v.name);

                        $('#pagination-container').pagination({
                            dataSource: function (done) {
                                $.ajax({
                                    type: 'GET',
                                    url: '/api/AdminAPI/GetThesisPaging',
                                    data: { id: v.id },
                                    contentType: "application/json; charset=utf-8",
                                    dataType: "json",
                                    success: function (response) {
                                        done(response);
                                    }
                                });
                            },
                            callback: function (data, pagination) {
                                var html = simpleTemplating2(data);
                                $('#loadThesis').html(html);
                            }
                        })
                    }
                });
            });
        });
    }
});
//upload file khóa luận
$("#uploadThesis").click(function () {
    clearFormFile();
    var files = $("#fileInputT").get(0).files;
    var fileData = new FormData();

    for (var i = 0; i < files.length; i++) {
        fileData.append("fileInput", files[i]);
    }
    //cái này là lấy thông tin trên text box  title là key tiltle.val() là giá trị

    fileData.append("title", $("#titleT").val());
    fileData.append("describe", $("#describeT").val());
    fileData.append("instructor", $("#instructorT").val());
    fileData.append("student1", $("#student1T").val());
    fileData.append("student2", $("#student2T").val());
    fileData.append("userid", @Session["user_id"]);
// fileData.append("subid", $("#subjectT").val());
fileData.append("subid", $("#subjectT option:selected").val());
fileData.append("course", $("#courseT").val());

console.log(fileData);
$.ajax({
    type: "POST",
    url: "/api/AdminAPI/UploadFilesThesis",
    dataType: "json",
    contentType: false, // Not to set any content header
    processData: false, // Not to process data
    data: fileData,
    success: function (result) {
        if (result.isSuccess) {
            alert("Upload thành công.");
        }
        else {
            alert("Upload không thành công.");
        }
        clearFormFile();
    }
});

    });

//SIGNALR
$(function () {
    $.connection.hub.logging = true;
    var chat = $.connection.myHub;


    //SignalR thêm file
    chat.client.postFileThesisToClient = function (id, thesis_id, title, instructor, executor1, executor1,
        describe, filename, date_upload, user, subject) {
        var data = "<div class='col-lg-4 col-md-6 mb-4' data-bind='attr: { id: id }'>";
        data += "<div class='card h-100'>";
        data += "<a href='#'><img class='card-img-top' src='http://placehold.it/700x400' alt=''></a>";
        data += "<div class='card-body'>";
        data += "<h4 class='card-title'>";
        data += "<a href='/Details/DetailsThesis?thesis_id=" + thesis_id + "'" + "data-bind='text:title'>" + title + "</a> </h4>";
        data += "<h5>GVHD: " + instructor + "</h5>";
        data += "<p class='card-text' data-bind='text:author'></p>";
        data += "<div class='card-footer'></div>";
        data += "<small class='text-muted'>&#9733; &#9733; &#9733; &#9733; &#9733;</small>";
        data += " </div></div></div>";

        $("#loadThesis").append(data);
    };
    $.connection.hub.start().done(function () {
        //alert("started");
    }).fail(function (result) {
        alert(result);
    });

});
$.ajax({
    type: "GET",
    url: "/api/AdminAPI/GetEbook1",
    contentType: "application/json; charset=utf-8",
    dataType: "json",
    success: function (result) {

        $.each(result, function (i, v) {
            var data = "<div class='col-lg-4 col-md-6 mb-4' data-bind='attr: { id: id }'>";
            data += "<div class='card h-100'>";
            data += "<a href='#'><img class='card-img-top' src='http://placehold.it/700x400' alt=''></a>";
            data += "<div class='card-body'>";
            data += "<h4 class='card-title'>";//
            data += "<a href='/Details/Details?ebook_id=" + v.ebook_id + "'" + "data-bind='text:title'>" + v.title + "</a> </h4>";
            data += "<h5>tác giả: " + v.author + "</h5>";
            data += "<p class='card-text' data-bind='text:author'></p>";
            //data += "<div class='card-footer'></div>";
            data += "<small class='text-muted'>&#9733; &#9733; &#9733; &#9733; &#9733;</small>";
            data += " </div></div></div>";

            $("#loadEbook").append(data);

        });
    }
});


$.ajax({
    type: "GET",
    url: "/api/AdminAPI/GetEssay1",
    contentType: "application/json; charset=utf-8",
    dataType: "json",
    success: function (result) {

        $.each(result, function (i, v) {
            var data1 = "<div class='col-lg-4 col-md-6 mb-4' data-bind='attr: { id: id }'>";
            data1 += "<div class='card h-100'>";
            data1 += "<a href='#'><img class='card-img-top' src='http://placehold.it/700x400' alt=''></a>";
            data1 += "<div class='card-body'>";
            data1 += "<h4 class='card-title'>";//
            data1 += "<a href='/Details/DetailsEssay?essay_id=" + v.essay_id + "'" + "data-bind='text:title'>" + v.title + "</a> </h4>";
            data1 += "<h5>giáo viên hd: " + v.instructor + "</h5>";
            data1 += "<p class='card-text' data-bind='text:author'></p>";
            //data += "<div class='card-footer'></div>";
            data1 += "<small class='text-muted'>&#9733; &#9733; &#9733; &#9733; &#9733;</small>";
            data1 += " </div></div></div>";

            $("#loadEssay").append(data1);

        });
    }
});

$.ajax({
    type: "GET",
    url: "/api/AdminAPI/GetThesis1",
    contentType: "application/json; charset=utf-8",
    dataType: "json",
    success: function (result) {

        $.each(result, function (i, v) {
            var data1 = "<div class='col-lg-4 col-md-6 mb-4' data-bind='attr: { id: id }'>";
            data1 += "<div class='card h-100'>";
            data1 += "<a href='#'><img class='card-img-top' src='http://placehold.it/700x400' alt=''></a>";
            data1 += "<div class='card-body'>";
            data1 += "<h4 class='card-title'>";//
            data1 += "<a href='/Details/DetailsThesis?thesis_id=" + v.thesis_id + "'" + "data-bind='text:title'>" + v.title + "</a> </h4>";
            data1 += "<h5>giáo viên hd: " + v.instructor + "</h5>";
            data1 += "<p class='card-text' data-bind='text:author'></p>";
            //data += "<div class='card-footer'></div>";
            data1 += "<small class='text-muted'>&#9733; &#9733; &#9733; &#9733; &#9733;</small>";
            data1 += " </div></div></div>";

            $("#loadThesis").append(data1);

        });
    }
});
