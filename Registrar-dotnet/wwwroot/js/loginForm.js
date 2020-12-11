window.addEventListener("message", evento => {
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
            res = JSON.parse(res);
            if(res.ok){
                //`{user: ${username}, token: ${res.token}}`
                evento.source.postMessage(JSON.stringify({user: username, token: res.token}), evento.origin);
            }else{
                document.getElementById("charging").style.display = "none";
                login.style.display = "block";
                document.getElementById("alerta").style.display = "block";
            }
        });
    })
})