const tokenKey = "@ventura/token"

export function saveToken(token) {
    localStorage.setItem(tokenKey, token);
}

export function getToken() {
  let local = null
  if (process.client) {
    local = localStorage.getItem(tokenKey);
  }
  return local
}

export function deleteToken() {
  return localStorage.removeItem(tokenKey);
}
