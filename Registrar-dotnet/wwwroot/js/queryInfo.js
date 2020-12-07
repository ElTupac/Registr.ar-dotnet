window.addEventListener('load', e => {
    const urlParams = new URLSearchParams(window.location.search);
    const reg_id = urlParams.get("reg_id");
    const creator_id = urlParams.get("creator_id");

    const register = document.getElementById("registerForm");
    if(register != null){
        register.action += `?reg_id=${reg_id}&creator_id=${creator_id}`;
    }else{
        const login = document.getElementById("loginForm");
        login.action += `?reg_id=${reg_id}&creator_id=${creator_id}`;
    }
});