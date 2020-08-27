$(document).ready(function () {    
    $("#Country").append("<option value=''>Select country</option>");
    $("#State").attr("disabled", "true");
    $("#City").attr("disabled", "true");    
    CountryAjax();
    TriggerCountry();
    TriggerState();    
});

$("#Country").change(function () {
    //alert($(this).val());    
    CountryReset();
    if ($("#Country").val() != "") {
        StateAjax($("#Country").val());
    }    
});

$("#State").change(function () {
    StateReset();
    if ($("#State").val() != "") {
        CityAjax($("#State").val());
    }
});

function TriggerCountry() {      
    StateAjax(localStorage.getItem('SelectedCountry'));
}

function TriggerState() {   
    CityAjax(localStorage.getItem('SelectedState'));    
}

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

function CountryAjax() {
    $.ajax({
        type: "GET",
        url: "http://localhost:63364/User/GetCountry",
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                //$("#State").removeAttr("disabled");                
                $("#Country").append("<option value =" + data[i].Id + ">" + data[i].Countryname + "</option>");
            }
            if (localStorage.getItem("SelectedCountry")) {
                $("#Country").val(localStorage.getItem("SelectedCountry"));
            }
        },
        error: function () {
            alert("Something is wrong");
        }
    }); 
}
function StateAjax(Id) {
    //alert(Id+" C");
    //Id of Countrychange
    $.ajax({
        type: "GET",
        url: "http://localhost:63364/User/GetState",
        data: { "Id": Id },
        success: function (data) {
            $("#State").append("<option val=''>Select State</option>");
            for (var i = 0; i < data.length; i++) {
                $("#State").removeAttr("disabled");
                $("#State").append("<option value =" + data[i].Id + ">" + data[i].Statename + "</option>");
            }
            if (localStorage.getItem("SelectedState")) {
                $("#State").val(localStorage.getItem("SelectedState"));
            }
        },
        error: function () {
            alert("Something is wrong");
        }
    });
}

function CityAjax(Id) {
    // Id of Statechange
    //alert(Id + " s");
    $.ajax({
        type: "GET",
        url: "http://localhost:63364/User/GetCity",
        data: { "Id": Id },
        success: function (data) {
            $("#City").append("<option val='0'>Select City</option>");
            for (var i = 0; i < data.length; i++) {
                $("#City").removeAttr("disabled");
                $("#City").append("<option value =" + data[i].Id + ">" + data[i].Cityname + "</option>");
            }
            if (localStorage.getItem("SelectedCity")) {
                $("#City").val(localStorage.getItem("SelectedCity"));
            }
        },
        error: function () {
            alert("Something is wrong");
        }
    });
    $("#City").val(localStorage.getItem("SelectedCity"));
}
