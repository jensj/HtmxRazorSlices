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

// Activate delete on notifications
document.addEventListener('htmx:afterRequest', () => {
    (document.querySelectorAll('.notification .delete') || []).forEach(($delete) => {
        const $notification = $delete.parentNode;

        $delete.addEventListener('click', () => {
            $notification.parentNode.removeChild($notification);
        });
    });
});