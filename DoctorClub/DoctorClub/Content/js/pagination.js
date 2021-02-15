function PostPagination(page,Id,type) {
    var sort = $('#sortBy').find('.active-short').data('sort');
    var title = $('#title-post').val();
    if (type == null) {
        $.ajax({
            type: 'get',
            url: '/Posts/Pagination?page=' + page + '&sortBy=' + sort + '&title=' + title + '&subcat=' + Id,
            success: function (data) {
                $('#post-list').html(data);
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
        $.ajax({
            type: 'get',
            url: '/Tags/Pagination?page=' + page + '&sortBy=' + sort + '&title=' + title + '&tag=' + Id,
            success: function (data) {
                $('#post-list').html(data);
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
}

function loadPagination(Id,type) {
    var page = $('.post-pagination').find('li .active').text();
    PostPagination(page, Id,type);
}