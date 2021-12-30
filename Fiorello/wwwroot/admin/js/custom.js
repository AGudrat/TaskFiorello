
$(function () {
    $('#photoUpload').change(
        function () {
            var files = $('input[type="file"]')[0].files;
            if (files.length > 5 || files.length == 0) {
                $('#btnUpload').addClass('d-none');
                $('#myLabel').html("*Maksimum 5, minimum 1 şəkil yüklənməlidir");
            }
            else {
                $('#btnUpload').removeClass('d-none');
                $('#myLabel').html(" ");
            }
        })
})