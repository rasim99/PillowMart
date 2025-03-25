let dropDown = document.querySelector(".select__option__list")
let optionMenu = document.querySelector(".select__option__dropdown")
dropDown.addEventListener("click",function() {
    const icon = dropDown.querySelector("i")
    if (icon.classList.contains("fa-square-caret-down")) {
        icon.classList.remove("fa-square-caret-down")
        icon.classList.add("fa-square-caret-up")
    }
    else {
        icon.classList.remove('fa-square-caret-up');
        icon.classList.add('fa-square-caret-down');
    }
   optionMenu.classList.toggle("d-none")
})