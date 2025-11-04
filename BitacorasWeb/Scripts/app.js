// assets/js/app.js

const AUTH_KEY = "bitacoras:user";

// Login simulado
function login(e){
  if(e) e.preventDefault();
  const user = document.getElementById("usuario")?.value?.trim();
  const pass = document.getElementById("password")?.value?.trim();

  if(!user || !pass){
    alert("Ingrese usuario y contraseña.");
    return false;
  }

  // Guarda “sesión” simple (solo nombre)
  sessionStorage.setItem(AUTH_KEY, JSON.stringify({ user }));

  // Redirige al Home
  window.location.href = "home.html";
  return false;
}

// Logout: limpiar sesión y volver al login
function logout(){
  sessionStorage.removeItem(AUTH_KEY);
  window.location.href = "Login.html";
}

// Mostrar el usuario en la barra si existe #whoami
(function showUserIfAny(){
  const span = document.getElementById("whoami");
  const raw = sessionStorage.getItem(AUTH_KEY);
  if(span && raw){
    const { user } = JSON.parse(raw);
    span.textContent = `Usuario: ${user}`;
  }
})();