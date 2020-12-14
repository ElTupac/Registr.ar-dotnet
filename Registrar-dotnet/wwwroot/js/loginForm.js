let once = false;

window.addEventListener("message", evento => {
    if(!once){
        once = true;
        const login = document.getElementById("loginForm");
        login.addEventListener('submit', e => {
            e.preventDefault();
            const username = document.getElementById("username").value;
            const password = document.getElementById("password").value;
            const fd = new FormData();
            fd.append("username", username);
            fd.append("password", password);

            login.style.display = "none";
            document.getElementById("charging").style.display = "block";
            const query = window.location.search;
            let URL = window.location.href.split(window.location.pathname)[0];
            URL += `/Clientes/LoginFinal${query}`;
            fetch(URL, {
                method: "POST",
                body: fd
            }).then(res => res.json())
            .then(res => {
                //res = (res.replace("\\u0022", "\""));
                console.log(res);
                res = JSON.parse(res);
                if(res.creds.ok){
                    evento.source.postMessage(JSON.stringify(res), evento.origin);
                }else{
                    document.getElementById("charging").style.display = "none";
                    password.value = '';
                    login.style.display = "flex";
                    document.getElementById("alerta").style.display = "block";
                }
            });
        });
    }
});