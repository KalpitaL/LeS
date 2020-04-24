function ValidatePage() {
    ClearErrors();

    if (typeof (Page_ClientValidate) == "function") {

        if (!Page_ClientValidate()) {
            ShowErrors();
        }
    }

    return Page_IsValid;
}

function ClearErrors() {
    for (i = 0; i < Page_Validators.length; i++) {
        var control = $("#" + Page_Validators[i].controltovalidate);

        if (control.hasClass('errorBorder')) {
            control.removeClass('errorBorder');
        }
    }
}

function ShowErrors() {
    alert('You must enter all required data.');

    for (i = 0; i < Page_Validators.length; i++) {
        var control = $("#" + Page_Validators[i].controltovalidate);

        if (!Page_Validators[i].isvalid) {
            if (!control.hasClass('errorBorder')) {
                control.addClass('errorBorder');
            }
        }
    }
}