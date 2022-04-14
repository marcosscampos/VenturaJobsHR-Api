import axios from "axios";

const httpClient = axios.create();

if (sessionStorage.authToken) {
  httpClient.defaults.headers.common['Authorization'] = 'Bearer ' + sessionStorage.authToken;
}

httpClient.interceptors.request.use(config => {
  return config
});

httpClient.interceptors.response.use(response => {
  return response
});

export default httpClient;