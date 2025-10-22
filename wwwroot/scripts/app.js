// User identifier management
function getUserId() {
    let userId = localStorage.getItem('userId');
    if (!userId) {
        userId = generateGuid();
        localStorage.setItem('userId', userId);
    }
    return userId;
}

function generateGuid() {
    return 'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx'.replace(/x/g, function() {
        return (Math.random() * 16 | 0).toString(16);
    });
}

// Add user ID to all HTMX requests
document.addEventListener('htmx:configRequest', (event) => {
    event.detail.headers['X-User-Id'] = getUserId();
});

// Check for new user ID in response headers
document.addEventListener('htmx:afterRequest', (event) => {
    const newUserId = event.detail.xhr.getResponseHeader('X-User-Id');
    if (newUserId) {
        localStorage.setItem('userId', newUserId);
    }
    
    // Activate delete on notifications
    (document.querySelectorAll('.notification .delete') || []).forEach(($delete) => {
        const $notification = $delete.parentNode;

        $delete.addEventListener('click', () => {
            $notification.parentNode.removeChild($notification);
        });
    });
});

async function myFunction() {
    let arr = document.getElementsByClassName("component");

    for (let i = 0; i < arr.length; i++) {
        new WinBox("Mount DOM " + i, { mount: arr[i] });
    }
}

async function loadW(caller, target) {
    let t = document.getElementById(target)
    caller.style.display = caller.style.display === 'none' ? '' : 'none';
    await new WinBox("Pop-up ", {
        mount: t, onclose: function () {

            caller.style.display = caller.style.display === 'none' ? '' : 'none';
        }
    });
}