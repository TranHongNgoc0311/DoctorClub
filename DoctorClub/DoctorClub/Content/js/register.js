/*$(function () {
    var exp_list = new Array();
    $('#exp-box').hide();
    $('#Specializations').on('select2:select', function () {
        var list = $('#Specializations').val();
        if (list.length > 0) {
            $('#exp-box').show();
        }
        var html = '<div class="card doc_accordion" id="' + list.slice(-1) + '">';
        html += '<div class="card-header" id="heading' + list.slice(-1) + '">';
        html += '<h5 class="mb-0">';
        html += '<button type="button" class="btn btn-link collapsed" data-toggle="collapse" data-target="#card' + list.slice(-1) + '" aria-expanded="false" aria-controls="' + list.slice(-1) + '">';
        html += list.slice(-1) + '<i class="icon_plus"></i><i class="icon_minus-06"></i>';
        html += '</button></h5></div>';
        html += '<div id="card' + list.slice(-1) + '" class="collapse" aria-labelledby="heading' + list.slice(-1) + '" data-parent="#accordion">';
        html += '<div class="card-body toggle_body"><div class="row"><div class="col-sm-6 form-group">';
        html += '<div class="small_text">Start <small>year</small></div>';
        html += '<input type="number" class="form-control" name="from[]" id="from' + list.slice(-1) + '" placeholder="When do you start...">';
        html += '</div><div class="col-sm-6 form-group"><div class="small_text">End <small>year</small></div>';
        html += '<input id="to' + list.slice(-1) + '" name="to[]" type="number" class="form-control" placeholder="Not required if you are not finish ."></div>';
        html += '<div class="col-sm-6 form-group"><div class="small_text">Position</div>';
        html += '<input type="text" class="form-control" name="position[]" id="position' + list.slice(-1) + '" placeholder="Position that you apply to training specialization">';
        html += '</div><div class="col-sm-6 form-group"><div class="small_text">Place</div>';
        html += '<input id="place' + list.slice(-1) + '" name="place[]" type="text" class="form-control" placeholder="Where do you training your specialization.">';
        html += '</div></div></div></div></div>';
        exp_list.push(list.slice(-1));
        $('#exp-form').append(html);
    });
    $('#Specializations').on('select2:unselect', function (e) {
        var tg = e.params.data.id;
        $('#' + tg).remove();
        exp_list.splice(exp_list.indexOf(tg), 1);
        if (exp_list.length == 0) {
            $('#exp-box').hide();
        }
    });
});

function nextTab(tab, formId) {
    switch (tab) {
        case 1:
            $.when(CheckExistData(tab, formId, 'username')).done(function () {
                if (!$('.error')[0]) {
                    $("#tab" + tab).removeClass("active-tab");
                    $("#tab" + (tab + 1)).addClass("active-tab");
                }

            });
            break;
        case 2:
            $.when(CheckExistData(tab, formId, 'phone'), CheckExistData(tab, formId, 'email')).done(function () {
                if (!$('.error')[0]) {
                    $("#tab" + tab).removeClass("active-tab");
                    $("#tab" + (tab + 1)).addClass("active-tab");
                }

            });
            break;
        default:
            $("#tab" + tab).removeClass("active-tab");
            $("#tab" + (tab + 1)).addClass("active-tab");
    }
}*/

function CheckExistData(tab, field) {
    $('#error_' + field).remove();
    if ($('#' + formId).valid()) {
        return $.ajax({
            type: 'GET',
            url: 'Users/CheckExistData?data=' + $('#' + field).val(),
            success: function (data) {
                data = data.toLowerCase();
                if (data === 'false') {
                    var html = '<span for="' + field + '" id="error_' + field + '" class="error help-inline">This ' + field + ' is already used.</span>';
                    $('#' + field).after(html);
                }
            },
            error: function (msg) {
                alert(msg.statusText);
            },
        });
    }
}

