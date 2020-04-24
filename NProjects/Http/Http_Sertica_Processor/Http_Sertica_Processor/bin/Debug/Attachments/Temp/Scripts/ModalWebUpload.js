function ShowWebUpload(control, heading) {   
    console.log(control.getBoundingClientRect().top);
    
    //document.getElementById("ModalWebUpload").style.top = control.getBoundingClientRect().top;
    document.getElementById("UploadHeading").textContent = heading;
    document.getElementById("ModalWebUpload").style.display = "block";
}

function HideWebUpload() {
    document.getElementById("ModalWebUpload").style.display = "none";
}