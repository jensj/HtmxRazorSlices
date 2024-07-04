async function initWindows() {
    const components = document.getElementsByClassName("component");

    for (let i = 0; i < components.length; i++) {
        new WinBox("component_" + i, { mount: components[i] });
    }
}

async function loadW(caller, target) {
    const t = document.getElementById(target);
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