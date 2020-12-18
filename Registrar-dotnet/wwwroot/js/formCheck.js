document.getElementById("registerForm").addEventListener('submit', e => {
    const pass1 = document.getElementById("password").value;
    const pass2 = document.getElementById("password2").value;
    if(pass1 != pass2){
        e.preventDefault();
        document.getElementById("alerta").style.display = "block";
    }else{
        document.getElementById("alerta").style.display = "none";
    }
})

let open = false;
document.getElementById("to-login").addEventListener('click', e => {
    //--var-heigth
    if(!open){
        open = true;
        const loginContainer = document.getElementById("login-container");
        makeTaller(loginContainer, 100);
    }
});

function makeTaller(element, until){
    let stopId;
    let progress = Number(getComputedStyle(element).getPropertyValue("--var-heigth").split("px")[0])
    function step(timestamp){
        if(progress > until){
            cancelAnimationFrame(stopId);
            const formContainer = document.getElementById("form-container");
            fadeIn(formContainer);
            return;
        }else{
            progress += 1;
            element.style.setProperty("--var-heigth", (progress + "px"));
            stopId = window.requestAnimationFrame(step);
        }
    }
    window.requestAnimationFrame(step);
}

function fadeIn(element){
    let stopId;
    let progress = 0;
    element.style.opacity = progress;
    element.style.display = "flex";
    function step(timestamp){
        if(progress > 1){
            cancelAnimationFrame(stopId);
            return;
        }else{
            progress += 0.05;
            element.style.opacity = progress;
            stopId = window.requestAnimationFrame(step);
        }
    }
    window.requestAnimationFrame(step);
}