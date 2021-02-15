$('.exp_box').hide();

$('select[name=specialization]').select2({
    width: '100%',
    dropdownCssClass: 'fs-14px'
});

$(function () {
    $('select[name=specialization]').on('select2:select', function (e) {
        $('.exp_box').show();

        var value = e.params.data.id;
        var text = e.params.data.text;

        var html = '<div class="col-lg-4 col-sm-6" id="item_' + value + '"> <div class="media theme_doc_item wow fadeInUp"> <div class="form-group"> <label for="">' + text + '</label> <input type="number" class="form-control" name="years[]" id="exp_' + value +'"> </div> </div> </div>';
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
            "exp[]": {
                required: true,
                max: 100,
                min: 1
            }
        },
        messages: {
            "exp[]": {
                required: "You must enter the number of years.",
                max: "Oop...Can't be greater than 100 years.",
                min: "Sorry. Can't be less than 1 year.",
            }
        }
    });

});