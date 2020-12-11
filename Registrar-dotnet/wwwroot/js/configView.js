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

    const textAreas = document.querySelectorAll("textarea");
    for(let i = 0; i < textAreas.length; i++){
        textAreas[i].style.height = textAreas[i].scrollHeight + "px";
    }

    document.getElementById("download-script").addEventListener('click', e => {
        let URL = window.location.href.split(window.location.pathname)[0];
        URL += "/libreriaRegistrar/registrarBeta.js";
        fetch(URL).then(res => res.blob())
        .then(res => {
            const link = document.createElement('a');
            link.href = window.URL.createObjectURL(new Blob([res]));
            link.setAttribute("download", "registrarBeta.js");
            document.body.appendChild(link);
            link.click();
            link.parentElement.removeChild(link);
        });
    });
    
    document.getElementById("download-script-react").addEventListener('click', e => {
        let URL = window.location.href.split(window.location.pathname)[0];
        URL += "/libreriaRegistrar/registrarBeta-react.js";
        fetch(URL).then(res => res.blob())
        .then(res => {
            const link = document.createElement('a');
            link.href = window.URL.createObjectURL(new Blob([res]));
            link.setAttribute("download", "registrarBeta-react.js");
            document.body.appendChild(link);
            link.click();
            link.parentElement.removeChild(link);
        });
    });

    document.getElementById("download-styles").addEventListener('click', e => {
        let URL = URLhostname();
        URL += "/libreriaRegistrar/registrarBeta.css";
        fetch(URL).then(res => res.blob())
        .then(res => {
            const link = document.createElement('a');
            link.href = window.URL.createObjectURL(new Blob([res]));
            link.setAttribute("download", "registrarBeta.css");
            document.body.appendChild(link);
            link.click();
            link.parentElement.removeChild(link);
        });
    });

    document.getElementById("btn-login").addEventListener('click', e => {
        e.target.select();
        document.execCommand("copy");
        spawnLilModal("Copiado!", e.clientX, e.clientY);
    });

    document.getElementById("btn-register").addEventListener('click', e => {
        e.target.select();
        document.execCommand("copy");
        spawnLilModal("Copiado!", e.clientX, e.clientY);
    });

    document.getElementById("btn-volver").addEventListener('click', e => {
        window.location.href = URLhostname();
    });
});

function EliminarAdministrador(reg_id){
    const selector = document.getElementById(`${reg_id}-select`);
    if(selector.value != "0"){
        let URL = URLhostname();
        URL = `${URL}/Home/EliminarAdministrador?reg_id=${reg_id}&admin_id=${selector.value}`;
        window.location.href = URL;
    }
}

function PausarLog(reg_id){
    let URL = `${URLhostname()}/Home/PausarLogs?reg_ids=[${reg_id}]`;
    window.location.href = URL;
}

function PausarReg(reg_id){
    let URL = `${URLhostname()}/Home/PausarRegs?reg_ids=[${reg_id}]`;
    window.location.href = URL;
}

function URLhostname(){ return window.location.href.split(window.location.pathname)[0]; }

function spawnLilModal(text, xPos, yPos){
    const modal = document.createElement('div');
    modal.classList.add("lil-modal");
    modal.innerHTML = `
        <p>${text}</p>
    `;
    modal.style.top = (yPos + 20) + "px";
    modal.style.left = xPos + "px";
    document.body.appendChild(modal);
    setTimeout(() => {
        fadeOut(modal);
    }, 1500);
}

function fadeOut(element){
    let stopId;
    let progress = 1;
    function step(timestamp){
        if(progress < 0){
            cancelAnimationFrame(stopId);
            element.parentElement.removeChild(element);
            return;
        }else{
            progress -= 0.05;
            element.style.opacity = progress;
            stopId = window.requestAnimationFrame(step);
        }
    }
    window.requestAnimationFrame(step);
}