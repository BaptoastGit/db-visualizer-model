document.addEventListener('mouseup', (event) => {
    window.mouseDown = false;
});


document.addEventListener('mousemove', (event) => {
    let delta = event.clientX - window.mouseX;
    if (window.mouseDown && (document.getElementById(window.columnId).getBoundingClientRect().width > 150 || delta > 0)) {
        let delta = event.clientX - window.mouseX;
        document.getElementById(window.columnId).width = Math.min(window.columnSize + delta, 600);
    }
});

function getColumnSize(el) {
    window.columnSize = el.parentElement.parentElement.getBoundingClientRect().width;
    window.columnId = el.parentElement.parentElement.id;
    window.mouseX = event.clientX;
    window.mouseDown = true; 


}
