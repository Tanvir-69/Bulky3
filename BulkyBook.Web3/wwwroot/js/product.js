$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#productDataTable').DataTable({
        ajax: { url: '/admin/product/getall' },
        columns: [
            { data: 'title', width: "20%" },
            { data: 'isbn', width: "10%" },
            { data: 'author', width: "10%" },
            { data: 'listPrice', width: "10%" },
            { data: 'category.name', width: "10%" },
            {
                data: 'id',
                "render": function (data) {
                    return `
                    <div class="btn btn-group gap-3" role="group">
                    <a href='/admin/product/upsert/${data}' class="btn btn-primary"><i class="bi bi-pencil-square"></i> Edit</a>
                    <a onClick=Delete('/admin/product/delete/${data}') class="btn btn-danger"><i class="bi bi-trash-fill"></i> Delete</a>
                    </div>
                    `
                },
                width: "20%"
            }
        ]
    });
}

function Delete(url) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    dataTable.ajax.reload(),
                        Swal.fire({
                            title: data.success == true ? "Deleted!" : "Failed",
                            text: data.message,
                            icon: data.success == true ? "success" : "error"
                        });
                }
            })

        }
    });
} 