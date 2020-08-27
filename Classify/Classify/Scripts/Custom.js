$(document).ready(function () {    
    $("#Country").append("<option val='0'>Select country</option>");
    $("#State").attr("disabled", "true");
    $("#City").attr("disabled", "true");
    $.ajax({
        type: "GET",
        url: "http://localhost:63364/User/GetCountry",
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                //$("#State").removeAttr("disabled");
                $("#Country").append("<option value =" + data[i].Id + ">" + data[i].Countryname + "</option>");
            }
        },
        error: function () {
            alert("Something is wrong");
        }
    });
});
$("#Country").change(function () { 
    CountryReset();
    if ($(this).val() !="Select country") {
        $.ajax({
            type: "GET",
            url: "http://localhost:63364/User/GetState",
            data: { "Id": $(this).val() },
            success: function (data) {
                $("#State").append("<option val='0'>Select State</option>");
                for (var i = 0; i < data.length; i++) {
                    $("#State").removeAttr("disabled");
                    $("#State").append("<option value =" + data[i].Id + ">" + data[i].Statename + "</option>");
                }
            },
            error: function () {
                alert("Something is wrong");
            }
        });
    }    
});

$("#State").change(function () {
    StateReset();
    if ($(this).val() != "Select State") {
        $.ajax({
            type: "GET",
            url: "http://localhost:63364/User/GetCity",
            data: { "Id": $(this).val() },
            success: function (data) {
                $("#City").append("<option val='0'>Select City</option>");
                for (var i = 0; i < data.length; i++) {
                    $("#City").removeAttr("disabled");
                    $("#City").append("<option value =" + data[i].Id + ">" + data[i].Cityname + "</option>");
                }
            },
            error: function () {
                alert("Something is wrong");
            }
        });
    }    
});

function CountryReset() {
    $("#State").empty();
    $("#State").attr("disabled", "true");
    $("#City").empty();
    $("#City").attr("disabled", "true");
}

function StateReset() {    
    $("#City").empty();
    $("#City").attr("disabled", "true");
}