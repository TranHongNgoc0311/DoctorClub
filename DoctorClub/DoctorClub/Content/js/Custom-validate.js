$(function () {
    $.validator.addMethod("greaterThan", function (value, element, params) {
        var v = value.split("/");
        var val = new Date(v[2], v[1] - 1, v[0]);

        return new Date() > val && new Date().getFullYear() - v[2] > 18;
    }, 'Must be greater than {0}.');


    $('#Post').validate({
        onfocusout: false,
        onkeyup: false,
        onclick: false,
        errorClass: 'error help-inline',
        validClass: 'success',
        errorElement: 'span',
        highlight: function (element, errorClass, validClass) {
            $(element).parents("div.control-group").addClass(errorClass).removeClass(validClass);
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).parents(".error").removeClass(errorClass).addClass(validClass);
        },
        rules: {
            "Title": {
                required: true,
                maxlength: 200
            },
            "SubCatId": {
                required: true
            },
            "Tags": {
                required: true
            }
        },
        messages: {
            "Title": {
                required: "Title field can not be empty.",
                maxlength: "Title can not more than 200 characters."
            },
            "SubCatId": {
                required: "Post category is required."
            },
            "Tags": {
                required: "Post tag is required."
            }
        }
    });

    $('#change_password').validate({
        onfocusout: false,
        onkeyup: false,
        onclick: false,
        errorClass: 'error help-inline',
        validClass: 'success',
        errorElement: 'span',
        highlight: function (element, errorClass, validClass) {
            $(element).parents("div.control-group").addClass(errorClass).removeClass(validClass);
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).parents(".error").removeClass(errorClass).addClass(validClass);
        },
        rules: {
            "current_password": {
                required: true,
                minlength: 8
            },
            "password": {
                required: true,
                minlength: 8
            },
            "Confirm_passord": {
                equalTo: "#password"
            }
        },
        messages: {
            "current_password": {
                required: "Enter your current password.",
                maxlength: "Password can not be less than 8 character."
            },
            "password": {
                required: "Enter your new password here.",
                maxlength: "Password can not be less than 8 character."
            },
            "Confirm_passord": {
                equalTo: "Confirm password Confirm password does not match."
            }
        }
    });
});

    