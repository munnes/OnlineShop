
function ToggleImg(id) {
     // must add position relative to img in razor to add toggle zIndex
    var img = document.getElementById(id);

    if (img.style.transform == "scale(1)") {
        img.style.transform = "scale(2)";
        img.style.zIndex = "2"
    }
    else {
        img.style.transform = "scale(1)"
        img.style.zIndex = "1"
    }
    img.style.transition = "transform 0.25s ease";
}