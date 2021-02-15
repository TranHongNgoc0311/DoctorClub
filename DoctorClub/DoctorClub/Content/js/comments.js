function subComment(parent) {
    $('#sub-comment-modal').modal('show');
    $('#sub-comment-modal').find('input[name="parent"]').val(parent);
}

function postComment() {
    var content = $('#sub-comment-modal').find('textarea[name="Content"]').val();
    var parent = $('#sub-comment-modal').find('input[name="parent"]').val();
    let err = false;
    var error = '';
    if (content == '') {
        error = 'Content can not be empty';
        err = true;
    }
    else if (content.length <= 10) {
        error = 'Content can not less than 10 characters.';
        err = true;
    }
    else if (content.length >= 1000) {
        error = 'Content can not more than 1000 characters.';
        err = true;
    }

    if (!err) {
        $.ajax({
            type: 'POST',
            url: '/Comments/Comments',
            data: {
                Content: content,
                __RequestVerificationToken: $('#sub-comment-modal').find('input[name="__RequestVerificationToken"]').val(),
                postSlug: $('#sub-comment-modal').find('input[name="postSlug"]').val(),
                ParrentId: parent
            },
            success: function () {
                Swal.fire({
                    icon: 'success',
                    title: '<strong class="text-success">Success</strong>',
                    text: 'Your commentYour comment has been successfully posted'
                });
                commentChildLoad(parent);
                $('#sub-comment-modal').modal('hide');
            },
            error: function (msg) {
                Swal.fire({
                    icon: 'error',
                    title: '<strong class="text-danger">Oops...</strong>',
                    text: msg.statusText
                });
            }
        });
    } else {
        errorAlert(error);
    }
}

$('#CommentForm').on('submit', function (e) {
    e.preventDefault();
    var content = $('#contentCmt').val();

    let err = false;
    var error = '';
    if (content == '') {
        error = 'Content can not be empty';
        err = true;
    }
    else if (content.length <= 10) {
        error = 'Content can not less than 10 characters.';
        err = true;
    }
    else if (content.length >= 1000) {
        error = 'Content can not more than 1000 characters.';
        err = true;
    }

    if (!err) {
        successAlert('Your comment has been successfully posted.');
        setTimeout(function () {
            e.target.submit();
        }, 1500);
    } else {
        errorAlert(error);
    }
});

function LikeComment(status, cmt, username) {
    if (status == 1) {
        if ($('#comment-' + cmt).find('.dislike').find('.btn_active')) {
            $('#comment-' + cmt).find('.dislike').removeClass('btn_active');
        }
        $('#comment-' + cmt).find('.like').toggleClass('btn_active');
    } else {
        if ($('#comment-' + cmt).find('.like').find('.btn_active')) {
            $('#comment-' + cmt).find('.like').removeClass('btn_active');
        }
        $('#comment-' + cmt).find('.dislike').toggleClass('btn_active');
    }

    $.ajax({
        type: 'POST',
        url: '/Comments/Likes',
        data: {
            status: (status == 1) ? true : false,
            userId: username,
            cmtId: cmt
        },
        success: function (data) {
            var count = $('#useful-' + cmt).find('.useful-content').text();
            switch (parseInt(data)) {
                case 1:
                    count = parseInt(count) - 1;
                    $('#useful-' + cmt).find('.useful-content').text(count);
                    snackbarAlert('Vote success', 'notice-success');
                    break;
                case 2:
                    count = parseInt(count) + 1;
                    $('#useful-' + cmt).find('.useful-content').text(count);
                    snackbarAlert('Vote success', 'notice-success');
                    break;
                case 3:
                    count = parseInt(count) + 2;
                    $('#useful-' + cmt).find('.useful-content').text(count);
                    snackbarAlert('Vote success', 'notice-success');
                    break;
                case 4:
                    count = parseInt(count) - 2;
                    $('#useful-' + cmt).find('.useful-content').text(count);
                    snackbarAlert('Vote success', 'notice-success');
                    break;
                default:
                    Swal.fire({
                        icon: 'warning',
                        title: '<strong class="text-warning">Sorry!!!</strong>',
                        text: 'Your reputation must be more than 15 to vote.'
                    });
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

function pagination(PostId, page) {
    var sort = $('#sortBy').val()? $('#sortBy').val() : 1; 
    $.ajax({
        type: 'get',
        url: '/Comments/CommentListPagination?postId=' + PostId + '&page=' + page + '&sortBy=' + sort,
        success: function (data) {
            $('.comment-list').html(data);
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

function edit_comment(id) {
    var content = $('#content-' + id).text();
    $.ajax({
        type: 'get',
        url: '/Comments/Edit?id=' + id,
        success: function (data) {
            if (data === "True") {
                $('#edit-comment').find('input[name=Id]').val(id);
                $('#edit-comment').find('textarea[name=Content]').text(content);
                $('#edit-comment').modal('show');
            } else {
                errorAlert('You only can edit your comment.');
            }
        }
    });
}

function delete_comment(id) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: 'get',
                url: '/Comments/Delete?id=' + id,
                success: function (data) {
                    if (data === "True") {
                        successAlert('Your comment has been successfully deleted.');

                        setTimeout(function () {
                            location.reload();
                        }, 1500);
                    } else {
                        errorAlert('You only can delete your comment.');
                    }
                }
            });
        }
    });
}

$('#edit-comment').find('form').on('submit', function (e) {
    e.preventDefault();

    var content = $('#edit-comment').find('textarea[name=Content]').val();

    let err = false;
    var error = '';
    if (content == '') {
        error = 'Content can not be empty';
        err = true;
    }
    else if (content.length <= 10) {
        error = 'Content can not less than 10 characters.';
        err = true;
    }
    else if (content.length >= 1000) {
        error = 'Content can not more than 1000 characters.';
        err = true;
    }

    if (!err) {
        successAlert('Your comment has been successfully updated.');
        setTimeout(function () {
            e.target.submit();
        }, 1500);
    } else {
        errorAlert(error);
    }
});