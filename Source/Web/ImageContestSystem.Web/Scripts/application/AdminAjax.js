$("#show-contests").click(function () {
    $("#main-window").html("");

    $.get("/Administration/Contest/Get", function (result) {
        $("#wrapper").html(result);
    });
});

$("#show-users").click(function () {
    $("#main-window").html("");

    $.get("/Administration/User/Get", function (result) {
        $("#wrapper").html(result);
    });
});

$("#show-photos").click(function () {
    $("#main-window").html("");

    $.get("/Administration/Photo/Get", function (result) {
        $("#wrapper").html(result);
    });
});