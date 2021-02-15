function snackbarAlert(str, alert_type) {
    // Get the snackbar DIV
    $('.snackbar-alert').find('.media').addClass(alert_type);
    $('.snackbar-alert').find('.content-alert').html(str);
    $('.snackbar-alert').addClass("show");

    // After 3 seconds, remove the show class from DIV
    setTimeout(function () {
        $('.snackbar-alert').removeClass("show");
        $('.snackbar-alert').find('.alert').removeClass(alert_type);
    }, 3000);
}
$(function () {
    if ($('#private_option').is(':checked')) {
        $(".private").css("color", "#6b707f");
    } else {
        $(".private").css("color", "#10b3d6");
    }

    $('#private_option').change(function () {
        if ($(this).is(':checked')) {
            $(".private").css("color", "#6b707f");
            snackbarAlert('We are changing your account mode to be private. <p>please wait 10 second...</p>', 'notice-success');
            privateAccount(true);
        } else {
            $(".private").css("color", "#10b3d6");
            snackbarAlert('We are changing your account mode to be public. <p>please wait 10 second...</p>', 'notice-success');
            privateAccount(false);
        }
    });

    function privateAccount(private) {
        //disable button
        $("#private_option").css("background-color", "#6b707f");
        $("#private_option").attr("disable");
        //wait 10s
        setTimeout(function () {
            $.ajax({
                type: 'GET',
                url: '/Users/PrivateYourAccount?mode=' + private,
                success: function () {
                    successAlert('Set up private account mode.');
                },
                error: function (msg) {
                    errorAlert('msg.');
                }
            });

            $("#private_option").removeAttr("style");
            $("#private_option").removeAttr("disable");

        }, 10000);
    }

});

function errorAlert(err) {
    Swal.fire({
        icon: 'error',
        title: '<strong class="text-danger">Oops...</strong>',
        text: err
    });
}

function successAlert(success) {
    Swal.fire({
        icon: 'success',
        title: '<strong class="text-success">Success!</strong>',
        text: success
    });
}

function UserPostPagination(page) {
    var sort = $('#sortBy').find('.active-short').data('sort');
    var title = $('#title-my-post').val();
    $.ajax({
        type: 'get',
        url: '/Users/PostsPagination?page=' + page + '&sortBy=' + sort + '&title=' + title,
        success: function (data) {
            $('#table-my-post').html(data);
        },
        error: function (msg) {
            Swal.fire({
                icon: 'error',
                title: '<strong class="text-danger">Oops...</strong>',
                text: msg.statusText
            });
        }
    });
}

function userDeletePost(id) {
    Swal.fire({
        title: 'Are you sure?',
        text: "Please contact the administrator if you mistakenly deleted it!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: 'get',
                url: '/Posts/Delete?id=' + id,
                success: function () {
                    Swal.fire(
                        'Deleted!',
                        'Your file has been deleted.',
                        'success'
                    )
                    $('#my_posts_table').find('#' + id).remove();
                },
                error: function (msg) {
                    Swal.fire({
                        icon: 'error',
                        title: '<strong class="text-danger">Oops...</strong>',
                        text: msg.statusText
                    });
                }
            });
        }
    });
}

$('#change_password_button').on('click', function () {
    if ($('#change_password').valid()) {
        var current_password = $('input[name=current_password]').val();
        var password = $('input[name=password]').val();
        $.ajax({
            type: 'post',
            url: '/Setting/ChangePassword',
            data: {
                __RequestVerificationToken: $('#change_password').find('input[name="__RequestVerificationToken"]').val(),
                current_password : current_password,
                password : password
            },
            success: function (data) {
                if (data == null || data == '') {
                    $('#change_password').find('input').val('');
                    successAlert('Password changed successfully.');
                } else {
                    var html = '<span for="Confirm_passord" class="error help-inline">' + data + '.</span>';
                    $('input[name=current_password]').after(html);
                }
            },
            error: function (msg) {
                Swal.fire({
                    icon: 'error',
                    title: '<strong class="text-danger">Oops...</strong>',
                    text: msg.statusText
                });
            }
        });
    }
});

$('#Post').on('submit', function (e) {
    e.preventDefault();

    var content = tinyMCE.get('content').getContent();

    let err = false;
    if ($('#Post').valid()) {
        var error = '';
        if (content == '') {
            error = 'Content can not be empty';
            err = true;
        }
        else if (content.length <= 50) {
            error = 'Content can not less than 50 characters.';
            err = true;
        }

        if (!err) {
            $.ajax({
                type: "Get",
                url: "/Posts/GetPostByTitle?title=" + $('#Post').find('#title').val(),
                success: function (data) {
                    if (data === "True") {
                        successAlert('Post successfully. Please wait for the administrator to approve the post.');
                        setTimeout(function () {
                            $('#content').text(content);
                            e.target.submit();
                        }, 3000);
                    } else {
                        errorAlert('Title was used.')
                    }
                }
            });
        } else {
            errorAlert(error);
        }
    }
});

$('#addTags').select2({
    tags: true,
    placeholder: "Enter all tags of this post",
    width: 'resolve'
});

function loadPost() {
    var page = $('.post-pagination').find('li .active').text();
    UserPostPagination(page);
}

function followedPostPagination(page) {
    var sort = $('#sortBy').find('.active-short').data('sort');
    var title = $('#title-my-post').val();
    $.ajax({
        type: 'get',
        url: '/Posts/followedPostPagination?page=' + page + '&sortBy=' + sort + '&title=' + title,
        success: function (data) {
            $('#table-my-post').html(data);
        },
        error: function (msg) {
            Swal.fire({
                icon: 'error',
                title: '<strong class="text-danger">Oops...</strong>',
                text: msg.statusText
            });
        }
    });
}

function loadFollowedPost() {
    var page = $('.post-pagination').find('li .active').text();
    followedPostPagination(page);
}