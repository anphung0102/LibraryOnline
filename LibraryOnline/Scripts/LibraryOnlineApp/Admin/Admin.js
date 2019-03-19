//khi muon sử dụng e phải bind thằng này vô 1 view nào đó

//đầu tiên phải khai báo model

function AdminViewModel() {
    var self = this;
    // à giới thiệu, nó có 2 loại tạo cơ bản :D observable cho dữ liệu đơn, và observableArray cho mảng
    // giờ test thằng đơn trước
    self.testSingle = ko.observable(""); //khai bao rong biến đơn
    self.resultArr = ko.observableArray([]);//khai báo rỗng biến mảng
    self.get = function () {
        $.ajax({
            type: "GET",
            url: "/api/FileAPI/Get", //vai hàm Get ko vậy thôi á hả? :D da chi v thoi  e dang test ok ;e ):)D a đêt chữ test tính coi có gì copy vô 
            // ai ngờ méo có gì :D trùng hợp vl
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                self.resultArr(result);//gán dữ liệu cho mảng
            }
        });
    }
}

var AdminViewModel = new AdminViewModel();
AdminViewModel.get();//ý là khởi tạo model, xong gọi hàm get lấy data
ko.applyBindings(AdminViewModel, $("#admin-container")[0]);
