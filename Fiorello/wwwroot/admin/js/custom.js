// $(document).ready(function (){
//         $('#FileUpload2').change(function () {
//             var files = $(this)[0].files;
//             if (files.lenght != 6) {
//                 alert("Six files have to be selected at a time!");
//             }
//             else {
//                 submit();//your custom method to submit the form
//             }
//         })
// });


$(function () {
    $('#photoUpload').change(
        function () {
            var files = $('input[type="file"]')[0].files;
            if (files.length > 5) {
                $('#btnUpload').addClass('d-none');
                $('#myLabel').html("*5 dən çox şəkil yükləmək olmaz.");
            }
            else {
                $('#btnUpload').removeClass('d-none');
                $('#myLabel').html(" ");
            }
        })
}) 