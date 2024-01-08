$('.eliminar-usuario').on('click', function() {
    var username = $(this).data('username');
    var deleteUserUrl = $(this).data('url');

    Swal.fire({
        title: '¿Estás seguro?',
        text: 'No podrás revertir esto',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Sí, eliminarlo'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: 'POST',
                url: deleteUserUrl,
                data: { username: username },
                success: function () {
                    Swal.fire('Eliminado', 'El usuario ha sido eliminado correctamente.', 'success');
                    location.reload();
                },
                error: function () {
                    Swal.fire('Error', 'Hubo un problema al intentar eliminar el usuario.', 'error');
                }
            });
        }
    });
});

    

document.getElementById('btnUpdateUser').addEventListener('click', function () {
    Swal.fire({
        title: '¿Estás seguro?',
        text: 'Se actualizará la información del usuario.',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Sí, actualizar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            document.getElementById('updateUserForm').submit();
        }
    });
});
