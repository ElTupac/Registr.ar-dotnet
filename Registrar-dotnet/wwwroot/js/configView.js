window.addEventListener('load', e => {
    const btnLogueos = document.getElementById("pausar-logueos");
    const btnRegistros = document.getElementById("pausar-registros");
    let pausesInfo = document.getElementById("pauses-info").value;
    pausesInfo = JSON.parse(pausesInfo);
    if(pausesInfo[0] == "false"){
        btnLogueos.innerText = "Despausar logueos";
    }else{
        btnLogueos.innerText = "Pausar logueos";
    }

    if(pausesInfo[1] == "false"){
        btnRegistros.innerText = "Despausar registros";
    }else{
        btnRegistros.innerText = "Pausar registros";
    }

    const login = document.getElementById("btn-login");
    login.style.height = login.scrollHeight+"px";
    const register = document.getElementById("btn-register");
    register.style.height = register.scrollHeight+"px";
});

function EliminarAdministrador(reg_id){
    const selector = document.getElementById(`${reg_id}-select`);
    if(selector.value != "0"){
        let URL = window.location.href;
        URL = URL.split(window.location.pathname)[0];
        URL = `${URL}/Home/EliminarAdministrador?reg_id=${reg_id}&admin_id=${selector.value}`;
        window.location.href = URL;
    }
}