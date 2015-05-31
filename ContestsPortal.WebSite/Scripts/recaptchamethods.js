function addErrorSpan() {
    var widget = $("div.rc-anchor").first();
    if (widget != null) {
        var errormessage = $("#recapthcaerror");
        if (errormessage != null) errormessage.remove();
        var span = $("<span>").addClass("field-validation-error").attr("id", "recapthcaerror").css("display", "inline").text("Необходимо пройти верификацию через капчу");
        $("#captcha").after(span);
    }
}

function deleteErrorSpan() {
    var errormessage = $("#recaptchaerror");
    if (errormessage != null)
        errormessage.remove();
}

var callback = function () {
    grecaptcha.render(document.getElementById("captcha"), { 'sitekey': "6Lc5hAUTAAAAANlQPFvl_SBNfBDjiXIQ7TdZuuz7", 'callback': captchaCompletedCallback });
};

var captchaCompletedCallback = function (response) {
    deleteErrorSpan();
    console.log(response);
};

$(function () {
    $("#datepicker").attr("readonly", "true").datepicker({
        dateFormat: "dd.mm.yy",
        daterange: '1930:2014',
        changeYear: true,
        changeMonth: true
    });

    $("form").submit(function (e) {
        var elem = $(".recaptcha-checkbox-unchecked").first();
        if (elem.attr("aria-checked") == false) {
            addErrorSpan();
            e.preventDefault();
        }
    });
});