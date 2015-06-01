$(function () {
    $("#take-part").on("click", function () {
        var id = $(this).attr("href").substring(1);
        alert(id);
        $.ajax({
            url: "/Home/TakePart",
            type: "get",
            data: {
                "contestId": id
            }
        })
        .done(function (data) {
            alert("Вы стали участником олимпиады. Поздравляем.");
        })
        .fail(function (data) {
            console.log("Error in ajax get request. Details: " + JSON.stringify(data));
        });
    });
});
