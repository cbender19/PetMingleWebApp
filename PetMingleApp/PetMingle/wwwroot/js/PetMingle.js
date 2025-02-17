
function UnfocusElement(element) {
    element.blur();
}

function UpdateQuantityIndicator() {
    var parent = document.querySelector('.cart-indicator-wrapper');
    var element = document.querySelector('.cart-indicator');
    var quantityVal = document.querySelectorAll('.quantity-value');

    var count = 0;
    for (var i = 0; i < quantityVal.length; i++) {
        count = count + Number(quantityVal[i].innerText);
    }

    if (count == 0) {
        parent.classList.add("hidden");
    }
    else {
        parent.classList.remove("hidden");
    }

    element.innerText = count;
}