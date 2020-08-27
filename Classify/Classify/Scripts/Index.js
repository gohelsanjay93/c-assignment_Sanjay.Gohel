$(document).ready(function () {
    //$("#MultipleDelbutton").attr("disabled","disabled");
});
$(".DeleteProduct").click(function () {   
    swal({
        title: "Are you sure?",
        text: "Once deleted, you will not be able to recover",
        icon: "warning",
        buttons: true,
        dangerMode: true,
    })
        .then((willDelete) => {
            if (willDelete) {
                $("." + $(this).val()+ "").submit();
            } else {
                swal("Your data is safe!");
            }
        });
});

function readURL(input) {
    debugger;
    if (input.files && input.files[0]) {
        var ext = $('#UploadImage').val().split('.').pop().toLowerCase();
        var picsize = (input.files[0].size);
        if ($.inArray(ext, ['png', 'jpg', 'jpeg']) == -1) {
            swal({
                title: "Invalid file",
                text: "Use jpg ,png or jpeg",
                icon: "error",
            });
            $("#UploadImage").val("");
        }
        else
        {            
            if (picsize > 1000000) {
                swal({
                    title: "File is Too large",
                    text: "File should be less then 1 mb",
                    icon: "error",
                });
                $("#UploadImage").val("");
            } else {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#UploadProductImage').attr('src', e.target.result);
                }
                reader.readAsDataURL(input.files[0]);
            }
        }        
    }
}

$("#UploadImage").change(function () {
    readURL(this);
});

$("#MultipleDelbutton").click(function () {    
    var Arr = [];
    var data = $("input[type=checkbox]:checked").serializeArray();
    //console.log(data);
    for (var i = 0; i < data.length; i++) {
        //alert(data[i].value);
        Arr.push(data[i].value);
    }
    if (Arr.length > 0) {
        swal({
            title: "Are you sure?",
            text: "Once deleted, you will not be able to recover",
            icon: "warning",
            buttons: true,
            dangerMode: true,
        })
            .then((willDelete) => {
                if (willDelete) {
                    $("#Ids").val(Arr);
                    $("#MultipleDelForm").submit();
                } else {
                    swal("Your Products is safe!");
                }
            });        
    }
    else {
        swal({
            title: "Product not Selected",
            text: "Select atleast one Product",
            icon: "error",
        });
    }
    
});
