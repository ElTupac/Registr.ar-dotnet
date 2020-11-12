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