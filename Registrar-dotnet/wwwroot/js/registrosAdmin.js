window.onload = () => {
    const checkboxes = document.querySelectorAll('.registrosadmin');
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

    
    const pausarLogs = document.getElementById("pausar-logs-admin");
    const pausarRegs = document.getElementById("pausar-regs-admin");
    const pausarTodo = document.getElementById("pausar-todo-admin");

    pausarLogs.addEventListener("click", e => {
        let URL = window.location.href;
        URL = URL.split(window.location.pathname)[0];
        URL = `${URL}/Home/PausarLogs?reg_ids=${JSON.stringify(checked)}`;
        window.location.href = URL;
    });

    pausarRegs.addEventListener("click", e => {
        let URL = window.location.href;
        URL = URL.split(window.location.pathname)[0];
        URL = `${URL}/Home/PausarRegs?reg_ids=${JSON.stringify(checked)}`;
        window.location.href = URL;
    });

    pausarTodo.addEventListener("click", e => {
        let URL = window.location.href;
        URL = URL.split(window.location.pathname)[0];
        URL = `${URL}/Home/PausarTodo?reg_ids=${JSON.stringify(checked)}`;
        window.location.href = URL;
    });

    function addChecks(id){
        checked.push(id);
        if(checked.length == 1) {
            pausarLogs.innerText = 'Pausar/despausar logueo';
            pausarRegs.innerText = 'Pausar/despausar registro';
            pausarLogs.style.display = "block";
            pausarRegs.style.display = "block";
            pausarTodo.style.display = "none";
        }else if(checked.length > 1){
            pausarLogs.innerText = 'Pausar logueos';
            pausarRegs.innerText = 'Pausar registros';
            pausarTodo.innerText = 'Pausar todo';
            pausarTodo.style.display = "block";
        }
    }
    
    function removeChecks(id){
        const index = checked.indexOf(c => c == id);
        checked.splice(index, 1);
        if(!checked.length) {
            pausarLogs.style.display = "none";
            pausarRegs.style.display = "none";
            pausarTodo.style.display = "none";
        }else if(checked.length == 1){
            pausarLogs.innerText = 'Pausar/despausar logueo';
            pausarRegs.innerText = 'Pausar/despausar registro';
            pausarLogs.style.display = "block";
            pausarRegs.style.display = "block";
            pausarTodo.style.display = "none";
        }else{
            pausarLogs.innerText = 'Pausar logueos';
            pausarRegs.innerText = 'Pausar registros';
            pausarTodo.innerText = 'Pausar todo';
            pausarTodo.style.display = "block";
        }
    }
}