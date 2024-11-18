function confirmLogout(warningMessage, redirectUrl) {
    Swal.fire({
        icon: 'warning',
        title: 'Are you sure?',
        text: warningMessage,
        showCancelButton: true,
        confirmButtonText: 'Yes, Logout',
        cancelButtonText: 'Cancel'
    }).then((result) => {
        if (result.isConfirmed) {
            // Redirect to the login page
            window.location.href = redirectUrl;
        }
    });
}
