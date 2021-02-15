$('.select2_ac').select2({
    tags: true,
    width: "100%",
    placeholder: "Enter your university"
});

$(document).ajaxComplete(function () {
    $('.select2_ac').select2({
        tags: true,
        width: "100%",
        placeholder: "Enter your university"
    });
});

$('select[name=specialization]').select2({
    tags: true,
    width: '100%',
    dropdownCssClass: 'fs-14px',
    placeholder: "choose at least one"
});

$('select[name=award]').select2({
    width: '100%',
    placeholder: "If you don't have. please, skip this one.",
    tags: true
});

$('.exp_box').hide();

$(function () {
    $('select[name=specialization]').on('select2:select', function (e) {
        $('.exp_box').show();

        var value = e.params.data.id;
        var text = e.params.data.text;

        var html = '<div class="col-lg-4 col-sm-6" id="item_' + value + '"> <div class="media theme_doc_item wow fadeInUp"> <div class="form-group"> <label for="">' + text + '</label> <div class="row"> <div class="col-md-6"> <div class="form-group"> <input type="number" min="1" class="form-control" name="From_spe[]" placeholder="From year ..."> </div> </div> <div class="col-md-6"> <div class="form-group"> <input type="number" min="1" class="form-control" name="to_spe[]" placeholder="To year ..."> </div> </div> </div> <div class="row"> <div class="container-fluid"> <div class="form-group"> <input type="text" class="form-control" name="Place[]" placeholder="Where?"> </div> <div class="form-group"> <input type="text" class="form-control" name="position[]" placeholder="Position?"> </div> </div> </div> </div> </div> </div>';
        $('.exp_item').append(html);
    });

    $('select[name=specialization]').on('select2:unselect', function (e) {
        var value = e.params.data.id;

        $('.exp_box').find('#item_' + value).remove();

        if ($('select[name=specialization]').val().length == 0) {
            $('.exp_box').hide();
        }
    });

    $('form').validate({
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
            "Academy": {
                required: true
            },
            "position[]": {
                required: true
            },
            "Place[]": {
                required: true
            },
            "From_spe[]": {
                required: true,
                number: true,
                max: new Date().getFullYear(),
                min: 1970
            },
            "to_spe[]": {
                required: true,
                number: true,
                max: new Date().getFullYear(),
                min: 1970
            },
            "From": {
                required: true,
                number: true,
                max: new Date().getFullYear(),
                min: 1970
            },
            "to": {
                required: true,
                number: true,
                max: new Date().getFullYear(),
                min: 1970
            },
            "image": {
                required: true
            },
            "specialization": {
                required: true
            }
        },
        messages: {
            "Academy": {
                required: "Academy names cannot be left blank."
            },
            "position[]": {
                required: "The position you assumed in this specialization."
            },
            "Place[]": {
                required: "Where you are working."
            },
            "From_spe[]": {
                required: "Enter the year you started in this specialization.",
                number: "Only numberic please.",
                min: "The year value is too small.",
                max: "The year value is too big."
            },
            "to_spe[]": {
                required: "Enter the year you finish in this specialization.",
                number: "Only numberic please.",
                min: "The year value is too small.",
                max: "The year value is too big."
            },
            "From": {
                required: "Enter the year you started studying here.",
                number: "Only numberic please.",
                min: "The year value is too small.",
                max: "The year value is too big."
            },
            "to": {
                required: "Please enter the year you finished your studies here.",
                number: "Only numberic please.",
                min: "The year value is too small.",
                max: "The year value is too big."
            },
            "image": {
                required: "upload your diploma photo."
            },
            "specialization": {
                required: "Determine your specialization."
            },
        },
        errorPlacement: function (error, element) {
            if (element.attr("name") == 'image') {
                error.appendTo("#errorImg");
            } else {
                error.appendTo(element.parent());
            }
        }
    });

    $('form').on('submit', function (e) {
        e.preventDefault();

        let err = false;
        if ($(this).valid()) {
            var error = '';
            if ($('input[name=to]').val() != '' && $('input[name=to]').val() != null) {
                if ($('input[name=From]').val() > $('input[name=to]').val()) {
                    error = 'From year can not greater than to year';
                    err = true;
                }
            }

            if (!err) {
                e.target.submit();
            } else {
                Swal.fire({
                    icon: 'error',
                    title: '<strong class="text-danger">Oops...</strong>',
                    text: error
                });
            }
        }
    });
});