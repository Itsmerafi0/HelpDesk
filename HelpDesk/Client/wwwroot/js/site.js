let menuicn = document.querySelector(".menuicn");
let nav = document.querySelector(".navcontainer");

menuicn.addEventListener("click", () => {
	nav.classList.toggle("navclose");
})


/*Modal*/

var modal = document.getElementById('image-modal');

// Get the image and modal content elements
var img = document.getElementsByClassName('modal-image')[0];
var modalImg = document.getElementById('modal-content');

// Open the modal when image is clicked
img.onclick = function () {
    modal.style.display = 'block';
    modalImg.src = this.src;
};

// Close the modal when the 'x' close button is clicked
var closeBtn = document.getElementsByClassName('close')[0];
closeBtn.onclick = function () {
    modal.style.display = 'none';
};

// Close the modal when user clicks outside the modal
window.onclick = function (event) {
    if (event.target == modal) {
        modal.style.display = 'none';
    }
};
