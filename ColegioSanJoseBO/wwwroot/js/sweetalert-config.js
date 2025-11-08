const SwalConfig = {
    primaryColor: '#005792',
    secondaryColor: '#00406b',
    
    getDefaultConfig() {
        return {
            confirmButtonColor: this.primaryColor,
            cancelButtonColor: '#6c757d',
            customClass: {
                popup: 'swal-custom-popup',
                title: 'swal-custom-title',
                confirmButton: 'swal-custom-button',
                cancelButton: 'swal-custom-button'
            }
        };
    },

    success(title, message = '') {
        return Swal.fire({
            icon: 'success',
            title: title,
            text: message,
            confirmButtonText: 'Entendido',
            ...this.getDefaultConfig()
        });
    },

    error(title, message = '') {
        return Swal.fire({
            icon: 'error',
            title: title,
            html: message,
            confirmButtonText: 'Entendido',
            ...this.getDefaultConfig()
        });
    },

    warning(title, message = '') {
        return Swal.fire({
            icon: 'warning',
            title: title,
            text: message,
            confirmButtonText: 'Entendido',
            ...this.getDefaultConfig()
        });
    },

    info(title, message = '') {
        return Swal.fire({
            icon: 'info',
            title: title,
            text: message,
            confirmButtonText: 'Entendido',
            ...this.getDefaultConfig()
        });
    },

    confirm(title, message = '', confirmText = 'Sí, continuar', cancelText = 'Cancelar') {
        return Swal.fire({
            icon: 'question',
            title: title,
            text: message,
            showCancelButton: true,
            confirmButtonText: confirmText,
            cancelButtonText: cancelText,
            ...this.getDefaultConfig()
        });
    },

    confirmDelete(itemName = 'este elemento') {
        return Swal.fire({
            icon: 'warning',
            title: '¿Está seguro?',
            text: `Esta acción eliminará ${itemName}. Esta acción no se puede deshacer.`,
            showCancelButton: true,
            confirmButtonText: 'Sí, eliminar',
            cancelButtonText: 'Cancelar',
            confirmButtonColor: '#dc3545',
            ...this.getDefaultConfig()
        });
    },

    toast(icon, title, position = 'top-end', timer = 3000) {
        return Swal.fire({
            toast: true,
            position: position,
            icon: icon,
            title: title,
            showConfirmButton: false,
            timer: timer,
            timerProgressBar: true,
            customClass: {
                popup: 'swal-toast-popup'
            }
        });
    }
};

function showTempDataMessages() {
    const successMessage = document.querySelector('[data-success-message]');
    if (successMessage) {
        const message = successMessage.getAttribute('data-success-message');
        SwalConfig.toast('success', message);
    }

    const errorMessage = document.querySelector('[data-error-message]');
    if (errorMessage) {
        const message = errorMessage.getAttribute('data-error-message');
        SwalConfig.toast('error', message);
    }

    const infoMessage = document.querySelector('[data-info-message]');
    if (infoMessage) {
        const message = infoMessage.getAttribute('data-info-message');
        SwalConfig.toast('info', message);
    }

    const warningMessage = document.querySelector('[data-warning-message]');
    if (warningMessage) {
        const message = warningMessage.getAttribute('data-warning-message');
        SwalConfig.toast('warning', message);
    }
}

document.addEventListener('DOMContentLoaded', function() {
    showTempDataMessages();
});
