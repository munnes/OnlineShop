$('#submit').click(function () {
    if ($("#imgFile").val() != "") {
        //chk if img extention is valid
        var filename = document.getElementById("imgFile").value;
        //tiger.png ===> png
        var extensionImg = filename.substr(filename.lastIndexOf('.') + 1)
        var validExtentions = ['jpg', 'png', 'gif', 'bmp'];
     
        if ($.inArray(extensionImg, validExtentions) == -1) {
            $("#error-div").fadeIn()
            $("#view-err").append("Please choose an image with the correct extension");
            return false;
        }
        //chk if img size valid(2mb)
        var myFileSize = document.getElementById('imgFile').files[0].size / 1024 / 1024;
        if (myFileSize > 2) {
            $("#error-div").fadeIn()
            $("#view-err").append("Please choose an image whose size is less than 2MB");
            return false;
        }
    }
})