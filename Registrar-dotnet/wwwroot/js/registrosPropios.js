window.addEventListener('load', e => {
    const checkboxes = document.querySelectorAll('.registrospropios');
    let checked = [];

    for(let i in checkboxes){
        checkboxes[i].onchange = e => {
            const _id = parseInt(checkboxes[i].id.split("-")[0]);
            if(checkboxes[i].checked){
                addChecks(_id);
            }else{
                removeChecks(_id);
            }
        };
    }


    const eliminar = document.getElementById("btn-eliminar");
    const pausarLogs = document.getElementById("pausar-logs");
    const pausarRegs = document.getElementById("pausar-regs");
    const pausarTodo = document.getElementById("pausar-todo");
    eliminar.addEventListener("click", e => {
        let URL = URLhostname();
        URL = `${URL}/Home/EliminarRegistros?reg_ids=${JSON.stringify(checked)}`;
        window.location.href = URL;
    });

    pausarLogs.addEventListener("click", e => {
        let URL = URLhostname();
        URL = `${URL}/Home/PausarLogs?reg_ids=${JSON.stringify(checked)}`;
        window.location.href = URL;
    });

    pausarRegs.addEventListener("click", e => {
        let URL = URLhostname();
        URL = `${URL}/Home/PausarRegs?reg_ids=${JSON.stringify(checked)}`;
        window.location.href = URL;
    });

    pausarTodo.addEventListener("click", e => {
        let URL = URLhostname();
        URL = `${URL}/Home/PausarTodo?reg_ids=${JSON.stringify(checked)}`;
        window.location.href = URL;
    });

    function addChecks(id){
        checked.push(id);
        if(checked.length == 1) {
            pausarLogs.innerText = 'Pausar/despausar logueo';
            pausarRegs.innerText = 'Pausar/despausar registro';
            eliminar.style.display = "inline-flex";
            pausarLogs.style.display = "inline-flex";
            pausarRegs.style.display = "inline-flex";
            pausarTodo.style.display = "inline-flex";
        }else if(checked.length > 1){
            pausarLogs.innerText = 'Pausar logueos';
            pausarRegs.innerText = 'Pausar registros';
            pausarTodo.innerText = 'Pausar todo';
            pausarTodo.style.display = "inline-flex";
        }
    }
    
    function removeChecks(id){
        const index = checked.indexOf(c => c == id);
        checked.splice(index, 1);
        if(!checked.length) {
            eliminar.style.display = "none";
            pausarLogs.style.display = "none";
            pausarRegs.style.display = "none";
            pausarTodo.style.display = "none";
        }else if(checked.length == 1){
            pausarLogs.innerText = 'Pausar/despausar logueo';
            pausarRegs.innerText = 'Pausar/despausar registro';
            eliminar.style.display = "inline-flex";
            pausarLogs.style.display = "inline-flex";
            pausarRegs.style.display = "inline-flex";
            pausarTodo.style.display = "inline-flex";
        }else{
            pausarLogs.innerText = 'Pausar logueos';
            pausarRegs.innerText = 'Pausar registros';
            pausarTodo.innerText = 'Pausar todo';
            pausarTodo.style.display = "inline-flex";
        }
    }
});

function EliminarAdministrador(reg_id){
    const selector = document.getElementById(`${reg_id}-select`);
    if(selector.value != "0"){
        let URL = window.location.href;
        URL = URL.split(window.location.pathname)[0];
        URL = `${URL}/Home/EliminarAdministrador?reg_id=${reg_id}&admin_id=${selector.value}`;
        window.location.href = URL;
    }
};

function URLhostname(){ return window.location.href.split(window.location.pathname)[0]; }