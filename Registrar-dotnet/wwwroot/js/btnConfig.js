function entrarConfig(reg_id){
    let URL = window.location.href;
    URL = URL.split(window.location.pathname)[0];
    URL = `${URL}/Home/RegSettings/${reg_id}`;
    window.location.href = URL;
}