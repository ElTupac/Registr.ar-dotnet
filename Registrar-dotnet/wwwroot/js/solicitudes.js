window.addEventListener('load', e => {
    const btnsCancelar = document.querySelectorAll('.cancelar-soli');
    const btnsAceptar = document.querySelectorAll('.aceptar-soli');
    const btnsRechazar = document.querySelectorAll('.rechazar-soli');

    for(let i in btnsCancelar){
        btnsCancelar[i].onclick = e => {
            const _id = parseInt(btnsCancelar[i].id.split("-")[0]);
            let URL = window.location.href;
            URL = URL.split(window.location.pathname)[0];
            URL = `${URL}/Home/CancelarSoli?soli_id=${_id}`;
            window.location.href = URL;
        };
    }

    for(let i in btnsAceptar){
        btnsAceptar[i].onclick = e => {
            const _id = parseInt(btnsAceptar[i].id.split("-")[0]);
            let URL = window.location.href;
            URL = URL.split(window.location.pathname)[0];
            URL = `${URL}/Home/AceptarSoli?soli_id=${_id}`;
            window.location.href = URL;
        };
    }
   
    for(let i in btnsRechazar){
        btnsRechazar[i].onclick = e => {
            const _id = parseInt(btnsRechazar[i].id.split("-")[0]);
            let URL = window.location.href;
            URL = URL.split(window.location.pathname)[0];
            URL = `${URL}/Home/CancelarSoli?soli_id=${_id}`;
            window.location.href = URL;
        };
    }
});