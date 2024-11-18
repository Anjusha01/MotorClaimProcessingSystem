function showSuccessAlert(title, message, redirectUrl) {
    Swal.fire({
        icon: 'success',
        title: title,
        text: message,
        confirmButtonText: 'OK'
    }).then((result) => {
        if (result.isConfirmed && redirectUrl) {
            window.location.href = redirectUrl;
        }
    });
}

function showInfoAlert(title, message, redirectUrl) {
    Swal.fire({
        icon: 'info',
        title: title,
        text: message,
        confirmButtonText: 'OK'
    }).then((result) => {
        if (result.isConfirmed && redirectUrl) {
            window.location.href = redirectUrl;
        }
    });
}

function showErrorAlert(title, message, redirectUrl) {
    Swal.fire({
        icon: 'error',
        title: title,
        text: message,
        confirmButtonText: 'OK'
    }).then((result) => {
        if (result.isConfirmed && redirectUrl) {
            window.location.href = redirectUrl;
        }
    });
}


function confirmLogout(logoutMessage, redirectUrl) {
    Swal.fire({
        icon: 'warning',
        title: 'Exit?',
        text: 'Are You Sure want to exit?',
        showCancelButton: true,
        confirmButtonText: 'Yes, Logout',
        cancelButtonText: 'Cancel'
    }).then((result) => {
        if (result.isConfirmed) {

            Swal.fire({
                icon: 'info',
                title: 'Logout',
                text: logoutMessage,
                confirmButtonText: 'OK'
            });
            window.location.href = redirectUrl;
        } else {
         
        }
    });
}




